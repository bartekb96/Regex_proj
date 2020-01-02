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
            double result = (double)(getBitsAmount(tag) / (double)7);
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

            _identificator[nOctets-1] = (byte)(_identificator[nOctets-1] | (0 << 7));
            _identificator[nOctets - 1] = (byte)(_identificator[nOctets - 1] | (bitExtracted(tag, len - 7*(nOctets - 2), 1) << (7-(len - 7 * (nOctets - 2)))));

            for (int i = 1; i < nOctets -1; i++)
            {
                _identificator[i] = (byte)(_identificator[i] | (1 << 7));
                _identificator[i] = (byte)(_identificator[i] | (bitExtracted(tag, 7, len - i*7 + 1) << 0));
            }

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
}
