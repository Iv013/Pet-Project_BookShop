using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace MyBookShop_Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration; // Получаем доступ к файлу конфигурации
        public MailJetSettings _mailJetSettings { get; set; }   //создали клас с ключами для почты


        public EmailSender( IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);



        }

        public async Task Execute(string email, string subject, string body)
        {

            _mailJetSettings = _configuration.GetSection("MailJet").Get<MailJetSettings>();  //получаем ключи почты из файла конфигурации
          //  MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable("c2d979e0ae0fe615fceb51faccc51767"), Environment.GetEnvironmentVariable("a13fc35977d1d71b41d46c5c5816a02f"))

          MailjetClient client = new MailjetClient(_mailJetSettings.ApiKey, _mailJetSettings.SecretKey)
            {
               Version = ApiVersion.V3_1,
              BaseAdress = "https://api.mailjet.com/",
                //  BaseAdress = "https://api.us.mailjet.com"

            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",

       new JObject {
        {"Email", "iamolokov@gmail.com"},
        {"Name", "Ivan"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "DotNetMastery"
         }
        }
       }
      }, {
       "Subject",
       subject
      }, {
       "HTMLPart",
       body
      }
     }
             });
            MailjetResponse response = await client.PostAsync(request);

        }
    }
}
