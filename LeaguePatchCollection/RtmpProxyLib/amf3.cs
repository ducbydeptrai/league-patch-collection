using System;
using System.Collections.Generic;
using System.Text;

namespace RtmpProxyLib;

public static class Amf3Constants
{
    public const byte TYPE_UNDEFINED = 0x00;
    public const byte TYPE_NULL = 0x01;
    public const byte TYPE_BOOL_FALSE = 0x02;
    public const byte TYPE_BOOL_TRUE = 0x03;
    public const byte TYPE_INTEGER = 0x04;
    public const byte TYPE_NUMBER = 0x05;
    public const byte TYPE_STRING = 0x06;
    public const byte TYPE_XML = 0x07;
    public const byte TYPE_DATE = 0x08;
    public const byte TYPE_ARRAY = 0x09;
    public const byte TYPE_OBJECT = 0x0A;
    public const byte TYPE_XMLSTRING = 0x0B;
    public const byte TYPE_BYTEARRAY = 0x0C;
    public const byte TYPE_DICTIONARY = 0x11;

    public const int MAX_29B_INT = 0x0FFFFFFF;
    public const int MIN_29B_INT = -0x10000000;
}

// Represents an undefined AMF3 value.
public class Amf3Undefined
{
    public override string ToString() => "AMF3_UNDEFINED";
}

// Wraps a date value.
public class Amf3Date
{
    public int Ref { get; set; }
    public double Date { get; set; }

    public Amf3Date(int reference, double date)
    {
        Ref = reference;
        Date = date;
    }

    public string ToJson()
    {
        // Convert milliseconds timestamp to UTC DateTime.
        DateTime dt = DateTimeOffset.FromUnixTimeMilliseconds((long)Date).UtcDateTime;
        return dt.ToString("yyyy-MM-ddTHH:mm:ss.ffffff");
    }

    public override string ToString() => ToJson();
}

// A dictionary with an optional type name.
public class TypedObject : Dictionary<string, object>
{
    public string Name { get; set; }

    public TypedObject(string name = "", Dictionary<string, object> data = null) : base()
    {
        Name = name;
        if (data != null)
        {
            foreach (var kv in data)
            {
                this[kv.Key] = kv.Value;
            }
        }
    }

    public object ToJson()
    {
        var attrs = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(Name))
            attrs["__class"] = Name;
        foreach (var kv in this)
        {
            attrs[kv.Key] = kv.Value;
        }
        return attrs;
    }
}

// Wraps a byte array.
public class ByteArray
{
    public byte[] Data { get; private set; }

    public ByteArray(byte[] data)
    {
        Data = data;
    }
}

// Holds class metadata for objects.
public class ClassDefinition
{
    public bool Externalizable { get; set; }
    public int Encoding { get; set; }
    public List<string> StaticProperties { get; set; }
    public string Name { get; set; }

    public ClassDefinition(bool externalizable, int encoding, List<string> properties, string name)
    {
        Externalizable = externalizable;
        Encoding = encoding;
        StaticProperties = properties;
        Name = name;
    }
}

// Utility functions for encoding/decoding AMF3 integers.
public static class Amf3Utils
{
    private static Dictionary<int, byte[]> encodedIntCache = new Dictionary<int, byte[]>();

    public static byte[] EncodeInt(int n)
    {
        if (n < Amf3Constants.MIN_29B_INT || n > Amf3Constants.MAX_29B_INT)
            throw new OverflowException("Out of range");

        if (encodedIntCache.TryGetValue(n, out byte[] cached))
            return cached;

        int original = n;
        if (n < 0)
            n += 0x20000000;

        List<byte> data = new List<byte>();
        int realValue = -1;
        if (n > 0x1FFFFF)
        {
            realValue = n;
            n >>= 1;
            data.Add((byte)(0x80 | ((n >> 21) & 0xFF)));
        }
        if (n > 0x3FFF)
        {
            data.Add((byte)(0x80 | ((n >> 14) & 0xFF)));
        }
        if (n > 0x7F)
        {
            data.Add((byte)(0x80 | ((n >> 7) & 0xFF)));
        }
        if (realValue != -1)
            n = realValue;

        if (n > 0x1FFFFF)
            data.Add((byte)(n & 0xFF));
        else
            data.Add((byte)(n & 0x7F));

        byte[] result = data.ToArray();
        encodedIntCache[original] = result;
        return result;
    }

    public static int DecodeInt(ByteStreamReader stream, bool signed = false)
    {
        int n = 0;
        int result = 0;
        byte b = (byte)stream.ReadUChar();
        while ((b & 0x80) != 0 && n < 3)
        {
            result = (result << 7) | (b & 0x7F);
            b = (byte)stream.ReadUChar();
            n++;
        }
        if (n < 3)
        {
            result = (result << 7) | b;
        }
        else
        {
            result = (result << 8) | b;
            if ((result & 0x10000000) != 0)
            {
                if (signed)
                    result -= 0x20000000;
                else
                {
                    result <<= 1;
                    result += 1;
                }
            }
        }
        return result;
    }
}

// Decoder for AMF3 format.
public class Amf3Decoder
{
    // Static lists to track references.
    public static List<object> KnownObjects = new List<object>();
    public static List<string> KnownStrings = new List<string>();
    public static List<ClassDefinition> KnownClasses = new List<ClassDefinition>();

    public ByteStreamReader Stream { get; private set; }

    public Amf3Decoder(byte[] buffer)
    {
        Stream = new ByteStreamReader(buffer);
    }

    public object Decode()
    {
        byte type = Stream.Read(1)[0];
        switch (type)
        {
            case Amf3Constants.TYPE_UNDEFINED:
                return ReadUndefined();
            case Amf3Constants.TYPE_NULL:
                return ReadNull();
            case Amf3Constants.TYPE_BOOL_FALSE:
                return ReadFalse();
            case Amf3Constants.TYPE_BOOL_TRUE:
                return ReadTrue();
            case Amf3Constants.TYPE_INTEGER:
                return ReadInteger();
            case Amf3Constants.TYPE_NUMBER:
                return ReadNumber();
            case Amf3Constants.TYPE_STRING:
                return ReadString();
            case Amf3Constants.TYPE_XML:
                return ReadXML();
            case Amf3Constants.TYPE_DATE:
                return ReadDate();
            case Amf3Constants.TYPE_ARRAY:
                return ReadArray();
            case Amf3Constants.TYPE_OBJECT:
                return ReadObject();
            case Amf3Constants.TYPE_XMLSTRING:
                return ReadXML();
            case Amf3Constants.TYPE_BYTEARRAY:
                return ReadByteArray();
            case Amf3Constants.TYPE_DICTIONARY:
                return ReadDictionary();
            default:
                throw new Exception("Unknown AMF3 type");
        }
    }

    public object ReadUndefined() => new Amf3Undefined();
    public object ReadNull() => null;
    public bool ReadFalse() => false;
    public bool ReadTrue() => true;
    public int ReadInteger(bool signed = true) => Amf3Utils.DecodeInt(Stream, signed);
    public double ReadNumber() => Stream.ReadDouble();

    public string ReadString()
    {
        int type = Amf3Utils.DecodeInt(Stream, false);
        if ((type & 0x01) != 0)
        {
            int length = type >> 1;
            if (length == 0)
                return "";
            byte[] strBytes = Stream.Read(length);
            string s = Encoding.UTF8.GetString(strBytes);
            KnownStrings.Add(s);
            return s;
        }
        else
        {
            return KnownStrings[type >> 1];
        }
    }

    public object ReadXML()
    {
        throw new NotImplementedException("AMF3 unsupported XML reading");
    }

    public Amf3Date ReadDate()
    {
        int reference = Amf3Utils.DecodeInt(Stream, false);
        double date = Stream.ReadDouble();
        return new Amf3Date(reference, date);
    }

    public object ReadArray()
    {
        int type = Amf3Utils.DecodeInt(Stream, false);
        if ((type & 0x01) == 0)
        {
            return KnownObjects[type >> 1];
        }

        int size = type >> 1;
        string key = ReadString();
        if (string.IsNullOrEmpty(key))
        {
            List<object> result = new List<object>();
            for (int i = 0; i < size; i++)
            {
                result.Add(Decode());
            }
            return result;
        }
        else
        {
            throw new NotImplementedException("AMF3 mixed arrays are not supported");
        }
    }

    public object ReadObject()
    {
        int type = ReadInteger(false);
        if ((type & 0x01) == 0)
        {
            return KnownObjects[type >> 1];
        }

        bool shouldDefine = ((type >> 1) & 0x01) != 0;
        ClassDefinition classDefinition;
        if (shouldDefine)
        {
            bool externalizable = (((type >> 2) & 0x01) != 0);
            int encoding = (type >> 2) & 0x03;
            List<string> properties = new List<string>();
            string name = ReadString();
            int count = type >> 4;
            for (int i = 0; i < count; i++)
            {
                properties.Add(ReadString());
            }
            classDefinition = new ClassDefinition(externalizable, encoding, properties, name);
            KnownClasses.Add(classDefinition);
        }
        else
        {
            classDefinition = KnownClasses[type];
        }

        TypedObject typedObject = new TypedObject(classDefinition.Name);
        foreach (string prop in classDefinition.StaticProperties)
        {
            typedObject[prop] = Decode();
        }
        if (classDefinition.Encoding == 0x02)
        {
            while (true)
            {
                string key = ReadString();
                if (string.IsNullOrEmpty(key))
                    break;
                typedObject[key] = Decode();
            }
        }
        KnownObjects.Add(typedObject);
        return typedObject;
    }

    public object ReadByteArray()
    {
        int type = ReadInteger(false);
        if ((type & 0x01) == 0)
        {
            return KnownObjects[type >> 1];
        }
        byte[] buffer = Stream.Read(type >> 1);
        ByteArray arr = new ByteArray(buffer);
        KnownObjects.Add(arr);
        return arr;
    }

    public object ReadDictionary()
    {
        throw new NotImplementedException("AMF3 dictionary is not supported");
    }
}

// Encoder for AMF3 format.
public class Amf3Encoder
{
    public ByteStreamReader Stream { get; private set; }

    public Amf3Encoder()
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
        if (t == typeof(Amf3Undefined))
            WriteUndefined();
        else if (t == typeof(bool))
            WriteBoolean((bool)obj);
        else if (t == typeof(int))
            WriteInteger((int)obj);
        else if (t == typeof(float) || t == typeof(double))
            WriteNumber(Convert.ToDouble(obj));
        else if (t == typeof(string))
            WriteString((string)obj);
        else if (t == typeof(Amf3Date))
            WriteDate((Amf3Date)obj);
        else if (obj is System.Collections.IEnumerable && !(obj is string))
            WriteList(obj);
        else if (obj is TypedObject)
            WriteObject((TypedObject)obj);
        else
            throw new Exception($"Unimplemented AMF3 encode for type {t}");
    }

    public void WriteNull()
    {
        Stream.Write(new byte[] { Amf3Constants.TYPE_NULL });
    }

    public void WriteUndefined()
    {
        Stream.Write(new byte[] { Amf3Constants.TYPE_UNDEFINED });
    }

    public void WriteBoolean(bool b)
    {
        Stream.Write(new byte[] { b ? Amf3Constants.TYPE_BOOL_TRUE : Amf3Constants.TYPE_BOOL_FALSE });
    }

    public void WriteInteger(int n)
    {
        if (n < Amf3Constants.MIN_29B_INT || n > Amf3Constants.MAX_29B_INT)
        {
            WriteNumber((double)n);
            return;
        }
        Stream.Write(new byte[] { Amf3Constants.TYPE_INTEGER });
        Stream.Write(Amf3Utils.EncodeInt(n));
    }

    public void WriteNumber(double n)
    {
        Stream.Write(new byte[] { Amf3Constants.TYPE_NUMBER });
        Stream.WriteDouble(n);
    }

    public void WriteString(string s)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(s);
        Stream.Write(new byte[] { Amf3Constants.TYPE_STRING });
        int header = (bytes.Length << 1) | 0x01;
        Stream.Write(Amf3Utils.EncodeInt(header));
        Stream.Write(bytes);
    }

    public void WriteDate(Amf3Date d)
    {
        Stream.Write(new byte[] { Amf3Constants.TYPE_DATE });
        Stream.Write(Amf3Utils.EncodeInt(d.Ref)); // Adjust as needed
        Stream.WriteDouble(d.Date);
    }

    public void WriteList(object enumerable)
    {
        Stream.Write(new byte[] { Amf3Constants.TYPE_ARRAY });
        int count = 0;
        var list = new List<object>();
        foreach (var item in (System.Collections.IEnumerable)enumerable)
        {
            list.Add(item);
            count++;
        }
        int header = (count << 1) | 0x01;
        Stream.Write(Amf3Utils.EncodeInt(header));
        // Write empty string (as a single byte 0x01) to denote no associative portion.
        Stream.Write(new byte[] { 0x01 });
        foreach (var x in list)
        {
            Encode(x);
        }
    }

    public void WriteObject(TypedObject o)
    {
        Stream.Write(new byte[] { Amf3Constants.TYPE_OBJECT });
        if (string.IsNullOrEmpty(o.Name))
        {
            // Dynamic object without a class name.
            Stream.Write(new byte[] { 0x0b });
            Stream.Write(new byte[] { 0x01 });
            foreach (var key in o.Keys)
            {
                string s = key;
                byte[] keyBytes = Encoding.UTF8.GetBytes(s);
                int header = (keyBytes.Length << 1) | 0x01;
                Stream.Write(Amf3Utils.EncodeInt(header));
                Stream.Write(keyBytes);
                Encode(o[key]);
            }
            Stream.Write(new byte[] { 0x01 });
        }
        else
        {
            int headerValue = (o.Count << 4) | 3;
            Stream.Write(Amf3Utils.EncodeInt(headerValue));
            byte[] nameBytes = Encoding.UTF8.GetBytes(o.Name);
            int nameHeader = (nameBytes.Length << 1) | 0x01;
            Stream.Write(Amf3Utils.EncodeInt(nameHeader));
            Stream.Write(nameBytes);
            List<string> keys = new List<string>();
            foreach (var key in o.Keys)
            {
                string s = key;
                byte[] keyBytes = Encoding.UTF8.GetBytes(s);
                int keyHeader = (keyBytes.Length << 1) | 0x01;
                Stream.Write(Amf3Utils.EncodeInt(keyHeader));
                Stream.Write(keyBytes);
                keys.Add(s);
            }
            foreach (var key in keys)
            {
                Encode(o[key]);
            }
        }
    }
}
