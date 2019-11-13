using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Parser
{
    public string path;

    private string content; 

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
        string status;
       
        List<string> obiekty = new List<string>();

        string OID_pattern = @"::= {[ ]+[^ ]+ [0-9]+";
        string name_pattern = @"\w*\s*OBJECT-TYPE.*?";
        string parrent_name_pattern = "::= {[ ]+[^ ]+";
        string syntax_pattern = @"SYNTAX[ ]+[^ ]+";
        string access_pattern = @"ACCESS[ ]+[^ ]+";
        string status_pattern = "STATUS[ ]+[^ ]+";
        string Node_pattern = @"\w*\s*OBJECT-TYPE\s*SYNTAX.*?ACCESS.*?STATUS.*?DESCRIPTION\s*.*?\s*::=\s*{.*?}";

        foreach (Match match in Regex.Matches(content, Node_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))    //ignorowanie wielkości znaków, odczyt tekstu jako jednej lini (enter jest tez znakiem '.')
        {
            obiekty.Add(match.Value);       //podział biblioteki na pojedyncze węzły/obiekty
            //Console.WriteLine(match.Value);
        }

        foreach (string ob in obiekty)
        {
            parrent_name = Regex.Replace(Regex.Match(ob, parrent_name_pattern).Value, "::= { ", String.Empty);      //odczyt poszczególnych danych z jednego węzła/obiektu w postaci stringa
            name = Regex.Replace(Regex.Match(ob, name_pattern).Value, " OBJECT-TYPE", String.Empty);
            syntax = Regex.Replace(Regex.Match(ob, syntax_pattern).Value, "SYNTAX[ ]+", String.Empty);
            access = Regex.Replace(Regex.Match(ob, access_pattern).Value, @"ACCESS[ ]+", String.Empty);
            status = Regex.Replace(Regex.Match(ob, status_pattern).Value, "STATUS[ ]+", String.Empty);
            Int32.TryParse(Regex.Replace(Regex.Match(ob, OID_pattern).Value, "::= {[ ]+[^ ]+ ", String.Empty), out OID);

            access = Regex.Replace(access, @"\s*", String.Empty, RegexOptions.Singleline);
            status = Regex.Replace(status, @"\s*", String.Empty, RegexOptions.Singleline);
            syntax = Regex.Replace(syntax, @"\s*", String.Empty, RegexOptions.Singleline);

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

        string Identifier_pattern = @"\w*\s*OBJECT\s*IDENTIFIER\s*::=\s*{\s*[^ ]+\s*[0-9]*\s*}";
        string name_pattern = @"\w*\s*OBJECT\s*IDENTIFIER.*?";
        string OID_pattern = @"::= {[ ]+[^ ]+ [0-9]+";
        string parrent_name_pattern = "::= {[ ]+[^ ]+";

        foreach (Match match in Regex.Matches(content, Identifier_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))    //ignorowanie wielkości znaków, odczyt tekstu jako jednej lini (enter jest tez znakiem '.')
        {
            obiekty.Add(match.Value);
            //Console.WriteLine(match.Value);
        }

        foreach (string ob in obiekty)
        {
            parrent_name = Regex.Replace(Regex.Match(ob, parrent_name_pattern).Value, "::= { ", String.Empty);      //odczyt poszczególnych danych z jednego węzła/obiektu w postaci stringa
            name = Regex.Replace(Regex.Match(ob, name_pattern).Value, " OBJECT IDENTIFIER", String.Empty);
            Int32.TryParse(Regex.Replace(Regex.Match(ob, OID_pattern).Value, "::= {[ ]+[^ ]+ ", String.Empty), out OID);

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
        string from_pattern = @"FROM\s*[^ ;]+";

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
            foreach (Match m in Regex.Matches(imp, from_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
            {
                importPath = Regex.Replace(m.Value, @"FROM\s *", String.Empty);
                importPath = Regex.Replace(importPath, @"\s*", String.Empty, RegexOptions.Singleline);
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

        return insideFileNodes;
    }
}
