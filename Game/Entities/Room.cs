using System;

using Game.Objects;

namespace Game.Entities
{
    public class Room
    {
        public uint ID { get; private set; }
       // public Ch
        public Room()
        {
            ID = 1;
        }

        public void Send(byte [] buffer)
        { 
        }
        public void Remove(Entities.User u)
        {

        }
    }
}
