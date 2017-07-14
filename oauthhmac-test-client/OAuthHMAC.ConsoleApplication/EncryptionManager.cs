using System;
using System.Security.Cryptography;
using System.Text;

namespace OAuthHMAC.ConsoleApplication
{
    public class EncryptionManager
    {
        // private static readonly string _privateKey = ConfigurationManager.AppSettings["privateKey"];
        // private static readonly string _initializeVector = ConfigurationManager.AppSettings["initializeVector"];

        public static string Encrypt(string jsonObject)
        {

            if (jsonObject.Contains("•"))
            {
                var replaceBulletPoint = jsonObject.Replace("•", "-");

                // Set up byte array for the plain text that needs to be encrypted/decrypted
                byte[] retrievedValueBytesBulletPoint = Encoding.UTF8.GetBytes(replaceBulletPoint);

                AesCryptoServiceProvider aesServiceProvider = new AesCryptoServiceProvider
                {
                    // Set the AES block size to be 128 bits.
                    BlockSize = 128,

                    // Set the AES key size to be 256 bits.
                    KeySize = 256,

                    // Ensure that the key provided is looking at the Key variable.
                    Key = Encoding.UTF8.GetBytes("qwertyuiopasdfghjklzxcvbnmqwerty"),

                    // Set the AES Initialization Vector to be the IV variable.
                    IV = Encoding.UTF8.GetBytes("poiuytrewqlkjhgf"),

                    // Set the AES Padding to be that of PKCS7 (the value of each added byte is the number of bytes that are added. E.g:
                    // 01
                    // 02 02
                    // 03 03 03
                    // 04 04 04 04
                    // Etc.
                    Padding = PaddingMode.PKCS7,

                    // Set the AES Cipher Mode to CBC. In CBC mode, each block of plain text is XORed with the previous cipher block before being encrypted.
                    // See the following link for a more detailed explanation (https://en.wikipedia.org/wiki/Block_cipher_mode_of_operation#Cipher_Block_Chaining_.28CBC.29).
                    Mode = CipherMode.CBC
                };

                // Transforms the AES Crypto Service Provider to look at the specified Key and IV.
                ICryptoTransform cryptoTransform = aesServiceProvider.CreateEncryptor(aesServiceProvider.Key, aesServiceProvider.IV);

                // Transforms the specified region of the input byte array and copies the resulting transform to the specified region of the output byte array.
                byte[] encryptedMessage = cryptoTransform.TransformFinalBlock(retrievedValueBytesBulletPoint, 0, jsonObject.Length);

                // Chuck the cryptoTransform in the bin, no longer wanted or needed here on out.
                cryptoTransform.Dispose();

                // Return the plain text as a encrypted base 64 string.
                return Convert.ToBase64String(encryptedMessage);
            }

            else
            {

                // Set up byte array for the plain text that needs to be encrypted/decrypted
                byte[] retrievedValueBytes = Encoding.UTF8.GetBytes(jsonObject);

                AesCryptoServiceProvider aesServiceProvider = new AesCryptoServiceProvider
                {
                    // Set the AES block size to be 128 bits.
                    BlockSize = 128,

                    // Set the AES key size to be 256 bits.
                    KeySize = 256,

                    // Ensure that the key provided is looking at the Key variable.
                    Key = Encoding.UTF8.GetBytes("qwertyuiopasdfghjklzxcvbnmqwerty"),

                    // Set the AES Initialization Vector to be the IV variable.
                    IV = Encoding.UTF8.GetBytes("poiuytrewqlkjhgf"),

                    // Set the AES Padding to be that of PKCS7 (the value of each added byte is the number of bytes that are added. E.g:
                    // 01
                    // 02 02
                    // 03 03 03
                    // 04 04 04 04
                    // Etc.
                    Padding = PaddingMode.PKCS7,

                    // Set the AES Cipher Mode to CBC. In CBC mode, each block of plain text is XORed with the previous cipher block before being encrypted.
                    // See the following link for a more detailed explanation (https://en.wikipedia.org/wiki/Block_cipher_mode_of_operation#Cipher_Block_Chaining_.28CBC.29).
                    Mode = CipherMode.CBC
                };

                // Transforms the AES Crypto Service Provider to look at the specified Key and IV.
                ICryptoTransform cryptoTransform = aesServiceProvider.CreateEncryptor(aesServiceProvider.Key, aesServiceProvider.IV);

                // Transforms the specified region of the input byte array and copies the resulting transform to the specified region of the output byte array.
                byte[] encryptedMessage = cryptoTransform.TransformFinalBlock(retrievedValueBytes, 0, jsonObject.Length);

                // Chuck the cryptoTransform in the bin, no longer wanted or needed here on out.
                cryptoTransform.Dispose();

                // Return the plain text as a encrypted base 64 string.
                return Convert.ToBase64String(encryptedMessage);
            }
        }
    }
}