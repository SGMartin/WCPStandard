using System.Collections.Generic;
using Serilog;
namespace Game.Unit_tests
{
   public class WeaponTest
    {

        public WeaponTest()
        {
            //Creating fake weapon: AK47 -> 500 damage
            /*
             * 
             * 	PERSONAL                    =	100,80,60
			    SURFACE                     =	2,0,0
		    	AIR                         =	2,0,0
		    	SHIP                        =	2,0,0
             * 
             * 
             * 
             */

            DamageOutput();
            RadiusTest();
            MaximumDamageReached();
            TryToEquipWeapon();
        }

        public void DamageOutput()
        {
           Dictionary<byte, bool> thirdSlot = new Dictionary<byte, bool>();

            for (byte i = 0; i < 7; i++) //0-7 slots
                thirdSlot.TryAdd(i, false);

            thirdSlot[3] = true; //enabled for third slot only

            Objects.Weapon AK47 = new Objects.Weapon("DC01", true, thirdSlot);

            byte[,] damageTable = new byte[4, 3] { { 100, 80, 60 },{ 2, 0, 0 },{ 2, 0, 0 },{ 2, 0, 0 } };

            AK47.SetRawDamage(500);
            AK47.SetDamageTable(damageTable);

            // 500 * 1,2  * 0,6 = 360
           if(AK47.GetDamageTo(Enums.DamageClasses.Personal, Enums.DamageLocations.Headshot, Enums.DistanceTypes.Long) != 360)
               Log.Fatal("Weapon Damage TEST: FAILED");
            else
            Log.Information("Weapon Damage TEST: SUCCESS");
        }

        public void MaximumDamageReached()
        {
            Dictionary<byte, bool> thirdSlot = new Dictionary<byte, bool>();

            for (byte i = 0; i < 7; i++) //0-7 slots
                thirdSlot.TryAdd(i, false);

            thirdSlot[3] = true; //enabled for third slot only

            Objects.Weapon AK47 = new Objects.Weapon("DC01", true, thirdSlot);

            byte[,] damageTable = new byte[4, 3] { { 100, 80, 60 }, { 2, 0, 0 }, { 2, 0, 0 }, { 2, 0, 0 } };

            AK47.SetRawDamage(50000000);
            AK47.SetDamageTable(damageTable);

            // if value > 1000, it should be set to 1000 when AK47.SetRawDamage is called
            if (AK47.GetDamageTo(Enums.DamageClasses.Personal, Enums.DamageLocations.Headshot, Enums.DistanceTypes.Long) == 720)
                Log.Information("Weapon MAXIMUM Damage TEST: SUCESS");
            else
                Log.Fatal("Weapon MAXIUM Damage TEST: FAILED");
        }

        public void TryToEquipWeapon()
        {
            Dictionary<byte, bool> thirdSlot = new Dictionary<byte, bool>();

            for (byte i = 0; i < 7; i++) //0-7 slots
                thirdSlot.TryAdd(i, false);

            thirdSlot[3] = true; //enabled for third slot only

            Objects.Weapon AK47 = new Objects.Weapon("DC01", true, thirdSlot);

            if (AK47.CanBeEquippedOnSlot(3)) {
                Log.Information("Weapon EQUIP TEST: SUCESS"); }
            else {
                Log.Fatal("WEAPON EQUIP TEST: FAILED"); }

           if (AK47.CanBeEquippedOnSlot(6))
                Log.Fatal("WEAPON FALSE EQUIP TEST: FAILED!");


        }

        public void RadiusTest()
        {
            Dictionary<byte, bool> thirdSlot = new Dictionary<byte, bool>();

            for (byte i = 0; i < 7; i++) //0-7 slots
                thirdSlot.TryAdd(i, false);

            thirdSlot[3] = true; //enabled for third slot only

            Objects.Weapon GRENADE = new Objects.Weapon("DC01", true, thirdSlot);

            byte[,] damageTable = new byte[4, 3] { { 100, 80, 60 }, { 2, 0, 0 }, { 2, 0, 0 }, { 2, 0, 0 } };

            GRENADE.SetRawDamage(800);
            GRENADE.SetDamageTable(damageTable);

            // GRENADE DAMAGE: 800 * 0.02 (aircraft lololol) * 0,8 (damage from center)
            if (GRENADE.GetRadiusDamageTo(Enums.DamageClasses.Aircraft, 80) !=  12.8)
                Log.Information("Weapon Radius Damage TEST: SUCESS");
            else
                Log.Fatal("Weapon Radius Damage TEST: FAILED");
        }
    }
}
