namespace GeneralServices.Models
{
    public static class ButtonGeneralClass
    {
        public static string TypicalButtonClassName(this ButtonIconClassesTypes buttonIconClassesTypes)
        {
            switch (buttonIconClassesTypes)
            {
                case ButtonIconClassesTypes.Add:
                    return "typcn typcn-document-add";

                case ButtonIconClassesTypes.Remove:
                    return "typcn typcn-trash";

                case ButtonIconClassesTypes.Import:
                    return "las la-file-import";

                case ButtonIconClassesTypes.Export:
                    return "las la-file-export";

                case ButtonIconClassesTypes.Refresh:
                    return "typcn typcn-refresh";

                default:
                    return "";
            }
        }
    }

    public enum ButtonModalSizes : byte
    {
        None = 0,
        ModalSmall = 1,
        ModalMedium = 2,
        ModalLarge = 3
    }

    public enum ButtonIconClassesTypes : byte
    {
        Add = 0,
        Remove = 1,
        Import = 2,
        Export = 3,
        Refresh = 4
    }

    public enum ButtonActionModes : byte
    {
        ShowModal = 0,
        PostAction = 1,
        RedirectUrl = 2
    }

    public class ButtonModel
    {
        public ButtonModel(string title, string cssClass, ButtonIconClassesTypes iconClass, string url, ButtonModalSizes modalSize = ButtonModalSizes.ModalMedium, bool justIcon = true, ButtonActionModes buttonActionMode = ButtonActionModes.ShowModal,string id = null)
        {
            Title = title;
            CssClass = cssClass;
            IconClass = iconClass.TypicalButtonClassName();
            ModalSize = modalSize;
            JustIcon = justIcon;
            ButtonAction = buttonActionMode;
            Url = url;
            Id = id;
        }

        public ButtonModel(string title, string cssClass, string iconClass, string url, ButtonModalSizes modalSize = ButtonModalSizes.ModalMedium, bool justIcon = true, ButtonActionModes buttonActionMode = ButtonActionModes.ShowModal,string id=null)
        {
            Title = title;
            CssClass = cssClass;
            IconClass = iconClass;
            ModalSize = modalSize;
            JustIcon = justIcon;
            Url = url;
            ButtonAction = buttonActionMode;
            Id = id;
        }

        public string Id { get; set; }

        public string Title { get; set; }
        public string CssClass { get; set; }
        public string IconClass { get; set; }
        public ButtonModalSizes ModalSize { get; set; }
        public bool JustIcon { get; set; }
        public ButtonActionModes ButtonAction { get; set; }

        public string Url { get; set; }

        public string GetModalSizeString()
        {
            switch (ModalSize)
            {
                case ButtonModalSizes.ModalSmall:
                    return "btnModal modalSmall";

                case ButtonModalSizes.ModalMedium:
                    return "btnModal";

                case ButtonModalSizes.ModalLarge:
                    return "btnModal modalLarge";

                default:
                    return "";

            }
        }
    }
}
