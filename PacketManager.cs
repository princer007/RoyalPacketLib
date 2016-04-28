using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RoyalPacketLib
{
    public static class PacketManager
    {
        static Dictionary<uint, Type> registeredPackets = new Dictionary<uint, Type>();
        static Dictionary<uint, PropertyInfo[]> packetsMetadata = new Dictionary<uint, PropertyInfo[]>();
        static Type packetType = typeof(Packet);

        static public void RegisterPacket<T>(uint header) where T : Packet
        {
            Type t = typeof(T);
            //if (!t.IsSubclassOf(packetType))
                //throw new ArgumentException("T should inherit class Packet");
            registeredPackets.Add(header, t);
            packetsMetadata.Add(header, t.GetProperties().Where(x => x.CanWrite).ToArray());
        }

        static public T Parse<T>(byte[] content) where T:Packet
        {
            T packet = (T)Activator.CreateInstance(typeof(T), content);
            /*
                Convert.ChangeType(packet, registeredPackets[packet.Header]);
            */
            try
            {
                foreach (var v in packetsMetadata[packet.Header])
                    v.SetValue(packet, packet.ReadInternal(v.PropertyType), null);
            }
            catch (KeyNotFoundException)
            { throw new ArgumentException("Current packet is not registered in PacketManager: " + packet.Header); }
            packet.Flush();

            return packet;
        }
    }
}
