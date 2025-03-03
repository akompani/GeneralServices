using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace GeneralServices.SmsService
{

    public class FaraPaymakService : ISmsService
    {
        private readonly string _baseAddress = "http://rest.payamak-panel.com/api/SendSMS";

        private readonly SmsSettingModel _smsSetting;

        private readonly Dictionary<string, int> _generateCodes;
        private readonly Dictionary<string, DateTime> _generateTimes;

        public FaraPaymakService(SmsSettingModel setting)
        {
            _smsSetting = setting;
            _generateCodes = new Dictionary<string, int>();
            _generateTimes = new Dictionary<string, DateTime>();
        }

        public async Task<bool> SendCode( string phoneNo)
        {
            if (_generateTimes.ContainsKey(phoneNo))
            {
                var oldTime = _generateTimes[phoneNo];

                if (oldTime > DateTime.Now) return true;

                _generateTimes.Remove(phoneNo);
                _generateCodes.Remove(phoneNo);
            }

            var code = new Random().Next(1111, 9999);

            var response = await SendMessage(phoneNo, $"پرومن {Environment.NewLine} کد اعتبار سنجی شما {code} می باشد");

            if (response.IsSuccessful)
            {
                _generateTimes.Add(phoneNo, DateTime.Now.AddMinutes(2));
                _generateCodes.Add(phoneNo, code);

                return true;
            }

            return false;
        }

        public async Task<bool> CheckCode(string phoneNo, string code)
        {
            if (!_generateCodes.ContainsKey(phoneNo)) return false;

            var time = _generateTimes[phoneNo];

            if(time < DateTime.Now) return false;

            var item = _generateCodes[phoneNo];
            
            return item == int.Parse(code);
        }

        public async Task<RestResponse> SendMessage(string phoneNo, string message)
        {
            return await SendMessage(new string[] { phoneNo }, message);
        }

        public async Task<RestResponse> SendMessage(string[] phoneNo, string message)
        {
            var client = new RestClient();
            var request = new RestRequest($"{_baseAddress}/SendSMS", Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("postman-token", "fcddb5f4-dc58-c7d5-4bf9-9748710f8789");
            request.AddHeader("cache-control", "no-cache");

            string requestValue = string.Format("username={0}&password={1}&to={2}&from={3}&text={4}&isflash=true",
                _smsSetting.Username, _smsSetting.Password, JsonConvert.SerializeObject(phoneNo), _smsSetting.ServiceNumber, message);

            request.AddParameter("application/x-www-form-urlencoded", requestValue, ParameterType.RequestBody);

            var response = client.Execute(request);

            return response;
        }
    }
}
