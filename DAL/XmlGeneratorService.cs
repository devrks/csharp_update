using DataManager;
using Models.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Models
{
    public class XmlGeneratorService
    {
        public void Generate(string path, IEnumerable<Person> persons, ILogger logger)
        {
            try
            {
                var xDoc = new XDocument();
                xDoc.Root.Add(new XElement("Persons", persons));

                xDoc.Save(path);
                XmlSchemaInference infer = new XmlSchemaInference();
                XmlSchemaSet schemaSet = infer.InferSchema(new XmlTextReader(path));

                using (var w = XmlWriter.Create(Path.ChangeExtension(path, "xsd")))
                {
                    foreach (XmlSchema schema in schemaSet.Schemas())
                    {
                        schema.Write(w);
                    }
                    w.Close();
                }
                logger?.Log("Создан xml-документ.");
            }
            catch (Exception ex)
            {
                logger?.Log(ex);
            }
        }
    }
}