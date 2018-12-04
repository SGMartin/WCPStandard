/*
 * 
 *                  This class separates premium funcionality from User.cs in an attempt to limit its responsabilities and improve overall design.
 * 
 */

using System;

using Core.Databases;
using Game.Enums;

namespace Game.Objects
{
    public class UserPremium
    {
       
        public Premium Premium               { get; private set; }
        public long  RemainingPremiumSeconds { get;  private set; }

        private ulong _premiumExpireDate   = 0;

        private readonly Entities.User User;

        public UserPremium(Entities.User user, Premium premiumType, ulong premiumExpireDate)
        {
            User = user;
            Premium                 = premiumType;
           _premiumExpireDate       = premiumExpireDate;
            RemainingPremiumSeconds = 0;
        }


        public void UpdatePremiumState()
        {
            if (_premiumExpireDate > 0 || Premium != Premium.Free2Play)
            {
                uint currentTimestamp = (uint)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
                RemainingPremiumSeconds = (long)(_premiumExpireDate - currentTimestamp);

                if (RemainingPremiumSeconds <= 0) // The Premium expired.
                { 
                    RemainingPremiumSeconds = 0;
                    _premiumExpireDate = 0;
                    Premium = Premium.Free2Play;

                    UpdatePremiumRecord();
                }
            }
            else
            {
                RemainingPremiumSeconds = 0;
                Premium = Premium.Free2Play;
            }
        }

        private async void UpdatePremiumRecord()
        {
            using (Database DbConnection = new Database(Config.GAME_CONNECTION))
            {
                await DbConnection.AsyncQuery("UPDATE user_details SET premium=" + (byte)Premium + ", premium_expiredate=" + _premiumExpireDate + " WHERE id=" + User.ID);
            }
        }

    }
}
