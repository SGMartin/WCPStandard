/*

        This class is used by both Auth and Game server to send packets. There are a lot of implementations of the Append() method depending
        on data types, but all of them perform the same basic task: appending data to packets. When all data is added, the packet is built. 
        The Build and BuildEncrypted are similar, but the later uses a XORKey to crypt the packets.

        For more information about WarRock packets, check the wiki.

 */


using System;
using System.Text;
using Core.Networking;

namespace Core.Networking {
    public abstract class OutPacket {
 
        private StringBuilder builder;
        private byte xOrKey = Constants.xOrKeyClientSend; 
        public OutPacket(ushort packetId) : this(packetId, Constants.xOrKeyClientSend)
        {
                //Default constructor. Uses default xorkey
        }

        //This constructor can be called when a different xOr key is desired
        public OutPacket(ushort packetId, byte newXOrKey)
        {
            xOrKey = newXOrKey;
            builder = new StringBuilder();
            Append(Environment.TickCount);
            Append(packetId);
        }


        public void Fill(byte count, bool data) {
            for (byte i = 0; i < count; i++) {
                Append(data);
            }
        }

        public void Fill(byte count, double data) {
            for (byte i = 0; i < count; i++) {
                Append(data);
            }
        }

        public void Fill(byte count, int data) {
            for (byte i = 0; i < count; i++) {
                Append(data);
            }
        }

        public void Fill(byte count, uint data) {
            for (byte i = 0; i < count; i++) {
                Append(data);
            }
        }

        public void Fill(byte count, long data) {
            for (byte i = 0; i < count; i++) {
                Append(data);
            }
        }

        public void Fill(byte count, ulong data) {
            for (byte i = 0; i < count; i++) {
                Append(data);
            }
        }

        public void Fill(byte count, byte data) {
            for (byte i = 0; i < count; i++) {
                Append(data);
            }
        }

        public void Fill(byte count, sbyte data) {
            for (byte i = 0; i < count; i++) {
                Append(data);
            }
        }

        public void Fill(byte count, string data) {
            for (byte i = 0; i < count; i++) {
                Append(data);
            }
        }

        public void Append(bool data) {
            builder.Append(data ? 1 : 0);
            builder.Append(" ");
        }

        public void Append(double data) {
            builder.Append(data);
            builder.Append(" ");
        }

        public void Append(int data) {
            builder.Append(data);
            builder.Append(" ");
        }

        public void Append(uint data) {
            builder.Append(data);
            builder.Append(" ");
        }

        public void Append(long data) {
            builder.Append(data);
            builder.Append(" ");
        }

        public void Append(ulong data) {
            builder.Append(data);
            builder.Append(" ");
        }

        public void Append(byte data) {
            builder.Append(data);
            builder.Append(" ");
        }

        public void Append(sbyte data) {
            builder.Append(data);
            builder.Append(" ");
        }

        public void Append(string data) {
            builder.Append(data.Replace(' ', (char)(0x1D)));
            builder.Append(" ");
        }

        public string Build() {
            string strOutput = builder.ToString();
            Console.WriteLine("OUT :: " + strOutput);
            return string.Concat(strOutput, (char)(0x0A));
        }

        public byte[] BuildEncrypted() {
            byte[] buffer = Encoding.UTF8.GetBytes(this.Build());

            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = (byte)(buffer[i] ^ xOrKey);

            return buffer;
        }
    }
}