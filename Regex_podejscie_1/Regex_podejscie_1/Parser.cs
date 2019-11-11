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

    public List<Wezel> pharse()
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


}
