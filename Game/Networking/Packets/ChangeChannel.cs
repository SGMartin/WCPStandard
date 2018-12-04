using System;
namespace Game.Networking.Packets
{
    class ChangeChannel : Core.Networking.OutPacket
    {
        public ChangeChannel(Enums.ChannelType channelType)
            : base((ushort)Networking.PacketList.ChannelSelection)
        {
            Append(Core.Networking.Constants.ERROR_OK);
            Append((sbyte)channelType);
        }
    }
}
