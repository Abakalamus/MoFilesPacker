﻿using FilesBoxing.Interface.BusinessLogic;
using FilesBoxing.Interface.BusinessLogic.FilesCollector;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FilesBoxing.Class.BusinessLogic.FilesCollector
{
    internal class FilesPatternCollector : IFilesCollector
    {
        private List<IFileDirectoryInfo> _fileDirectoryInfo;
        public ICollection<IFileDirectoryInfo> FileDirectoryInfo
        {
            get => _fileDirectoryInfo;
            set
            {//.Where(x => x.IsEnabled)
                var check = value.GroupBy(x => new { x.ParentFileDirectory, x.ExtensionFile }).Where(gr => gr.Count() > 1);
                if (check.Any())
                    throw new ApplicationException("Не допускается использование одинаковых каталогов с указанием одного и того же расширения файлов");
                _fileDirectoryInfo = new List<IFileDirectoryInfo>(value);
            }
        }
        public ICollection<string> FilesForPattern(string pattern)
        {
            var collectionFiles = new List<string>();
            foreach (var info in FileDirectoryInfo)
                collectionFiles.AddRange(GetEntityFilesFromDirectory(info, pattern));
            return collectionFiles;

            IEnumerable<string> GetEntityFilesFromDirectory(IFileDirectoryInfo info, string entity)
            {
                var files = info.ParentFileDirectory.GetFiles($"*{entity}*{info.ExtensionFile}").Select(x => x.FullName);
                return files;
            }
        }
    }
}
