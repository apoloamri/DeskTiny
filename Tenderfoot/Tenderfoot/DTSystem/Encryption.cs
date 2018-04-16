using Tenderfoot.Tools;

namespace Tenderfoot.DTSystem
{
    public static class Encryption
    {
        public static string Encrypt(string input, bool getFromConfig = false)
        {
            if (getFromConfig && Settings.Encryption.Active == false)
            {
                return input;
            }

            StringCipher stringCipher = new StringCipher
            {
                PasswordHash = Settings.Encryption.PasswordHash,
                SaltKey = Settings.Encryption.SaltKey,
                VIKey = Settings.Encryption.VIKey
            };

            return stringCipher.Encrypt(input);
        }

        public static string Decrypt(string input, bool getFromConfig = false)
        {
            if (getFromConfig && Settings.Encryption.Active == false)
            {
                return input;
            }

            StringCipher stringCipher = new StringCipher
            {
                PasswordHash = Settings.Encryption.PasswordHash,
                SaltKey = Settings.Encryption.SaltKey,
                VIKey = Settings.Encryption.VIKey
            };

            return stringCipher.Decrypt(input);
        }
    }
}
