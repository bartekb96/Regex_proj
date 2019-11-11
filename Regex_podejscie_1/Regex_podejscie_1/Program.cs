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

            Parser parser = new Parser(@"C:\Users\Bartek\source\repos\Regex_proj\Regex_podejscie_1\Regex_podejscie_1\test.txt");
            parser.fileOpen();
            drzewo = parser.pharse();

            //----------TEST----------------------------------------------------------------
            /*foreach (Wezel w in drzewo)
            {
                Console.WriteLine("OID: " + w.ID);
                Console.WriteLine("RODZIC: " + w.parrent_name);
                Console.WriteLine("NAZWA: " + w.name);
                Console.WriteLine("STATUS: " + w.status);
                Console.WriteLine("SYNTAX: " + w.syntax);
                Console.WriteLine("ACCESS: " + w.access);
                Console.WriteLine("-------------------------------");
            }*/
            //------------------------------------------------------------------------------

            TreeBrowser szukacz = new TreeBrowser(drzewo);
            foreach(Wezel w in drzewo)
            {
                Wezel rodzic = szukacz.findParrent(w);
                rodzic.AddChild(w);
            }

            foreach(Wezel w in drzewo)
            {
                foreach (Wezel dziecko in w.children)
                    Console.WriteLine(w.name + " MA DZIECKO: " + dziecko.name);
            }

        }
    }
}
