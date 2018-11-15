using System.IO;
using System.Xml.Serialization;

namespace SimpleHelper
{
    /// <summary>
    /// Class SerializeHelper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
            StreamWriter oStreamWriter = new StreamWriter(pPath);
            oXmlSerializer.Serialize(oStreamWriter, pObject);
            oStreamWriter.Close();
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="pPath">The path.</param>
        /// <returns>T.</returns>
        public static T Deserialize(string pPath)
        {
            XmlSerializer oXmlSerializer = new XmlSerializer(typeof(T));
            StreamReader oStreamReader = new StreamReader(pPath);
            T obj = (T)oXmlSerializer.Deserialize(oStreamReader);
            oStreamReader.Close();
            return obj;
        }
    }
}