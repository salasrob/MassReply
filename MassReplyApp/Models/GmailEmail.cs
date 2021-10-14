using System;
using System.Collections.Generic;

namespace MassReplyApp.Models
{
    public class GmailEmail
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string MailDateTime { get; set; }
        public List<string> Attachments { get; set; }
        public string MessageId { get; set; }
    }
}
