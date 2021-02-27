using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace BasicEmailer
{
    public class Emailer
    {
        public BasicEmailerSettings Settings { get; private set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        private List<MailAddress> _recievers;
        private List<Attachment> _attachments { get; set; }

        public Emailer()
        {
            Settings = new BasicEmailerSettings();
            Initialize();
        }

        public Emailer(BasicEmailerSettings settings)
        {
            Settings = settings;
            Initialize();
        }

        private void Initialize()
        {
            _recievers = new List<MailAddress>();
            _attachments = new List<Attachment>();
        }

        public void ClearReceivers()
        {
            _recievers.Clear();
        }

        public void AddReceiver(params string[] email)
        {
            if (email == null || email.Length == 0)
                throw new ArgumentException("No email address specified");

            var to = email.Where(x => x != null).Select(x => new MailAddress(x));
            _recievers.AddRange(to);
        }

        public void ClearAttachments()
        {
            _attachments.Clear();
        }

        public void AddAttachment(params FileInfo[] files)
        {
            AddAttachment(files?.Select(x => x?.FullName).ToArray());
        }

        public void AddAttachment(params string[] path)
        {
            if (path == null || path.Length == 0)
                throw new ArgumentException("No files specified");

            var atts = path.Where(x => x != null).Select(x => new Attachment(x));
            _attachments.AddRange(atts);
        }

        public void Send()
        {
            var smtp = new SmtpClient
            {
                EnableSsl = Settings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Settings.FromAccount, Settings.FromAccountPassword),
                Host = Settings.SmtpServerHost,
                Port = Settings.SmtpServerPort
            };
            using (var message = new MailMessage()
            {
                From = new MailAddress(Settings.FromAccount),
                Subject = Subject,
                Body = Content,
                IsBodyHtml = Settings.IsBodyHtml
            })
            {
                _recievers.ForEach(x => message.To.Add(x));
                _attachments.ForEach(x => message.Attachments.Add(x));
                smtp.Send(message);
            }
        }
    }
}
