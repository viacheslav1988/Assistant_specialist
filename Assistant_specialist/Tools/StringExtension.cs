using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace Assistant_specialist.Tools
{
    static class StringExtension
    {
        public static string OnlyNumbersToConvert(this string line)
        {
            if (string.IsNullOrEmpty(line)) return string.Empty;
            bool foundNumber = false;
            bool foundPoint = false;
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                if (foundNumber)
                {
                    if (!foundPoint)
                    {
                        if (line[i].Equals('.') | line[i].Equals(','))
                        {
                            foundPoint = true;
                            result.Append(',');
                        }
                    }
                }
                if (line[i] == '0' || line[i] == '1' || line[i] == '2' || line[i] == '3' ||
                    line[i] == '4' || line[i] == '5' || line[i] == '6' || line[i] == '7' ||
                    line[i] == '8' || line[i] == '9')
                {
                    if (!foundNumber) foundNumber = true;
                    result.Append(line[i]);
                }
            }
            string raw = result[result.Length - 1].Equals(',') ? result.ToString(0, result.Length - 1) : result.ToString();
            foundPoint = raw.Substring(0, (raw.Length > 28 ? 28 : raw.Length)).Contains(',');
            return raw.Length > (foundPoint ? 29 : 28) ? raw.Substring(0, (foundPoint ? 29 : 28)) : raw;
        }

        public static TOutput XamlStringToObject<TOutput>(this string xmlString)
        {
            XamlSchemaContext schemaContext = System.Windows.Markup.XamlReader.GetWpfSchemaContext();
            XamlXmlReaderSettings settings = new XamlXmlReaderSettings()
            {
                LocalAssembly = Assembly.GetExecutingAssembly()
            };
            using (XamlXmlReader reader = new XamlXmlReader(new StringReader(xmlString), schemaContext, settings))
            using (XamlObjectWriter writer = new XamlObjectWriter(reader.SchemaContext))
            {
                while (reader.Read()) writer.WriteNode(reader);
                return (TOutput)writer.Result;
            }
        }
    }
}