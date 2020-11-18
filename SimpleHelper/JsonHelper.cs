using Newtonsoft.Json;

namespace SimpleHelper
{
    public static class JsonHelper
    {
        /// <summary>
        /// Will Convert a json string to object.
        /// </summary>
        /// <typeparam name="T">Type of Json to return</typeparam>
        /// <param name="json">Json String</param>
        /// <returns>The object Deserialized.</returns>
        public static T ToJsonObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Will convert a object, to json string.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>Json string serialized.</returns>
        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
