namespace Game.Enums
{
    public enum DamageClasses : byte
    {
       Personal = 0,
       Ground,
       Aircraft,
       Ship
    }

    public enum  DistanceTypes : byte
    {
        Short = 0,
        Medium,
        Long,
        COUNT
    }

    public enum DamageLocations : uint //reverse engineered based on Papaya´s WarRock balance video.
    {
        Headshot     = 120,
        TorsoLegs    = 60,
        FootArmHand  = 30

    }
}
