using System;
using System.Text;
using PCLCrypto;

namespace Inside.Xamarin.Helpers
{
    public static class CryptoHelper
    {
        public static byte[] CreateSalt(int lengthInBytes)
        {
            return WinRTCrypto.CryptographicBuffer.GenerateRandom(lengthInBytes);
        }
       
        public static byte[] CreateDerivedKey(string password, byte[] salt, int keyLengthInBytes = 32, int iterations = 1000)
        {
            byte[] key = NetFxCrypto.DeriveBytes.GetBytes(password, salt, iterations, keyLengthInBytes);
            return key;
        }
       
        public static string EncryptAes(string data, string password, byte[] salt)
        {
            byte[] key = CreateDerivedKey(password, salt);

            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            var bytes = WinRTCrypto.CryptographicEngine.Encrypt(symetricKey, Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(bytes);
        }
       
        public static string DecryptAes(string data, string password, byte[] salt)
        {
            byte[] dataBytes = Convert.FromBase64String(data);
            byte[] key = CreateDerivedKey(password, salt);

            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            var bytes = WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, dataBytes);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

    }

}

