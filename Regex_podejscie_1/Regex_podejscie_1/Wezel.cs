using System;
using System.Collections.Generic;

public class Wezel
{
    public int ID;
    public string name;
    public DataType syntax;
    public string access;
    public string status;
    public string parrent_name;

    public LinkedList<Wezel> children;

    public Wezel(int iod, string name, DataType sntx, string accs, string stat, string parrent)
    {
        this.ID = iod;
        this.name = name;
        this.syntax = sntx;
        this.access = accs;
        this.status = stat;
        this.parrent_name = parrent;
        children = new LinkedList<Wezel>();
    }

    public void AddChild(Wezel W)
    {
        children.AddFirst(W);
    }

    public Wezel GetAllChild(int i)
    {
        foreach (Wezel n in children)
            if (--i == 0)
                return n;
        return null;
    }

}
