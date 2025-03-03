namespace GeneralServices.Models
{
    public class ModalInfo
    {
        public ModalInfo(object obj, string submitId = "submitData", string nextId = "submitNext"
             , string cancelId = "btn-cancel", bool onlyCancel = false, bool showCloseIcon = true
             , string submitAction = null, string title = null, string submitText = null, string alertText = null, string alertType = "danger", string submitClass = "success")
        {
            if (obj != null)
            {
                var id = obj.GetType().GetProperty("Id")?.GetValue(obj, null).ToString() ?? "0";

                SubmitText = submitText ?? (id == "0" ? "Add" : "Edit");

                Heading = SubmitText + " " + obj.GetType().Name;
            }
            else
            {
                SubmitText = submitText ?? "Confirm";
                Heading = title ?? "";
            }

            SubmitId = submitId;
            NextId = nextId;
            CancelId = cancelId;

            OnlyCancelButton = onlyCancel;
            ShowCloseIcon = showCloseIcon;
            SubmitAction = submitAction;
            SubmitClass = submitClass;

            AlertText = alertText;
            AlertType = alertType;
        }


        public readonly string Heading;

        public readonly string SubmitText;
        public readonly string SubmitAction;
        public readonly string SubmitClass;

        public readonly string SubmitId;

        public readonly string NextId;

        public readonly string CancelText = "Cancel";

        public readonly string CancelId;

        public readonly bool OnlyCancelButton;

        public readonly bool ShowCloseIcon;

        public readonly string AlertText;

        public readonly string AlertType;


    }
}
