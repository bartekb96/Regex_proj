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
        string value;
        List<string> obiekty = new List<string>();

        string OID_pattern = @"OID: [0-9]";
        string Value_pattern = @"[ ]WARTOSC: [^ ]+";
        string Object_pattern = @"OID: [0-9] +WARTOSC: +[^ ]+";

        foreach (Match match in Regex.Matches(content, Object_pattern, RegexOptions.IgnoreCase))
        {
            obiekty.Add(match.Value);
        }

        foreach (string ob in obiekty)
        {
            Int32.TryParse(Regex.Replace(Regex.Replace(ob, Value_pattern, String.Empty), "OID: ", String.Empty), out OID);
            value = (Regex.Replace(Regex.Replace(ob, OID_pattern, String.Empty), "WARTOSC: ", String.Empty));
            wezly.Add(new Wezel(OID, value));
        }

        return wezly;
    }


}
