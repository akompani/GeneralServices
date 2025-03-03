using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralServices.FileServices
{
    public class FileTools
    {

        public static List<List<string>> ConvertFileToList(string content)
        {
            List<List<string>> result = new List<List<string>>();

            char[] array1 = { '\n', '\r' };
            char[] array2 = { ',', ';' };

            string[] mp = content.Split(array1);
            mp = mp.Where(r => r != "").ToArray();

            List<string> mpos;

            foreach (string m in mp)
            {
                mpos = m.Split(array2).ToList();
                mpos = mpos.Where(r => r != "").ToList();

                result.Add(mpos);
            }

            return result;
        }
    }

}