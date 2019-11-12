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
}
