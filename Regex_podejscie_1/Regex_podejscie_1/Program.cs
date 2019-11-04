using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Regex_podejscie_1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Wezel> wezly = new List<Wezel>();

            Parser parser = new Parser(@"C:\Users\Bartek\source\repos\Regex_proj\Regex_podejscie_1\Regex_podejscie_1\test.txt");
            parser.fileOpen();
            wezly = parser.pharse();

            //---------------SPRAWDZENIE---------------------------

            foreach (Wezel w in wezly)
            {
                Console.WriteLine("oid wynosi: " + w.OID + ", wartosc wynosi: " + w.Value );
            }

            //-----------------------------------------------------
        }
    }
}
