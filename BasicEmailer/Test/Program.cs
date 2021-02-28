using BasicEmailer;
using Microsoft.Extensions.Configuration;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = ReadSettings();
            var mailer = new Emailer(settings);            
            mailer.Subject = "Linux Tux";
            mailer.Content = "<html><head></head><body><h1>This is a test</h1></body></html>";
            mailer.AddReceiver("email@gmail.com");
            mailer.AddAttachment("tux.png");
            mailer.Send();
        }


        private static BasicEmailerSettings ReadSettings()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

            var section = Configuration.GetSection("ApplicationSettings:BasicEmailManager");

            return new BasicEmailerSettings
            {
                IsBodyHtml = Convert.ToBoolean(section.GetSection("IsBodyHtml").Value),
                EnableSsl = Convert.ToBoolean(section.GetSection("EnableSsl").Value),
                FromAccount = section.GetSection("FromAccount").Value,
                FromAccountPassword = section.GetSection("FromAccountPassword").Value,
                SmtpServerHost = section.GetSection("SmtpServerHost").Value,
                SmtpServerPort = Convert.ToInt32(section.GetSection("SmtpServerPort").Value)
        };
    }
    }
}
