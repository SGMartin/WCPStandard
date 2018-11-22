
using System.Collections;
using System.Collections.Concurrent;

using Serilog;

using Game.Networking;

namespace Game.Managers
{
    class PacketManager
    {

        private Hashtable _externalPacketList = new Hashtable();
        private Hashtable _internalPacketList = new Hashtable();

     //   private ConcurrentDictionary<ushort, Networking.GameDataHandler> _gameHandlers = new ConcurrentDictionary<ushort, GameDataHandler>();


        public PacketManager()
        {
            _internalPacketList.Clear();
            _externalPacketList.Clear();
            //_gameHandlers.Clear();

            // Internal Packets//
            AddInternal(Core.Networking.PacketList.Connection,           new Networking.Handlers.Internal.Connection());
            AddInternal(Core.Networking.PacketList.ServerAuthentication, new Networking.Handlers.Internal.Authorization());
            AddInternal(Core.Networking.PacketList.Ping                , new Networking.Handlers.Internal.Ping());
           // AddInternal(Core.Networking.PacketList.PlayerAuthentication, new Networking.Handlers.Internal.PlayerAuthorization());

            /*
            // External Packets //
            AddExternal(Enums.Packets.LeaveServer, new Handlers.LeaveServer());
            AddExternal(Enums.Packets.ServerTime, new Handlers.RequestServerTime());
            AddExternal(Enums.Packets.CMDFindPlayer, new Handlers.CMDFindPlayer());
            AddExternal(Enums.Packets.Authorization, new Handlers.Authorization());
            AddExternal(Enums.Packets.ChannelSelection, new Handlers.ChangeChannel());
            AddExternal(Enums.Packets.RoomCreation, new Handlers.RoomCreation());
            AddExternal(Enums.Packets.RoomJoin, new Handlers.RoomJoin());
            AddExternal(Enums.Packets.RoomLeave, new Handlers.RoomLeave());
            AddExternal(Enums.Packets.RoomInvite, new Handlers.RoomInvite());
            AddExternal(Enums.Packets.Equipment, new Handlers.Equipment());
            AddExternal(Enums.Packets.Chat, new Handlers.Chat());
            AddExternal(Enums.Packets.RoomList, new Handlers.RoomList());
            AddExternal(Enums.Packets.Itemshop, new Handlers.Itemshop());
            AddExternal(Enums.Packets.GamePacket, new Handlers.RoomData());
            AddExternal(Enums.Packets.Explosives, new Handlers.Explosives());
            AddExternal(Enums.Packets.Scoreboard, new Handlers.Scoreboard());
            AddExternal(Enums.Packets.Ping, new Handlers.Ping());
            AddExternal(Enums.Packets.UserList, new Handlers.UserList());
            AddExternal(Enums.Packets.Coupon, new Handlers.Coupons());
            AddExternal(Enums.Packets.RoomQuickJoin, new Handlers.RoomQuickJoin());
            AddExternal(Enums.Packets.RoomKick, new Handlers.RoomKick());
            AddExternal(Enums.Packets.LeaveVehicle, new Handlers.LeaveVehicle());

            // Game Data Handlers //
            AddGameHandler(Enums.GameSubs.Start, new Handlers.Game.Start());
            AddGameHandler(Enums.GameSubs.RoundEndConfirm, new Handlers.Game.Ingame.RoundReady());
            AddGameHandler(Enums.GameSubs.BackToLobby, new Handlers.Game.Ingame.BackToLobby());
            AddGameHandler(Enums.GameSubs.Ready, new Handlers.Game.ToggleReady());
            AddGameHandler(Enums.GameSubs.MapUpdate, new Handlers.Game.ChangeMap());
            AddGameHandler(Enums.GameSubs.ModeUpdate, new Handlers.Game.ChangeMode());
            AddGameHandler(Enums.GameSubs.ChangeRounds, new Handlers.Game.ChangeSubMode());
            AddGameHandler(Enums.GameSubs.ChangeKills, new Handlers.Game.ChangeSubMode());
            AddGameHandler(Enums.GameSubs.ChangeKills, new Handlers.Game.ChangeSubMode());
            AddGameHandler(Enums.GameSubs.SideUpdate, new Handlers.Game.ChangeSide());
            AddGameHandler(Enums.GameSubs.Userlimit, new Handlers.Game.ToggleUserlimit());
            AddGameHandler(Enums.GameSubs.Pinglimit, new Handlers.Game.ChangePinglimit());
            AddGameHandler(Enums.GameSubs.EnviromentDamage, new Handlers.Game.Ingame.EnvironmentDamage());
            AddGameHandler(Enums.GameSubs.AmmoRecharge, new Handlers.Game.Ingame.AmmoRecharge());
            AddGameHandler(Enums.GameSubs.PlaceItemOnGround, new Handlers.Game.Ingame.PlaceItemOnGround());
            AddGameHandler(Enums.GameSubs.UseItemOnGround, new Handlers.Game.Ingame.UseItemsOnGround());
            AddGameHandler(Enums.GameSubs.FlagCapture, new Handlers.Game.Ingame.FlagCapture());
            AddGameHandler(Enums.GameSubs.Suicide, new Handlers.Game.Ingame.Suicide());
            AddGameHandler(Enums.GameSubs.Repair, new Handlers.Game.Ingame.VehicleRepair());
            AddGameHandler(Enums.GameSubs.Autostart, new Handlers.Game.ToggleAutostart());
            AddGameHandler(Enums.GameSubs.ConfirmSpawn, new Handlers.Game.Ingame.ConfirmSpawn());
            AddGameHandler(Enums.GameSubs.Spawn, new Handlers.Game.Ingame.Spawn());
            AddGameHandler(Enums.GameSubs.PlayerDamage, new Handlers.Game.Ingame.PlayerDamage());
            AddGameHandler(Enums.GameSubs.PlayerDeath, new Handlers.Game.Ingame.PlayerDeath());
            AddGameHandler(Enums.GameSubs.GetMission, new Handlers.Game.Setup());
            AddGameHandler(Enums.GameSubs.Heal, new Handlers.Game.Ingame.Heal());
            AddGameHandler(Enums.GameSubs.WeaponSwapping, new Handlers.Game.Ingame.WeaponSwitch());
            AddGameHandler(Enums.GameSubs.PlayerDeath, new Handlers.Game.Ingame.PlayerDeath());
            AddGameHandler(Enums.GameSubs.EnterVehicle, new Handlers.Game.Ingame.VehicleEnter());
            AddGameHandler(Enums.GameSubs.LeaveVehicle, new Handlers.Game.Ingame.VehicleLeave());
            AddGameHandler(Enums.GameSubs.ChangeVehicleSeat, new Handlers.Game.Ingame.VehicleChangeSeat());
            AddGameHandler(Enums.GameSubs.ObjectDamage, new Handlers.Game.Ingame.ObjectDamage());
            AddGameHandler(Enums.GameSubs.Artillery, new Handlers.Game.Ingame.Artillery());
            AddGameHandler(Enums.GameSubs.ConfirmDeath, new Handlers.Game.Ingame.KillConfirmed());

            //Dev handlers here
            if (Config.SERVER_DEBUG == 1)
            {
                AddExternal(Enums.Packets.RoomSpectate, new Handlers.RoomSpectate());
            }
            */
        }
        /*
        private void AddGameHandler(Enums.GameSubs subId, Networking.GameDataHandler handler)
        {
            try
            {
                if (!_gameHandlers.ContainsKey((ushort)subId))
                {
                    _gameHandlers.TryAdd((ushort)subId, handler);
                }
            }
            catch { }
        }
        */
        private void AddInternal(Core.Networking.PacketList packetType, PacketHandler handler)
        {
            if (!_internalPacketList.ContainsKey(packetType))
            {
                _internalPacketList.Add((ushort)packetType, handler);
            }
        }

        public PacketHandler FindInternal(Core.Networking.InPacket inPacket)
        {
            if (_internalPacketList.ContainsKey(inPacket.Id))
            {
                return (Networking.PacketHandler)_internalPacketList[inPacket.Id];
            }
            else
            {
                Log.Error("UNKNOWN PACKET :: " + inPacket.fullPacket.Remove(inPacket.fullPacket.Length - 1));
            }
            return null;
        }
        /*
        private void AddExternal(Enums.Packets packetType, PacketHandler handler)
        {
            if (!_externalPacketList.ContainsKey(packetType))
            {
                _externalPacketList.Add((ushort)packetType, handler);
            }
        }

        public GameDataHandler GetHandler(ushort id)
        {
            GameDataHandler handler = null;
            if (_gameHandlers.ContainsKey(id))
            {
                try
                {
                    _gameHandlers.TryGetValue(id, out handler);
                }
                catch { handler = null; }
            }
            return handler;
        }

        public PacketHandler FindExternal(Core.Networking.InPacket inPacket)
        {
            if (_externalPacketList.ContainsKey(inPacket.Id))
            {
                return (Networking.PacketHandler)_externalPacketList[inPacket.Id];
            }
            else
            {
                Log.Instance.WriteBoth("UNKNOWN PACKET :: " + inPacket.fullPacket.Remove(inPacket.fullPacket.Length - 1));
            }
            return null;
        }
        */

        private static PacketManager instance = null;
        public static PacketManager Instance { get { if (instance == null) instance = new PacketManager(); return instance; } set { } }
    }
}