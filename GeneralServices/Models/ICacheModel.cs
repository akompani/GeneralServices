using System.Linq;

namespace GeneralServices.Models
{
    public interface ICacheModel
    {

        public string[] DefaultCacheNames();
        //{
        //    return new[] { this.GetType().Name };
        //}


        //protected string CreateCacheName(params string[] cacheParams)
        //{
        //    string result = "";

        //    foreach (var cacheParam in cacheParams)
        //    {
        //        if (result.Length > 0) result += "-";
        //        result += cacheParam;
        //    }

        //    return result;
        //} 

        public static string CreateCacheName(params object[] cacheParams)
        {
            string result = "";

            foreach (var cacheParam in cacheParams)
            {
                if (result.Length > 0) result += "-";
                result += cacheParam.ToString();
            }

            return result;
        }

        

    }
}
