/*
 *            Basic weapon class: contains all weapon relevant data and damage methods
 * 
 */

/*                           DamageTable
    * -----------------------------------------------------------------------------
    *                      Low  | Mid | Long       Distance
    * Power    Personal      x     y       z
    *          Ground        a     b       c
    *          Aircraft
    *          Ship
   
*/
using System;
using Game.Enums;

namespace Game.Objects.Weapons
{
    
 public class Weapon:Item
 {
    public uint RawPower { get; private set; }
    
    private byte[,] _damageTable                = new byte[4, 3];  //[rows, columns]  
    
   public Weapon(uint weaponId, string weaponCode, bool isActive, ShopData shop)
       :base(weaponId, weaponCode, isActive, shop)
   {
       RawPower      = 0;
       this.IsWeapon = true;
   }
   
 
  
   public void SetDamageTable(byte[,] newDamageTable)
   {
       _damageTable = newDamageTable;
   }

   public void UpdateDamageRow(DamageClasses damageClass, byte[] modifiers)
   {
       for(byte i = 0; i < (byte)DistanceTypes.COUNT; i++)
       {
           _damageTable[(byte)damageClass, i] = modifiers[i];
       }
   }

   public void SetRawDamage(uint newDamage)
   {
       RawPower = (newDamage > 1000) ? 1000 : newDamage;
   }

   public uint GetDamageTo(DamageClasses typeOfDamage, DamageLocations location, DistanceTypes distance)
   {
       uint totalDamage = 0;

       double vulnerabilityModifier = _damageTable[(byte)typeOfDamage, (byte)distance]; //gets the modifier based on distance and type of entity attacked: ie ship, or player        
       double locationModifier = (uint)location;

       locationModifier      = locationModifier / 100;
       vulnerabilityModifier = vulnerabilityModifier / 100;

       totalDamage = (uint)Math.Round(RawPower * vulnerabilityModifier * locationModifier);

       totalDamage = (totalDamage > 1000) ? 1000 : totalDamage;

       return totalDamage;
   }

   public uint GetRadiusDamageTo(DamageClasses typeOfDamage, uint explosionModifier)
   {
       uint    totalDamage = 0;
       double explosionMuliplier = explosionModifier; // (ranges from 100 to 0)
       double vulnerabilityModifier = _damageTable[(byte)typeOfDamage, 1];

       explosionModifier     = explosionModifier / 100;
       vulnerabilityModifier = vulnerabilityModifier / 100;

       totalDamage = (uint)Math.Round(RawPower * explosionModifier * vulnerabilityModifier);

       totalDamage = (totalDamage > 1000) ? 1000 : totalDamage;

       return totalDamage;
   }

   
   
   
   
 }
}