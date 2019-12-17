using System;
using System.Collections.Generic;

public class Walidator
{
    private List<Wezel> liscie = new List<Wezel>();

    private string OID;
    private long value;

    public Walidator(string _oid, long _val, List<Wezel> _liscie)
    {
        OID = _oid;
        value = _val;
        liscie = _liscie;
    }


    public bool validade()
    {
        bool isCorrect = true;

        TreeBrowser treeBrowser = new TreeBrowser(this.liscie);

        Wezel w = treeBrowser.findByOid(this.OID);

        //Console.WriteLine(w.name);

        long elementSize = System.Runtime.InteropServices.Marshal.SizeOf(value); //Size

        if (w.syntax.Sequence.Count == 0)
        {
            if (w.syntax.ancestorType == "")
            {
                //Console.WriteLine("JESTEM TUU!!!");

                if (w.syntax.MaxRange != 0 || w.syntax.MinRange != 0)
                {
                    if (this.value > w.syntax.MaxRange)
                    {
                        isCorrect = false;
                    }
                    if (this.value < w.syntax.MinRange)
                    {
                        isCorrect = false;
                    }
                }

                if (w.syntax.MaxSize != 0 || w.syntax.MinSize != 0)
                {
                    if (elementSize < w.syntax.MinSize)
                    {
                        isCorrect = false;
                    }
                    if (elementSize > w.syntax.MaxSize)
                    {
                        isCorrect = false;
                    }
                }

                if (w.syntax.size != 0 && w.syntax.size != elementSize)
                {
                    isCorrect = false;
                }
            }
            else if (w.syntax.ancestorType == "INTEGER")
            {
                //Console.WriteLine("LUDZI TŁUUUM!!!");

                if (w.syntax.size == 0 && w.syntax.MinSize == 0 && w.syntax.MinRange == 0 && w.syntax.MaxSize == 0 && w.syntax.MaxRange == 0)    //ZEROWE OGRANICZENIA
                {
                    if (this.value > sizeof(int))
                    {
                        isCorrect = false;
                    }
                    if (this.value < 0)
                    {
                        isCorrect = false;
                    }

                    if (elementSize < 0)
                    {
                        isCorrect = false;
                    }
                    if (elementSize > sizeof(int))
                    {
                        isCorrect = false;
                    }
                }
                else    //NIEZEROWE OGRANICZENIA
                {
                    if (w.syntax.MaxRange != 0 || w.syntax.MinRange != 0)
                    {
                        if (this.value > w.syntax.MaxRange)
                        {
                            isCorrect = false;
                        }
                        if (this.value < w.syntax.MinRange)
                        {
                            isCorrect = false;
                        }
                    }

                    if (w.syntax.MaxSize != 0 || w.syntax.MinSize != 0)
                    {
                        if (elementSize < w.syntax.MinSize)
                        {
                            isCorrect = false;
                        }
                        if (elementSize > w.syntax.MaxSize)
                        {
                            isCorrect = false;
                        }
                    }

                    if (w.syntax.size != 0 && w.syntax.size != elementSize)
                    {
                        isCorrect = false;
                    }
                }

            }
            else if (w.syntax.ancestorType == "OCTET STRING")
            {
                //Console.WriteLine("LUDZI TŁUUUM!!!");

                if (w.syntax.size == 0 && w.syntax.MinSize == 0 && w.syntax.MinRange == 0 && w.syntax.MaxSize == 0 && w.syntax.MaxRange == 0)    //ZEROWE OGRANICZENIA
                {
                    if (this.value > Math.Pow(2, 32) - 1)
                    {
                        isCorrect = false;
                    }
                    if (this.value < 0)
                    {
                        isCorrect = false;
                    }

                    if (elementSize < 0)
                    {
                        isCorrect = false;
                    }
                    if (elementSize > 32)
                    {
                        isCorrect = false;
                    }
                }
                else    //NIEZEROWE OGRANICZENIA
                {
                    if (w.syntax.MaxRange != 0 || w.syntax.MinRange != 0)
                    {
                        if (this.value > w.syntax.MaxRange)
                        {
                            isCorrect = false;
                        }
                        if (this.value < w.syntax.MinRange)
                        {
                            isCorrect = false;
                        }
                    }

                    if (w.syntax.MaxSize != 0 || w.syntax.MinSize != 0)
                    {
                        if (elementSize < w.syntax.MinSize * 8)
                        {
                            isCorrect = false;
                        }
                        if (elementSize > w.syntax.MaxSize * 8)
                        {
                            isCorrect = false;
                        }
                    }

                    if (w.syntax.size != 0 && w.syntax.size != elementSize)
                    {
                        isCorrect = false;
                    }
                }

            }
        }
        else // sequence.count != 0
        {
            foreach(DataType dt in w.syntax.Sequence)
            {
                long userInput;

                Console.WriteLine("PODAJ WARTOSC DO WALIDACJI DLA PODANYCH OGRANICZEN:");
                Console.WriteLine("Minimalna warosc liczby: " + dt.MinRange);
                Console.WriteLine("Maksymalna warosc liczby: " + dt.MaxRange);
                Console.WriteLine("Minimalny rozmiar liczby (w bajtach): " + dt.MinSize);
                Console.WriteLine("Maksymalny rozmiar liczby (w bajtach): " + dt.MaxSize);
                Console.WriteLine("Rozmiar liczby (w bajtach): " + dt.size);

                Console.Write("Twoje dane: ");
                userInput = Console.Read();

                long userSize = System.Runtime.InteropServices.Marshal.SizeOf(userInput);

                if ((dt.MinRange !=0 || dt.MaxRange != 0) && !(dt.MinRange <= userInput && userInput <= dt.MaxRange))
                {
                    Console.WriteLine("NIEPRAWIDLOWE DANE!!! PODAJ DANE PONOWNIE!");
                    Console.Write("Twoje dane: ");
                    userInput = Console.Read();
                }
                else if ((dt.MinSize != 0 || dt.MaxSize != 0) && !(dt.MinSize <= userSize && userSize <= dt.MaxSize))
                {
                    Console.WriteLine("NIEPRAWIDLOWE DANE!!! PODAJ DANE PONOWNIE!");
                    Console.Write("Twoje dane: ");
                    userInput = Console.Read();
                }
                else if(dt.size != 0 && userSize != dt.size)
                {
                    Console.WriteLine("NIEPRAWIDLOWE DANE!!! PODAJ DANE PONOWNIE!");
                    Console.Write("Twoje dane: ");
                    userInput = Console.Read();
                }
            }
        }

        return isCorrect;
    }
}
