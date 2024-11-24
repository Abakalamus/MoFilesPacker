using System;
using System.Collections.Generic;
using System.Linq;
using FilesBoxing.Interface.BusinessLogic;

namespace FilesBoxing.Class.BusinessLogic
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
                _fileDirectoryInfo = new List<IFileDirectoryInfo>(value.Where(x => x.IsEnabled));
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

        public EventHandler<string> OnLogging { get; set; }

        public Dictionary<string, IEnumerable<string>> GetFilesForEntities()
        {
            if (!UniqueEntities.Any())
                throw new ApplicationException("Коллекция сущностей не содержит данные");
            if (!FileDirectoryInfo.Any())
                throw new ApplicationException("Коллекция информаций о каталогах не содержит данные");
            return UniqueEntities.ToDictionary(entity => entity, GetAllEntityFiles);

            IEnumerable<string> GetAllEntityFiles(string entity)
            {
                //OnLogging?.Invoke(this, $"Список каталогов для выгрузки {string.Join(Environment.NewLine, FileDirectoryInfo.Select(x => x.ParentFileDirectory.FullName))}");
                var collectionFiles = new List<string>();
                foreach (var info in FileDirectoryInfo)
                    collectionFiles.AddRange(GetEntityFilesFromDirectory(info, entity));
                return collectionFiles;
            }
            IEnumerable<string> GetEntityFilesFromDirectory(IFileDirectoryInfo info, string entity)
            {
                var files = info.ParentFileDirectory.GetFiles($"*{entity}*{info.ExtensionFile}").Select(x => x.FullName);
               // OnLogging?.Invoke(this, $"Список файлов в {info.ParentFileDirectory.Name} - {string.Join(Environment.NewLine, files)}");
                return files;
            }
        }
    }
}