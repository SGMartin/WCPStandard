
/*

                     Various utils such as XP/Level calculator

 */
using System.Text;
using System.Security.Cryptography;

using Core;

namespace Core
{

    public class Utils
    {
        public static bool isAlphaNumeric(string input) 
        {
            System.Text.RegularExpressions.Regex objAlphaNumericPattern = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]");
            return !objAlphaNumericPattern.IsMatch(input);
         }

         public static string CreateSHAHash (string input)
         {
              SHA256 SHA = SHA256.Create();

              byte[] inputBytes = Encoding.ASCII.GetBytes(input);
              byte[] hash       = SHA.ComputeHash(inputBytes);

            StringBuilder stringbuilder = new StringBuilder();
            
            for (int i = 0; i < hash.Length; i++)
            {
                stringbuilder.Append(hash[i].ToString("x2"));
            }
            
            return stringbuilder.ToString();

         }

         public static byte GetLevelforExp(ulong Exp)
        {
            byte lvl = 0;
            do
            {
                if (Exp < Constants.EXPTable[lvl]) break;
                ++lvl;
            } while (lvl < 101);
            return lvl;
        }

        public static ulong GetExpForLevel(int Level)
        {
            if (Level > 0) return Constants.EXPTable[Level - 1];
            return 0;
        }


    }

}