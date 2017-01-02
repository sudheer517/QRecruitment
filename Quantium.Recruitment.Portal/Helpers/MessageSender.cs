using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using Microsoft.Extensions.Options;

namespace Quantium.Recruitment.Portal.Helpers
{
    public class MessageSender 
    {
       
        public MessageSender()
        {

        }
      
        public Task SendEmailAsync(string email, string subject, string message)
        {
            string apiKey = "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";//Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY", EnvironmentVariableTarget.User);
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

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
