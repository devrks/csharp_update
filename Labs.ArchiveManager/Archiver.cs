using Ionic.Zip;
using Models;
using Models.Loggers;
using Models.Options;
using System.IO;

namespace Labs.ArchiveManager
{
    public class Archiver
    {
        private readonly ILogger logger;
        private SettingsManager settings;

        public Archiver(ILogger _logger = null)
        {
            logger = _logger;
            settings = new SettingsManager();
        }

        public string CreateArchive(string fileName)
        {
            var options = settings.GetSection<ArchiveOptions>();
            var targetPath = Path.Combine(options.TargetFolder, Path.ChangeExtension(Path.GetFileName(fileName), "zip"));
            if (!File.Exists(targetPath))
            {
                using (var zip = new ZipFile(targetPath))
                {
                    zip.CompressionLevel = options.CompressingLevel;
                    zip.Password = settings.GetSection<CryptingOptions>().Password;

                    var start = options.SourceFolder.Length;
                    if (!options.SourceFolder.EndsWith("\\"))
                    {
                        start++;
                    }

                    zip.AddFile(fileName, Path.GetDirectoryName(fileName).Substring(start));
                    zip.Save();
                }
            }
            logger?.Log($"Создан архив: {targetPath}");
            return targetPath;
        }

        public void ExtractArchive(string fileName)
        {
            var options = settings.GetSection<ArchiveOptions>();
            using (var zip = ZipFile.Read(fileName))
            {
                zip.Password = settings.GetSection<CryptingOptions>().Password;
                zip.ExtractExistingFile = ExtractExistingFileAction.DoNotOverwrite;
                zip.ExtractAll(options.TargetFolder);
            }
            logger?.Log($"Распакован архив: {fileName}");
        }
    }
}