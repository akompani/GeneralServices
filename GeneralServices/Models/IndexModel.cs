using System.Collections.Generic;

namespace GeneralServices.Models
{
   

    public class IndexModel
    {
        public IndexModel(string title
            , string returnUrl = null
            , object refId = null
            , string refName = null
            , List<string> headerSectionNames = null
            , List<ButtonModel> buttons = null
            , bool showAddButton = true
            , ButtonModalSizes modalAddButton = ButtonModalSizes.None
            , string groupListName = null
            , object groupValue = null
            , bool groupAllOptionShow = false
            , string groupGetListUrl = null
            , string groupAddButtonUrl = null
            , string groupDeleteButtonUrl = null)
        {
            ShowAddButton = showAddButton;
            ModalAddButton = modalAddButton;
            ReturnUrl = returnUrl;
            Title = title;
            RefId = refId;
            RefName = refName;
            GroupListName = groupListName;
            GroupValue = groupValue;
            GroupGetListUrl = groupGetListUrl;
            GroupAllOptionShow = groupAllOptionShow;
            HeaderSectionNames = headerSectionNames == null ? new List<string>() : headerSectionNames;
            Buttons = buttons == null ? new List<ButtonModel>() : buttons;
            GroupAddButtonUrl = groupAddButtonUrl;
            GroupDeleteButtonUrl = groupDeleteButtonUrl;
        }

        public List<string> HeaderSectionNames { get; set; }
        public bool ShowAddButton { get; set; } = true;
        public ButtonModalSizes ModalAddButton { get; set; } = ButtonModalSizes.None;
        public List<ButtonModel> Buttons { get; set; }
        public string ReturnUrl { get; set; }


        public string Title { get; set; }
        public object RefId { get; set; }
        public string RefName { get; set; }
        public string GroupListName { get; set; }
        public object GroupValue { get; set; }
        public bool GroupAllOptionShow { get; set; }

        public string GroupGetListUrl { get; set; }
        public string GroupAddButtonUrl { get; set; }
        public string GroupDeleteButtonUrl { get; set; }
    }
}
