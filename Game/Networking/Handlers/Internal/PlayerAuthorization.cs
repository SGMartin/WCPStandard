using Serilog;

namespace Game.Networking.Handlers.Internal
{
    class PlayerAuthorization : PacketHandler
    {
        protected override void Process(Networking.AuthenticationClient s)
        {
            Log.Debug("Played authorized");
          //  s.Disconnect(true);
        }
    }
}
