using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MD.PersianDateTime;

namespace GeneralServices.FileServices
{
    public static class ExcelImportService
    {

        public async static Task<DataTable> GetDataTable(string path, string sheetName = null)
        {
            try
            {
                string extension = Path.GetExtension(path).ToLower();

                //activities and wbses ------------------------------------------------------
                if (extension == ".csv" | String.IsNullOrEmpty(sheetName))
                {
                    return ConvertCSVtoDataTable(path);
                }
                else
                {
                    return await ConvertXSLXtoDataTable(path, sheetName);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }


            return dt;
        }

        private async static Task<DataTable> ConvertXSLXtoDataTable(string strFilePath, string sheetName)
        {
            string extension = System.IO.Path.GetExtension(strFilePath).ToLower();
            string connString = "";

            if (extension.Trim() == ".xls")
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (extension.Trim() == ".xlsx")
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }

            var oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
                await oledbConn.OpenAsync();

                OleDbCommand cmd = new OleDbCommand($"SELECT * FROM [{sheetName}$]", oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                DataSet ds = new DataSet();
                oleda.Fill(ds);

                dt = ds.Tables[0];

                await oledbConn.CloseAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {

                oledbConn.Close();
            }

            return dt;

        }

        public static string GetStringFromDataTable(this DataTable dt, int rowIndex, string columnName)
        {
            return dt.Rows[rowIndex][columnName].ToString();
        }

        public static bool GetBooleanFromDataTable(this DataTable dt, int rowIndex, string columnName)
        {
            var booleanValue = dt.Rows[rowIndex][columnName].ToString();

            return (booleanValue.ToUpper() == "TRUE");
        }

        public static string GetStringDateTimeFromDataTable(this DataTable dt, int rowIndex, string columnName)
        {
            var str = dt.Rows[rowIndex][columnName].ToString();

            if (!str.Contains("/"))
            {
                if (long.TryParse(str, out long numDate))
                {
                    var year = (int)(numDate / 10000);
                    var mon = (int)((numDate - year * 10000) / 100);
                    var day = numDate - year * 10000 - mon * 100;
                    str = $"{year:0000}/{mon:00}/{day:00}";
                }
                else
                {
                    return null;
                }
            }

            if (DateTime.TryParse(str, out DateTime ps))
            {
                var strDate = ps.ToString("yyyy/MM/dd");

                if (!PersianDateTime.IsChristianDate(strDate))
                {
                    return strDate;
                }
                else
                {
                    return new PersianDateTime(ps).ToShortDateString();
                }
            }

            return null;
        }

        public static double GetDoubleFromDataTable(this DataTable dt, int rowIndex, string columnName)
        {
            try
            {
                var str = dt.Rows[rowIndex][columnName];
                return Convert.ToDouble(str);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static decimal GetDecimalFromDataTable(this DataTable dt, int rowIndex, string columnName)
        {
            try
            {
                var str = dt.Rows[rowIndex][columnName];
                return Convert.ToDecimal(str);
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public static long GetLongFromDataTable(this DataTable dt, int rowIndex, string columnName)
        {
            try
            {
                var str = dt.Rows[rowIndex][columnName];
                return Convert.ToInt64(str);
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public static byte GetByteFromDataTable(this DataTable dt, int rowIndex, string columnName)
        {
            try
            {
                var str = dt.Rows[rowIndex][columnName];
                return Convert.ToByte(str);
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public static int GetIntegerFromDataTable(this DataTable dt, int rowIndex, string columnName)
        {
            try
            {
                var str = dt.Rows[rowIndex][columnName];
                return Convert.ToInt32(str);
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public static ushort GetUShortFromDataTable(this DataTable dt, int rowIndex, string columnName)
        {
            try
            {
                var str = dt.Rows[rowIndex][columnName];
                return Convert.ToUInt16(str);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static object GetDynamicTypeFromDataTable(this object destination, DataTable dt, int rowIndex, string columnName, string propName = null)
        {
            if (propName == null) propName = columnName;

            var prop = destination.GetType().GetProperty(propName);

            if (prop != null)
            {
                var pt = prop.PropertyType;

                object val = null;

                if (pt == typeof(string))
                {
                    if (propName.ToUpper().EndsWith("DATE"))
                    {
                        val = GetStringDateTimeFromDataTable(dt, rowIndex, columnName);
                    }
                    else
                    {
                        val = GetStringFromDataTable(dt, rowIndex, columnName);
                    }
                }
                else if (pt == typeof(double))
                {
                    val = GetDoubleFromDataTable(dt, rowIndex, columnName);
                }
                else if (pt == typeof(decimal))
                {
                    val = GetDecimalFromDataTable(dt, rowIndex, columnName);
                }
                else if (pt == typeof(int))
                {
                    val = GetIntegerFromDataTable(dt, rowIndex, columnName);
                }
                else if (pt == typeof(long))
                {
                    val = GetLongFromDataTable(dt, rowIndex, columnName);
                }

                if (val != null)
                {
                    prop.SetValue(destination, val);
                }
            }

            return destination;
        }


    }
}

