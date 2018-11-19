namespace Authentication.Networking.Handlers.Internal
{
    class Ping : Networking.PacketHandler
    {
        protected override void Process(Entities.Server s)
        {
            uint errorCode = GetuInt(0);
     //    dateTimeTicks   = GetuInt(1); //TODO: UNUSED
            ushort playerCount = GetUShort(2);
            ushort roomCount = GetUShort(3);

            if (s.Authorized && errorCode == 1)
            {
                try
                {
                    foreach (Entities.Server server in Managers.ServerManager.Instance.GetAllAuthorized())
                    {
                        if (server.ID == s.ID)
                        {
                            server.AddPlayers(playerCount);
                            server.AddRooms(roomCount);

                            //TODO: LOG TO DB?
                        }
                    }
                }
                catch
                {
                    //Log.Instance.WriteLine("An unregistered gameserver sent ping");
                }

            }

        }
    }
}