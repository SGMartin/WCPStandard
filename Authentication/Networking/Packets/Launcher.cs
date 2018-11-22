/*

                This packet is disabled by default, but it can be enabled in server config. It will make legacy client launchers work.
                Check the wiki for more info.

 */


namespace Authentication.Networking.Packets 
{
    class Launcher : Core.Networking.OutPacket {
        public Launcher()
            : base((ushort)Networking.PacketList.Launcher)
        {     
                Append(Config.FORMAT);              // Format
                Append(Config.LAUNCHER);            // Launcher Version
                Append(Config.UPDATER);             // Updater Version
                Append(Config.CLIENT);              // Client Version
                Append(Config.SUB);                 // Sub Version
                Append(Config.OPTION);              // Option
                Append(Config.URL);                 // URL
        }
    }
}