using System.Collections.Generic;
using System.Linq;

namespace GeneralServices.Models
{
    public class ImportSampleColumn
    {
        public ImportSampleColumn()
        {

        }

        public ImportSampleColumn(string name, string sheetName, string examples)
        {
            Name = name;
            SheetName = sheetName;
            Examples = string.IsNullOrEmpty(examples) ? new List<string>() : examples.Split(",").ToList();
        }

        public string Name { get; set; }

        public string SheetName { get; set; }

        public List<string> Examples { get; set; }
    }


    public class ImportSampleModel
    {
        public ImportSampleModel()
        {
            DeleteOld = true;
            Columns = new List<ImportSampleColumn>();
        }

        public ImportSampleModel(string title, string sheetName, string importCheckUrl, string acceptImportUrl, string redirectUrl, bool defaultDeleteOld = true, long refId = 0)
        {
            Title = title;
            SheetName = sheetName;
            DeleteOld = defaultDeleteOld;
            Columns = new List<ImportSampleColumn>();

            ImportCheckUrl = importCheckUrl;
            AcceptImportUrl = acceptImportUrl;
            RedirectUrl = redirectUrl;
            RefId = refId;
        }

        public string Title { get; set; }
        public string SheetName { get; set; }

        public string ImportCheckUrl { get; set; }
        public string AcceptImportUrl { get; set; }
        public string RedirectUrl { get; set; }

        public bool DeleteOld { get; set; }

        public long RefId { get; set; }

        public List<ImportSampleColumn> Columns { get; set; }



    }
}
