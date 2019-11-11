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
            Wezel root = new Wezel(0, "root", "root", "root", "root", "root");
            Wezel rodzic = new Wezel(0, "rodzic", "rodzic", "rodzic", "rodzic", "rodzic");

            foreach (Wezel w in liscie)
            {
                if (w.name == dziecko.parrent_name)
                {
                    isRoot = false;
                    rodzic = w;
                    //return w;
                }
            }

            if(isRoot == true)
            {
                return root;
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
}
