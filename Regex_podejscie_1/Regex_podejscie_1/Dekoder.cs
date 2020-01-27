using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class Dekoder
{
    public Dekoder()
    {

    }

    private string content;
    private List<byte> bytes = new List<byte>();
    private int lenght;
    private int decodedContent;
    private int tag;
    private string decodedStringData;

    public void setContent(string Content)
    {
        this.content = Content;
    }

    public void decode()
    {
        string bytesPattern = @"(?<byte>\d*)\s*";
        int tmp;

        foreach (Match m in Regex.Matches(content, bytesPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
        {
            Int32.TryParse(Regex.Match(m.Value, @"\d*", RegexOptions.IgnoreCase | RegexOptions.Singleline).Value, out tmp);
            bytes.Add(Convert.ToByte(tmp));
        }
        bytes.RemoveAt(bytes.Count - 1);

        switch(bytes[0])
        {
            case 2:
                decodeInteger();
                break;
            case 4:
                decodeOctetString();
                break;
            case 5:
                decodeNull();
                break;
            case 6:
                decodeObjectIdentifier();
                break;
            case 48:
                decodeSequence();
                break;
            default:
                decodeUniversal();
                break;
        }
    }

    private void decodeInteger()
    {
        this.tag = this.bytes[0];
        if(this.bytes[1] < 128)
        {
            this.lenght = this.bytes[1];
            for(int i = 0; i<this.lenght; i++)
            {
                this.decodedContent += bytes[2 + i] * (int)Math.Pow(256, this.lenght - 1 - i);
            }
        }
        else 
        {
            int lengthOfTheLength = this.bytes[1] - 128;
            for(int i = 0; i< lengthOfTheLength; i++)
            {
                this.lenght += bytes[i + 2];
            }
            for(int i = 0; i<this.lenght; i++)
            {
                this.decodedContent += bytes[lengthOfTheLength + 1 + i] * (int)Math.Pow(256, this.lenght - 1 - i);
            }
        }
    }

    private void decodeOctetString()
    {
        this.tag = bytes[0];
        this.lenght = bytes[1];

        for(int i = 0; i<this.lenght; i++)
        {
            this.decodedStringData += Convert.ToChar(bytes[2 + i]);
        }
    }

    private void decodeNull()
    {
        Console.WriteLine("nie wiem jak zdekodowac nulla ale jakbym wiedzial to tu bylby kod ktory to by robil");
    }

    private void decodeObjectIdentifier()
    {
        this.tag = bytes[0];
        this.lenght = bytes[1];
        this.decodedStringData += "1 3 ";
        for(int i = 0; i<this.lenght - 1; i++)
        {
            this.decodedStringData += bytes[i + 3];
            this.decodedStringData += " "; 
        }
    }

    private void decodeSequence()
    {

    }

    private void decodeUniversal()
    {

    }
}
