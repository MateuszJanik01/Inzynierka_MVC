using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fences.Model.DataModels
{
    public class SmtpSettings
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
