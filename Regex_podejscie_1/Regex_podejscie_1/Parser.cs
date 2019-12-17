using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Parser
{
    public string path;
    private string content;
    public List<DataType> DataTypesList = new List<DataType>();

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
        List<DataType> tmpDataTypeList = new List<DataType>();

        int OID;
        string name;
        string parrent_name;
        DataType syntax = new DataType("", "", 0, "", "", 0, 0, 0, 0, 0);
        string access;
        string description;
        string status;

        //------NEW DATA TYPE--------
        DataType tmpDataType = new DataType("", "", 0, "", "", 0, 0, 0, 0, 0);

        string type;
        int size;
        int MaxSize;
        int MinSize;

        long MaxRange;
        long MinRange;

        string sequence;
        string sequenceName;
        //---------------------------

        List<string> obiekty = new List<string>();

        string Node_pattern = @"(?<name>\S+)\sOBJECT-TYPE\s*SYNTAX\s+(?<syntax>.*?)\s*ACCESS\s+(?<access>\S*)\s+STATUS\s+(?<status>\w*)\s*DESCRIPTION\s*(?<description>.*?)::=\s{\s(?<rodzic>\S*)\s(?<OID>\d+)\s}";
        //string Syntax_pattern = @"SYNTAX\s*(SEQUENCE\sOF\s*(?<sequence_name>\S*)|((?<type1>\S*)\s*(\n|(\((?<min_range>\d*..(?<max_range>\d*)\)))|(\(SIZE\s\((?<min_size>\d*)..(?<max_size>\d*)\)\))|(\(SIZE\s*\((?<size>\d*)\)\))|({\n(?<sequence>.*?)}))))";
        string Syntax_pattern = @"SYNTAX\s*(SEQUENCE\sOF\s*(?<sequence_name>\S*)|((?<type1>\S*)\s*(\n|(\((?<min_range>\d*)..(?<max_range>\d*)\))|(\(SIZE\s\((?<min_size>\d*)..(?<max_size>\d*)\)\))|(\(SIZE\s*\((?<size>\d*)\)\))|(\{.*?\}))))";

        foreach (Match match in Regex.Matches(content, Node_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
        {
            obiekty.Add(match.Value);
        }

        foreach (string ob in obiekty)
        {
            parrent_name = Regex.Match(ob, Node_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[6].Value;
            name = Regex.Match(ob, Node_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[1].Value;

            foreach (Match obj in Regex.Matches(ob, Syntax_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
            {
                //-----------NEW DATA TYPE----------------------------
                sequenceName = Regex.Match(obj.Value, Syntax_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[8].Value;
                sequence = Regex.Match(obj.Value, Syntax_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[15].Value;
                type = Regex.Match(obj.Value, Syntax_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[9].Value;
                    if(type == "")
                    {
                        type = sequenceName;
                    }
                Int64.TryParse(Regex.Match(obj.Value, Syntax_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[10].Value, out MinRange);
                Int64.TryParse(Regex.Match(obj.Value, Syntax_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[11].Value, out MaxRange);
                Int32.TryParse(Regex.Match(obj.Value, Syntax_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[14].Value, out size);
                Int32.TryParse(Regex.Match(obj.Value, Syntax_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[12].Value, out MinSize);
                Int32.TryParse(Regex.Match(obj.Value, Syntax_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[13].Value, out MaxSize);

                tmpDataType = new DataType(type, "", 0, "", "", size, MinSize, MaxSize, MinRange, MaxRange);
                tmpDataTypeList.Add(tmpDataType);

                //----------------------------------------------------
                //-----------NEW SEQUENCE-----------------------------
                //
                //              NO NEED OF NEW SEQUENTIONS
                //
                //----------------------------------------------------
                //----------------------------------------------------
                if (this.findTheSameDataType(tmpDataType) == null)
                {
                    syntax = tmpDataType;
                    DataTypesList.Add(syntax);
                }
                else
                {
                    //syntax = tmpDataType;
                    syntax = this.findTheSameDataType(tmpDataType);
                }
            }

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

        foreach (Match match in Regex.Matches(content, Identifier_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
        {
            obiekty.Add(match.Value);
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
            string path = @"C:\Users\Marianka\Desktop\Bartek\Regex_proj\Regex_podejscie_1\Regex_podejscie_1\" + s;

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
        int MaxSize;
        int MinSize;

        long MaxRange;
        long MinRange;

        string subcontent = "";
        
        try
        {
            using (StreamReader sr = new StreamReader(@"C:\Users\Marianka\Desktop\Bartek\Regex_proj\Regex_podejscie_1\Regex_podejscie_1\RFC1155-SMI.txt"))
            {
                subcontent = sr.ReadToEnd();
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("CANNOT PHARSE DATA TYPE! :");
            Console.WriteLine(e.Message);
        }

        DataTypesList.Add(new DataType("DisplayString", "", 0, "", "OCTET STRING", 0, 0, 255, 0, 0));
        DataTypesList.Add(new DataType("PhysAddress", "", 0, "", "OCTET STRING", 0, 0, 0, 0, 0));
        DataTypesList.Add(new DataType("INTEGER", "", 2, "", "", 0, 0, 0, -2147483648, 2147483647));
        DataTypesList.Add(new DataType("OCTET STRING", "", 4, "", "", 4, 0, 0, 0, 0));
        DataTypesList.Add(new DataType("NULL", "", 5, "", "", 0, 0, 0, 0, 0));

        string DataType_pattern = @"(?<type>\S*)\s*::=\s*\[(?<DT_class>\S*)\s(?<tag>\d*)]\s*(--.*?\n\s*|\s*)(?<CodeingType>\S*)\s*(?<ancestorType>.*?)\s*(--|\n|\(((?<min_range>\d*..(?<max_range>\d*))\)|SIZE\s*\(((?<size>\d*)|((?<min_size>\d*)..(?<max_size>\d*)))\)\)))";

        foreach (Match ob in Regex.Matches(subcontent, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
        {
            type = Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[6].Value;
            DT_Class = Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[7].Value;
            Int32.TryParse(Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[8].Value, out tag);
            CodeingType = Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[9].Value;
            ancestorType = Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[10].Value;
            Int64.TryParse(Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[11].Value, out MinRange);
            Int64.TryParse(Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[12].Value, out MaxRange);
            Int32.TryParse(Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[13].Value, out size);
            Int32.TryParse(Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[14].Value, out MinSize);
            Int32.TryParse(Regex.Match(ob.Value, DataType_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[15].Value, out MaxSize);

            DataTypesList.Add(new DataType(type, DT_Class, tag, CodeingType, ancestorType, size, MinSize, MaxSize, MinRange, MaxRange));
        }

        /*foreach(DataType d in DataTypesList)
        {
            Console.WriteLine("TYPE: " + d.type);
            Console.WriteLine("CLASS: " + d.DT_Class);
            Console.WriteLine("TAG: " + d.tag);
            Console.WriteLine("CODEING TYPE: " + d.CodeingType);
            Console.WriteLine("ANCESTOR TYPE: " + d.ancestorType);
            Console.WriteLine("MIN_SIZE: " + d.MinSize);
            Console.WriteLine("MAX_SIZE: " + d.MaxSize);
            Console.WriteLine("SIZE: " + d.size);
            Console.WriteLine("MIN_RANGE: " + d.MinRange);
            Console.WriteLine("MAX_RANGE: " + d.MaxRange);
            Console.WriteLine("---------------------------------------------");
        }*/

    }

    public List<Wezel> pharseMIBfile()
    {
        List<Wezel> drzewo = new List<Wezel>();

        this.fileOpen();

        //--------------------ADDING ROOT-------------------------------------
        //--------------------------------------------------------------------
        drzewo.Add(new Wezel(1, "internet", null, null, null, "dod"));
        drzewo.Add(new Wezel(6, "dod", null, null, null, "org"));
        drzewo.Add(new Wezel(3, "org", null, null, null, "iso"));
        drzewo.Add(new Wezel(1, "iso", null, null, null, null));
        //--------------------------------------------------------------------

        foreach (Wezel w in this.pharseImport())
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

    public void pharseSequences()
    {
        string sequence_pattern = @"(?<sequence_type>\S*)\s*::=\s*SEQUENCE\s*{\s*((?<members>.*?)})";
        string member_pattern = @"(?<type>\S*)\s*(?<class>\S*)(,\s*|(\s\((?<min_range>\d*)..(?<max_range>\d*)\)(,\s*|\s*))|(\s\(SIZE\((?<size>\d*)\)\)(,\s*|\s*))|(\s\(SIZE\((?<min_size>\d*)..(?<max_size>\d*)\)\))(,\s*|\s*)|(\s*}))";

        //---------------NEW SEQUENCE------------------
        DataType sequence;
        string sequenceType;
        //---------------------------------------------

        //---------------SEQUENCE MEMBER---------------
        DataType sequenceMember;

        string MemberType;
        string MemberClass;
        long MaxRange;
        long MinRange;
        int Size;
        int MaxSize;
        int MinSize;
        //---------------------------------------------


        foreach (Match ob in Regex.Matches(content, sequence_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
        {
            sequenceType = Regex.Match(ob.Value, sequence_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[2].Value;

            sequence = new DataType(sequenceType, "", 0, "", "", 0, 0, 0, 0, 0);

            foreach (Match obj in Regex.Matches(ob.Value, member_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
            {
                //--------------------------------------------------------------------------SETTING MEMBERS PARAMETERS-----------------------------------------------------------
                //---------------------------------------------------------------------------------------------------------------------------------------------------------------
                if (Regex.Match(obj.Value, member_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[9].Value != "")
                {
                    MemberType = Regex.Match(obj.Value, member_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[9].Value;
                    MemberClass = Regex.Match(obj.Value, member_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[10].Value;
                    Int64.TryParse(Regex.Match(obj.Value, member_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[12].Value, out MaxRange);
                    Int64.TryParse(Regex.Match(obj.Value, member_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[11].Value, out MinRange);
                    Int32.TryParse(Regex.Match(obj.Value, member_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[13].Value, out Size);
                    Int32.TryParse(Regex.Match(obj.Value, member_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[15].Value, out MaxSize);
                    Int32.TryParse(Regex.Match(obj.Value, member_pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups[14].Value, out MinSize);

                    sequenceMember = new DataType(MemberType, MemberClass, 0, "", "", Size, MinSize, MaxSize, MinRange, MaxRange);
                    sequence.AddDataTypeToSequence(sequenceMember);
                }
            }

            DataTypesList.Add(sequence);
        }
    }



    //COŚ NIE DZIAŁA - ZLOKALIZOWAĆ CO
    public DataType findTheSameDataType(DataType dt)
    {
        //DataType tmpDT = new DataType("", "", 0, "", "", 0, 0, 0, 0, 0);

        foreach(DataType d in DataTypesList)
        {
            //if(d.type == dt.type && d.size == dt.size && d.MinSize == dt.MinSize && d.MinRange == dt.MinRange && d.MaxSize == dt.MaxSize && d.MaxRange == dt.MaxRange)
            /*if(d.type == dt.type)
            {
                if(dt.size == 0 && dt.MinSize == 0 && dt.MinRange == 0 && dt.MaxSize == 0 && dt.MaxRange == 0)
                {
                    return d;
                }
            }*/



            if (dt.size == 0 && dt.MinSize == 0 && dt.MinRange == 0 && dt.MaxSize == 0 && dt.MaxRange == 0)
            {
                if(d.type == dt.type)
                {
                    return d;
                }
            }
            else if(d.type == dt.type && d.size == dt.size && d.MinSize == dt.MinSize && d.MinRange == dt.MinRange && d.MaxSize == dt.MaxSize && d.MaxRange == dt.MaxRange)
            {
                return d;
            }

        }

        return null;
    }
}
