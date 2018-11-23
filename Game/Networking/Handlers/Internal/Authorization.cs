/*
 *              The Authentication server sends a response to the game server authentication request.
 *              The response is managed here
 * 
 */ 

using System;
using Core.Networking;

using Serilog;

namespace Game.Networking.Handlers.Internal
{
    class Authorization : Networking.PacketHandler
    {
        protected override void Process(Networking.AuthenticationClient s)
        {
            ushort errorCode = GetUShort(0);
            if (s.Authorized) return; // Ignore other packets.
            if (Enum.IsDefined(typeof(ErrorCodes), errorCode))
            {
                ErrorCodes enumErrorCode = (ErrorCodes)errorCode;
                switch (enumErrorCode)
                {

                    case ErrorCodes.Success:
                        {
                            s.OnAuthorize(GetByte(1));
                            
                            if (!s.IsFirstConnect) //TODO: UPDATE
                            {
                                // We disconnected, sync the server!
                            }

                            break;
                        }

                    case ErrorCodes.InvalidKeyOrSession:
                        {
                            Log.Error("Error while authorizing: the authorization key didn't match.");
                            s.Disconnect(true);
                            break;
                        }

                    case ErrorCodes.EntityAlreadyAuthorized:
                        {
                            Log.Error("Error while authorizing: a server with the same ip address is already online.");
                            s.Disconnect(true);
                            break;
                        }

                    case ErrorCodes.ServerLimitReached:
                        {
                            Log.Error("Error while authorizing: maximum amount of servers reached.");
                            s.Disconnect(true);
                            break;
                        }

                    case ErrorCodes.ServerNameInUse:
                        {
                            Log.Error("Error while authorizing: the server name is already in use.");
                            s.Disconnect(true);
                            break;
                        }

                    default:
                        {
                            Log.Error(string.Concat("An unknown(", errorCode.ToString("x2"), ") error occured while authorizing the server."));
                            s.Disconnect(true);
                            break;
                        }

                }
            }
            else
            {
                // Unknown error
                Log.Error(string.Concat("An unknown(", errorCode.ToString("x2"), ") error occured while authorizing the server."));
                s.Disconnect(true);
            }
        }
    }
}