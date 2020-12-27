using Models;
using Models.Loggers;
using Models.Options;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Labs.ArchiveManager
{
    public class XmlWatcher
    {
        public delegate void CreatedHandler(string fullPath);

        public event CreatedHandler Created;

        private const string fileRegEx = @".*\.xml";
        private readonly ILogger logger;
        private FileSystemWatcher watcher;
        private Regex mask;

        public XmlWatcher(ILogger _logger = null)
        {
            logger = _logger;
            var path = new SettingsManager().GetSection<ArchiveOptions>().SourceFolder;
            watcher = new FileSystemWatcher(path);
            watcher.IncludeSubdirectories = true;

            if (!path.EndsWith("\\"))
            {
                path += '\\';
            }
            mask = new Regex($"{path.Replace("\\", "\\\\")}{fileRegEx}");

            watcher.Created += OnCreate;
            watcher.EnableRaisingEvents = true;
        }

        private async void OnCreate(object sender, FileSystemEventArgs e)
        {
            if (mask.IsMatch(e.FullPath))
            {
                Task waiter = null;
                using (var tokenSource = new CancellationTokenSource(2000))
                {
                    waiter = Task.Run(async () =>
                    {
                        while (true)
                        {
                            if (tokenSource.IsCancellationRequested || File.Exists(e.FullPath))
                            {
                                break;
                            }
                            await Task.Delay(50);
                        }
                    }, tokenSource.Token);
                    await waiter;
                }

                if (waiter.IsCompleted)
                {
                    logger?.Log($"Создан файл {e.FullPath}");
                    Created?.Invoke(e.FullPath);
                }
            }
        }
    }
}