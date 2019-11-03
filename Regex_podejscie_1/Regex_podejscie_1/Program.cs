using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Regex_podejscie_1
{
    class Program
    {
        static void Main(string[] args)
        {
            int OID = 0;
            string value = "1111";
            List<string> obiekty = new List<string>();
            List<Wezel> wezly = new List<Wezel>();

            string OID_pattern = @"OID: [0-9]";
            string Value_pattern = @"[ ]WARTOSC: [^ ]+";
            string Object_pattern = @"OID: [0-9] +WARTOSC: +[^ ]+";

            string input = "OID: 1 WARTOSC: kot OID: 2 WARTOSC: pies OID: 3 WARTOSC: ryba";

            foreach (Match match in Regex.Matches(input, Object_pattern, RegexOptions.IgnoreCase))
            {
                obiekty.Add(match.Value);
            }

            foreach (string ob in obiekty)
            {
                Int32.TryParse(Regex.Replace(Regex.Replace(ob, Value_pattern, String.Empty), "OID: ", String.Empty), out OID);
                value = (Regex.Replace(Regex.Replace(ob, OID_pattern, String.Empty), "WARTOSC: ", String.Empty));
                wezly.Add(new Wezel(OID, value));
            }

            //---------------SPRAWDZENIE---------------------------

            /*foreach (Wezel w in wezly)
            {
                Console.WriteLine("oid wynosi: " + w.OID + ", wartosc wynosi: " + w.Value );
            }*/

            //-----------------------------------------------------

            Parser parser = new Parser(@"C:\Users\Bartek\source\repos\Regex_podejscie_1\Regex_podejscie_1\test.txt");
            parser.fileOpen();
        }
    }
}
