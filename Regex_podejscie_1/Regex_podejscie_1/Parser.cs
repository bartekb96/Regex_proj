using System;

public class Parser
{
    public string path;

    public Parser(string path)
    {
        this.path = path;
    }

    public void fileOpen()
    {
        string text = System.IO.File.ReadAllText(this.path);
        
        //----test------
        Console.WriteLine(text);
        //--------------
    }
}
