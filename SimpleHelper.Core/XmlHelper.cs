using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleHelper
{
    public static class XmlHelper
    {
        /// <summary>
        /// Will Convert a xml string to object.
        /// </summary>
        /// <typeparam name="T">Type of Json to return</typeparam>
        /// <param name="xml">xml String</param>
        /// <returns>The object Deserialized.</returns>
        public static T ToXmlObject<T>(this string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var memStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                var resultingMessage = (T)serializer.Deserialize(memStream);
                return resultingMessage;
            }
        }

        /// <summary>
        /// Will convert a object, to xml string.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>Xml string serialized.</returns>
        public static string ToXmlString<T>(this object obj) where T : new()
        {
            using (var ms = new MemoryStream())
            {
                using (var xw = XmlWriter.Create(ms))
                {
                    var serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(xw, obj);
                    xw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    var sr = new StreamReader(ms, Encoding.UTF8);
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
