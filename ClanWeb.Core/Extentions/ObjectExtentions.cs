using System.Web.Script.Serialization;

namespace System
{
    public static class ObjectExtentions
    {


        /// <summary>
        /// Creates a json from a object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            JavaScriptSerializer jsonConverter = new JavaScriptSerializer();
            return jsonConverter.Serialize(obj);
        }

        /// <summary>
        /// Creates a json string object from a object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj)
        {
            JavaScriptSerializer jsonConverter = new JavaScriptSerializer();
            return jsonConverter.Serialize(obj);
        }


        /// <summary>
        /// Creates a object from a string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string str)
        {
            JavaScriptSerializer jsonConverter = new JavaScriptSerializer();
            return jsonConverter.Deserialize<T>(str);
        }


        /// <summary>
        /// Creates a object from a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object FromJson(this string str)
        {
            JavaScriptSerializer jsonConverter = new JavaScriptSerializer();
            return jsonConverter.Deserialize<object>(str);
        }


    }
}
