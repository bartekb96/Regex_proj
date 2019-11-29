using System;
using System.Collections.Generic;
using System.Text;


public class DataType
{
    public string type;
    public string DT_Class;
    public int tag;
    public string CodeingType;
    public string ancestorType;

    public int size;
    public int MinSize;
    public int MaxSize;

    public long MinRange;
    public long MaxRange;

    public List<DataType> Sequence = new List<DataType>();

    public DataType(string Type, string dt_class, int Tag, string _codeingType, string _ancestorType, int Size, int _MinSize, int _MaxSize, long _MinRange, long _MaxRange)
    {
        type = Type;
        DT_Class = dt_class;
        tag = Tag;
        CodeingType = _codeingType;
        ancestorType = _ancestorType;
        size = Size;
        MinSize = _MinSize;
        MaxSize = _MaxSize;
        MinRange = _MinRange;
        MaxRange = _MaxRange;
    }

    public void AddDataTypeToSequence(DataType member)
    {
        Sequence.Add(member);
    }
}
