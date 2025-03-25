using System;
using System.Collections.Generic;
using System.Text;

namespace RtmpProxyLib;

public static class Amf0Constants
{
    public const byte TYPE_NUMBER = 0x00;
    public const byte TYPE_BOOL = 0x01;
    public const byte TYPE_STRING = 0x02;
    public const byte TYPE_OBJECT = 0x03;
    public const byte TYPE_MOVIECLIP = 0x04;
    public const byte TYPE_NULL = 0x05;
    public const byte TYPE_UNDEFINED = 0x06;
    public const byte TYPE_REFERENCE = 0x07;
    public const byte TYPE_MIXEDARRAY = 0x08;
    public const byte TYPE_OBJECTTERM = 0x09;
    public const byte TYPE_ARRAY = 0x0A;
    public const byte TYPE_DATE = 0x0B;
    public const byte TYPE_LONGSTRING = 0x0C;
    public const byte TYPE_UNSUPPORTED = 0x0D;
    public const byte TYPE_RECORDSET = 0x0E;
    public const byte TYPE_XML = 0x0F;
    public const byte TYPE_TYPEDOBJECT = 0x10;
    public const byte TYPE_AMF3 = 0x11;
}

// Represents an undefined AMF0 value.
public class Amf0Undefined
{
    public override string ToString() => "AMF0_UNDEFINED";
}

// An ASObject is simply a dictionary.
public class ASObject : Dictionary<string, object>
{
}

// A mixed array is represented as a dictionary.
public class MixedArray : Dictionary<string, object>
{
}

// Wraps an AMF3 value within an AMF0 envelope.
public class Amf0Amf3
{
    public object Value { get; set; }
    public Amf0Amf3(object value)
    {
        Value = value;
    }
    public override string ToString() => Value?.ToString() ?? "null";
}

// AMF0 Date extends Amf3Date by storing a timezone.
public class Amf0Date : Amf3Date
{
    public short Timezone { get; set; }
    public Amf0Date(double date, short timezone) : base(0, date)
    {
        Timezone = timezone;
    }
}

public class Amf0Decoder
{
    // Keeps track of previously decoded objects.
    public static List<object> KnownObjects = new List<object>();

    public ByteStreamReader Stream { get; private set; }

    public Amf0Decoder(byte[] buffer)
    {
        Stream = new ByteStreamReader(buffer);
    }

    public object Decode()
    {
        byte type = Stream.Read(1)[0];
        switch (type)
        {
            case Amf0Constants.TYPE_NUMBER:
                return ReadNumber();
            case Amf0Constants.TYPE_BOOL:
                return ReadBoolean();
            case Amf0Constants.TYPE_STRING:
                return ReadString();
            case Amf0Constants.TYPE_OBJECT:
                return ReadObject();
            case Amf0Constants.TYPE_NULL:
                return ReadNull();
            case Amf0Constants.TYPE_UNDEFINED:
                return ReadUndefined();
            case Amf0Constants.TYPE_REFERENCE:
                return ReadReference();
            case Amf0Constants.TYPE_MIXEDARRAY:
                return ReadMixedArray();
            case Amf0Constants.TYPE_ARRAY:
                return ReadList();
            case Amf0Constants.TYPE_DATE:
                return ReadDate();
            case Amf0Constants.TYPE_LONGSTRING:
                return ReadLongString();
            case Amf0Constants.TYPE_UNSUPPORTED:
                return ReadNull();
            case Amf0Constants.TYPE_XML:
                return ReadXML();
            case Amf0Constants.TYPE_TYPEDOBJECT:
                return ReadTypedObject();
            case Amf0Constants.TYPE_AMF3:
                return ReadAMF3();
            default:
                throw new Exception("Unknown AMF0 type");
        }
    }

    public object ReadNumber()
    {
        double d = Stream.ReadDouble();
        return CheckForInt(d);
    }

    public bool ReadBoolean()
    {
        return Stream.ReadUChar() != 0;
    }

    /// <summary>
    /// Reads a short string. Returns the UTF8 string.
    /// </summary>
    public string ReadString()
    {
        ushort length = Stream.ReadUShort();
        byte[] bytes = Stream.Read(length);
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// Reads the string as raw bytes.
    /// </summary>
    public byte[] ReadStringRaw()
    {
        ushort length = Stream.ReadUShort();
        return Stream.Read(length);
    }

    public object ReadNull() => null;

    public object ReadUndefined() => new Amf0Undefined();

    public object ReadMixedArray()
    {
        uint length = Stream.ReadULong();
        MixedArray result = new MixedArray();
        for (uint i = 0; i < length; i++)
        {
            string key = ReadString();
            result[key] = Decode();
        }
        KnownObjects.Add(result.Values);
        return result;
    }

    public List<object> ReadList()
    {
        List<object> list = new List<object>();
        uint length = Stream.ReadULong();
        for (uint i = 0; i < length; i++)
        {
            list.Add(Decode());
        }
        return list;
    }

    public object ReadTypedObject()
    {
        string objectName = ReadString();
        object objectValue = ReadObject();
        // Here we construct a TypedObject with the name and the decoded object (which should be a dictionary)
        return new TypedObject(objectName, objectValue as Dictionary<string, object>);
    }

    public object ReadAMF3()
    {
        Stream.RemoveAlreadyRead();
        Amf3Decoder amf3Decoder = new Amf3Decoder(Stream.Data);
        object decoded = amf3Decoder.Decode();
        // Advance our stream offset by the number of bytes consumed by the AMF3 decoder.
        Stream.Offset += amf3Decoder.Stream.Offset;
        return new Amf0Amf3(decoded);
    }

    public ASObject ReadObject()
    {
        ASObject obj = new ASObject();
        // In AMF0, object keys are written as strings with a 16-bit length.
        // We read raw bytes and decode them.
        byte[] keyBytes = ReadStringRaw();
        string key = Encoding.UTF8.GetString(keyBytes);
        // Continue until we hit the object termination marker.
        while (Stream.Peek(1).Length > 0 && Stream.Peek(1)[0] != Amf0Constants.TYPE_OBJECTTERM)
        {
            obj[key] = Decode();
            keyBytes = ReadStringRaw();
            key = Encoding.UTF8.GetString(keyBytes);
        }
        // Discard the end marker (3 bytes: 0x00, 0x00, 0x09)
        Stream.Read(3);
        return obj;
    }

    public object ReadReference()
    {
        ushort idx = Stream.ReadUShort();
        if (idx < KnownObjects.Count)
            return KnownObjects[idx];
        else
            throw new IndexOutOfRangeException("Unknown AMF0 object reference");
    }

    public object ReadDate()
    {
        double date = Stream.ReadDouble();
        short timezone = Stream.ReadShort();
        return new Amf0Date(date, timezone);
    }

    public string ReadLongString()
    {
        uint length = Stream.ReadULong();
        byte[] bytes = Stream.Read((int)length);
        return Encoding.UTF8.GetString(bytes);
    }

    public object ReadXML()
    {
        throw new NotImplementedException("AMF0 unsupported XML reading");
    }

    private static object CheckForInt(double x)
    {
        int y = (int)x;
        // If the conversion to int doesn't lose any data, return an int.
        if (Math.Abs(x - y) < double.Epsilon)
            return y;
        return x;
    }
}

public class Amf0Encoder
{
    public ByteStreamReader Stream { get; private set; }

    public Amf0Encoder()
    {
        Stream = new ByteStreamReader();
    }

    public void Encode(object obj)
    {
        if (obj == null)
        {
            WriteNull();
            return;
        }

        Type t = obj.GetType();
        if (t == typeof(int) || t == typeof(double) || t == typeof(float))
        {
            WriteNumber(Convert.ToDouble(obj));
        }
        else if (t == typeof(bool))
        {
            WriteBoolean((bool)obj);
        }
        else if (t == typeof(string))
        {
            WriteString((string)obj);
        }
        else if (obj is ASObject)
        {
            WriteObject((ASObject)obj);
        }
        else if (obj is Amf0Undefined)
        {
            WriteUndefined();
        }
        else if (obj is MixedArray)
        {
            WriteMixedArray((MixedArray)obj);
        }
        else if (obj is System.Collections.IEnumerable && !(obj is string))
        {
            WriteList(obj);
        }
        else if (obj is Amf0Date)
        {
            WriteDate((Amf0Date)obj);
        }
        else if (obj is TypedObject)
        {
            WriteTypedObject((TypedObject)obj);
        }
        else if (obj is Amf0Amf3)
        {
            WriteAMF3((Amf0Amf3)obj);
        }
        else
        {
            throw new Exception($"Unimplemented AMF0 encode for type {t}");
        }
    }

    public void WriteNumber(double n)
    {
        Stream.Write(new byte[] { Amf0Constants.TYPE_NUMBER });
        Stream.WriteDouble(n);
    }

    public void WriteBoolean(bool b)
    {
        Stream.Write(new byte[] { Amf0Constants.TYPE_BOOL });
        Stream.WriteUChar(b ? 1 : 0);
    }

    public void WriteString(string s)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(s);
        int l = bytes.Length;
        if (l > 0xffff)
        {
            Stream.Write(new byte[] { Amf0Constants.TYPE_LONGSTRING });
            Stream.WriteULong((uint)l);
        }
        else
        {
            Stream.Write(new byte[] { Amf0Constants.TYPE_STRING });
            Stream.WriteUShort((ushort)l);
        }
        Stream.Write(bytes);
    }

    public void WriteStringKey(string s)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(s);
        int l = bytes.Length;
        if (l > 0xffff)
        {
            Stream.WriteULong((uint)l);
        }
        else
        {
            Stream.WriteUShort((ushort)l);
        }
        Stream.Write(bytes);
    }

    public void WriteObject(ASObject o)
    {
        Stream.Write(new byte[] { Amf0Constants.TYPE_OBJECT });
        foreach (var key in o.Keys)
        {
            WriteStringKey(key);
            Encode(o[key]);
        }
        // Write the object termination marker: 0x00, 0x00, 0x09.
        Stream.WriteUChar(0x00);
        Stream.WriteUChar(0x00);
        Stream.Write(new byte[] { Amf0Constants.TYPE_OBJECTTERM });
    }

    public void WriteNull()
    {
        Stream.Write(new byte[] { Amf0Constants.TYPE_NULL });
    }

    public void WriteUndefined()
    {
        Stream.Write(new byte[] { Amf0Constants.TYPE_UNDEFINED });
    }

    public void WriteMixedArray(MixedArray o)
    {
        Stream.Write(new byte[] { Amf0Constants.TYPE_MIXEDARRAY });
        Stream.WriteULong((uint)o.Count);
        foreach (var key in o.Keys)
        {
            WriteStringKey(key);
            Encode(o[key]);
        }
        // End marker.
        Stream.WriteUChar(0x00);
        Stream.WriteUChar(0x00);
        Stream.Write(new byte[] { Amf0Constants.TYPE_OBJECTTERM });
    }

    public void WriteList(object enumerable)
    {
        Stream.Write(new byte[] { Amf0Constants.TYPE_ARRAY });
        // Count the elements.
        int count = 0;
        List<object> list = new List<object>();
        foreach (var item in (System.Collections.IEnumerable)enumerable)
        {
            list.Add(item);
            count++;
        }
        Stream.WriteULong((uint)count);
        foreach (var data in list)
        {
            Encode(data);
        }
    }

    public void WriteDate(Amf0Date d)
    {
        Stream.Write(new byte[] { Amf0Constants.TYPE_DATE });
        Stream.WriteDouble(d.Date);
        Stream.WriteShort(d.Timezone);
    }

    public void WriteTypedObject(TypedObject o)
    {
        Stream.Write(new byte[] { Amf0Constants.TYPE_TYPEDOBJECT });
        WriteStringKey(o.Name);
        foreach (var key in o.Keys)
        {
            WriteStringKey(key);
            Encode(o[key]);
        }
        // End marker.
        Stream.WriteUChar(0x00);
        Stream.WriteUChar(0x00);
        Stream.Write(new byte[] { Amf0Constants.TYPE_OBJECTTERM });
    }

    public void WriteAMF3(Amf0Amf3 o)
    {
        Stream.Write(new byte[] { Amf0Constants.TYPE_AMF3 });
        Amf3Encoder amf3Encoder = new Amf3Encoder();
        amf3Encoder.Encode(o.Value);
        // Append the encoded AMF3 data to our stream.
        Stream.Append(amf3Encoder.Stream.Data);
    }
}
