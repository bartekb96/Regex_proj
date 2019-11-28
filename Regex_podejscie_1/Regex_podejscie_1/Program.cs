using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Regex_podejscie_1
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<Wezel> drzewo = new List<Wezel>();
            List<DataType> datas = new List<DataType>();

            Parser parser = new Parser(@"C:\Users\Bartek\source\repos\Regex_proj\Regex_podejscie_1\Regex_podejscie_1\RFC1213-MIB.txt");
            parser.pharseDataType();
            drzewo = parser.pharseMIBfile();

            TreeBrowser szukacz = new TreeBrowser(drzewo);
            szukacz.addParrent(drzewo);



            /*foreach (Wezel w in drzewo)
            {
                if (w.children.Count != 0)
                {
                    foreach (Wezel dziecko in w.children)
                    {
                        Console.WriteLine(w.name + " MA DZIECKO: " + dziecko.name);
                    }
                    Console.WriteLine("-----------------------------------");
                }
            }*/
        }
    }
}
