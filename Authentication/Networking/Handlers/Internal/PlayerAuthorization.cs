
           /* Packet Structure v2 
                                 * 
                                 * Connection Time
                                 * Ping
                                 * 
                                 * [Sub Type]
                                 *  - 1 - Update Channel
                                 *  - 2 - Update Session Information
                                 *  - 3 - Update Action
                                 *  - 4 - Update Lobby Room information
                                 *  - 5 - Update Ingame Room information
                                 *  
                                 * [Data Blocks]
                                 *  - [1] - Update Channel
                                 *      - Current channel Id
                                 *      - Channel Slot
                                 *      
                                 *  - [2] - Update Session Information
                                 *      - Session - Kills
                                 *      - Session - Deaths
                                 *      - Session - Xp Earned
                                 *      - Session - Dinar Earned
                                 *      - Session - Dinar Spend
                                 *      
                                 *  - [3] - Update Action
                                 *      - Update Type:
                                 *      
                                 *          [1]: Join Room
                                 *               - Room ID
                                 *               - Room Slot
                                 *               - Room Is Master
                                 *               
                                 *          [2]: Leave Room
                                 *              - Room ID
                                 *              - Room Old Slot
                                 *              - Room Was Master?
                                 *                  - New master slot
                                 *                  
                                 *          [3]: Room Start
                                 *              - Team
                                 *              
                                 *          [4]: Room Stop
                                 *              - Kills
                                 *              - Deaths
                                 *              - Flags
                                 *              - Points
                                 *              - Xp Earned
                                 *              - Dinar Earned
                                 *              - xp Bonusses (%-Name:%-Name)
                                 *              - dinar bonusses (%-Name:%-Name)
                                 *      
                                 *  - [4] - Update Lobby Room information
                                 *      - Update Type:
                                 *      
                                 *          [1]: Switch Side
                                 *               - Room ID
                                 *               - Room Slot
                                 *               - Room Is Master
                                 *               
                                 *          [2]:                
                                 *
                                 *  - [5] - Update Ingame Room information
                                 *      - Update Type:
                                 *      
                                 *          [1]: Score Update (Player Kill/Player Death)
                                 *               - Room ID
                                 *               - Room Kills
                                 *               - Room Deaths
                                 *               - Room Flags
                                 *               - Room Points 
                                 *               
                                 *          [2]: 
                                 */

using System;
using Authentication.Entities;


namespace Authentication.Networking.Handlers.Internal
{
    class PlayerAuthorization : Networking.PacketHandler
    {
        protected override void Process(Entities.Server s)
        {
            ushort errorCode = GetUShort(0);

            if (Enum.IsDefined(typeof(Core.Networking.ErrorCodes), errorCode))
            {
                Core.Networking.ErrorCodes enumErrorCode = (Core.Networking.ErrorCodes)errorCode;
                uint targetId = GetuInt(1);
                string username = GetString(3);
                byte accessLevel = GetByte(4);
                switch (enumErrorCode)
                {
                    // A new player logs in.
                    //Raptor 1/6/18: Added more checks just in case someone managed to perform a MiM while another player is choosing server.
                    //That would lead to a registered, not active session which could be stolen if targetId is known

                    case Core.Networking.ErrorCodes.Success:
                        {
                            Session session = Managers.SessionManager.Instance.Get(targetId);
                            if (session != null)
                            {
                                if (!session.IsActivated && session.Name == username && (byte)session.AccessLevel == accessLevel)
                                {
                                    session.Activate((byte)s.ID);
                                    s.Send(new Packets.Internal.PlayerAuthentication(session));
                                }
                                else
                                {
                                    s.Send(new Packets.Internal.PlayerAuthentication(Core.Networking.ErrorCodes.EntityAlreadyAuthorized, targetId));
                                }
                            }
                            else
                            {
                                s.Send(new Packets.Internal.PlayerAuthentication(Core.Networking.ErrorCodes.InvalidKeyOrSession, targetId));
                            }

                            break;
                        }

                    // Update the information of a player.
                    //TODO: DARK INVESTIGATE THIS
                    case Core.Networking.ErrorCodes.Update:
                        {
                            Session session = Managers.SessionManager.Instance.Get(targetId);
                            if (session != null)
                            {
                                

                            }
                            else
                            {
                                // Force a closure of the connection.
                                s.Send(new Packets.Internal.PlayerAuthentication(Core.Networking.ErrorCodes.InvalidKeyOrSession, targetId));
                            }

                            break;
                        }

                    // A player logs out of the server.
                    case Core.Networking.ErrorCodes.EndConnection:
                        {
                            Session session = Managers.SessionManager.Instance.Get(targetId);
                            if (session != null)
                            {
                                if (session.IsActivated)
                                {
                                    session.End();
                                }
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
                Console.WriteLine(string.Concat("Unknown PlayerAuthorization error: ", errorCode));
            }
        }
    }
}