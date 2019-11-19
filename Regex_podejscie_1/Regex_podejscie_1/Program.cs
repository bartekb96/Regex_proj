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

            Parser parser = new Parser(@"C:\Users\Marianka\Desktop\Bartek\Regex_proj\Regex_podejscie_1\Regex_podejscie_1\RFC1213-MIB.txt");
            parser.fileOpen();
            drzewo = parser.pharseImport();

            //Console.WriteLine("-------------------------------------------------");
            //Console.WriteLine("PARSE OBJECT TYPE");
            foreach (Wezel w in parser.pharseObjectType())
            {
                //Console.WriteLine(w.name);
                drzewo.Add(w);
            }

            //Console.WriteLine("-------------------------------------------------");
            //Console.WriteLine("PARSE OBJECT IDENTIFIER");
            foreach (Wezel w in parser.pharseObjectIdentifier())
            {
                //Console.WriteLine(w.name);
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
                        //Console.WriteLine("znaleziono goscia bez rodzica: " + w.name);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Could not link parrent to child: " + e.Message);
                }
            }

            /*foreach(Wezel w in drzewo)
            {
                foreach (Wezel dziecko in w.children)
                {
                    Console.WriteLine(w.name + " MA DZIECKO: " + dziecko.name);
                }
            }*/

            foreach (Wezel w in drzewo)
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("IMIE: " + w.name);
                foreach (Wezel dziecko in w.children)
                {
                    Console.WriteLine("DZIECKO: " + dziecko.name);
                }
            }

        }
    }
}
