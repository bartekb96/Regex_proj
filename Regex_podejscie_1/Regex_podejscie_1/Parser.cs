﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Parser
{
    public string path;
    private string content;
    public List<DataType> DataTypesList;

    public Parser(string path)
    {
        this.path = path;
    }

    public void fileOpen()
    {

        try
        {
            using (StreamReader sr = new StreamReader(this.path))
            {
                content = sr.ReadToEnd();
                //Console.WriteLine(content);
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }

    }

    public List<Wezel> pharseObjectType()
    {
        List<Wezel> wezly = new List<Wezel>();

        int OID;
        string name;
        string parrent_name;
        string syntax;
        string access;
        string description;
        string status;
       
        List<string> obiekty = new List<string>();

        string Node_pattern = @"(?<name>\S+)\sOBJECT-TYPE\s*SYNTAX\s+(?<syntax>.*?)\s*ACCESS\s+(?<access>\S*)\s+STATUS\s+(?<status>\w*)\s*DESCRIPTION\s*(?<description>.*?)::=\s{\s(?<rodzic>\S*)\s(?<OID>\d+)\s}";

        foreach (Match match in Regex.Matches(content, Node_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))    //ignorowanie wielkości znaków, odczyt tekstu jako jednej lini (enter jest tez znakiem '.')
        {
            obiekty.Add(match.Value);       //podział biblioteki na pojedyncze węzły/obiekty
            //Console.WriteLine(match.Value);
        }

        foreach (string ob in obiekty)
        {
            parrent_name = Regex.Match(ob, Node_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[6].Value;     //odczyt poszczególnych danych z jednego węzła/obiektu w postaci stringa
            name = Regex.Match(ob, Node_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[1].Value;
            syntax = Regex.Match(ob, Node_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[2].Value;
            access = Regex.Match(ob, Node_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[3].Value;
            status = Regex.Match(ob, Node_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[4].Value;
            description = Regex.Match(ob, Node_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[5].Value;
            Int32.TryParse(Regex.Match(ob, Node_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[7].Value , out OID);

            wezly.Add(new Wezel(OID, name, syntax, access, status, parrent_name));
        }

        return wezly;
    }


    public List<Wezel> pharseObjectIdentifier()
    {
        List<Wezel> wezly = new List<Wezel>();

        int OID;
        string name;
        string parrent_name;

        List<string> obiekty = new List<string>();

        string Identifier_pattern = @"(?<name>\S+)\s+OBJECT\sIDENTIFIER\s::=\s{\s(?<rodzic>\S+)\s(?<ID>\d+)\s}";

        foreach (Match match in Regex.Matches(content, Identifier_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))    //ignorowanie wielkości znaków, odczyt tekstu jako jednej lini (enter jest tez znakiem '.')
        {
            obiekty.Add(match.Value);
            //Console.WriteLine(match.Value);
        }

        foreach (string ob in obiekty)
        {
            parrent_name = Regex.Match(ob, Identifier_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[2].Value;
            name = Regex.Match(ob, Identifier_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[1].Value;
            Int32.TryParse(Regex.Match(ob, Identifier_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[3].Value, out OID);

            wezly.Add(new Wezel(OID, name, null , null, null, parrent_name));
        }


        /*foreach (Wezel w in wezly)
        {
            Console.WriteLine("OID: " + w.ID);
            Console.WriteLine("NAME: " + w.name);
            Console.WriteLine("PARRENT NAME: " + w.parrent_name);
            Console.WriteLine("---------------------------------------------");
        }*/

        return wezly;
    }


    public List<Wezel> pharseImport()
    {
        List<string> importsBlock = new List<string>();
        List<string> importsList = new List<string>();

        string importPath;

        string Imports_pattern = @"IMPORTS.*?;";
        string from_pattern = @"FROM\s(?<nazwa>\S*)\s";

        List<Wezel> insideFileNodes = new List<Wezel>();
        List<Wezel> insideFileNodes1 = new List<Wezel>();
        List<Wezel> insideFileNodes2 = new List<Wezel>();
        List<Wezel> insideFileNodes3 = new List<Wezel>();

        foreach (Match match in Regex.Matches(content, Imports_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
        { 
            importsBlock.Add(match.Value);
        }

        foreach (string imp in importsBlock)
        {
            foreach (Match m in Regex.Matches(imp, from_pattern))
            {
                importPath = Regex.Match(imp, from_pattern).Groups[1].Value;
                importsList.Add(importPath + ".txt");
            }
        }


        foreach (string s in importsList)
        {
            string path = @"C:\Users\Bartek\source\repos\Regex_proj\Regex_podejscie_1\Regex_podejscie_1\" + s;

            if (File.Exists(path))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        Parser newParser = new Parser(path);
                        newParser.fileOpen();
                        insideFileNodes1 = newParser.pharseImport();
                        insideFileNodes2 = newParser.pharseObjectType();
                        insideFileNodes3 = newParser.pharseObjectIdentifier();
                        //----------------------------------------------------------------------------------------
                        //--------------------------------DODAĆ POZOSTAŁE PARSOWANIA------------------------------
                        //----------------------------------------------------------------------------------------
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("The file could not be read or doesn't exist:");
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("FILE: " + path + " DOES NOT EXIST!");
            }
        }

        foreach (Wezel w in insideFileNodes1)
        {
            insideFileNodes.Add(w);
        }

        foreach (Wezel w in insideFileNodes2)
        {
            insideFileNodes.Add(w);
        }

        foreach (Wezel w in insideFileNodes3)
        {
            insideFileNodes.Add(w);
        }


        /*foreach (Wezel w in insideFileNodes3)
        {
            Console.WriteLine("OID: " + w.ID);
            Console.WriteLine("NAME: " + w.name);
            Console.WriteLine("PARRENT NAME: " + w.parrent_name);
            //Console.WriteLine("---------------------------------------------");
        }*/

        return insideFileNodes;
    }


    public void pharseDataType()
    {
        string type;
        string DT_Class;
        int tag;
        string CodeingType;
        string ancestorType;

        int size;
        long range;

        string subcontent = "";
        DataTypesList = new List<DataType>();

        try
        {
            using (StreamReader sr = new StreamReader(@"C:\Users\Bartek\source\repos\Regex_proj\Regex_podejscie_1\Regex_podejscie_1\RFC1155-SMI.txt"))
            {
                subcontent = sr.ReadToEnd();
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("CANNOT PHARSE DATA TYPE! :");
            Console.WriteLine(e.Message);
        }

        string DataType_pattern = @"(?<type>\S*)\s*::=\s*\[(?<DT_class>\S*)\s(?<tag>\d*)]\s*(--.*?\n\s*|\s*)(?<CodeingType>\S*)\s*(?<ancestorType>.*?)\s*(--|\((0..(?<range>\d+)|SIZE\s*\((?<size>\d+)\))\)|\n)";
        //(?< type >\S *)\s *::=\s *\[(?< DT_class >\S *)\s(?< tag >\d *)]\s* (--.*?\n\s*|\s*)(?<CodeingType>\S*)\s* (?<ancestorType>.*?)\s* (--|\n|\(((?<min_range>\d*..(?<max_range>\d*))\)|SIZE\s*\(((?<size>\d*)|((?<min_size>\d*)..(?<max_size>\d*)))\)\)))

        foreach (Match ob in Regex.Matches(subcontent, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
        {
            type = Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[4].Value;     //odczyt poszczególnych danych z jednego węzła/obiektu w postaci stringa
            DT_Class = Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[5].Value;
            Int32.TryParse(Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[6].Value, out tag);
            CodeingType = Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[7].Value;
            ancestorType = Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[8].Value;
            Int64.TryParse(Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[9].Value, out range);
            Int32.TryParse(Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[10].Value, out size);

            DataTypesList.Add(new DataType(type, DT_Class, tag, CodeingType, ancestorType, size, range));
        }

        /*foreach(DataType d in lista)
        {
            Console.WriteLine("TYPE: " + d.type);
            Console.WriteLine("CLASS: " + d.DT_Class);
            Console.WriteLine("TAG: " + d.tag);
            Console.WriteLine("CODEING TYPE: " + d.CodeingType);
            Console.WriteLine("ANCESTOR TYPE: " + d.ancestorType);
            Console.WriteLine("SIZE: " + d.size);
            Console.WriteLine("RANGE: " + d.range);
            Console.WriteLine("---------------------------------------------");
        }*/

        //DataTypesList.Add(new DataType("1", "1", 1, "1", "1", 1, 1));
    }

    public List<Wezel> pharseMIBfile()
    {
        List<Wezel> drzewo = new List<Wezel>();

        this.fileOpen();

        drzewo.Add(new Wezel(1, "internet", null, null, null, "dod"));
        drzewo.Add(new Wezel(6, "dod", null, null, null, "org"));
        drzewo.Add(new Wezel(3, "org", null, null, null, "iso"));
        drzewo.Add(new Wezel(1, "iso", null, null, null, null));  //korzeń

        foreach(Wezel w in this.pharseImport())
        {
            drzewo.Add(w);
        }

        foreach (Wezel w in this.pharseObjectType())
        {
            drzewo.Add(w);
        }

        foreach (Wezel w in this.pharseObjectIdentifier())
        {
            drzewo.Add(w);
        }

        return drzewo;
    }
}
