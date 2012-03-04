using System.Linq;
using System.Security.Cryptography;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Metadata
{
    internal static class StrongNameTools
    {
        //HACK!!! MS use this to sign own assemblies!
        private static readonly byte[] s_NeutralPublicKey = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] s_NeutralPublicKeyToken = new byte[] { 0xb7, 0x7a, 0x5c, 0x56, 0x19, 0x34, 0xe0, 0x89 };
        private const int s_TokenLength = 8;

        public static byte[] ComputePublicKeyToken(this byte[] key, HashAlgorithmId algorithm)
        {
            var arPKT = new byte[0];
            switch (algorithm)
            {
                case HashAlgorithmId.None:
                    break;

                case HashAlgorithmId.MD5:
                    break;

                case HashAlgorithmId.SHA1:
                    if (key.Length == s_NeutralPublicKey.Length)
                    {
                        bool isMatch = !s_NeutralPublicKey.Where((b, i) => b != key[i]).Any();
                    	if (isMatch)
                        {
                            arPKT = new byte[s_NeutralPublicKeyToken.Length];
                            s_NeutralPublicKeyToken.CopyTo(arPKT, 0);
                            return arPKT;
                            //return s_NeutralPublicKeyToken;
                        }
                    }

                    var hashProv = new SHA1Managed();
                    arPKT = new byte[s_TokenLength];
                    var hash = hashProv.ComputeHash(key);

                    for (int i = 0; i < s_TokenLength; i++)
                        arPKT[s_TokenLength - (i + 1)] = hash[i + (hash.Length - s_TokenLength)];
                    break;
            }
            return arPKT;
        }
    }
}