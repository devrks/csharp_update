using DAL;
using DataManager;
using Ftp;
using Labs.ArchiveManager;
using Models;
using Models.Loggers;
using Models.Options;
using System;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var logger = new DbLogger();
            var archiver = new Archiver(logger);
            var sw = new XmlWatcher(logger);
            sw.Created += path =>
            {
                var archive = archiver.CreateArchive(path);
                // And then we pass it to us server

                var ftp = new FileTransferService();
                ftp.UploadFile(path, logger);
                // archiver.ExtractArchive(archive);
            };
            var service = new PersonsService();
            var persons = service.GetAll().Take(10);

            var xmlService = new XmlGeneratorService();
            xmlService.Generate(Path.Combine(new SettingsManager().GetSection<ArchiveOptions>().SourceFolder, "persons.xml"),
                persons,
                logger);

            Console.ReadKey();
        }
    }
}