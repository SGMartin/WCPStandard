namespace Core.Networking
{
    public class Constants
    {
         public const byte ERROR_OK = 1;
        public const byte xOrKeyClientSend = 0x96;
        public const byte xOrKeyClientRecieve = 0xC3;

        public const byte xOrKeyServerSend = 0x23;
        public const byte xOrKeyServerRecieve = 0xA3;

        public enum Ports:int
        {
            Internal = 5012, // Used by Auth-Game comm.
            Login = 5330,   // Used by Auth-Client comm.
            Game = 5340,    // Used by Game comm.
            UDP1 = 5350,    // Peer to Peer com.
            UDP2 = 5351     // Peer to Peer com.
        }
    }
}