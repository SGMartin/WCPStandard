/*

     Entities all share an ID, Name and AccessLevel (akin to rights). It might be an user, a server or even a room.

 */


namespace Core.Entities
{
    public abstract class Entity
    {
        public uint ID { get; protected set; }
        public string Name { get; protected set; }
        public byte AccessLevel { get; protected set; }

        public Entity(uint id, string name, byte _accessLevel)
        {
            this.ID = id;
            this.Name = name;
            this.AccessLevel = _accessLevel;
        }
    }
}