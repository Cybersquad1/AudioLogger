namespace AudioLogger.Services
{
    public interface IEncryptionService
    {
        byte[] Encrypt(byte[] dataBytes);
        byte[] Decrypt(byte[] encryptedBytes);
    }
}