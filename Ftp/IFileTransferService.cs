using Models.Loggers;

namespace Ftp
{
    internal interface IFileTransferService
    {
        void UploadFile(string path, ILogger logger = null);
    }
}