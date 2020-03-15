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
}