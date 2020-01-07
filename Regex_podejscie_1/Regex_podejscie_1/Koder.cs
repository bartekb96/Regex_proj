using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Koder
{
    /*public enum Klasa 
    { 
        Universal = 0,
        Application = 1,
        ContextSpecyfic = 2,
        Private = 3
    };

    public enum PrimitiveOrConstructed
    {
        Primitive = 0,
        Constructed = 1
    };*/

    private int tag;
    private long content;
    private int klasa;
    private int primitiveOrConstructed;

    private List<byte> lengthAndContent = new List<byte>();
    private List<byte> identyficator = new List<byte>();

    public Koder()
    {

    }

    public void setParams(int _Tag, long _Content, int _klasa, int _primitiveOrConstructed)
    {
        this.tag = _Tag;
        this.content = _Content;
        this.klasa = _klasa;
        this.primitiveOrConstructed = _primitiveOrConstructed;
    }

    public List<byte> codeUniversal()
    {
        List<byte> lst = new List<byte>();
        lst.AddRange(getIdentyficator());
        lst.AddRange(getLengthAndContent());

        //Data Visualization
            foreach(byte b in lst)
            {
                Console.Write(b + "  ");
            }

        return lst;
    }

    public List<byte> codeInteger()  //dokończyć
    {
        this.primitiveOrConstructed = 0; //kodowanie prymitywne

        List<byte> lst = new List<byte>();
        lst.AddRange(getIdentyficator());
        lst.AddRange(getLengthAndContent());

        //Data Visualization
        foreach (byte b in lst)
        {
            Console.Write(b + "  ");
        }

        return lst;
    }

    private List<byte> getIdentyficator()
    {
        byte[] _identificator;

        if (this.tag < 31)
        {
            _identificator = new byte[1] { 0b_0000_0000};

            //----------CLASS SETTING---------------------
            switch (klasa)
            {
                case 0:
                    _identificator[0] = (byte)(_identificator[0] & ~(1 << 6));
                    _identificator[0] = (byte)(_identificator[0] & ~(1 << 7));
                    break;
                case 1:
                    _identificator[0] = (byte)(_identificator[0] | (1 << 6));
                    break;
                case 2:
                    _identificator[0] = (byte)(_identificator[0] | (1 << 7));
                    break;
                case 3:
                    _identificator[0] = (byte)(_identificator[0] | (1 << 7));
                    _identificator[0] = (byte)(_identificator[0] | (1 << 6));
                    break;
            }
            //----------P/C SETTING---------------------
            switch (primitiveOrConstructed)
            {
                case 0:
                    _identificator[0] = (byte)(_identificator[0] & ~(1 << 5));
                    break;
                case 1:
                    _identificator[0] = (byte)(_identificator[0] | (1 << 5));
                    break;
            }
            //----------TAG SETTING---------------------
            // od zerowego bitu wpisz kolejne 5 pierwszych bitów wyciętych z reprezentacji binarnej pola tag
            _identificator[0] = (byte)(_identificator[0] | (bitExtracted(tag, 5, 1) << 0));
            //identyficator = Convert.ToInt32(_identificator[0]);
        }
        else //----------TAG >= 31 ---------------------
        {
            double result = (double)(getBitsAmount(tag) / (double)7);   //ile bajtów zajmie tag
            int nOctets = (int)Math.Ceiling(result) + 1;

            _identificator = new byte[nOctets];

            //----------CLASS SETTING---------------------
            switch (klasa)
            {
                case 0:
                    _identificator[0] = (byte)(_identificator[0] & ~(1 << 6));
                    _identificator[0] = (byte)(_identificator[0] & ~(1 << 7));
                    break;
                case 1:
                    _identificator[0] = (byte)(_identificator[0] | (1 << 6));
                    break;
                case 2:
                    _identificator[0] = (byte)(_identificator[0] | (1 << 7));
                    break;
                case 3:
                    _identificator[0] = (byte)(_identificator[0] | (1 << 7));
                    _identificator[0] = (byte)(_identificator[0] | (1 << 6));
                    break;
            }
            //----------P/C SETTING---------------------
            switch (primitiveOrConstructed)
            {
                case 0:
                    _identificator[0] = (byte)(_identificator[0] & ~(1 << 5));
                    break;
                case 1:
                    _identificator[0] = (byte)(_identificator[0] | (1 << 5));
                    break;
            }

            _identificator[0] = (byte)(_identificator[0] | (1 << 4));
            _identificator[0] = (byte)(_identificator[0] | (1 << 3));
            _identificator[0] = (byte)(_identificator[0] | (1 << 2));
            _identificator[0] = (byte)(_identificator[0] | (1 << 1));
            _identificator[0] = (byte)(_identificator[0] | (1 << 0));

            //----------TAG SETTING---------------------
            int len = getBitsAmount(tag);

            _identificator[1] = (byte)(_identificator[1] | (1 << 7));      //ustaw 1 w ósmym bicie pierwszego oktetu
            _identificator[1] = (byte)(_identificator[1] | (bitExtracted(tag, len - (nOctets - 2) * 7 , (nOctets - 2) * 7 + 1) << 0));

            int j = 0;
            for (int i = nOctets - 1; i > 1; i--)
            {
                _identificator[i] = (byte)(_identificator[i] | (1 << 7));
                _identificator[i] = (byte)(_identificator[i] | (bitExtracted(tag, 7, j*7 + 1) << 0));
                j++;
            }

            _identificator[nOctets - 1] = (byte)(_identificator[nOctets-1] & ~(1 << 7));      //ustaw 0 w ósmym bicie ostatniego oktetu

            //konkatenacja do identyfikatora
            //this.identyficator += (int)Math.Pow(256, nOctets - 1 - i) * (int)_identificator[i];
            for (int i = 0; i < nOctets; i++)
            {
                this.identyficator.Add(_identificator[i]);
            }
        }

        List<byte> lst = _identificator.OfType<byte>().ToList();
        return lst;
    }

    private List<byte> getLengthAndContent()
    {
        double result = (double)getBitsAmountLong(this.content)/(double)8;
        int _lenghtInBytes = (int)Math.Ceiling(result);

        int len = getBitsAmount((int)this.content);
        byte[] frame;

        if (_lenghtInBytes -1 < 128)
        {
            frame = new byte[_lenghtInBytes + 1];

            //pierwszy oktet, kodowanie długości
            frame[0] = (byte)(frame[0] & ~(1 << 7));
            frame[0] = (byte)(frame[0] | (bitExtracted(_lenghtInBytes, 7, 1) << 0));

            //kodowanie zawartości
            frame[1] = (byte)(frame[1] | (bitExtracted((int)this.content, len - (_lenghtInBytes - 1) * 8, (_lenghtInBytes - 1) * 8 + 1) << 0));

            int j = 0;
            for (int i = _lenghtInBytes; i > 1; i--)
            {
                frame[i] = (byte)(frame[i] | (bitExtracted((int)this.content, 8, j * 8 + 1) << 0));
                j++;
            }

            //konkatenacja bajtów tworzących ramkę
            //this.lengthAndContent += (int)Math.Pow(256, _lenghtInBytes - 1 - i) * (int)frame[i];
            for (int i = 0; i < _lenghtInBytes; i++)
            {
                this.identyficator.Add(frame[i]);
            }
        }
        else if(_lenghtInBytes-1 > 128 && _lenghtInBytes-1 < Math.Pow(2, 1008))     //do poprawy!
        {
            int contentLenght = (int)getBitsAmountLong(this.content);
            int lenghtOfTheLenght = getBitsAmount(contentLenght);
            int totalLenght = contentLenght + lenghtOfTheLenght;

            int contentLenghtBytes = (int)Math.Ceiling((double)contentLenght / (double)8);
            int lenghtOfTheLenghtBytes = (int)Math.Ceiling((double)lenghtOfTheLenght / (double)8);
            int totalLenghtBytes = contentLenghtBytes + lenghtOfTheLenghtBytes + 1;

            frame = new byte[totalLenghtBytes];

            //pierwszy oktet
            frame[0] = (byte)(frame[0] | (1 << 7));
            frame[0] = (byte)(frame[0] | (bitExtracted(lenghtOfTheLenght, 7, 1) << 0));

            //kodowanie długości

        }
        else
        {
            Console.WriteLine("ZA DUZY ROZMIAR DANYCH!");
            return null;
        }

        List<byte> lst = frame.OfType<byte>().ToList();
        return lst;
    }

    public void printIdentycicator()
    {
        Console.WriteLine(this.identyficator);
    }

    static int bitExtracted(int number, int k, int p)
    {
        return (((1 << k) - 1) & (number >> (p - 1)));
    }

    private int getBitsAmount(int tag)
    {
        int amount = 0;
        int tmp = tag;

        do
        {
            tmp = tmp / 2;
            amount++;
        }
        while (tmp > 0);

        return amount;
    }

    private long getBitsAmountLong(long content)
    {
        long amount = 0;
        long tmp = content;

        do
        {
            tmp = tmp / 2;
            amount++;
        }
        while (tmp > 0);

        return amount;
    }
}
