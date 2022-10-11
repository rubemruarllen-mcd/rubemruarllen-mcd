using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml;

namespace PollingPoc.Domain.Converters
{
    /// <summary>
    /// In this class, there are Parsers as extend to 
    /// </summary>
    public static class ParseHelpers
    {
        /// <summary>
        /// This method parse a xml string to object.
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="this">string xml</param>
        /// <returns></returns>
        public static T? ParseXML<T>(this string @this) where T : class
        {
            var reader = XmlReader.Create(@this.Trim().ToStream(), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Document });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }


        /// <summary>
        /// This method parse string to Stream.
        /// </summary>
        /// <param name="this">string to be parsed</param>
        /// <returns>the Stream</returns>
        public static Stream ToStream(this string @this)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(@this);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// This method convert a T to string
        /// </summary>
        /// <typeparam name="T">Type to be converted</typeparam>
        /// <param name="value">object value</param>
        /// <returns></returns>
        public static string Serialize<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var xmlserializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, value);
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// This Method decode a base64 in xml.
        /// </summary>
        /// <param name="this">string with xml base64</param>
        /// <returns>xml decoded</returns>
        public static string ParseXmlBase64ToString(this string @this)
        {
            const string PATTERN = @"(?:<base64>)(\S*)(?:<\/base64>)";

            var options = RegexOptions.Multiline;
            var matches = Regex.Matches(@this, PATTERN, options);

            if (matches.Count == 0 || matches[0].Groups.Count <= 0 || matches[0].Groups[1].Value == "")
                return @this;

            var encodedString = matches[0].Groups[1].Value;
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);
            @this = @this.Replace(matches[0].Groups[1].Value, decodedString);

            return @this;
        }

        public static string XmlToString(this XmlNode node, int indentation)
        {
            using (var sw = new StringWriter())
            {
                using (var xw = new XmlTextWriter(sw))
                {
                    xw.Formatting = Formatting.Indented;
                    xw.Indentation = indentation;
                    node.WriteContentTo(xw);
                }
                return sw.ToString();
            }
        }
    }
}
