using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace GeneralServices.FileServices
{
    
    public class ExcelExportService
    {
        private Dictionary<string, IXLStyle> _styles;
        protected readonly string _listSeperator = ",";


        public enum MyStyles : byte
        {
            Default = 0,
            NormalRow = 1,
            HeaderRow = 2,
            Title = 3,
            PriceCell = 4,
            PercentageCell = 5,
            SumPriceCell = 6,
            FactorCell=7
        }

        public ExcelExportService()
        {
            CreateStyles();
        }

        private void CreateStyles()
        {
            //Styles --------------------------------------------------------------------------
            _styles = new Dictionary<string, IXLStyle>();

            //Normal Row ----------------------------------------------
            var normalRow = XLWorkbook.DefaultStyle;
            normalRow.Font.FontName = "B Mitra";
            normalRow.Font.FontCharSet = XLFontCharSet.Arabic;
            normalRow.Font.FontSize = 12;

            normalRow.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            normalRow.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            normalRow.Alignment.WrapText = true;

            normalRow.Border.BottomBorder = XLBorderStyleValues.Thin;
            normalRow.Border.TopBorder = XLBorderStyleValues.Thin;
            normalRow.Border.LeftBorder = XLBorderStyleValues.Thin;
            normalRow.Border.RightBorder = XLBorderStyleValues.Thin;
            //normalRow.Border.OutsideBorderColor=XLColor.Black;


            _styles.Add("NORMALROW", normalRow);


            //Table Header And Footer Row
            var headerRow = XLWorkbook.DefaultStyle;
            headerRow.Font.FontName = "B Mitra";
            headerRow.Font.FontCharSet = XLFontCharSet.Arabic;
            headerRow.Font.FontSize = 14;

            headerRow.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRow.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            headerRow.Alignment.WrapText = true;

            headerRow.Border.BottomBorder = XLBorderStyleValues.Thin;
            headerRow.Border.TopBorder = XLBorderStyleValues.Thin;
            headerRow.Border.LeftBorder = XLBorderStyleValues.Thin;
            headerRow.Border.RightBorder = XLBorderStyleValues.Thin;

            headerRow.Fill.PatternType = XLFillPatternValues.Solid;
            headerRow.Fill.BackgroundColor = XLColor.LightGray;

            _styles.Add("HEADERROW", headerRow);

            //Title ----------------------------------------------------------------------
            var title = XLWorkbook.DefaultStyle;
            title.Font.FontName = "B Mitra";
            title.Font.FontCharSet = XLFontCharSet.Arabic;
            title.Font.FontSize = 14;
            title.Font.Bold = true;

            title.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            title.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            _styles.Add("TITLE", title);

            //Price Cells --------------------------------------------------------------------------
            var priceCell = XLWorkbook.DefaultStyle;
            priceCell.Font.FontName = "B Mitra";
            priceCell.Font.FontCharSet = XLFontCharSet.Arabic;
            priceCell.Font.FontSize = 12;
            priceCell.NumberFormat.SetFormat("#,##0");

            priceCell.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            priceCell.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            priceCell.Alignment.WrapText = true;

            priceCell.Border.BottomBorder = XLBorderStyleValues.Thin;
            priceCell.Border.TopBorder = XLBorderStyleValues.Thin;
            priceCell.Border.LeftBorder = XLBorderStyleValues.Thin;
            priceCell.Border.RightBorder = XLBorderStyleValues.Thin;

            _styles.Add("PRICECELL", priceCell);

            //Price Cells --------------------------------------------------------------------------
            var percentageCell = XLWorkbook.DefaultStyle;
            percentageCell.Font.FontName = "B Mitra";
            percentageCell.Font.FontCharSet = XLFontCharSet.Arabic;
            percentageCell.Font.FontSize = 12;
            percentageCell.NumberFormat.SetFormat("%#0.00");

            percentageCell.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            percentageCell.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            percentageCell.Alignment.WrapText = true;

            percentageCell.Border.BottomBorder = XLBorderStyleValues.Thin;
            percentageCell.Border.TopBorder = XLBorderStyleValues.Thin;
            percentageCell.Border.LeftBorder = XLBorderStyleValues.Thin;
            percentageCell.Border.RightBorder = XLBorderStyleValues.Thin;

            _styles.Add("PERCENTAGECELL", percentageCell);

            //Sum Price Cells --------------------------------------------------------------------------
            var sumPriceCell = XLWorkbook.DefaultStyle;
            sumPriceCell.Font.FontName = "B Mitra";
            sumPriceCell.Font.FontCharSet = XLFontCharSet.Arabic;
            sumPriceCell.Font.FontSize = 14;
            sumPriceCell.Font.Bold = true;
            sumPriceCell.NumberFormat.SetFormat("#,##0");

            sumPriceCell.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sumPriceCell.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sumPriceCell.Alignment.WrapText = true;

            sumPriceCell.Border.BottomBorder = XLBorderStyleValues.Thin;
            sumPriceCell.Border.TopBorder = XLBorderStyleValues.Thin;
            sumPriceCell.Border.LeftBorder = XLBorderStyleValues.Thin;
            sumPriceCell.Border.RightBorder = XLBorderStyleValues.Thin;

            sumPriceCell.Fill.PatternType = XLFillPatternValues.Solid;
            sumPriceCell.Fill.BackgroundColor = XLColor.LightGray;

            _styles.Add("SUMPRICECELL", sumPriceCell);


            //Coef Cells --------------------------------------------------------------------------
            var coefCell = XLWorkbook.DefaultStyle;
            coefCell.Font.FontName = "B Mitra";
            coefCell.Font.FontCharSet = XLFontCharSet.Arabic;
            coefCell.Font.FontSize = 14;
            coefCell.Font.Bold = true;
            coefCell.NumberFormat.SetFormat("0.0######");

            coefCell.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            coefCell.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            coefCell.Alignment.WrapText = true;

            coefCell.Border.BottomBorder = XLBorderStyleValues.Thin;
            coefCell.Border.TopBorder = XLBorderStyleValues.Thin;
            coefCell.Border.LeftBorder = XLBorderStyleValues.Thin;
            coefCell.Border.RightBorder = XLBorderStyleValues.Thin;

            coefCell.Fill.PatternType = XLFillPatternValues.Solid;
            coefCell.Fill.BackgroundColor = XLColor.LightGray;

            _styles.Add("COEFCELL", coefCell);
        }

        public void SetColumnWidths(IXLWorksheet setSheet, List<double> colWidths)
        {
            for (int i = 0; i < colWidths.Count; i++)
            {
                setSheet.Column(i + 1).Width = colWidths[i];

                if (colWidths[i] == 0) setSheet.Column(i + 1).Hide();
            }

            setSheet.SetRightToLeft(true);
        }

        public void SetRangeStyle(IXLRange cells, string styleName)
        {
            styleName = styleName.ToUpper();
            cells.Style = _styles[styleName];
        }

        protected void SetRangeStyle(IXLRange cells, MyStyles style)
        {
            SetRangeStyle(cells, GetEnumStyleName(style));
        }

        public void SetValueAndStyle(IXLWorksheet sheet, int row, int col, int count, object value,
            string style, bool convert1ToNull = false, XLColor backgroundColor = null)
        {
            var range = sheet.Range(row, col, row, col + count - 1);
            if (convert1ToNull && value.ToString() == "1")
            {
                range.Value = "";
            }
            else
            {
                range.Value = (XLCellValue)value;
            }

            if (backgroundColor != null) range.Style.Fill.BackgroundColor = backgroundColor;

            if (count > 1) range.Merge();
            SetRangeStyle(range, style);
        }

        public void SetValueAndStyle(IXLWorksheet sheet, int row, int col, int count, object value,
                  MyStyles style = MyStyles.NormalRow, bool convert1ToNull = false, XLColor backgroundColor = null)
        {
            SetValueAndStyle(sheet, row, col, count, value,GetEnumStyleName(style), convert1ToNull, backgroundColor);
        }

        public void SetFormulaAndStyle(IXLWorksheet sheet, int row, int col, int count, string formula,
            string style)
        {
            var range = sheet.Range(row, col, row, col + count - 1);
            range.FormulaA1 = formula;
            if (count > 1) range.Merge();
            SetRangeStyle(range, style);
        }

        public void SetFormulaAndStyle(IXLWorksheet sheet, int row, int col, int count, string formula,
            MyStyles style = MyStyles.NormalRow)
        {
            SetFormulaAndStyle(sheet,row,col,count,formula,GetEnumStyleName(style));
        }

        public virtual void SetSheetFooter(IXLWorksheet sheet)
        {
            return;
        }

        private string GetEnumStyleName(MyStyles style)
        {
            return Enum.GetName(typeof(MyStyles), style)?.ToUpper() ?? "NORMALROW";
        }

        public void SetPrintRange(IXLWorksheet sheet, int row, int col)
        {
            var rng = sheet.Range(1, 1, row, col);
            sheet.PageSetup.PrintAreas.Clear();
            sheet.PageSetup.PrintAreas.Add(rng.RangeAddress.ToStringFixed());
            sheet.SheetView.View = XLSheetViewOptions.PageBreakPreview;
            sheet.SheetView.ZoomScale = 100;
            sheet.PageSetup.FitToPages(1,0);
        }

        public static string StandardSheetName(string sheetName)
        {
            var result = "";

            foreach (var c in sheetName)
            {
                if (!char.IsWhiteSpace(c) && !char.IsLetterOrDigit(c)) result += c;
            }

            return result;
        }

    }
}
