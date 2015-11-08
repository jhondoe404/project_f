using System;
using System.Diagnostics;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace Frink.Helpers
{
    class EncryptHelper
    {
        /// <summary>
        ///     Basic AES encryption function
        /// </summary>
        /// <param name="input">string to be encrypted</param>
        /// <param name="pass">encryption key</param>
        /// <returns></returns>
        public static string AES_Encrypt(string input, string pass)
        {
            var sap = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            var hap = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            var hashaes = hap.CreateHash();

            try
            {
                var hash = new byte[32];
                hashaes.Append(CryptographicBuffer.CreateFromByteArray(Encoding.UTF8.GetBytes(pass)));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(hashaes.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);

                var aes = sap.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));

                var buffer = CryptographicBuffer.CreateFromByteArray(Encoding.UTF8.GetBytes(input));
                return CryptographicBuffer.EncodeToBase64String(CryptographicEngine.Encrypt(aes, buffer, null));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }


        /// <summary>
        ///     Decryption function
        /// </summary>
        /// <param name="input">string to be decrypted</param>
        /// <param name="pass">decryption key</param>
        /// <returns></returns>
        public static string AES_Decrypt(string input, string pass)
        {
            var sap = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            var hap = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            var hashaes = hap.CreateHash();

            try
            {
                var hash = new byte[32];
                hashaes.Append(CryptographicBuffer.CreateFromByteArray(Encoding.UTF8.GetBytes(pass)));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(hashaes.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);

                var aes = sap.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));

                var buffer = CryptographicBuffer.DecodeFromBase64String(input);
                byte[] decryptedByteArray;
                CryptographicBuffer.CopyToByteArray(CryptographicEngine.Decrypt(aes, buffer, null),
                    out decryptedByteArray);
                return Encoding.UTF8.GetString(decryptedByteArray, 0, decryptedByteArray.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
