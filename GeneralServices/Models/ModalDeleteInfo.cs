namespace GeneralServices.Models
{
    public class ModalDeleteInfo
    {
        public ModalDeleteInfo(string objectName, string objectCaption,  string action, string emergencyMessage = null, string buttonId = "btnDelete", bool isSummaryObjects = false)
        {
            ObjectCaption = objectCaption;
            ObjectName = objectName;
            Action = action;
            EmergencyMessage = emergencyMessage;
            ButtonId = buttonId;
        }

        public ModalDeleteInfo(object obj, string objectCaption,  string action, string emergencyMessage = null, string buttonId = "btnDelete",bool isSummaryObjects = false)
        {
            ObjectCaption = objectCaption;
            ObjectName = obj.GetType().Name;
            Action = action;
            EmergencyMessage = emergencyMessage;
            ButtonId = buttonId;
        }

        public string ButtonId { get; set; }

        public string ObjectCaption { get; set; }

        public string ObjectName { get; set; }

        public string Action { get; set; }

        public string EmergencyMessage { get; set; }

        public bool IsSummaryObjects { get; set; }

        public object Id { get; set; }
    }
}
