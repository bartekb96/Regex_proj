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
                foreach (Wezel dziecko in w.children)
                    Console.WriteLine(w.name + " MA DZIECKO: " + dziecko.name);
            }

        }
    }
}
