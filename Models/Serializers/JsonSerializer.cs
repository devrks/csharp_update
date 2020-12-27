using Models.Loggers;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Models.Serializers
{
    public class JsonSerializer : ISerializer
    {
        private JObject jobject;
        private ILogger logger;

        public JsonSerializer(ILogger _logger = null)
        {
            logger = _logger;
        }

        void ISerializer.Load(string path)
        {
            jobject = JObject.Parse(File.ReadAllText(path));
        }

        void ISerializer.Save(string path)
        {
            File.WriteAllText(path, jobject.ToString());
        }

        T ISerializer.GetSection<T>()
        {
            var section = jobject["Settings"][typeof(T).Name];
            return section == null ? default : section.ToObject<T>();
        }

        void ISerializer.SetSection<T>(T data)
        {
            jobject["Settings"][typeof(T).Name] = JToken.FromObject(data);
        }
    }
}