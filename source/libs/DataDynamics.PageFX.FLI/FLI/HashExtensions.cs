using System;
using System.Security.Cryptography;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.FLI
{
    public static class HashExtensions
    {
        public const string TypeSHA256 = "SHA-256";

        private static bool IsSHA256(string hashType)
        {
            if (string.IsNullOrEmpty(hashType)
                || string.Compare(hashType, "SHA256", true) == 0
                || string.Compare(hashType, "SHA-256", true) == 0)
                return true;
            return false;
        }

        public static byte[] ComputeHash(this byte[] data, string hashType)
        {
        	if (IsSHA256(hashType))
            {
                var sha = SHA256.Create();
                return sha.ComputeHash(data);
            }

        	throw new NotSupportedException();
        }

    	public static string GetHashString(this byte[] data, string hashType)
        {
            var h = data.ComputeHash(hashType);
            return Hex.ToString(h, false);
        }
    }
}