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
        foreach(Wezel w in liscie)
        {
            if(w.name == dziecko.parrent_name)
            {
                return w;
            }
        }
        return null;
    }

}
