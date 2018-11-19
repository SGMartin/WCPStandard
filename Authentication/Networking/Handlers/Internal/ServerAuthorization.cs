/*
 *                  This handler is triggered by the Game server attempting to connect to the Authentication.  
 *                  It is added to the server pool when the conditions are met.  On a successful  authentication,  a packet is sent.        
 */


namespace Authentication.Networking.Handlers.Internal
{
    class ServerAuthorization : Networking.PacketHandler
    {
        protected override void Process(Entities.Server s)
        {
            uint ErrorCode = GetuInt(0);
            if (ErrorCode == Core.Networking.Constants.ERROR_OK)
            {
                string globalKey = GetString(1);
                string serverName = GetString(2);
                string ipAddress = GetString(3);
                int port = GetInt(4);
                byte type = GetByte(5);

                Core.GameConstants.ServerTypes enumType = Core.GameConstants.ServerTypes.Normal;

                if (System.Enum.IsDefined(typeof(Core.GameConstants.ServerTypes), type))
                {
                    enumType = (Core.GameConstants.ServerTypes)type;
                }
                else
                {
                    s.Disconnect(); return;
                }

                byte serverId = Managers.ServerManager.Instance.Add(s, serverName, ipAddress, port, enumType);
                if (serverId > 0)
                {
                    s.Send(new Packets.Internal.Authorize(serverId));
                 
                }
                else
                {
                    s.Send(new Packets.Internal.Authorize(Core.Networking.ErrorCodes.ServerLimitReached));
                    s.Disconnect();
                }

            }
            else
            {
                s.Disconnect();
            }
        }
    }
}