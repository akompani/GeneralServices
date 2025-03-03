using System;
using System.Threading.Tasks;
using System.Timers;
using GeneralServices.Models;
using Microsoft.Extensions.Options;

namespace GeneralServices.SmsService
{
    public class SmsCenter
    {
        private readonly ISmsService _smsService;
        private readonly SmsSettingModel _smsSetting;

        public SmsCenter(IOptions<SmsSettingModel> smsOptions)
        {
            _smsSetting = smsOptions.Value;
           
            switch (_smsSetting.ServiceName.ToUpper())
            {
                case "FARAPAYAMAK":
                    _smsService = new FaraPaymakService(_smsSetting);
                    break;

                case "RAYGANSMS":
                    _smsService = new RayganSmsService(_smsSetting);
                    break;

                default: throw new ArgumentException("Error in service name");
            }


        }


        public async Task<bool> SendCode(string phoneNo)
        {
            return await _smsService.SendCode( phoneNo);
        }

        public async Task<bool> CheckCode(string phoneNo, string code)
        {
            return await _smsService.CheckCode(phoneNo, code);
        }

        public async Task<GeneralServiceResponse> SendMessage(string phoneNo, string message)
        {
            return await SendMessage(new string[] { phoneNo }, message);
        }

        public async Task<GeneralServiceResponse> SendMessage(string[] phoneNo, string message)
        {
            if (_smsSetting.Notice)
            {
                var response= await _smsService.SendMessage(phoneNo, message);

                if (!response.IsSuccessful) return new GeneralServiceResponse(GeneralResponseStatus.Fail,response.ErrorMessage);
                
            }

            return new GeneralServiceResponse();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                e.SignalTime);
        }
    }
}
