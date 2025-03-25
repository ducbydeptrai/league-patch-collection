using System;
using System.Text;

namespace RtmpProxyLib;
public class ByteStreamReader
{
    // Endianness constants.
    public const string ENDIAN_NETWORK = "!";
    public const string ENDIAN_NATIVE = "@";
    public const string ENDIAN_LITTLE = "<";
    public const string ENDIAN_BIG = ">";

    // System endianness based on BitConverter.
    public static readonly string SYSTEM_ENDIAN = BitConverter.IsLittleEndian ? ENDIAN_LITTLE : ENDIAN_BIG;

    private string _endian = ENDIAN_NETWORK;
    /// <summary>
    /// Gets or sets the endian format. Expected values are "!", "@", "<", or ">".
    /// </summary>
    public string Endian
    {
        get => _endian;
        set => _endian = value;
    }

    /// <summary>
    /// The underlying byte array buffer.
    /// </summary>
    public byte[] Data { get; set; }

    /// <summary>
    /// The current read offset.
    /// </summary>
    public int Offset { get; set; }

    public ByteStreamReader(byte[] data = null)
    {
        Data = data ?? new byte[0];
        Offset = 0;
    }

    public int Length => Data.Length;

    public bool AtEof() => Offset >= Data.Length;

    /// <summary>
    /// Appends new data to the unread portion.
    /// </summary>
    public void Append(byte[] newData)
    {
        int unreadLength = Data.Length - Offset;
        byte[] combined = new byte[unreadLength + newData.Length];
        Array.Copy(Data, Offset, combined, 0, unreadLength);
        Array.Copy(newData, 0, combined, unreadLength, newData.Length);
        Data = combined;
        Offset = 0;
    }

    /// <summary>
    /// Alias for Append.
    /// </summary>
    public void Write(byte[] data) => Append(data);

    /// <summary>
    /// Removes data that has already been read.
    /// </summary>
    public void RemoveAlreadyRead()
    {
        int unreadLength = Data.Length - Offset;
        byte[] newData = new byte[unreadLength];
        Array.Copy(Data, Offset, newData, 0, unreadLength);
        Data = newData;
        Offset = 0;
    }

    /// <summary>
    /// Returns a copy of the next 'length' bytes without advancing the offset.
    /// </summary>
    public byte[] Peek(int length = 1)
    {
        if (Offset + length > Data.Length)
            length = Data.Length - Offset;
        byte[] result = new byte[length];
        Array.Copy(Data, Offset, result, 0, length);
        return result;
    }

    /// <summary>
    /// Reads 'length' bytes and advances the offset.
    /// </summary>
    public byte[] Read(int length)
    {
        if (Offset + length > Data.Length)
            throw new Exception("Not enough data to read");
        byte[] chunk = new byte[length];
        Array.Copy(Data, Offset, chunk, 0, length);
        Offset += length;
        return chunk;
    }

    /// <summary>
    /// Reads a single unsigned byte.
    /// </summary>
    public int ReadUChar()
    {
        return Read(1)[0];
    }

    /// <summary>
    /// Writes a single unsigned byte.
    /// </summary>
    public void WriteUChar(int c)
    {
        if (c < 0 || c > 255)
            throw new ArgumentOutOfRangeException(nameof(c), "Value must be between 0 and 255.");
        Write(new byte[] { (byte)c });
    }

    /// <summary>
    /// Reads a signed byte.
    /// </summary>
    public sbyte ReadChar()
    {
        return (sbyte)Read(1)[0];
    }

    /// <summary>
    /// Writes a signed byte.
    /// </summary>
    public void WriteChar(sbyte c)
    {
        Write(new byte[] { (byte)c });
    }

    /// <summary>
    /// Determines if the current target endianness is big endian.
    /// </summary>
    private bool IsBigEndian()
    {
        if (Endian == ENDIAN_NATIVE)
            return SYSTEM_ENDIAN == ENDIAN_BIG;
        return Endian == ENDIAN_BIG || Endian == ENDIAN_NETWORK;
    }

    /// <summary>
    /// Reads an unsigned short (2 bytes).
    /// </summary>
    public ushort ReadUShort()
    {
        byte[] bytes = Read(2);
        if (NeedsReverse(bytes.Length))
            Array.Reverse(bytes);
        return BitConverter.ToUInt16(bytes, 0);
    }

    /// <summary>
    /// Writes an unsigned short (2 bytes).
    /// </summary>
    public void WriteUShort(ushort s)
    {
        byte[] bytes = BitConverter.GetBytes(s);
        if (NeedsReverse(bytes.Length))
            Array.Reverse(bytes);
        Write(bytes);
    }

    /// <summary>
    /// Reads a signed short (2 bytes).
    /// </summary>
    public short ReadShort()
    {
        byte[] bytes = Read(2);
        if (NeedsReverse(bytes.Length))
            Array.Reverse(bytes);
        return BitConverter.ToInt16(bytes, 0);
    }

    /// <summary>
    /// Writes a signed short (2 bytes).
    /// </summary>
    public void WriteShort(short s)
    {
        byte[] bytes = BitConverter.GetBytes(s);
        if (NeedsReverse(bytes.Length))
            Array.Reverse(bytes);
        Write(bytes);
    }

    /// <summary>
    /// Reads an unsigned 32-bit integer (4 bytes).
    /// </summary>
    public uint ReadULong()
    {
        byte[] bytes = Read(4);
        if (NeedsReverse(bytes.Length))
            Array.Reverse(bytes);
        return BitConverter.ToUInt32(bytes, 0);
    }

    /// <summary>
    /// Writes an unsigned 32-bit integer (4 bytes).
    /// </summary>
    public void WriteULong(uint l)
    {
        byte[] bytes = BitConverter.GetBytes(l);
        if (NeedsReverse(bytes.Length))
            Array.Reverse(bytes);
        Write(bytes);
    }

    /// <summary>
    /// Reads a signed 32-bit integer (4 bytes).
    /// </summary>
    public int ReadLong()
    {
        byte[] bytes = Read(4);
        if (NeedsReverse(bytes.Length))
            Array.Reverse(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }

    /// <summary>
    /// Writes a signed 32-bit integer (4 bytes).
    /// </summary>
    public void WriteLong(int l)
    {
        byte[] bytes = BitConverter.GetBytes(l);
        if (NeedsReverse(bytes.Length))
            Array.Reverse(bytes);
        Write(bytes);
    }

    /// <summary>
    /// Reads a 24-bit unsigned integer.
    /// </summary>
    public int Read24BitUInt()
    {
        int n = 0;
        int[] order = IsBigEndian() ? new int[] { 16, 8, 0 } : new int[] { 0, 8, 16 };
        for (int i = 0; i < 3; i++)
        {
            n += ReadUChar() << order[i];
        }
        return n;
    }

    /// <summary>
    /// Writes a 24-bit unsigned integer.
    /// </summary>
    public void Write24BitUInt(int n)
    {
        if (n < 0 || n > 0xffffff)
            throw new ArgumentOutOfRangeException(nameof(n), "n is out of range");
        int[] order = IsBigEndian() ? new int[] { 16, 8, 0 } : new int[] { 0, 8, 16 };
        for (int i = 0; i < 3; i++)
        {
            WriteUChar((n >> order[i]) & 0xff);
        }
    }

    /// <summary>
    /// Reads a 24-bit signed integer.
    /// </summary>
    public int Read24BitInt()
    {
        int n = Read24BitUInt();
        if ((n & 0x800000) != 0)
            n -= 0x1000000;
        return n;
    }

    /// <summary>
    /// Writes a 24-bit signed integer.
    /// </summary>
    public void Write24BitInt(int n)
    {
        if (n < -8388608 || n > 8388607)
            throw new ArgumentOutOfRangeException(nameof(n), "n is out of range");
        if (n < 0)
            n += 0x1000000;
        Write24BitUInt(n);
    }

    /// <summary>
    /// Reads an 8-byte double.
    /// </summary>
    public double ReadDouble()
    {
        byte[] bytes = Read(8);
        if (NeedsReverse(bytes.Length))
            Array.Reverse(bytes);
        return BitConverter.ToDouble(bytes, 0);
    }

    /// <summary>
    /// Writes an 8-byte double.
    /// </summary>
    public void WriteDouble(double d)
    {
        byte[] bytes = BitConverter.GetBytes(d);
        if (NeedsReverse(bytes.Length))
            Array.Reverse(bytes);
        Write(bytes);
    }

    /// <summary>
    /// Reads a 4-byte float.
    /// </summary>
    public float ReadFloat()
    {
        byte[] bytes = Read(4);
        if (NeedsReverse(bytes.Length))
            Array.Reverse(bytes);
        return BitConverter.ToSingle(bytes, 0);
    }

    /// <summary>
    /// Writes a 4-byte float.
    /// </summary>
    public void WriteFloat(float f)
    {
        byte[] bytes = BitConverter.GetBytes(f);
        if (NeedsReverse(bytes.Length))
            Array.Reverse(bytes);
        Write(bytes);
    }

    /// <summary>
    /// Reads a UTF-8 encoded string of a specified length.
    /// </summary>
    public string ReadUtf8String(int length)
    {
        byte[] bytes = Read(length);
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// Writes a UTF-8 encoded string.
    /// </summary>
    public void WriteUtf8String(string s)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(s);
        Write(bytes);
    }

    /// <summary>
    /// Helper method to decide if we need to reverse byte arrays.
    /// </summary>
    private bool NeedsReverse(int byteCount)
    {
        // If target is big endian but system is little endian, or vice versa.
        bool systemIsLittle = BitConverter.IsLittleEndian;
        bool targetIsBig = IsBigEndian();
        return (systemIsLittle && targetIsBig) || (!systemIsLittle && !targetIsBig);
    }
}
