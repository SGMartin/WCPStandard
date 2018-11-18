/* TODO: Is this class actually used???? */

using System;

namespace Core.Networking {
    class PacketStack {
        private byte[] buffer;

        public PacketStack() {
            buffer = new byte[0];
        }

        public void Append(OutPacket p) {
            byte[] packetBuffer = p.BuildEncrypted();
            if (buffer.Length == 0) {
                buffer = packetBuffer;
            } else {
                Array.Resize(ref buffer, buffer.Length + packetBuffer.Length);
                Array.Copy(packetBuffer, 0, buffer, buffer.Length - packetBuffer.Length, packetBuffer.Length);
            }
        }

        public byte[] Buffer { get { return this.buffer; } }
    }
}