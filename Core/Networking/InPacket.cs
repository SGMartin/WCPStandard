/*

        This class is used by both Auth and Game server to read incoming packets, no matter the origin.


 */



using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Networking {
    public class InPacket {

        private readonly long ticks;
        private readonly ushort id;
        private object attachment;

        public byte[] OriginalBuffer { get; private set; }
        public readonly string fullPacket;
        private readonly string[] _blocks = new string[0];

        public InPacket(byte[] inBuffer, object attachment) {
            this.attachment = attachment;
            this.OriginalBuffer = inBuffer;

            this.fullPacket = Encoding.UTF8.GetString(inBuffer);
            Console.WriteLine(" IN :: " + fullPacket.Remove(fullPacket.Length -1));
            string[] tempBlocks = this.fullPacket.Split(' ');

            if (!long.TryParse(tempBlocks[0], out this.ticks)) {
                throw new Exception("Invalid packet tick.");
            }

            if (!ushort.TryParse(tempBlocks[1], out this.id)) {
                throw new Exception("Invalid packet id.");
            }

            Array.Resize(ref this._blocks, tempBlocks.Length - 3);
            Array.Copy(tempBlocks, 2, _blocks, 0, tempBlocks.Length - 3);
        }

        public long Ticks { get { return this.ticks; } set { } }
        public ushort Id { get { return this.id; } set { } }
        public string[] Blocks { get { return this._blocks; } set { } }
        public object Attachment { get { return this.attachment; } set {  } }
    }
}