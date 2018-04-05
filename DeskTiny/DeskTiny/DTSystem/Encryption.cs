using DTCore.Tools;

namespace DTCore.DTSystem
{
    public static class Encryption
    {
        public static string Encrypt(string input, bool getFromConfig = false)
        {
            if (getFromConfig && ConfigurationBuilder.Encryption.Active == false)
            {
                return input;
            }

            StringCipher stringCipher = new StringCipher
            {
                PasswordHash = ConfigurationBuilder.Encryption.PasswordHash,
                SaltKey = ConfigurationBuilder.Encryption.SaltKey,
                VIKey = ConfigurationBuilder.Encryption.VIKey
            };

            return stringCipher.Encrypt(input);
        }

        public static string Decrypt(string input, bool getFromConfig = false)
        {
            if (getFromConfig && ConfigurationBuilder.Encryption.Active == false)
            {
                return input;
            }

            StringCipher stringCipher = new StringCipher
            {
                PasswordHash = ConfigurationBuilder.Encryption.PasswordHash,
                SaltKey = ConfigurationBuilder.Encryption.SaltKey,
                VIKey = ConfigurationBuilder.Encryption.VIKey
            };

            return stringCipher.Decrypt(input);
        }
    }
}
