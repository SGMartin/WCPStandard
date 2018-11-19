/*

                This packer is disabled by default, but it can be enabled in server config. It will make default client launchers work.
                Check the wiki for more info.

 */


namespace Authentication.Networking.Packets 
{
    class Launcher : Core.Networking.OutPacket {
        public Launcher()
            : base((ushort)Networking.PacketList.Launcher) {
             //   Append(AuthConfig.Format);              // Format
             //    Append(AuthConfig.Launcher);            // Launcher Version
             //    Append(AuthConfig.Updater);             // Updater Version
             //    Append(AuthConfig.Client);              // Client Version
             //    Append(AuthConfig.Sub);                 // Sub Version
             //    Append(AuthConfig.Option);              // Option
             //    Append(AuthConfig.URL);                 // URL
        }
    }
}