using System;
using System.Collections.Generic;
using System.Text;

namespace Scaffold.Domain.Settings
{
    public class MailSettings
    {
        public string EmailFrom { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string DisplayName { get; set; }
        public string DataEmail { get; set; }
        public string DataSubject { get; set; }
        public string TechnicalEmail { get; set; }
        
        public string TechnicalSubject { get; set; }
    }
    public class EmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FromName { get; set; }
    }
}
