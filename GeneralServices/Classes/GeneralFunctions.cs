using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Serialization;
using GeneralServices.Calendars;
using GeneralServices.Models;
using MD.PersianDateTime;
using Newtonsoft.Json;

namespace GeneralService.Classes
{
    public static class GeneralFunctions
    {
        public static string StoryName(int floorNo)
        {
            if (floorNo == 0)
            {
                return "طبقه همکف";
            }
            else if (floorNo == 1)
            {
                return "طبقه اول";
            }
            else if (floorNo > 1)
            {
                return "طبقه " + ConvertNumberToString(floorNo);
            }
            else
            {
                return "طبقه منفی " + ConvertNumberToString(Math.Abs(floorNo));
            }
        }

        public static string DeleteItemFromStringList(this string stringList, string item)
        {
            var arr = JsonConvert.DeserializeObject<List<string>>(stringList);

            var index = arr.FindIndex(a => a == item);

            if (index != -1)
            {
                arr.RemoveAt(index);
            }

            return JsonConvert.SerializeObject(arr);
        }

        public static string AddItemToStringList(this string stringList, string item)
        {
            var arr = JsonConvert.DeserializeObject<List<string>>(stringList);

            if (arr.All(a => a != item))
            {
                arr.Add(item);
            }

            return JsonConvert.SerializeObject(arr);
        }

        public static string ConvertNumberToString(int Number)
        {
            if (Number == 0 | Number > 999) return "";

            string result = "";
            string numStr = Number.ToString();

            string VA = " و ";

            if (Number >= 100)
            {
                int sIndex = Convert.ToInt32(numStr.Substring(numStr.Length - 3, 1));
                string[] MySTR = new string[10];
                MySTR[1] = "یکصد";
                MySTR[2] = "دویست";
                MySTR[3] = "سیصد";
                MySTR[4] = "چهارصد";
                MySTR[5] = "پانصد";
                MySTR[6] = "ششصد";
                MySTR[7] = "هفتصد";
                MySTR[8] = "هشتصد";
                MySTR[9] = "نهصد";
                result += MySTR[sIndex];
            }

            Number %= 100;
            numStr = Number.ToString();

            if (Number == 10 | Number >= 20)
            {
                int d2Index = Convert.ToInt32(numStr.Substring(numStr.Length - 2, 1));
                string[] MySTR = new string[10];
                MySTR[1] = "ده";
                MySTR[2] = "بیست";
                MySTR[3] = "سی";
                MySTR[4] = "چهل";
                MySTR[5] = "پنجاه";
                MySTR[6] = "شصت";
                MySTR[7] = "هفتاد";
                MySTR[8] = "هشتاد";
                MySTR[9] = "نود";
                result += (result.Length > 0 ? VA : "") + MySTR[d2Index];

                Number %= 10;
                numStr = Number.ToString();
            }

            if (Number < 20 & Number != 10 & Number > 0)
            {
                bool IsDah = (Number > 10 & Number < 20);

                int dIndex = Convert.ToInt32(numStr.Substring(numStr.Length - 1, 1));
                string[] MySTR = new string[10];
                MySTR[1] = IsDah ? "یازده" : "یک";
                MySTR[2] = IsDah ? "دوازده" : "دو";
                MySTR[3] = IsDah ? "سیزده" : "سه";
                MySTR[4] = IsDah ? "چهارده" : "چهار";
                MySTR[5] = IsDah ? "پانزده" : "پنج";
                MySTR[6] = IsDah ? "شانزده" : "شش";
                MySTR[7] = IsDah ? "هفده" : "هفت";
                MySTR[8] = IsDah ? "هجده" : "هشت";
                MySTR[9] = IsDah ? "نوزده" : "نه";
                result += (result.Length > 0 ? VA : "") + MySTR[dIndex];
            }

            return result;
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static long GetTimeStamp(this DateTime dt, DateTimeKind dtk)
        {
            Int64 unixTimestamp = (Int64)(dt.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, dtk))).TotalSeconds;
            return unixTimestamp;
        }

        public static string GetPropertyNameFormParams(params string[] propertyNames)
        {
            string result = "";

            foreach (var propertyName in propertyNames)
            {
                if (result.Length > 0 & propertyName.Length > 0) result += "_";

                result += propertyName;
            }

            return result;
        }

        public static T Clone<T>(this object entity) where T : new()
        {
            var entityProperties = TypeDescriptor.GetProperties(entity).Cast<PropertyDescriptor>();

            var clone = new T();

            foreach (var entityProperty in entityProperties)
            {
                entityProperty.SetValue(clone, entityProperty.GetValue(entity));
            }

            return clone;
        }

        public static TConvert ConvertTo<TConvert>(this object entity) where TConvert : new()
        {
            var convertProperties = TypeDescriptor.GetProperties(typeof(TConvert)).Cast<PropertyDescriptor>();
            var entityProperties = TypeDescriptor.GetProperties(entity).Cast<PropertyDescriptor>();

            var convert = new TConvert();

            foreach (var entityProperty in entityProperties)
            {
                var property = entityProperty;
                var convertProperty = convertProperties.FirstOrDefault(prop => prop.Name == property.Name);
                if (convertProperty != null)
                {
                    convertProperty.SetValue(convert, Convert.ChangeType(entityProperty.GetValue(entity), convertProperty.PropertyType));
                }
            }

            return convert;
        }

        public static object CopyFrom(this object destObject, object sourceObject)
        {
            var properties = sourceObject.GetType().GetProperties();

            foreach (var entityProperty in properties)
            {
                if (entityProperty.Name.ToUpper() != "ID")
                {
                    var destProp = destObject.GetType().GetProperty(entityProperty.Name);
                    if (destProp != null)
                    {
                        if (destProp.SetMethod != null)
                            destProp.SetValue(destObject, entityProperty.GetValue(sourceObject));
                    }
                }
            }

            return destObject;
        }

        public static void Convert3Quantity(this object obj, byte entryMode, double factor2, double factor3)
        {
            var quaProp = obj.GetType().GetProperty("Quantity");
            var qua2Prop = obj.GetType().GetProperty("Quantity2");
            var qua3Prop = obj.GetType().GetProperty("Quantity3");

            var qua = Convert.ToDouble(quaProp.GetValue(obj));

            switch (entryMode)
            {
                case 1:
                    qua2Prop.SetValue(obj, Math.Round(qua * factor2, 2));
                    qua3Prop.SetValue(obj, Math.Round(qua * factor3, 2));

                    break;

                case 2:
                    quaProp.SetValue(obj, factor2 != 0 ? Math.Round(qua / factor2, 2) : qua);
                    qua2Prop.SetValue(obj, qua);
                    qua3Prop.SetValue(obj, Math.Round(Convert.ToDouble(quaProp.GetValue(obj)) * factor3, 2));

                    break;

                case 3:
                    quaProp.SetValue(obj, factor3 != 0 ? Math.Round(qua / factor3, 2) : qua);
                    qua3Prop.SetValue(obj, qua);
                    qua2Prop.SetValue(obj, Math.Round(Convert.ToDouble(quaProp.GetValue(obj)) * factor2, 2));

                    break;
            }
        }
        public static string Tag3Quantity(this object obj, string desc1, string desc2, string desc3)
        {
            var quaProp = obj.GetType().GetProperty("Quantity");
            var qua2Prop = obj.GetType().GetProperty("Quantity2");
            var qua3Prop = obj.GetType().GetProperty("Quantity3");

            string result = "";

            result += $"<p class='mt-1 mb-1'>{quaProp.GetStringFromDoubleProperty(obj)} [ {desc1} ]</p>";

            if (!String.IsNullOrEmpty(desc2)) result += $"<p class='mt-1 mb-1'>{qua2Prop.GetStringFromDoubleProperty(obj)} [ {desc2} ]</p>";
            if (!String.IsNullOrEmpty(desc3)) result += $"<p class='mt-1 mb-1'>{qua3Prop.GetStringFromDoubleProperty(obj)} [ {desc3} ]</p>";

            return result.Replace("'", '"'.ToString());
        }

        public static string GetHtmlOfDifferentOfValues(IEnumerable<object> objects, string valueProperty, string nameProperty)
        {
            string result = "";

            if (objects.Count() > 0)
            {
                var items = objects.ToList();

                var valueProp = items[0].GetType().GetProperty(valueProperty);
                var nameProp = items[0].GetType().GetProperty(nameProperty);

                string tagString = $"{valueProp.GetValue(items[0])} [ {nameProp.GetValue(items[0])} ]";

                if (items.Count > 1)
                {
                    bool allSame = true;

                    for (int i = 0; i < items.Count - 1; i++)
                    {
                        if (!Equals(valueProp.GetValue(items[i]), valueProp.GetValue(items[i + 1])))
                        {
                            allSame = false;
                            break;
                        }
                    }

                    if (!allSame)
                    {

                        for (int i = 0; i < items.Count - 1; i++)
                        {
                            tagString = $"{valueProp.GetValue(items[i])} [ {nameProp.GetValue(items[i])} ]";

                            if (Equals(valueProp.GetValue(items[i]), valueProp.GetValue(items[i + 1])))
                            {
                                result += $"<p class='mt-0 mb-0'>{tagString}</p>";
                            }
                            else
                            {
                                result +=
                                    $"<p class='mt-0 mb-0' style='text-decoration-line: line-through;' >{tagString}</p>";
                            }
                        }

                        tagString = $"{valueProp.GetValue(items[items.Count - 1])} [ {nameProp.GetValue(items[items.Count - 1])} ]";

                        result += $"<p class='mt-0 mb-0'>{(Equals(valueProp.GetValue(items[items.Count - 1]), valueProp.GetValue(items[items.Count - 2])) ? "<i class='fa fa-check'></i>" : "")}{tagString}</p>";
                    }
                    else
                    {
                        return $"<p class='mt-0 mb-0'><i class='fa fa-check'></i>{tagString}</p>";
                    }

                }
                else
                {
                    result = $"<p class='mt-0 mb-0'>{tagString}</p>";
                }
            }

            return result.Replace(Char.ConvertFromUtf32(39), Char.ConvertFromUtf32(34));//.Replace(", '"');
        }

        public static string GetHtmlOfDifferentOfValues(List<string> values, List<string> names)
        {
            if (values.Count == 0 | values.Count != names.Count) return "";

            string result = "";

            string tagString = $"{values[0]} [ {names[0]} ]";

            if (values.Count > 1)
            {
                bool allSame = true;

                for (int i = 0; i < values.Count - 1; i++)
                {
                    if (!Equals(values[i], values[i + 1]))
                    {
                        allSame = false;
                        break;
                    }
                }

                if (!allSame)
                {

                    for (int i = 0; i < values.Count - 1; i++)
                    {
                        tagString = $"{values[i]} [ {names[i]} ]";

                        if (Equals(values[i], values[i + 1]))
                        {
                            result += $"<p class='mt-0 mb-0'>{tagString}</p>";
                        }
                        else
                        {
                            result +=
                                $"<p class='mt-0 mb-0' style='text-decoration-line: line-through;' >{tagString}</p>";
                        }
                    }

                    tagString = $"{values[values.Count - 1]} [ {names[names.Count - 1]} ]";

                    result += $"<p class='mt-0 mb-0'>{(Equals(values[values.Count - 1], values[values.Count - 2]) ? "<i class='fa fa-check'></i>" : "")}{tagString}</p>";
                }
                else
                {
                    return $"<p class='mt-0 mb-0'><i class='fa fa-check'></i>{tagString}</p>";
                }

            }
            else
            {
                result = $"<p class='mt-0 mb-0'>{tagString}</p>";
            }



            return result.Replace(Char.ConvertFromUtf32(39), Char.ConvertFromUtf32(34));//.Replace(", '"');
        }


        public static string GetStringFromDoubleProperty(this PropertyInfo prop, object obj, string format = "#,##0.###")
        {
            string f = "{0:" + format + "}";

            return string.Format(f, Convert.ToDouble(prop.GetValue(obj)));
        }

        public static string ConvertStringListToString(this List<string> list, string seperator = ",")
        {
            string result = "";

            foreach (var o in list)
            {
                if (result.Length > 0) result += ",";

                result += o;
            }

            return result;
        }

        public static List<byte> GetByteListFromString(this string input)
        {
            var listStr = input.Split(",").ToList();

            var listByte = new List<byte>();

            foreach (var str in listStr)
            {
                listByte.Add(byte.Parse(str));
            }

            return listByte;
        }

        public static List<short> GetShortListFromString(this string input)
        {
            var listStr = input.Split(",").ToList();

            var listShort = new List<short>();

            foreach (var str in listStr)
            {
                listShort.Add(short.Parse(str));
            }

            return listShort;
        }

        public static List<int> GetIntegerListFromString(this string input)
        {
            var listStr = input.Split(",").ToList();

            var listInt = new List<int>();

            foreach (var str in listStr)
            {
                listInt.Add(int.Parse(str));
            }

            return listInt;
        }

        public static string FactorsStr(params object[] factors)
        {
            string result = "";

            foreach (var factor in factors)
            {
                var cStr = factor?.ToString() ?? "1";

                if (cStr != "1")
                {
                    if (String.IsNullOrEmpty(result))
                    {
                        result = cStr;
                    }
                    else
                    {
                        result += $" * {cStr}";
                    }
                }
            }

            if (String.IsNullOrEmpty(result))
            {
                result = "1";
            }

            return result;
        }

        public static string CompareClasses<T>(T updated, string lastUpdate = null)
        {
            T orginial;

            if (lastUpdate == null)
            {
                orginial = (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                orginial = JsonConvert.DeserializeObject<T>(lastUpdate);
            }

            return CompareClasses(orginial, updated);
        }

        public static string CompareClasses<T>(T original, T updated)
        {
            if (original == null || updated == null)
                throw new ArgumentNullException("Both objects must be non-null.");

            if (original.GetType() != updated.GetType())
                throw new ArgumentException("Objects must be of the same type.");

            var changes = new Dictionary<string, (object OriginalValue, object UpdatedValue)>();

            // Get all properties of the type
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!property.CanRead) continue; // Skip properties that cannot be read

                var originalValue = property.GetValue(original);
                var updatedValue = property.GetValue(updated);

                // Compare values
                if (!Equals(originalValue, updatedValue))
                {
                    changes[property.Name] = (originalValue, updatedValue);
                }
            }

            return JsonConvert.SerializeObject(changes);
        }
        
    }


}
