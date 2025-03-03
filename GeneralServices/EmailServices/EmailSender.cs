using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using GeneralServices.Models;
using Microsoft.Extensions.Options;

namespace GeneralServices.EmailServices
{
    public class EmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<GeneralServiceResponse> SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false)
        {
            try
            {

                using (var client = new SmtpClient()
                       {
                           Host = _emailSettings.MailServer,
                           Port = _emailSettings.MailPort,
                           EnableSsl = _emailSettings.EnableSSL
                       }
                      )
                {

                    var credential = new NetworkCredential()
                    {
                        UserName = _emailSettings.Sender,
                        Password = _emailSettings.Password
                    };

                    client.Credentials = credential;
                    //client.Timeout = 10000;

                    var emailMessage = new MailMessage()
                    {
                        To = { new MailAddress(toEmail) },
                        From = new MailAddress(_emailSettings.Sender),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = isMessageHtml

                    };

                    try
                    {
                        await client.SendMailAsync(emailMessage);
                        return new GeneralServiceResponse();
                    }
                    catch (Exception e)
                    {
                        return new GeneralServiceResponse(GeneralResponseStatus.Fail, e.Message);
                    }

                }
            }
            catch (Exception e)
            {

                return new GeneralServiceResponse(GeneralResponseStatus.Fail, e.Message);
            }



        } 


    }
}
