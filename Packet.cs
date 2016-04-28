using System;
using System.Text;

namespace RoyalPacketLib
{
    public class Packet
    {
        private byte[] content;
        private int pointer;

        public UInt32 Header { get; set; } // "Recognizer" (c)

        public Packet(byte[] content)
        {
            this.content = content;
            Header = ReadUInt32();
        }

        public void Flush()
        {
            content = new byte[0];
            pointer = 0;
        }
        public void Reset()
        {
            pointer = 0;
        }
        public byte[] Content
        {
            get { return content; }
        }

        //Read

        public object ReadInternal(Type t)
        {
            switch(Type.GetTypeCode(t))
            {
                case TypeCode.Boolean:
                    return ReadBoolean();
                case TypeCode.Byte:
                    return ReadByte();
                case TypeCode.Char:
                    return ReadChar();
                case TypeCode.Decimal:
                    return ReadFloat();
                case TypeCode.Double:
                    return ReadDouble();
                case TypeCode.Int16:
                    return ReadInt16();
                case TypeCode.Int32:
                    return ReadInt32();
                case TypeCode.Int64:
                    return ReadInt64();
                case TypeCode.UInt16:
                    return ReadUInt16();
                case TypeCode.UInt32:
                    return ReadUInt32();
                case TypeCode.UInt64:
                    return ReadUInt64();
                case TypeCode.String:
                    return ReadString();
                default:
                    throw new ArgumentException("Packet contains unapproptiate field type: " + t);
            }
        }

        public byte ReadByte()
        {
            return content[pointer++];
        }
        public int ReadInt16()
        {
            pointer += 2;
            return BitConverter.ToInt16(content, pointer - 2);
        }
        public int ReadInt32()
        {
            pointer += 4;
            return BitConverter.ToInt32(content, pointer - 4);
        }
        public long ReadInt64()
        {
            pointer += 8;
            return BitConverter.ToInt64(content, pointer - 8);
        }
        public UInt64 ReadUInt64()
        {
            pointer += 8;
            return BitConverter.ToUInt64(content, pointer -8);
        }
        public UInt32 ReadUInt32()
        {
            pointer += 4;
            return BitConverter.ToUInt32(content, pointer - 4);
        }
        public UInt16 ReadUInt16()
        {
            pointer += 2;
            return BitConverter.ToUInt16(content, pointer - 2);
        }
        public double ReadDouble()
        {
            pointer += 8;
            return BitConverter.ToDouble(content, pointer - 8);
        }
        public float ReadFloat()
        {
            pointer += 4;
            return BitConverter.ToSingle(content, pointer - 4);
        }
        public string ReadString()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                byte b;
                //if (Remaining > 0)
                b = ReadByte();
                //else
                //   b = 0;

                if (b == 0) break;
                sb.Append((char)b);
            }
            return sb.ToString();
        }
        public bool ReadBoolean()
        {
            return BitConverter.ToBoolean(content, pointer++);
        }
        public char ReadChar()
        {
            return BitConverter.ToChar(content, pointer++);
        }
        public byte[] ReadBytes(int count)
        {
            byte[] temp = new byte[count];
            for (int i = 0; i < count; i++)
                temp[i] = content[pointer + i];
            pointer += count;
            return temp;
        }

        //Write

        public void WriteInternal(object content)
        {
            switch (Type.GetTypeCode(content.GetType()))
            {
                case TypeCode.Boolean:
                    Write((bool)content);
                    break;
                case TypeCode.Byte:
                    Write((byte)content);
                    break;
                case TypeCode.Char:
                    Write((char)content);
                    break;
                case TypeCode.Decimal:
                    Write((float)content);
                    break;
                case TypeCode.Double:
                    Write((double)content);
                    break;
                case TypeCode.Int16:
                    Write((Int16)content);
                    break;
                case TypeCode.Int32:
                    Write((Int32)content);
                    break;
                case TypeCode.Int64:
                    Write((Int64)content);
                    break;
                case TypeCode.UInt16:
                    Write((UInt16)content);
                    break;
                case TypeCode.UInt32:
                    Write((UInt32)content);
                    break;
                case TypeCode.UInt64:
                    Write((UInt64)content);
                    break;
                case TypeCode.String:
                    Write((string)content);
                    break;
                default:
                    throw new ArgumentException("Packet contains unapproptiate field type: " + content.GetType());
            }
        }

        public void Write(string content)
        {
            byte[] temp = new byte[content.Length];
            int i = 0;
            foreach (char c in content)
                temp[i++] = (byte)c;
            ContentUpdater(temp);
        }
        public void Write(char content)
        {
            ContentUpdater(new byte[] { (byte)content });
        }
        public void Write(bool content)
        {
            ContentUpdater(BitConverter.GetBytes(content));
        }
        public void Write(double content)
        {
            ContentUpdater(BitConverter.GetBytes(content));
        }
        public void Write(float content)
        {
            ContentUpdater(BitConverter.GetBytes(content));
        }
        public void Write(Int16 content)
        {
            ContentUpdater(BitConverter.GetBytes(content));
        }
        public void Write(Int32 content)
        {
            ContentUpdater(BitConverter.GetBytes(content));
        }
        public void Write(Int64 content)
        {
            ContentUpdater(BitConverter.GetBytes(content));
        }
        public void Write(UInt16 content)
        {
            ContentUpdater(BitConverter.GetBytes(content));
        }
        public void Write(UInt32 content)
        {
            ContentUpdater(BitConverter.GetBytes(content));
        }
        public void Write(UInt64 content)
        {
            ContentUpdater(BitConverter.GetBytes(content));
        }
        public void Write(byte[] content)
        {
            ContentUpdater(content);
        }
        public void Write(byte content)
        {
            ContentUpdater(new byte[1] { content });
        }
        void ContentUpdater(byte[] content)
        {
            byte[] temp = new byte[this.content.Length + content.Length];
            this.content.CopyTo(temp, 0);
            content.CopyTo(temp, this.content.Length);
            this.content = temp;
        }

    }
}
