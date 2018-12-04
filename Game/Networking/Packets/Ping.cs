using Game.Networking;

namespace Game.Packets
{
    class Ping : Core.Networking.OutPacket
    {

        public Ping(Entities.User u)
            : base((ushort)PacketList.Ping)
        {
            Append(5000); // Ping frequency
                          //    Append(u.Ping); // Ping
            Append(0);
            Append(-1);  // -1 = no evento  175 = evento de navidad
            Append(-1); // Duración del evento
            Append(4); // 3 exp weekend, 4 exp event, 0 = none
                       // Append(Game.GameConfig.ExpRate); // EXP Rate
                       // Append(Game.GameConfig.DinarRate); // Dinar Rate
            Append(1);
            Append(1);
            //  Append((u.PremiumTimeInSeconds > 0) ? u.PremiumTimeInSeconds : -1); // Premium Time
            Append(-1);
        }
    }
}
