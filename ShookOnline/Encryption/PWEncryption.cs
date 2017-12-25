using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ShookOnline.Encryption
{
    public class PWEncryption
    {
        public const int SALT_SIZE = 24;
        public const int HASH_SIZE = 24;
        public const int ITERATION_NUMBER = 500;

        public string createHash(string password)
        {
            //generate random salt
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_SIZE];
            rng.GetBytes(salt);

            //generate password hash
            byte[] hash = PBKDF2(password, salt, ITERATION_NUMBER, HASH_SIZE);
            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        private byte[] PBKDF2(string password, byte[] salt, int iTERATION_NUMBER, int outputBytes)
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt);
            rfc.IterationCount = iTERATION_NUMBER;
            return rfc.GetBytes(outputBytes);
        }

        private bool slowEqual(byte[] dbHash, byte[] passHash)
        {
            uint diff = (uint)dbHash.Length ^ (uint)passHash.Length;
            for (int i = 0; i < dbHash.Length && i < passHash.Length; i++)
                diff |= (uint)dbHash[i] ^ (uint)passHash[i];

            return diff == 0;
        }

        public bool validatePassword(string password , string dbHash)
        {
            char[] delimiter = { ':' };
            string[] split = dbHash.Split(delimiter);
            byte[] salt = Convert.FromBase64String(split[0]);
            byte[] hash = Convert.FromBase64String(split[1]);

            byte[] HashToValidate = PBKDF2(password, salt, ITERATION_NUMBER, hash.Length);

            return slowEqual(hash, HashToValidate);
        }
    }
}