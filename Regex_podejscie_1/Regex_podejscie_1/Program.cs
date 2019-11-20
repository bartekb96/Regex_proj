using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Regex_podejscie_1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Wezel> drzewo = new List<Wezel>();

            Parser parser = new Parser(@"C:\Users\Bartek\source\repos\Regex_proj\Regex_podejscie_1\Regex_podejscie_1\RFC1213-MIB.txt");
            parser.fileOpen();
            drzewo = parser.pharseImport();


            foreach (Wezel w in parser.pharseObjectType())
            {
                drzewo.Add(w);
            }

            foreach (Wezel w in parser.pharseObjectIdentifier())
            {
                drzewo.Add(w);
            }

            drzewo.Add(new Wezel(1, "internet", null, null, null, "dod"));
            drzewo.Add(new Wezel(6, "dod", null, null, null, "org"));
            drzewo.Add(new Wezel(3, "org", null, null, null, "iso"));
            drzewo.Add(new Wezel(1, "iso", null, null, null, null));  //korzeń

            TreeBrowser szukacz = new TreeBrowser(drzewo);
            foreach(Wezel w in drzewo)      //przypisywanie rodziów dziecom
            {
                try
                {
                    if (szukacz.findParrent(w) != null)
                    {
                        Wezel rodzic = szukacz.findParrent(w);
                        rodzic.AddChild(w);
                    }
                    else
                    {
                        Console.WriteLine("znaleziono goscia bez rodzica: " + w.name);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Could not link parrent to child: " + e.Message);
                }
            }

            foreach(Wezel w in drzewo)
            {
                if (w.children.Count != 0)
                {
                    foreach (Wezel dziecko in w.children)
                    {
                        Console.WriteLine(w.name + " MA DZIECKO: " + dziecko.name);
                    }
                    Console.WriteLine("-----------------------------------");
                }
            }
        }
    }
}
