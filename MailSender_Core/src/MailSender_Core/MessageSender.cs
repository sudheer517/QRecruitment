using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using SendGrid;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;

namespace Quantium.Recruitment.Portal.Helpers
{
    public class MessageSender 
    {
       
        public MessageSender()
        {

        }
      
        public async Task SendEmailAsync(string email, string subject, string templateName, Dictionary<string, string> parameters)
        {
           
            string apiKey = Decrypt("cMQ38XsoFwojn+dxM/IAJkboQR6MaFIE4DY+foFkO/osGWdGLuZzCaxyKIx03TOHWDgflQ6IFUzK3d0zxBBOyw/TsN3XNZESwU8N+8zu6xlx2nzcnp6QW0AGwJ+H9zleNCS0m2AJSbkiTnWNyAkiAevNGP6uvVOC9SeFnCI0kAFru+4JkbdZr7g0KdpN9bib");
            SendGridClient sg = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.From = new EmailAddress("QuantiumAdmin@quantium.com", "QuantiumAdmin@quantium.com");
            msg.Subject = subject;
            var output=File.ReadAllText("./"+templateName+".html");
            byte[] imageArray = File.ReadAllBytes("./qlogo.png");
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);          
            msg.AddAttachment("qlogo", base64ImageRepresentation, "image/png", "inline", "qlogo");          
            var Content = System.Net.WebUtility.HtmlDecode(output);                      
            foreach(var param in parameters)
            {

                Content= Content.Replace("<" + param.Key + ">", param.Value);
            }
                 
            msg.HtmlContent= Content;
            msg.PlainTextContent = Content;                         
            msg.AddTo(new EmailAddress(email));
            var response =await sg.SendEmailAsync(msg);         

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
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
