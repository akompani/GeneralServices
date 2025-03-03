using System.Threading.Tasks;
using RestSharp;

namespace GeneralServices.SmsService
{
    public interface ISmsService
    {
        Task<bool> SendCode(string phoneNo);

        Task<bool> CheckCode(string phoneNo, string code);

        Task<RestResponse> SendMessage(string[] phoneNo, string message);

    }
}
