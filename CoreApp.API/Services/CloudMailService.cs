using System.Diagnostics;

namespace CoreApp.API.Services
{
    public class CloudMailService : IMailService
    {
        private string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        public string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

        public void Send(string subject, string message)
        {
            Debug.WriteLine ($"Mail from {_mailFrom} to {_mailTo}, with local Cloud service.");
            Debug.WriteLine ($"Subject: {subject}");
            Debug.WriteLine ($"Message: {message}");
        }
    }
}