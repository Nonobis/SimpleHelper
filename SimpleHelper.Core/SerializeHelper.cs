using System.IO;
using System.Xml.Serialization;

namespace SimpleHelper
{
    public static class SerializeHelper<T>
    {
        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pPath">The path.</param>
        public static void Serialize(T pObject, string pPath)
        {
            XmlSerializer oXmlSerializer = new XmlSerializer(typeof(T));
            using (FileStream fileStream = new FileStream(pPath, FileMode.Create))
            {
                using (StreamWriter oStreamWriter = new StreamWriter(fileStream))
                {
                    oXmlSerializer.Serialize(oStreamWriter, pObject);
                }
            }
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="pPath">The path.</param>
        /// <returns></returns>
        public static T Deserialize(string pPath)
        {
            XmlSerializer oXmlSerializer = new XmlSerializer(typeof(T));
            using (FileStream fileStream = new FileStream(pPath, FileMode.Open))
            {
                using (StreamReader oStreamReader = new StreamReader(fileStream))
                {
                    T obj = (T)oXmlSerializer.Deserialize(oStreamReader);
                    return obj;
                }
            }
        }
    }
}