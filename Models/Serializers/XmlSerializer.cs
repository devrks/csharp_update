﻿using Models.Loggers;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Models.Serializers
{
    public class XmlSerializer : ISerializer
    {
        private XDocument xDoc;
        private ILogger logger;

        public XmlSerializer(ILogger logger = null)
        {
            this.logger = logger;
        }

        void ISerializer.Load(string path)
        {
            xDoc = XDocument.Load(path);
            var xsdPath = Path.ChangeExtension(path, "xsd");
            if (File.Exists(xsdPath))
            {
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add(null, xsdPath);
                xDoc.Validate(schemaSet, (o, e) =>
                {
                    logger?.Log(e.Message);
                    if (e.Severity == XmlSeverityType.Error)
                    {
                        throw e.Exception;
                    }
                });
            }
        }

        void ISerializer.Save(string path)
        {
            xDoc.Save(path);
        }

        T ISerializer.GetSection<T>()
        {
            var element = xDoc.Root.Element(typeof(T).Name);
            if (element == null)
            {
                return default;
            }
            using (var sr = new StringReader(element.ToString()))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                var obj = serializer.Deserialize(sr);
                return obj == null ? default : (T)obj;
            }
        }

        void ISerializer.SetSection<T>(T data)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var sw = new StringWriter())
            {
                serializer.Serialize(sw, data);
                var element = xDoc.Root.Element(typeof(T).Name);
                if (element == null)
                {
                    xDoc.Root.Add(XElement.Parse(sw.ToString()));
                }
                else
                {
                    element.ReplaceWith(XElement.Parse(sw.ToString()));
                }
            }
        }
    }
}