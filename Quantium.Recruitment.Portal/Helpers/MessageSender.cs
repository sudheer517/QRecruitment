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
           
            string apiKey = MessageSender.Decrypt("rsNyGTfOZohgNFxrmBIs0ebJKwhnOkCdXLh/6ODVd8sZNNtbE3jRH856JiPb/EyWMsfOmrIJGQOO8KYhTIm2ZrKEuQZV6+Vaj8t9Ef1rm6ZY50acX+akhc4uLs9gwquEutemXfvxc/DHI5wrEDgPeiL70tkvH2Dt9dL2vVcvmkzCI7+rubNVSsrYxAOGX7Lz");
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
            //Email to = new Email("Banu.Saladi@quantium.com.au");
            //string subject = "Hello World from the SendGrid CSharp Library!";
            //Email from = new Email("bhanu499@gmail.com");
            //Content content = new Content("text/plain", "Hello, Email!");
            //Mail mail = new Mail(from, subject, to, content);
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
