using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using log4net;

namespace AudioLogger.Services
{
    public class EncryptionService : IEncryptionService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (EncryptionService));

        // This key needs to be hardcoded and deleted when uploaded to repository!
        // It would also be best to change these if they might have been compromised
        // or uploaded by accident
        private static readonly byte[] U8Salt = {};

        private static readonly string Key = "";

        public EncryptionService()
        {
            if (U8Salt.Count() != 16) throw new ArgumentException("the encryption salt is not 16 bytes long or not set");
            if (Key.Length == 0) throw new ArgumentException("The encryption key is not set");
        }

        public string Encrypt(string plainText)
        {
            try
            {
                var pdb = new Rfc2898DeriveBytes(Key, U8Salt);
                using (var iAlg = new RijndaelManaged {Key = pdb.GetBytes(32), IV = pdb.GetBytes(16)})
                {
                    using (var memoryStream = new MemoryStream())
                    using (
                        var cryptoStream = new CryptoStream(memoryStream, iAlg.CreateEncryptor(),
                            CryptoStreamMode.Write))
                    {
                        var data = Encoding.UTF8.GetBytes(plainText);
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();

                        return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
                Logger.Warn("Can't encrypt the password");
            }
            return string.Empty;
        }

        public string Decrypt(string cipherText)
        {
            try
            {
                var pdb = new Rfc2898DeriveBytes(Key, U8Salt);

                using (
                    var iAlg = new RijndaelManaged
                    {
                        Padding = PaddingMode.Zeros,
                        Key = pdb.GetBytes(32),
                        IV = pdb.GetBytes(16)
                    })
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (
                            var cryptoStream = new CryptoStream(memoryStream, iAlg.CreateDecryptor(),
                                CryptoStreamMode.Write))
                        {
                            var data = Convert.FromBase64String(cipherText);
                            cryptoStream.Write(data, 0, data.Length);
                            cryptoStream.Flush();

                            return Encoding.UTF8.GetString(memoryStream.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
                Logger.Warn("Can't decrypt the password");
            }
            return string.Empty;
        }
    }
}