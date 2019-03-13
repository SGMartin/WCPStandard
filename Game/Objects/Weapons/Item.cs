/*
 *        Basic WarRock Item class, including Weapon and PX.
 *
 * 
 */

namespace Game.Objects.Weapons
{
    public class Item
    {
        
        public uint   ID { get; private set; }
        public string Code { get; private set; }
        
        public bool IsActive { get; private set; }
        public bool IsWeapon { get; protected set; }
        
        public ShopData ShopData { get; private set; }
        
        public Item(uint id,  string code, bool isActive, ShopData shopData)
        {
            this.ID       = id;
            this.Code     = code;
            this.IsActive = IsActive;
            this.IsWeapon = false;
            this.ShopData = shopData;
        }


        public void Enable()
        {
            if (!IsActive)
                IsActive = true;
        }

        public void Disable()
        {
            if (IsActive)
                IsActive = false;
        }
    }
}