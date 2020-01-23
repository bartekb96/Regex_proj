﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;

namespace Regex_podejscie_1
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<Wezel> drzewo = new List<Wezel>();

            Parser parser = new Parser(@"C:\Users\Bartek\source\repos\Regex_proj\Regex_podejscie_1\Regex_podejscie_1\RFC1213-MIB.txt");
            //Parser parser = new Parser(@"C:\Users\Marianka\Desktop\Bartek\Regex_proj\Regex_podejscie_1\Regex_podejscie_1\RFC1213-MIB.txt");
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



            /*Koder koder = new Koder();
            koder.setParams(4, "kasztan", 0, 0, false);
            koder.code();*/

            Koder koder = new Koder();
            koder.setParams(4, "kasztan", 0, 0, true);
            koder.code();


            Console.ReadKey();
        }
    }
}
