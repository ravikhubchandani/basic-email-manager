namespace BasicEmailer
{
    public class BasicEmailerSettings
    {
        public string FromAccount { get; set; }
        public string FromAccountPassword { get; set; }        
        public string SmtpServerHost { get; set; }
        public int SmtpServerPort { get; set; }
        public bool EnableSsl { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}
