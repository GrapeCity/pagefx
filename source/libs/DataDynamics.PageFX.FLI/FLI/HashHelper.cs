using System;
using System.Security.Cryptography;

namespace DataDynamics.PageFX.FLI
{
    public class HashHelper
    {
        public const string TypeSHA256 = "SHA-256";

        static bool IsSHA256(string hashType)
        {
            if (string.IsNullOrEmpty(hashType)
                || string.Compare(hashType, "SHA256", true) == 0
                || string.Compare(hashType, "SHA-256", true) == 0)
                return true;
            return false;
        }

        public static byte[] Compute(string hashType, byte[] data)
        {
            if (IsSHA256(hashType))
            {
                var sha = SHA256.Create();
                return sha.ComputeHash(data);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static string GetDigest(string hashType, byte[] data)
        {
            var h = Compute(hashType, data);
            return Hex.ToString(h, false);
        }
    }
}