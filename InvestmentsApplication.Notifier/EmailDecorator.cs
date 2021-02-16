using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace InvestmentsApplication.Notifier
{
    public class EmailDecorator : Decorator
    {
        private static readonly string[] Scopes = { GmailService.Scope.MailGoogleCom };
        private readonly UserCredential credential;

        public EmailDecorator(Notifier notifier) : base(notifier)
        {
            string credPath = "token.json";
            using var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read);

            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
        }

        public override void Notify(string message)
        {
            base.Notify(message);

            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Application Name",
            });

            var mailMessage = new MailMessage();
            
            mailMessage.Subject = "subject";
            mailMessage.Body = "body";

            var mime = MimeKit.MimeMessage.CreateFromMailMessage(mailMessage);

            service.Users.Messages.Send(new Message()
            {
                Raw = Encode(mime.ToString())
            }, "me").Execute();
        }
        public static string Encode(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);

            return System.Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
}
}