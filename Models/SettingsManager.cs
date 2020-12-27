using Models.Serializers;
using Models.Loggers;
using System;
using System.IO;

namespace Models
{
    public class SettingsManager
    {
        #region Fields

        private string configPath;
        private ISerializer provider;
        private ILogger logger;

        #endregion Fields

        public SettingsManager(string _configPath = null, ILogger _logger = null)
        {
            if (string.IsNullOrEmpty(_configPath))
            {
                _configPath = "appsettings.json";
            }
            if (!File.Exists(_configPath))
            {
                var ex = new FileNotFoundException(nameof(_configPath));
                logger?.Log(ex);
                throw ex;
            }
            switch (Path.GetExtension(_configPath))
            {
                case ".json":
                    provider = new JsonSerializer();
                    break;

                case ".xml":
                    provider = new XmlSerializer();
                    break;

                default:
                    var ex = new Exception($"Unknown extension {Path.GetExtension(_configPath)}.");
                    logger?.Log(ex);
                    throw ex;
            }
            configPath = _configPath;
            logger = _logger;
            AppDomain.CurrentDomain.ProcessExit += (s, e) => SaveSettings();
            LoadSettings();
        }

        public T GetSection<T>()
        {
            logger?.Log($"Чтение секции {typeof(T).Name}");
            return provider.GetSection<T>();
        }

        public void SetSection<T>(T data)
        {
            logger?.Log($"Запись секции {typeof(T).Name}");
            provider.SetSection<T>(data);
        }

        private void LoadSettings()
        {
            provider.Load(configPath);
        }

        public void SaveSettings()
        {
            provider.Save(configPath);
        }
    }
}