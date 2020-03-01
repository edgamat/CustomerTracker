using System.IO;

namespace CustomerTracker.Domain
{
    public interface IFileService
    {
        bool FileExists(string path);

        StreamWriter FileCreateText(string path);

        StreamReader FileOpenText(string path);

        void Copy(string sourceFileName, string destFileName);

        void Move(string sourceFileName, string destFileName);
    }

    public class FileService : IFileService
    {
        public bool FileExists(string path) => File.Exists(path);

        public StreamWriter FileCreateText(string path) => File.CreateText(path);

        public StreamReader FileOpenText(string path) => File.OpenText(path);

        public void Copy(string sourceFileName, string destFileName) => File.Copy(sourceFileName, destFileName);

        public void Move(string sourceFileName, string destFileName) => File.Move(sourceFileName, destFileName);
    }
}