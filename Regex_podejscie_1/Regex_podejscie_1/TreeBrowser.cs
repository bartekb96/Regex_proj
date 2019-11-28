using System;
using System.Collections.Generic;

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
        Wezel rodzic = new Wezel(0, "rodzic", "rodzic", "rodzic", "rodzic", "rodzic");

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


    //funkcja do wywalenia
    public List<int> findLongOID(string name)
    {
        List<int> oid = new List<int>();
        Wezel szukany = new Wezel(0, "szukany", "szukany", "szukany", "szukany", "szukany");
        bool doesExist = false;

        foreach(Wezel w in liscie)
        {
            if(w.name == name)
            {
                szukany = w;
                doesExist = true;
            }
        }

        if(doesExist)
        {
            oid.Add(szukany.ID);

        }
        else
        {
            return null;
        }

        return oid;
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
}
