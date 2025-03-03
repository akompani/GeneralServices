using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralServices.SmsService
{
    public class SmsSettingModel
    {
        public string ServiceName { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string ServiceNumber { get; set; }

        public bool Notice { get; set; }
    }
}
