using System;

namespace Game.Objects.Weapons
{
    public struct ShopData
    {

        public bool IsBuyable;
        public bool Default;
        public byte RequiredLevel;
        public bool RequiresPremium;

        public int[] Cost;


        public ShopData(bool isdefault, bool isBuyable, byte requiredLevel, bool requiresPremium, string cost)
        {
            this.Default           = isdefault;
            this.IsBuyable         = isBuyable;
            this.RequiredLevel     = requiredLevel;
            this.RequiresPremium   = requiresPremium;
            
            //generate cost array based on  cost string from Items.bin
            
          int i = 0;
          string[] strSplit = cost.Split(',');
          
            this.Cost = new int[strSplit.Length];
            
            foreach (string text in strSplit) 
            {
                int.TryParse(text, out Cost[i]); 
                ++i;
            }

            if (this.Cost.Length < 5)
                Array.Resize(ref this.Cost, 5);
        }
    }
}