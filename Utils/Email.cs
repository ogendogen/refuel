using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class Email
    {
        public string SmtpAddress { get; set; }
        public int Port { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Password { get; set; }
    }
}
