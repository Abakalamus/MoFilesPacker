using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FilesBoxing.Interface.BusinessLogic;
using FilesBoxing.Interface.BusinessLogic.FileNameHelper;
using FilesBoxing.Interface.Visual;

namespace FilesBoxing.Class.BusinessLogic
{
    public class FilesCollectorToPackingHandler : IFilesCollectorToPackingHandler
    {
        private readonly IFileBoxingToDirectory _fileBoxing;
        private readonly IFilesCollector _filesCollector;
        private readonly IPackageFileNameHelper _fileNameHelper;
        private readonly IFilesCollectorToPackingEventHandlerHelper _eventHelper;
        public DirectoryInfo SaveFilesDirectory => _fileBoxing.DirectoryForBoxingFile;

        public EventHandler<ISearchFileMoInfo> OnFilesSearchComplite { get; set; }
        public EventHandler<IProcessHandleMoInfo> OnMoPackingComplite { get; set; }

        public FilesCollectorToPackingHandler(IFileBoxingToDirectory fileBoxing, IFilesCollector filesCollector, IPackageFileNameHelper fileNameHelper,
            IFilesCollectorToPackingEventHandlerHelper eventHelper)
        {
            _fileBoxing = fileBoxing;
            _filesCollector = filesCollector;
            _fileNameHelper = fileNameHelper;
            _eventHelper = eventHelper;
        }

        public void CreatePackageFileForMoFiles(IFilesCollectorHandlerParameter parameter)
        {
            _filesCollector.FileDirectoryInfo = parameter.DirectoryInfoCollection;
            var nameArchive = string.IsNullOrEmpty(parameter.NameArchiveTemplate) ? _fileNameHelper.GetPackageFileDefaultName() : parameter.NameArchiveTemplate;
            var baseAnchors = GetPeriodAchorsValues();
            var idAnchorCodeMo = _fileNameHelper.GetAnchorInfoByFieldName("CODE_MO").Id;
            GetClearExistingDirectory(_fileBoxing.DirectoryForBoxingFile);
            foreach (var mo in parameter.MoCollection)
            {
                var filesMo = _filesCollector.GetFilesForPattern(mo).ToList();
                InformingFileSearchComplite();
                if (!filesMo.Any())
                {
                    InformingWorkComplite(false);
                    continue;
                }
                _fileBoxing.BoxFiles(filesMo, _fileNameHelper.GetTransformedValue(nameArchive, GetFullAnchorCollection(mo)));
                InformingWorkComplite();

                void InformingFileSearchComplite()
                {
                    OnFilesSearchComplite?.Invoke(this, _eventHelper.CreateSearchInfoParameter(mo, (byte)filesMo.Count));
                }
                void InformingWorkComplite(bool archiveCreated = true)
                {
                    OnMoPackingComplite?.Invoke(this,
                        archiveCreated
                            ? _eventHelper.CreateProcessHandleParameterCreateProcessHandleParameteArchiveComplite(mo)
                            : _eventHelper.CreateProcessHandleParameterNoArchive(mo));
                }
            }

            IList<IAnchorValue> GetPeriodAchorsValues()
            {
                return new List<IAnchorValue>
                    {
                       _fileNameHelper.GetAsAnchorValue(_fileNameHelper.GetAnchorInfoByFieldName("YEAR").Id, parameter.Year.ToString()),
                       _fileNameHelper.GetAsAnchorValue(_fileNameHelper.GetAnchorInfoByFieldName("MONTH").Id, parameter.Month.ToString()),
                    };
            }
            void GetClearExistingDirectory(DirectoryInfo directory)
            {
                if (directory.Exists)
                    directory.Delete(true);
                directory.Create();
            }
            ICollection<IAnchorValue> GetFullAnchorCollection(string codeMo)
            {
                var baseAnchorsCopy = new List<IAnchorValue>(baseAnchors)
                {
                    _fileNameHelper.GetAsAnchorValue(idAnchorCodeMo, codeMo)
                };
                return baseAnchorsCopy;
            }
        }
    }
}