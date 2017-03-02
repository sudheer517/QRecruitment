using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using SendGrid;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Quantium.Recruitment.Portal.Helpers
{
    public class MessageSender 
    {
       
        public MessageSender()
        {

        }
      
        public Task SendEmailAsync(string email, string subject, string message)
        {
           
            string apiKey = Decrypt("cMQ38XsoFwojn+dxM/IAJkboQR6MaFIE4DY+foFkO/osGWdGLuZzCaxyKIx03TOHWDgflQ6IFUzK3d0zxBBOyw/TsN3XNZESwU8N+8zu6xlx2nzcnp6QW0AGwJ+H9zleNCS0m2AJSbkiTnWNyAkiAevNGP6uvVOC9SeFnCI0kAFru+4JkbdZr7g0KdpN9bib");
            dynamic sg = new SendGridAPIClient(apiKey);

            string data = @"{
              'personalizations': [
                {
                  'to': [
                    {
                      'email': '"+ email +@"'
                    }
                  ],
                  'subject': '"+subject+@"'
                }
              ],
              'from': {
                'email': 'Banu.Saladi@quantium.com.au'
              },
              'content': [
                {
                  'type': 'text/plain',
                  'value': '"+message+@"'
                }
              ]
            }";           
            Object json = JsonConvert.DeserializeObject<Object>(data);

            dynamic response =  sg.client.mail.send.post(requestBody: json.ToString());
            return Task.FromResult(0);
        }

        private static string Decrypt(string cipherText)
        {
            string EncryptionKey = "1quantium1";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
