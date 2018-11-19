/*
            Enumeration of error codes used by packets. These are used whenever a connection to Auth server happens. It might be
            a game server or a player. Outcomes and errors are similar, so they have been grouped together.
     
 */



namespace Core.Networking
{

    public enum ErrorCodes: ushort
    {
        Success = 1,
        Update  = 2,
        EndConnection = 3,
        InvalidKeyOrSession = 0x100,
        InvalidSessionMatch = 0x110,     // <- Connection key or session is valid but some data does not match.
        EntityAlreadyAuthorized = 0x120, // <- Game server or user attempting to get Authentication is already authorized.
        ServerLimitReached = 0x130

        
    }

}
