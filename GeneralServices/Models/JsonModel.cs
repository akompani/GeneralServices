
namespace GeneralServices.Models
{
    public class JsonModel
    {
        public JsonModel(string title, bool addMode, string status = "success", string otherMessage = null, object returnValue = null)
        {
            Status = status;
            Title = title;
            AddMode = addMode;
            OtherMessage = otherMessage ?? "";
            ReturnValue = returnValue;
        }

        public JsonModel(object obj, string status = "success", string otherMessage = null, object returnValue = null)
        {
            Status = status;
            Title = obj.GetType().Name;

            var id = obj.GetType().GetProperty("Id").GetValue(obj, null);

            if (id.GetType() == typeof(int)) AddMode = (int)id == 0;
            if (id.GetType() == typeof(long)) AddMode = (long)id == 0;

            OtherMessage = otherMessage ?? "";
            ReturnValue = returnValue;
        }

        public string Status { get; set; }

        public string Title { get; set; }

        public bool AddMode { get; set; }

        public string Message
        {
            get { return string.Format("{0} با موفقیت {1} شد.", Title, AddMode ? "ایجاد" : "ویرایش"); }
        }

        public string OtherMessage { get; set; }

        public object ReturnValue { get; set; }

    }
}
