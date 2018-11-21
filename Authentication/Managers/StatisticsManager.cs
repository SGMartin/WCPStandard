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


 
   
        private static StatisticsManager instance;
        public static  StatisticsManager Instance { get { if (instance == null) { instance = new StatisticsManager(); } return instance; } }
    }
}
