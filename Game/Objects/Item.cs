/*
 *                      WarRock items basic class. Everything stored in items.bin is handled by this class and its childs
 *                      Any WarRock Item has two basic properties: item code and an active flag    
 */

using System;
using System.Collections.Generic;

namespace Game.Objects
{
    public abstract class Item
    {
    
        public bool Active { get; protected set; }
        public string Code { get; protected set; }

        protected Dictionary<byte, bool> _useableSlots;

        public Item(string itemCode, bool isActive, Dictionary<byte, bool> useableSlots)
        {
            Code          = itemCode;
            Active        = isActive;
           _useableSlots  = useableSlots;
        }

        public virtual void Disable()
        {
            if (Active)
                Active = false;
        }
        public virtual void Enable()
        {
            if (!Active)
                Active = true;
        }

        public virtual void EnableSlots(params byte[] slots)
        {
            for (byte i = 0; i < slots.Length; i++)
            {
                _useableSlots[i] = true;
            }

        }

        public virtual void DisableSlots(params byte[] slots)
        {
            for (byte i = 0; i < slots.Length; i++)
            {
                _useableSlots[i] = false;
            }

        }

        public virtual bool CanBeEquippedOnSlot(byte slot)
        {
            bool canBeUsed = false;

            if (_useableSlots.TryGetValue(slot, out canBeUsed)) //found a result, written to canBeUsed
                return canBeUsed;
            else
                throw new Exception("SLOT " + slot.ToString() + " does not exists in weapon dictionary");

        }

    }
}
