/*

                                All game-related constants are defined here for further use by both Auth and Game server such as EXP. table
                                or server type.

 */


using System;
using System.Text;

namespace Core
{
    public class GameConstants
    {

        //Only normal and adult are visible by regular users. Ids 21-29 are labeled "trainee"
        public enum ServerTypes : byte 
        {
        Normal = 0,
        Adult,
        Clan,
        Test,
        Broadcast,
        Development
        }

        public enum Rights : byte
        {
            Blocked = 0,
            Regular,
            Administrator = 3,
            Developer = 5
        }


        public const sbyte maxChannelsCount = 3;


       public static ulong[] EXPTable = {
        //1-10
        0,2250,6750,11250,16650,24750,32850,41625,50400,59175,
        //11-20
        67950,76725,94725,112725,130725,148725,166725,189225,211725,234225,
        //21-30
        256725,279225,306225,333225,360225,387225,414225,441225,497475,553725,
        //31-40
        609975,666225,722475,778725,857475,936225,1014975,1093725,1172475,1251225,
        //41-50
        1363725,1476225,1588725,1701225,1813725,1926225,2038725,2207475,2376225,2544975,
        //51-60
        2713725,2882475,3051225,3219975,3444975,3669975,3894975,4119975,4344975,4569975,
        //61-70
        4794975,5132475,5469975,5807475,6144975,6482475,6819975,7157475,7494975,7944975,
        //71-80
        8394975,8844975,9294975,9744975,10194975,10644975,11094975,11657475,12219975,12782475,
        //81-90
        13344975,13907475,14469975,15032475,15932475,17282475,18632475,19982475,21332475,22682475,
        //91-100
        24032475,25382475,26732475,28307475,29882475,31457475,33032475,34607475,36182475,37757475,
        //102
        2147483647};

    }
}
