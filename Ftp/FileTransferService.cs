using Models;
using Models.Loggers;
using Models.Options;
using System.IO;
using System.Net;

namespace Ftp
{
    public class FileTransferService : IFileTransferService
    {
        public void UploadFile(string path, ILogger logger)
        {
            var options = new SettingsManager().GetSection<FtpOptions>();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Path.Combine(options.FtpAddress, Path.GetFileName(path)));
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(options.Login, options.Password);
            request.EnableSsl = true;

            byte[] fileContents;

            using (var fs = new FileStream(path, FileMode.Open))
            {
                fileContents = new byte[fs.Length];
                fs.Read(fileContents, 0, fileContents.Length);
            }

            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            logger?.Log("Файл отправлен");
        }
    }
}