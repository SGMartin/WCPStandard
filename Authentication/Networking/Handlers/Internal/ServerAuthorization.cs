/*
 *                  This handler is triggered by the Game server attempting to connect to the Authentication.  
 *                  It is added to the server pool when the conditions are met.  On a successful  authentication,  a packet is sent.        
 */

using Serilog;

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

                if (globalKey == Config.GAMESERVERKEY) //attempt to match server keys
                {
                    Core.GameConstants.ServerTypes serverType;

                    //check for an existing server type. TODO: research the list for usable stuff and update wiki
                    if (System.Enum.IsDefined(typeof(Core.GameConstants.ServerTypes), type))
                    {
                        serverType = (Core.GameConstants.ServerTypes)type;
                   
                        //check if name is already in use, reject server if affirmative
                           foreach(Entities.Server authorizedServer in Managers.ServerManager.Instance.GetAllAuthorized())
                            {
                                if(authorizedServer.ServerName == serverName)
                                {
                                    s.Send(new Packets.Internal.Authorize(Core.Networking.ErrorCodes.EntityAlreadyAuthorized));
                                    Log.Information("Rejected server " + serverName + " . Name already in use");
                                    s.Disconnect();
                                    return;
                                }
                            }

                        byte serverId = Managers.ServerManager.Instance.Add(s, serverName, ipAddress, port, serverType);

                        if(serverId > 0)
                        {
                            s.Send(new Packets.Internal.Authorize(serverId));
                            Log.Information("New server registered as: " + serverName);
                        }    
                        else
                        {
                            s.Send(new Packets.Internal.Authorize(Core.Networking.ErrorCodes.ServerLimitReached));
                             s.Disconnect();
                        }
                    }
                    else
                    {
                        s.Send(new Packets.Internal.Authorize(Core.Networking.ErrorCodes.InvalidServerType));
                        s.Disconnect();
                    }

                }
                else
                {
                    s.Send(new Packets.Internal.Authorize(Core.Networking.ErrorCodes.InvalidKeyOrSession));
                    Log.Information("Rejecting server " + serverName + ": invalid key");
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
