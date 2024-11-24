using System.Collections.Generic;
using System.IO;

using FilesBoxing.Interface.BusinessLogic;

namespace FilesBoxing.Class.BusinessLogic
{
    public class FilesCollectorToBoxingHandler : IFilesCollectorToBoxingHandler
    {
        private readonly IFileBoxing _fileBoxing;
        private readonly IFilesCollectorNew _filesCollector;
        public DirectoryInfo SaveFilesDirectory { get; }
        public const string CodeMoAncor = "![CODE_MO]!"; // remove to IFileNameHelper

        public FilesCollectorToBoxingHandler(IFileBoxing fileBoxing, IFilesCollectorNew filesCollector, DirectoryInfo saveFilesDirectory)
        {
            _fileBoxing = fileBoxing;
            _filesCollector = filesCollector;
            SaveFilesDirectory = saveFilesDirectory;
        }

        public void CreateBoxingFileForMoCollectionFiles(IEnumerable<string> moCollection, IEnumerable<IFileDirectoryInfo> directoryInfoCollection, string nameArchive)
        {
            _filesCollector.FileDirectoryInfo = directoryInfoCollection;
            GetClearExistingDirectory(SaveFilesDirectory);
            foreach (var mo in moCollection)
            {
                var filesMo = _filesCollector.GetFilesForPattern(mo);
                _fileBoxing.BoxFiles(filesMo, GetNameArchiveWithCodeMo(nameArchive));
            }
            void GetClearExistingDirectory(DirectoryInfo directory)
            {
                if (directory.Exists)
                    directory.Delete(true);
                directory.Create();
            }
            string GetNameArchiveWithCodeMo(string codeMO)
            {
                return nameArchive.Replace(CodeMoAncor, codeMO);
            }
            //void TryMoveFilesToFinalPlace()
            //{
            //    if (!outboxDirectory.Exists)
            //    {
            //        AddToLogSafeThread(
            //            $"Каталог для финального расположения файлов не существует. Файлы хранятся во временном каталоге [{tempDirectory.FullName}]");
            //        return;
            //    }
            //    MovingFilesToFinalPlace();

            //    void MovingFilesToFinalPlace()
            //    {
            //        foreach (var file in files)
            //            File.Move(file.FullName, Path.Combine(outboxDirectory.FullName, file.Name));
            //        AddToLogSafeThread("Перемещение файлов завершено");
            //    }
            //}
        }
    }
}