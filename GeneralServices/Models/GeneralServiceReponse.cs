using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralServices.Models
{
    public enum GeneralResponseStatus : byte
    {
        Success = 0,
        Fail = 1,
        NotFound = 2
    }

    public class GeneralServiceResponse
    {
        public GeneralServiceResponse(GeneralResponseStatus status = GeneralResponseStatus.Success, string message = null, object returnValue = null)
        {
            Errors = new List<string>();

            switch (status)
            {
                case GeneralResponseStatus.Success:
                    Status = "success";
                    Message = message ?? "عملیات با موفقیت انجام شد";
                    break;

                case GeneralResponseStatus.Fail:
                    Status = "fail";
                    Errors.Add(message ?? "خطا در عملیات");
                    break;

                case GeneralResponseStatus.NotFound:
                    Status = "notfound";
                    Errors.Add("اطلاعات مورد نظر وجود ندارد");
                    break;

            }

            if (returnValue != null) ReturnValue = returnValue;
        }
        public GeneralServiceResponse(List<string> errors, object returnValue = null)
        {
            Errors = errors;
            Status = "fail";

            if (returnValue != null) ReturnValue = returnValue;
        }

        public GeneralServiceResponse(Exception error, object returnValue = null)
        {
            Errors = new List<string>() { error.Message };
            Status = "fail";

            if (returnValue != null) ReturnValue = returnValue;
        }

        public string Status { get; set; }

        public string Message { get; set; }

        public List<string> Errors { get; set; }

        public string ErrorMessage
        {
            get
            {
                string errorResult = "";

                if (Errors == null) return errorResult;

                foreach (var error in Errors)
                {
                    if (errorResult.Length > 0) errorResult += " - ";
                    errorResult += error;
                }

                return errorResult;
            }
        }

        public object ReturnValue { get; set; }

        public bool IsSucceeded
        {
            get
            {
                return Status == "success";
            }
        }
        public bool Failed
        {
            get
            {
                return Status != "success";
            }
        }

        public bool NotFound
        {
            get
            {
                return Status == "notfound";
            }
        }

    }
}
