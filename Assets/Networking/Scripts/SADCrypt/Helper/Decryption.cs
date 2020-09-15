using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace SADCrypt
{
    public class Decryption
    {
        internal static byte[] Decrypt(ICryptoTransform decryptor, byte[] encryptedData)
        {

            using (MemoryStream ms = new MemoryStream(encryptedData))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(encryptedData, 0, encryptedData.Length);
                    //using (StreamWriter sw = new StreamWriter(cs))
                    //{
                    //    //Write all data to the stream.
                    //    sw.Write(encryptedData);
                    //}
                }
                return ms.ToArray();
            }
        }
    }
}
