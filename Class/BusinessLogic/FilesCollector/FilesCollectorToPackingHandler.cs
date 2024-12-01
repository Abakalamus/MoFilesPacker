using FilesBoxing.Interface.BusinessLogic;
using FilesBoxing.Interface.BusinessLogic.FilesCollector;
using FilesBoxing.Interface.Visual;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FilesBoxing.Class.BusinessLogic.FilesCollector
{
    public class FilesCollectorToPackingHandler : IFilesCollectorToPackingHandler
    {
        private readonly IFileBoxingToDirectory _fileBoxing;
        private readonly IFilesCollector _filesCollector;
        private readonly IFilesCollectorToPackingEventHandlerHelper _eventHelper;
        private ICollection<IFileDirectoryInfo> _directoryInfoCollection;
        public DirectoryInfo SaveDirectory => _fileBoxing.DirectoryForBoxingFile;
        public EventHandler<ISearchFileMoInfo> OnFilesSearchComplite { get; set; }
        public EventHandler<IProcessHandleMoInfo> OnMoPackingComplite { get; set; }
        public ICollection<IFileDirectoryInfo> DirectoryInfoCollection
        {
            get => _directoryInfoCollection;
            set
            {
                _directoryInfoCollection = value;
                if (_filesCollector != null)
                    _filesCollector.FileDirectoryInfo = value;
            }
        }

        public FilesCollectorToPackingHandler(IFileBoxingToDirectory fileBoxing, IFilesCollector filesCollector,
            IFilesCollectorToPackingEventHandlerHelper eventHelper)
        {
            _fileBoxing = fileBoxing;
            _filesCollector = filesCollector;
            _eventHelper = eventHelper;
        }
        //public void CreatePackageFilesForMoFiles(IFilesCollectorHandlerParameter parameter)
        //{
        //    //_filesCollector.FileDirectoryInfo = parameter.DirectoryInfoCollection;
        //    //var nameArchive = string.IsNullOrEmpty(parameter.NameArchiveTemplate) ? _nameHelper.GetPackageFileDefaultName() : parameter.NameArchiveTemplate;
        //    //var baseAnchors = GetPeriodAchorsValues();
        //    //var idAnchorCodeMo = _nameHelper.GetAnchorInfoByFieldName("CODE_MO").Id;
        //    //GetClearExistingDirectory(_fileBoxing.DirectoryForBoxingFile);
        //    //var semaphore = new SemaphoreSlim(20, 20);
        //    //var taskCollection = new List<Task>();
        //    //foreach (var mo in parameter.MoCollection)
        //    //{
        //    //    semaphore.Wait();
        //    //    taskCollection.Add(Task.Run(() =>
        //    //    {
        //    //        try
        //    //        {
        //    //            PackageFilesForMo(mo);
        //    //        }
        //    //        finally
        //    //        {
        //    //            semaphore.Release();
        //    //        }
        //    //    }));
        //    //}
        //    //Task.WaitAll(taskCollection.ToArray());

        //    //IList<IAnchorValue> GetPeriodAchorsValues()
        //    //{
        //    //    return new List<IAnchorValue>
        //    //        {
        //    //           _nameHelper.GetAsAnchorValue(_nameHelper.GetAnchorInfoByFieldName("YEAR").Id, parameter.Year.ToString()),
        //    //           _nameHelper.GetAsAnchorValue(_nameHelper.GetAnchorInfoByFieldName("MONTH").Id, parameter.Month.ToString()),
        //    //        };
        //    //}
        //    //void GetClearExistingDirectory(DirectoryInfo directory)
        //    //{
        //    //    if (directory.Exists)
        //    //        directory.Delete(true);
        //    //    directory.Create();
        //    //}
        //    //ICollection<IAnchorValue> GetFullAnchorCollection(string codeMo)
        //    //{
        //    //    var baseAnchorsCopy = new List<IAnchorValue>(baseAnchors)
        //    //    {
        //    //        _nameHelper.GetAsAnchorValue(idAnchorCodeMo, codeMo)
        //    //    };
        //    //    return baseAnchorsCopy;
        //    //}

        //    //void PackageFilesForMo(string mo)
        //    //{
        //    //    var filesMo = _filesCollector.GetFilesForPattern(mo).ToList();
        //    //    InformingFileSearchComplite();
        //    //    if (!filesMo.Any())
        //    //    {
        //    //        InformingWorkComplite(false);
        //    //        return;
        //    //    }
        //    //    _fileBoxing.BoxFiles(filesMo, _nameHelper.GetTransformedValue(nameArchive, GetFullAnchorCollection(mo)));
        //    //    InformingWorkComplite();

        //    //    void InformingFileSearchComplite()
        //    //    {
        //    //        OnFilesSearchComplite?.Invoke(this, _eventHelper.CreateSearchInfoParameter(mo, (byte)filesMo.Count));
        //    //    }
        //    //    void InformingWorkComplite(bool archiveCreated = true)
        //    //    {
        //    //        OnMoPackingComplite?.Invoke(this,
        //    //            archiveCreated
        //    //                ? _eventHelper.CreateProcessHandleParameterCreateProcessHandleParameteArchiveComplite(mo)
        //    //                : _eventHelper.CreateProcessHandleParameterNoArchive(mo));
        //    //    }
        //    //}
        //}
        public void CreatePackageFileForMoFiles(IReadOnlyCollection<IMoWithName> collectionInfo, byte countTask)
        {
            var taskCollection = new List<Task>();
            var semaphore = new SemaphoreSlim(countTask, countTask);

            foreach (var info in collectionInfo)
            {
                semaphore.Wait();
                taskCollection.Add(CreateTaskWithSemaphoreRelease(()=> CreatePackageFileForMoFiles(info.CodeMo, info.Name)));
            }
            Task.WaitAll(taskCollection.ToArray());

            Task CreateTaskWithSemaphoreRelease(Action action)
            {
                return Task.Run(() =>
                {
                    try
                    {
                        action();
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
            }
        }
        public void CreatePackageFileForMoFiles(string codeMo, string archiveName)
        {
            var filesMo = _filesCollector.GetFilesForPattern(codeMo).ToList();
            InformingFileSearchComplite();
            if (!filesMo.Any())
            {
                InformingWorkComplite(false);
                return;
            }
            _fileBoxing.BoxFiles(filesMo, archiveName); //_nameHelper.GetTransformedValue(nameArchive, GetFullAnchorCollection(mo))
            InformingWorkComplite();

            void InformingFileSearchComplite()
            {
                OnFilesSearchComplite?.Invoke(this, _eventHelper.CreateSearchInfoParameter(codeMo, (byte)filesMo.Count));
            }
            void InformingWorkComplite(bool archiveCreated = true)
            {
                OnMoPackingComplite?.Invoke(this,
                    archiveCreated
                        ? _eventHelper.CreateProcessHandleParameterCreateProcessHandleParameteArchiveComplite(codeMo)
                        : _eventHelper.CreateProcessHandleParameterNoArchive(codeMo));
            }
        }
    }
}