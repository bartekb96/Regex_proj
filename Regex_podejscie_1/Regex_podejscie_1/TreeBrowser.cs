using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TreeBrowser
{
    private List<Wezel> liscie = new List<Wezel>();

    public TreeBrowser(List<Wezel> l)
    {
        this.liscie = l;
    }

    public Wezel findParrent(Wezel dziecko)
    {

        bool isRoot = true;
        Wezel rodzic = new Wezel(0, "rodzic", null, "rodzic", "rodzic", "rodzic");

            foreach (Wezel w in liscie)
            {
                if (w.name == dziecko.parrent_name)
                {
                    isRoot = false;
                    rodzic = w;
                }
            }

            if(isRoot == true)
            {
                return null;
            }
            else
            {
                return rodzic;
            }
            
    }

    public Wezel findByName(string name)
    {
            foreach (Wezel w in liscie)
            {
                if (w.name == name)
                {
                    return w;
                }
            }
            return null;
    }

    public Wezel findByOid(string OID)
    {
        string idPattern = @"(?<id>\d*)(\.|\s*)";

        int idSize = Regex.Matches(OID, idPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Count - 1;

        int[] localId = new int[idSize];

        int arrayIndex = 0;

        foreach(Match m in Regex.Matches(OID, idPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
        {
            if(arrayIndex < idSize)
            {
                Int32.TryParse(Regex.Match(m.Value, @"\d*", RegexOptions.IgnoreCase | RegexOptions.Singleline).Value, out localId[arrayIndex]);
                arrayIndex++;
            }
        }

        arrayIndex = 0;

        Wezel tmp = getChildById(this.getRoot(), localId[arrayIndex + 1]);
        arrayIndex++;

        for (int i=1; i<idSize-1; i++)
        {
            if(tmp != null)
            {
                tmp = getChildById(tmp, localId[arrayIndex + 1]);
                arrayIndex++;
            }
            else
            {
                Console.WriteLine("Obiekt o podanym OID nie istnieje!");
            }
        }

        return tmp;
    }

    public Wezel getChildById(Wezel parrent, int childId)
    {
        foreach(Wezel child in parrent.children)
        {
            if(child.ID == childId)
            {
                return child;
            }
        }

        Console.WriteLine(parrent.name + " hasn't got child with ID: " + childId);
        return null;
    }

    public Wezel getRoot()
    {
        foreach(Wezel w in liscie)
        {
            if (w.name == "iso")
                return w;
        }

        return null;
    }

    public void addParrent(List<Wezel> l)
    {
        foreach (Wezel w in l)      //przypisywanie rodziów dziecom
        {
            try
            {
                if (this.findParrent(w) != null)
                {
                    Wezel rodzic = this.findParrent(w);
                    rodzic.AddChild(w);
                }
                else
                {
                    Console.WriteLine("znaleziono goscia bez rodzica: " + w.name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not link parrent to child: " + e.Message);
            }
        }
    }

    public string getFullOid (string name)
    {
        Wezel w = this.findByName(name);
        string tmpOid = w.ID.ToString();

        Wezel tmpParrent = findByName(w.parrent_name);

        while(tmpParrent.name != "iso")
        {
            tmpOid += "." + tmpParrent.ID.ToString();
            tmpParrent = findByName(tmpParrent.parrent_name);
        }

        tmpOid += ".1";

        return tmpOid;
    }
}
