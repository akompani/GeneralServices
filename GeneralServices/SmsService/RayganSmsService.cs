using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GeneralServices.SmsService
{
    public class RayganSmsService : ISmsService
    {
        private const string FooterText = "mrmetror.ir";

        private readonly HttpClient _client;
        private readonly SmsSettingModel _smsSetting;

        public RayganSmsService(SmsSettingModel smsSetting)
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri("https://raygansms.com")
            };

            _smsSetting = smsSetting;
        }

        public async Task<bool> SendCode( string phoneNo)
        {
            string urlQueryStringParams = string.Format("AutoSendCode.ashx?UserName={0}&Password={1}&Mobile={2}&Footer={3}",
                _smsSetting.Username,_smsSetting.Password, phoneNo,FooterText);

            using (HttpResponseMessage res = await _client.GetAsync(urlQueryStringParams))
            {
                using (HttpContent content = res.Content)
                {
                    var data = await content.ReadAsStringAsync();
                    if (data != null)
                    {
                        if (int.Parse(data) > 2000)
                        {
                            return true;
                        }
                        else
                        {
                            throw new Exception("خطا در ارسال پیامک احراز هویت");
                        }
                    }
                }
            }

            return false;
        }

        public async Task<bool> CheckCode(string phoneNo, string code)
        {
            string urlQueryStringParams = string.Format("CheckSendCode.ashx?UserName={0}&Password={1}&Mobile={2}&Code={3}",
                _smsSetting.Username, _smsSetting.Password, phoneNo, code);

            using (HttpResponseMessage res = await _client.GetAsync(urlQueryStringParams))
            {
                using (HttpContent content = res.Content)
                {
                    var data = await content.ReadAsStringAsync();
                    if (data != "True")
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception("کد احراز هویت مطابقت ندارد");
                    }
                }
            }

            //return false;
        }

        public async Task<RestResponse> SendMessage(string[] phoneNo, string message)
        {
            return new RestResponse();
        }
    }
}
