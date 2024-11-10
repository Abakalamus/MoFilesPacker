using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FilesBoxing.Interface;

using LibraryKurguzov.Class.Archive;

namespace FilesBoxing.Class
{
    public class FileBoxing : IFileBoxingToDirectory
    {
        public DirectoryInfo DirectoryForBoxingFile { get; }

        public FileBoxing(DirectoryInfo directory)
        {
            DirectoryForBoxingFile = directory;
        }

        public void BoxFiles(ICollection<string> files, string archiveName)
        {
            if (!files.Any())
                throw new ApplicationException("Нет файлов для упаковки");
            DirectoryForBoxingFile.Refresh();
            if (!DirectoryForBoxingFile.Exists)
                throw new ApplicationException("Каталог для файла-архива не существует");
            var pathArchive = Path.Combine(DirectoryForBoxingFile.FullName, $"{archiveName}.ZIP");
            if (File.Exists(pathArchive))
                throw new ApplicationException("Файл для упаковки уже существует");
            if (files.Any(x => !File.Exists(x)))
                throw new ApplicationException("Один или несколько файлов не существуют");
            var archive = ArchiveWriterZip.CreateBuilder(pathArchive);
            foreach (var file in files)
                archive.AddFile(file);
            archive.Build().CreateArchive();
        }
    }
}