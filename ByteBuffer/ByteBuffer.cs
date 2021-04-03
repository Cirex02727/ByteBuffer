using System;
using System.Collections.Generic;
using System.Text;

public class ByteBuffer : IDisposable
{
    private readonly List<byte> Buff;
    private byte[] readBuff;
    private int readPos;
    private bool buffUpdated = false;

    public ByteBuffer()
    {
        Buff = new List<byte>();
        readPos = 0;
    }

    #region Interaction

    public int GetReadPos()
    {
        return readPos;
    }
    public void DeleteRange(int startIndex, int length)
    {
        Buff.RemoveRange(startIndex, length);

        if (readPos >= length)
            readPos -= length;
        else
            readPos = 0;
    }

    public byte[] ToArray()
    {
        return Buff.ToArray();
    }
    public byte[] ToCroppedArray()
    {
        byte[] result = new byte[Length()];
        Buff.CopyTo(readPos, result, 0, Length());
        return result;
    }

    public int Count()
    {
        return Buff.Count;
    }
    public int Length()
    {
        return Count() - readPos;
    }
    public void Clear()
    {
        Buff.Clear();
        readPos = 0;
    }

    #endregion

    #region Writing

    public void WriteByte(byte input)
    {
        Buff.Add(input);
        buffUpdated = true;
    }
    public void WriteBytes(byte[] input)
    {
        Buff.AddRange(input);
        buffUpdated = true;
    }
    public void WriteShort(short input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffUpdated = true;
    }
    public void WriteInteger(int input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffUpdated = true;
    }
    public void WriteBoolean(bool input)
    {
        WriteByte((byte)(input ? 1 : 0));
    }
    public void WriteLong(long input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffUpdated = true;
    }
    public void WriteFloat(float input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffUpdated = true;
    }
    public void WriteString(string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        WriteInteger(bytes.Length);
        Buff.AddRange(bytes);

        buffUpdated = true;
    }
    public void WriteVector2(float[] input)
    {
        WriteVector2(input[0], input[1]);
    }
    public void WriteVector2(float input1, float input2)
    {
        byte[] vectorArray = new byte[sizeof(float) * 2];

        Buffer.BlockCopy(BitConverter.GetBytes(input1), 0, vectorArray, 0 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(input2), 0, vectorArray, 1 * sizeof(float), sizeof(float));

        Buff.AddRange(vectorArray);
        buffUpdated = true;
    }
    public void WriteVector3(float[] input)
    {
        WriteVector3(input[0], input[1], input[2]);
    }
    public void WriteVector3(float input1, float input2, float input3)
    {
        byte[] vectorArray = new byte[sizeof(float) * 3];

        Buffer.BlockCopy(BitConverter.GetBytes(input1), 0, vectorArray, 0 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(input2), 0, vectorArray, 1 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(input3), 0, vectorArray, 2 * sizeof(float), sizeof(float));

        Buff.AddRange(vectorArray);
        buffUpdated = true;
    }
    public void WriteQuaternion(float[] input)
    {
        WriteQuaternion(input[0], input[1], input[2], input[3]);
    }
    public void WriteQuaternion(float input1, float input2, float input3, float input4)
    {
        byte[] vectorArray = new byte[sizeof(float) * 4];

        Buffer.BlockCopy(BitConverter.GetBytes(input1), 0, vectorArray, 0 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(input2), 0, vectorArray, 1 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(input3), 0, vectorArray, 2 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(input4), 0, vectorArray, 3 * sizeof(float), sizeof(float));

        Buff.AddRange(vectorArray);
        buffUpdated = true;
    }
    public void WriteColor(float[] input)
    {
        byte[] vectorArray = new byte[sizeof(float) * 4];

        Buffer.BlockCopy(BitConverter.GetBytes(input[0]), 0, vectorArray, 0 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(input[1]), 0, vectorArray, 1 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(input[2]), 0, vectorArray, 2 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(input[3]), 0, vectorArray, 3 * sizeof(float), sizeof(float));

        Buff.AddRange(vectorArray);
        buffUpdated = true;
    }

    #endregion

    #region Reading

    public byte ReadByte(bool Peek = true)
    {
        if (Buff.Count > readPos)
        {
            if (buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = true;
            }

            byte value = readBuff[readPos];
            if (Peek & Buff.Count > readPos)
            {
                readPos += 1;
            }
            return value;
        }
        else
        {
            throw new Exception("[BYTE] ERROR");
        }
    }
    public byte[] ReadBytes(int Length, bool Peek = true)
    {
        if (Buff.Count > readPos)
        {
            if (buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = true;
            }

            byte[] value = Buff.GetRange(readPos, Length).ToArray();
            if (Peek)
            {
                readPos += Length;
            }
            return value;
        }
        else
        {
            throw new Exception("[BYTE[]] ERROR");
        }
    }
    public short ReadShort(bool Peek = true)
    {
        if (Buff.Count > readPos)
        {
            if (buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = true;
            }

            short value = BitConverter.ToInt16(readBuff, readPos);
            if (Peek & Buff.Count > readPos)
            {
                readPos += 2;
            }
            return value;
        }
        else
        {
            throw new Exception("[SHORT] ERROR");
        }
    }
    public int ReadInteger(bool Peek = true)
    {
        if (Buff.Count > readPos)
        {
            if (buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = true;
            }

            int value = BitConverter.ToInt32(readBuff, readPos);
            if (Peek & Buff.Count > readPos)
            {
                readPos += 4;
            }
            return value;
        }
        else
        {
            throw new Exception("[INT] ERROR");
        }
    }
    public bool ReadBoolean(bool Peek = true)
    {
        return ReadByte(Peek) == 1;
    }
    public long ReadLong(bool Peek = true)
    {
        if (Buff.Count > readPos)
        {
            if (buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = true;
            }

            long value = BitConverter.ToInt64(readBuff, readPos);
            if (Peek & Buff.Count > readPos)
            {
                readPos += 8;
            }
            return value;
        }
        else
        {
            throw new Exception("[LONG] ERROR");
        }
    }
    public float ReadFloat(bool Peek = true)
    {
        if (Buff.Count > readPos)
        {
            if (buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = true;
            }

            float value = BitConverter.ToSingle(readBuff, readPos);
            if (Peek & Buff.Count > readPos)
            {
                readPos += 4;
            }
            return value;
        }
        else
        {
            throw new Exception("[FLOAT] ERROR");
        }
    }
    public string ReadString(bool Peek = true)
    {
        int length = ReadInteger(true);

        if (buffUpdated)
        {
            readBuff = Buff.ToArray();
            buffUpdated = true;
        }

        string value = Encoding.UTF8.GetString(readBuff, readPos, length);
        if (Peek & Buff.Count > readPos)
        {
            readPos += length;
        }
        return value;
    }
    public float[] ReadVector2(bool Peek = true)
    {
        if (buffUpdated)
        {
            readBuff = Buff.ToArray();
            buffUpdated = true;
        }

        byte[] value = Buff.GetRange(readPos, sizeof(float) * 2).ToArray();
        float[] Vector2 = new float[2];
        Vector2[0] = BitConverter.ToSingle(value, 0 * sizeof(float));
        Vector2[1] = BitConverter.ToSingle(value, 1 * sizeof(float));

        if (Peek)
        {
            readPos += sizeof(float) * 2;
        }
        return Vector2;
    }
    public float[] ReadVector3(bool Peek = true)
    {
        if (buffUpdated)
        {
            readBuff = Buff.ToArray();
            buffUpdated = true;
        }

        byte[] value = Buff.GetRange(readPos, sizeof(float) * 3).ToArray();
        float[] Vector3 = new float[3];
        Vector3[0] = BitConverter.ToSingle(value, 0 * sizeof(float));
        Vector3[1] = BitConverter.ToSingle(value, 1 * sizeof(float));
        Vector3[2] = BitConverter.ToSingle(value, 2 * sizeof(float));

        if (Peek)
        {
            readPos += sizeof(float) * 3;
        }
        return Vector3;
    }
    public float[] ReadQuaternion(bool Peek = true)
    {
        if (buffUpdated)
        {
            readBuff = Buff.ToArray();
            buffUpdated = true;
        }

        byte[] value = Buff.GetRange(readPos, sizeof(float) * 4).ToArray();
        float[] Quaternion = new float[4];
        Quaternion[0] = BitConverter.ToSingle(value, 0 * sizeof(float));
        Quaternion[1] = BitConverter.ToSingle(value, 1 * sizeof(float));
        Quaternion[2] = BitConverter.ToSingle(value, 2 * sizeof(float));
        Quaternion[3] = BitConverter.ToSingle(value, 3 * sizeof(float));

        if (Peek)
        {
            readPos += sizeof(float) * 4;
        }
        return Quaternion;
    }
    public float[] ReadColor(bool Peek = true)
    {
        if (buffUpdated)
        {
            readBuff = Buff.ToArray();
            buffUpdated = true;
        }

        byte[] value = Buff.GetRange(readPos, sizeof(float) * 4).ToArray();
        float[] Color = new float[4];
        Color[0] = BitConverter.ToSingle(value, 0 * sizeof(float));
        Color[1] = BitConverter.ToSingle(value, 1 * sizeof(float));
        Color[2] = BitConverter.ToSingle(value, 2 * sizeof(float));
        Color[3] = BitConverter.ToSingle(value, 3 * sizeof(float));

        if (Peek)
        {
            readPos += sizeof(float) * 4;
        }
        return Color;
    }

    #endregion

    #region Utils

    public static byte[] IntToByte(int input) => BitConverter.GetBytes(input);
    public static byte[] StringToByte(string input) => ConcatBytesArray(IntToByte(input.Length), Encoding.UTF8.GetBytes(input));

    public static byte[] ConcatBytesArray(byte[] b1, byte[] b2)
    {
        byte[] result = new byte[b1.Length + b2.Length];

        Array.Copy(b1, result, b1.Length);
        Array.Copy(b2, 0, result, b1.Length, b2.Length);

        return result;
    }

    protected virtual void Dispose(bool dispoing)
    {
        if (!dispoing)
        {
            if (dispoing)
            {
                Buff.Clear();
                readPos = 0;
            }
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
