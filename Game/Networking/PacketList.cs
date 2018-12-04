/*
 * 
 *                                          All known packet IDs are listed here, converted to HEX. You can convert them back and forth using Google.
 *                                          GamePacket(hex: 0x7530/ dec: 30000) is a special packet. The bytes meaning change depending on a certain byte called
 *                                          "subpacket" byte. It is handled directly by GameDataHandler.cs
 * 
 */

namespace Game.Networking
{
    public enum PacketList : ushort
    {
        LeaveServer = 0x6000,
        CMDFindPlayer = 0x6011,
        ServerTime = 0x6100,
        Authorization = 0x6200,
        Ping = 0x6400,
        ChannelSelection = 0x7001,
        UserList = 0x7100,
        RoomList = 0x7200,
        RoomListUpdate = 0x7210,
        RoomCreation = 0x7300,
        RoomJoin = 0x7310,
        RoomQuickJoin = 0x7320,
        RoomSpectate = 0x7330,
        RoomLeave = 0x7340,
        RoomKick = 0x7341,
        RoomInvite = 0x7350,
        Chat = 0x7400,
        PlayerInfo = 0x7500,
        SpectatorInfo = 0x7501,
        MapData = 0x7510,
        LeaveVehicle = 0x7511,
        Equipment = 0x7512,
        Explosives = 0x7520,
        GamePacket = 0x7530,
        GameCountDown = 0x7531,
        GameTick = 0x7540,
        Scoreboard = 0x7550,
        EndGame = 0x7560,
        Itemshop = 0x7600,
        Markt = 0x7700,
        UpdateInventory = 0x7900,
        Coupon = 0x7910,
        LevelUp = 0x7920, // Old: 0x8200

    }
}

