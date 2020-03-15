using System.IO;
using CustomerTracker.Domain;

namespace CustomerTracker.Api
{
    public class FileService : IFileService
    {
        public bool FileExists(string path) => File.Exists(path);

        public StreamWriter FileCreateText(string path) => File.CreateText(path);

        public StreamReader FileOpenText(string path) => File.OpenText(path);

        public void Copy(string sourceFileName, string destFileName) => File.Copy(sourceFileName, destFileName);

        public void Move(string sourceFileName, string destFileName) => File.Move(sourceFileName, destFileName);
    }
}