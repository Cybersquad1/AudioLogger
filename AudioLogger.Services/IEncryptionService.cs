namespace AudioLogger.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string dataToEncrypt);
        string Decrypt(string encryptedString);
    }
}