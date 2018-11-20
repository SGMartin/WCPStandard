/*

     Entities all share an ID, Name and AccessLevel (akin to rights). It might be an user, a server or even a room.
     Different entities expand these class, such as the user adding the Nickname field.

 */


namespace Core.Entities
{
    public abstract class Entity
    {
        public uint ID { get; protected set; }
        public string Name { get; protected set; }
        public GameConstants.Rights AccessLevel { get; protected set; }

        public Entity(uint id, string name, GameConstants.Rights rights)
        {
            this.ID = id;
            this.Name = name;
            this.AccessLevel = rights;
        }
    }
}