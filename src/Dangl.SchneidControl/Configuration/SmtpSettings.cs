namespace Dangl.SchneidControl.Configuration
{
    public class SmtpSettings
    {
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public bool UseTls { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public bool RequiresAuthentication { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// This is strongly recommended to be left at the default value of 'false',
        /// since it will then skip validation of Tls certificates when trying to
        /// connect to the SMTP server for sending emails.
        /// </summary>
        public bool IgnoreTlsCertificateErrors { get; set; }
    }
}
