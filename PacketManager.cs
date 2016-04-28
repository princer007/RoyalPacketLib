using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RoyalPacketLib
{
    public static class PacketManager
    {
        static Dictionary<uint, PropertyInfo[]> packetsMetadata = new Dictionary<uint, PropertyInfo[]>();
        static Type packetType = typeof(Packet);

        static public void RegisterPacket<T>(uint header) where T : Packet
        {
            Type t = typeof(T); // Does what it does
            packetsMetadata.Add(header, t.GetProperties().Where(x => x.CanWrite && x.Name != "Header").ToArray()); // Add new Header-PropertyInfo[] element, not including Header 
        }

        static public T Parse<T>(byte[] content) where T:Packet
        {
            T packet = (T)Activator.CreateInstance(typeof(T), content); //Creating new object inherited from Packet
            try
            {
                foreach (var v in packetsMetadata[packet.Header])
                    v.SetValue(packet, packet.ReadInternal(v.PropertyType), null); //Filling properties with data
            }
            catch (KeyNotFoundException)
            { throw new ArgumentException("Current packet is not registered in PacketManager: " + packet.Header); } 
            packet.Flush(); //Flushing "content" variable and setting pointer to zero

            return packet;
        }
        static public byte[] Generate<T>(T packet) where T:Packet
        {
            packet.Flush(); //Clearing packet content to replace it with something-something
            packet.Write(packet.Header); //Since there is no header - workaround
            foreach (var v in packetsMetadata[packet.Header])
                packet.WriteInternal(v.GetValue(packet, null)); //Writing properties

            return packet.Content;
        }
    }
}
