/*
 * 
 *                        Handler for the internal PlayerAuthorization packet sent by the Authentication server after GameServer request.
 * 
 * 
 */


using System;
using Core.Networking;

using Serilog;

namespace Game.Networking.Handlers.Internal
{
    class PlayerAuthorization : Networking.PacketHandler
    {
        protected override void Process(Networking.AuthenticationClient s)
        {
            ushort errorCode = GetUShort(0);

            if (Enum.IsDefined(typeof(ErrorCodes), errorCode))
            {
                ErrorCodes enumErrorCode = (ErrorCodes)errorCode;
                uint targetId = GetuInt(1);

                switch (enumErrorCode)
                {

                    // A new player logs in.
                    case ErrorCodes.Success:
                        {
                            Entities.User u = Managers.UserManager.Instance.Get(targetId);
                            if (u != null)
                            {
                                Log.Information("User " + u.DisplayName + " authorized.");
                                uint userId = GetuInt(2);
                                string userName = GetString(3);
                                string displayName = GetString(4);
                                byte  accessLevel = GetByte(5);
                                u.OnAuthorize(userId, displayName, accessLevel);
                            }
                            break;
                        }

                    // Update the information of a player.
                    case ErrorCodes.Update:
                        {
                            break;
                        }

                    // A player logs out of the server. //TODO: code this
                    case ErrorCodes.EndConnection:
                        {
                            break;
                        }

                    case ErrorCodes.InvalidKeyOrSession:
                        {
                            Entities.User u = Managers.UserManager.Instance.Get(targetId);
                            if (u != null)
                            {
                                if (!u.Authorized)
                                    u.Send(new Packets.Authorization(Packets.Authorization.ErrorCodes.NormalProcedure));
                                u.Disconnect();
                            }
                            break;
                        }

                    case ErrorCodes.InvalidSessionMatch:
                        {
                            Entities.User u = Managers.UserManager.Instance.Get(targetId);
                            if (u != null)
                            {
                                u.Send(new Packets.Authorization(Packets.Authorization.ErrorCodes.NormalProcedure));
                                u.Disconnect();
                            }
                            break;
                        }

                    case ErrorCodes.EntityAlreadyAuthorized:
                        {
                            Entities.User u = Managers.UserManager.Instance.Get(targetId);
                            if (u != null)
                            {
                                u.Send(new Packets.Authorization(Packets.Authorization.ErrorCodes.NormalProcedure));
                                u.Disconnect();
                            }
                            break;
                        }

                    default:
                        {
                            // Unused.
                            break;
                        }
                }
            }
            else
            {
              //  Log.Instance.WriteLine(string.Concat("Unknown PlayerAuthorization error: ", errorCode));
            }
        }
    }
}

