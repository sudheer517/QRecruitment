namespace AspNetCoreSpa.Server.Services.Abstract
{
    public class EmailModel
    {
        public string To { get; set; }
        public string From { get; set; }
        public string DisplayName { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
    }

    public enum MailType
    {
        None,
        Register,
        SecurityCode,
        ForgetPassword
    }
}