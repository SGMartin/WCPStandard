using Core.Databases;
namespace Authentication 
{
    class Databases {
        public static QueryManager Auth;

        public static bool Init() {
            Auth = new QueryManager(Config.AUTH_DATABASE[0], int.Parse(Config.AUTH_DATABASE[1]), Config.AUTH_DATABASE[2], Config.AUTH_DATABASE[3], Config.AUTH_DATABASE[4]);
            return Databases.Auth.Open();
        }
    }
}