/*
 * 
 *                  User statistics are stored here. Database operation pertaining these are also here.
 * 
 * 
 */ 

using System;
using Serilog;
using System.Threading.Tasks;
using System.Collections.Generic;
using Core.Databases;

namespace Game.Objects
{

    public class UserStats

    {
        public uint Kills { get; private set; }
        public uint Headshots { get; private set; }
        public uint Deaths { get; private set; }

        public uint BombsPlanted { get; private set; }
        public uint BombsDefused { get; private set; }
        public uint RoundsPlayed { get; private set; }
        public uint FlagsTaken   { get; private set; }
        public uint Victories    { get; private set; }
        public uint Defeats      { get; private set; }
        public uint VehiclesDestroyed { get; private set; }

        public UserStats()
        {
            SetDefaults();
        }
        
        public async Task<bool> GetDataBaseUserStats(uint userID)
        {
            List<object> Stats = new List<object>();

            Stats = await QueryUserStats(userID);

            if(Stats != null && Stats.Count > 0)
            {
                Kills       = Convert.ToUInt32(Stats[0]);
                Deaths      = Convert.ToUInt32(Stats[1]);
                Headshots   = Convert.ToUInt32(Stats[2]);

                BombsPlanted = Convert.ToUInt32(Stats[3]);
                BombsDefused = Convert.ToUInt32(Stats[4]);
                RoundsPlayed = Convert.ToUInt32(Stats[5]);

                FlagsTaken           = Convert.ToUInt32(Stats[6]);
                Victories            = Convert.ToUInt32(Stats[7]);
                Defeats              = Convert.ToUInt32(Stats[8]);
                VehiclesDestroyed    = Convert.ToUInt32(Stats[9]);
                return true;
            }
            else //Use default instead and send an error
            {
                Log.Warning("Could not retrieve stats for user " + userID.ToString());
                return false;
            }          
        }

        public void ResetKillsDeaths()
        {
            Kills = Deaths = 0;
        }

        public void ResetAllStats()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            Kills = Deaths = Headshots = BombsDefused = BombsPlanted = RoundsPlayed = FlagsTaken
                    = Victories = Defeats = VehiclesDestroyed = 0;
        }

        private async Task<List<object>> QueryUserStats(uint userID)
         {
            //MySQL query to load the data
            List<object> UserStats = new List<object>();

            using (Database UserData = new Database(Config.GAME_CONNECTION))
            {
                UserStats = await UserData.AsyncGetRowFromTable(new string[]
            { "kills","deaths","headshots","bombs_planted","bombs_defused","rounds_played", "flags_taken",
            "victories",  "defeats", "vehicles_destroyed"}
            ,
             "user_stats",
             new Dictionary<string, object>()
                        {
                                { "ID", userID },
                        });
            }
            return UserStats;
        }
    }


}
