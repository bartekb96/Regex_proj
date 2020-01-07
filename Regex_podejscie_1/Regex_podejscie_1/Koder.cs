using System;
using System.Collections;
using System.Collections.Generic;

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

    public int tag;
    public long content;
    public int klasa;
    public int primitiveOrConstructed;

    private int length;
    private int identyficator;

    public Koder(int _Tag, long _Content, int _klasa, int _primitiveOrConstructed)
    {
        this.tag = _Tag;
        this.content = _Content;
        this.klasa = _klasa;
        this.primitiveOrConstructed = _primitiveOrConstructed;
        this.setIdentyficaotr();
        this.setLengthAmdContent();
    }

    private void setIdentyficaotr()
    {
        if (this.tag < 31)
        {
            byte _identificator = 0b_0000_0000;

            //----------CLASS SETTING---------------------
            switch (klasa)
            {
                case 0:
                    _identificator = (byte)(_identificator & ~(1 << 6));
                    _identificator = (byte)(_identificator & ~(1 << 7));
                    break;
                case 1:
                    _identificator = (byte)(_identificator | (1 << 6));
                    break;
                case 2:
                    _identificator = (byte)(_identificator | (1 << 7));
                    break;
                case 3:
                    _identificator = (byte)(_identificator | (1 << 7));
                    _identificator = (byte)(_identificator | (1 << 6));
                    break;
            }
            //----------P/C SETTING---------------------
            switch (primitiveOrConstructed)
            {
                case 0:
                    _identificator = (byte)(_identificator & ~(1 << 5));
                    break;
                case 1:
                    _identificator = (byte)(_identificator | (1 << 5));
                    break;
            }
            //----------TAG SETTING---------------------
            // od zerowego bitu wpisz kolejne 5 pierwszych bitów wyciętych z reprezentacji binarnej pola tag
            _identificator = (byte)(_identificator | (bitExtracted(tag, 5, 1) << 0));
            identyficator = Convert.ToInt32(_identificator);
        }
        else //----------TAG >= 31 ---------------------
        {
            double result = (double)(getBitsAmount(tag) / (double)7);   //ile bajtów zajmie tag
            int nOctets = (int)Math.Ceiling(result) + 1;

            byte[] _identificator = new byte[nOctets];

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

            _identificator[nOctets-1] = (byte)(_identificator[nOctets-1] & ~(1 << 7));      //ustaw 0 w ósmym bicie ostatniego oktetu
            _identificator[nOctets - 1] = (byte)(_identificator[nOctets - 1] | (bitExtracted(tag, 7, 1) << 0));

            for (int i = 1; i < nOctets -1; i++)  //do poprawy!!!
            {
                _identificator[i] = (byte)(_identificator[i] | (1 << 7));
                _identificator[i] = (byte)(_identificator[i] | (bitExtracted(tag, 7, len - i*7 + 1) << 0));
            }

            //konkatenacja bajtów tworzących identyfikator
            for (int i = 0; i < nOctets; i++)
            {
                this.identyficator += (int)Math.Pow(256, nOctets-1-i) * (int)_identificator[i];
            }
        }

    }

    private void setLengthAmdContent()
    {
        double _len = (double)getBitsAmountLong(this.content)/(double)8;
        long _lenghtInBytes = (long)Math.Ceiling(_len) +1;

        int len = getBitsAmount((int)this.content);

        if (_lenghtInBytes -1 < 128)
        {
            byte[] frame = new byte[_lenghtInBytes];

            //pierwszy oktet, kodowanie długości
            frame[0] = (byte)(frame[0] & ~(1 << 7));
            frame[0] = (byte)(frame[0] | (bitExtracted(len, 7, 1) << 0));

            //kodowanie zawartości
            frame[_lenghtInBytes - 1] = (byte)(frame[_lenghtInBytes - 1] | (bitExtracted((int)this.content, len - 8 * ((int)_lenghtInBytes - 2), 1) << (8 - (len - 8 * ((int)_lenghtInBytes - 2)))));

            for (int i = 1; i < _lenghtInBytes - 1; i++)
            {
                frame[i] = (byte)(frame[i] | (bitExtracted((int)this.content, 8, len - i * 8 + 1) << 0));
            }

        }
        else if(_lenghtInBytes-1 > 128 && _lenghtInBytes-1 < Math.Pow(2, 1008))
        {
            int contentLenght = (int)getBitsAmountLong(this.content);
            int lenghtOfTheLenght = getBitsAmount(contentLenght);
            int totalLenght = contentLenght + lenghtOfTheLenght;

            int contentLenghtBytes = (int)Math.Ceiling((double)contentLenght / (double)8);
            int lenghtOfTheLenghtBytes = (int)Math.Ceiling((double)lenghtOfTheLenght / (double)8);
            int totalLenghtBytes = contentLenghtBytes + lenghtOfTheLenghtBytes + 1;

            byte[] frame = new byte[totalLenghtBytes];

            //pierwszy oktet
            frame[0] = (byte)(frame[0] | (1 << 7));
            frame[0] = (byte)(frame[0] | (bitExtracted(lenghtOfTheLenght, 7, 1) << 0));

            //kodowanie długości

        }
        else
        {
            Console.WriteLine("ZA DUZY ROZMIAR DANYCH!");
        }
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
