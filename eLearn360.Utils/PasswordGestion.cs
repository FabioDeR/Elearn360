using System.Security.Cryptography;

namespace eLearn360.Utils
{
    public class PasswordGestion
    {
        public static string PasswordGeneration(int length)
        {
            try
            {
                string passwordString = string.Empty;
                using (RNGCryptoServiceProvider cryptRNG = new RNGCryptoServiceProvider())
                {
                    byte[] tokenBuffer = new byte[length];
                    cryptRNG.GetBytes(tokenBuffer);
                    passwordString = Convert.ToBase64String(tokenBuffer) + "?";
                }

                return passwordString;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }
    }
}