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
            parser.fileOpen();
            parser.pharseSequences();
            parser.pharseDataType();
            drzewo = parser.pharseMIBfile();

            TreeBrowser szukacz = new TreeBrowser(drzewo);
            szukacz.addParrent(drzewo);

            //Wezel test = szukacz.findByOid("1.3.6.1.2.1.5.19");

            //Walidator W1 = new Walidator("1.3.6.1.2.1.5.19", 4294967294, drzewo);     //zwykły dataType
            //Console.WriteLine(W1.validade());

            //string oid = szukacz.getFullOid("ifEntry");   //ziomek co jest sekwencją

            //Walidator W1 = new Walidator("1.3.6.1.2.1.2.2.1", 4294967294, drzewo);      //walidowanie sekwencji
            //W1.validade();

            Koder koder = new Koder(196, 3252, 2, 0);
            Console.Write("identyfikator: ");
            koder.printIdentycicator();


            Console.ReadKey();
        }
    }
}
