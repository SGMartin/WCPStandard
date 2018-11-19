/* Statistics Manager handles async queries to the MasterServer DB 
 * Author: DarkRaptor
 * Time:   30-5-2018
 * 
 * 
*/
using System;

namespace Authentication.Managers
{
    class StatisticsManager
    {


        public void UpdatePlayerCount(ulong currentPlayers, ulong peakPlayers, ulong totalPlayers)
        {

            string updateQuery = string.Format("UPDATE player_statistics SET online={0}, peak={1}, total={2};", currentPlayers, peakPlayers, totalPlayers);
            //Databases.Auth.AsyncQuery(updateQuery);
            
        }
   
        private static StatisticsManager instance;
        public static  StatisticsManager Instance { get { if (instance == null) { instance = new StatisticsManager(); } return instance; } }
    }
}
