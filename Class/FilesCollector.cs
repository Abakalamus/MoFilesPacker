using System;
using System.Collections.Generic;
using System.Linq;

using FilesBoxing.Interface;

namespace FilesBoxing.Class
{
    public class FilesCollector : IFilesCollector
    {
        private List<string> _uniqueEntities;
        private List<IFileDirectoryInfo> _fileDirectoryInfo;

        public IEnumerable<IFileDirectoryInfo> FileDirectoryInfo
        {
            get => _fileDirectoryInfo;
            set
            {
                var check = value.Where(x => x.IsEnabled).GroupBy(x => new { x.ParentFileDirectory, x.ExtensionFile }).Where(gr => gr.Count() > 1);
                if (check.Any())
                    throw new ApplicationException("Не допускается использование одинаковых каталогов с указанием одного и того же расширения файлов");
                _fileDirectoryInfo = new List<IFileDirectoryInfo>(value);
            }
        }
        public IEnumerable<string> UniqueEntities
        {
            get => _uniqueEntities;
            set
            {
                var check = value.GroupBy(x => x).Where(gr => gr.Count() > 1);
                if (check.Any())
                    throw new ApplicationException("Не допускается использование одинаковых сущностей");
                _uniqueEntities = new List<string>(value);
            }
        }
        public Dictionary<string, IEnumerable<string>> GetFilesForEntities()
        {
            if (!UniqueEntities.Any())
                throw new ApplicationException("Коллекция сущностей не содержит данные");
            if (!FileDirectoryInfo.Any())
                throw new ApplicationException("Коллекция информаций о каталогах не содержит данные");
            return UniqueEntities.ToDictionary(entity => entity, GetAllEntityFiles);

            IEnumerable<string> GetEntityFilesFromDirectory(IFileDirectoryInfo info, string entity)
            {
                return info.ParentFileDirectory.GetFiles($"*{entity}*{info.ExtensionFile}").Select(x => x.FullName);
            }
            IEnumerable<string> GetAllEntityFiles(string entity)
            {
                var collectionFiles = new List<string>();
                foreach (var info in FileDirectoryInfo)
                    collectionFiles.AddRange(GetEntityFilesFromDirectory(info, entity));
                return collectionFiles;
            }
        }
    }
}