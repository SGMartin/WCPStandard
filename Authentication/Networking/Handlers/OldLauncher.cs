/*
 *                      Handler for the old client launcher. It sends: Format, client, sub, option etc... to the client
 * 
 * 
 */
using Serilog;
using Authentication.Entities;

namespace Authentication.Networking.Handlers
{
    class OldLauncher : PacketHandler
    {
        protected override void Process(User u)
        {
            if (Config.ENABLEOLDLAUNCHER){
                u.Send(new Packets.Launcher());
            }  
            else{ Log.Information("Player attempted to connect using old launcher");  }       
        }
    }
}
