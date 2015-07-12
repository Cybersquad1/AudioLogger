using System;

namespace AudioLogger.Services
{
    public class EncryptionService : IEncryptionService
    {
        private static byte[] _key;

        public byte[] Encrypt(byte[] dataBytes)
        {
            throw new NotImplementedException();
        }

        public byte[] Decrypt(byte[] encryptedBytes)
        {
            throw new NotImplementedException();
        }
    }
}