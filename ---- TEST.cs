using System;
using RoyalPacketLib;

namespace ConsoleApplication10
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] packet = new byte[] { 
                0x01, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x65,
                0x01,
            };
            PacketManager.RegisterPacket<UPacket>(1);
            UPacket var = PacketManager.Parse<UPacket>(packet);
            Console.WriteLine(var.Bool);
            Console.WriteLine(var.Header);
            Console.WriteLine(var.Something);
            Console.Read();
        }
    }

    class UPacket:Packet
    {
        public int Something { get; set; }
        public bool Bool { get; set; }

        public UPacket(byte[] content) : base(content)
        {

        }
    }
}
