using Quantium.Recruitment.Portal.Server.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreSpa.Server.Services.Abstract
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(MailType type, EmailModel emailModel, string extraData);
        bool SendEmail(EmailModel emailModel);
        Task SendEmailAsync(EmailModel model);
        //Task SendBatchEmailAsync(IList<UserCreationModel> userModels, EmailModel emailModel);
    }
}