using FilesBoxing.Interface.BusinessLogic;
using FilesBoxing.Interface.BusinessLogic.FilesCollector;
using FilesBoxing.Interface.BusinessLogic.NameHelper;
using FilesBoxing.Interface.DataBase;
using FilesBoxing.Interface.Factory;
using FilesBoxing.Interface.Settings;
using FilesBoxing.Interface.Visual;

using LibraryKurguzov.Class.Command;
using LibraryKurguzov.Class.Utility;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FilesBoxing.ViewModel
{
    public class MainViewModel : BaseWpfNotifyPropertyHandler
    {
        private readonly ISettingsFileBoxing _settings;
        private readonly IDataBaseController _dataBaseController;
        private readonly IFullFactoryFileBoxingHandler _factory;
        private readonly INameHelperController _nameHelper;
        private bool _isExecuting;
        private DateTime _selectedPeriod;
        private ITypeGroupingSettings _selectedGroup;
        private const byte CountHandleTasks = 20;
        public ObservableCollection<string> LogCollection { get; set; }
        public ObservableCollection<IMoProcessInfo> CodeMoCollection { get; set; }
        public ObservableCollection<IFileDirectoryInfo> DirectoryFilesInfo { get; set; }
        public ObservableCollection<ITypeGroupingSettings> UsingGroups { get; set; }
        public ITypeGroupingSettings SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                OnPropertyChanged();
            }
        }
        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                _isExecuting = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BoxingMoFilesCommand));
            }
        }
        public DateTime SelectedPeriod
        {
            get => _selectedPeriod;
            set
            {
                if (value.Date == _selectedPeriod.Date) return;
                _selectedPeriod = value;
                if (_nameHelper != null)
                {
                    _nameHelper.UpdateYearMonthInfo(value.Year, value.Month);
                    UpdateDirectoryFilesInfo();
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(BoxingMoFilesCommand));
            }
        }
        private void UpdateDirectoryFilesInfo()
        {
            if (_nameHelper == null)
                return;
            var updatedDirectoryInfoCollection = GetUpdatedDirectoryInfo();
            DirectoryFilesInfo?.Clear();
            DirectoryFilesInfo = new ObservableCollection<IFileDirectoryInfo>(updatedDirectoryInfoCollection);
            OnPropertyChanged(nameof(DirectoryFilesInfo));

            IEnumerable<IFileDirectoryInfo> GetUpdatedDirectoryInfo()
            {
                return _settings.ProgramSettings.FileDirectoriesInfo
                    .Select(info => _factory.GetNewDirectoryInfo(_nameHelper.GetTransformedValue(info.ParentFileDirectory.FullName), info.ExtensionFile, info.IdUsingGroups))
                    .ToList();
            }
        }
        public MainViewModel(IFullFactoryFileBoxingHandler factory, ISettingsFileBoxing settings)
        {
            _factory = factory;
            _settings = settings;
            SelectedPeriod = DateTime.Now.AddMonths(-1);
            LogCollection = new ObservableCollection<string>();
            try
            {
                _settings.RefreshDataFromFile();
            }
            catch (Exception ex)
            {
                LogCollection.Add($"Ошибка получения данных из файла настроек!{Environment.NewLine}{LibraryKurguzov.Class.Exception.ExceptionHanlder.GetAllMessages(ex)}");
                return;
            }
            _dataBaseController = factory.GetDataBaseController(settings.ProgramSettings.ConnectionString);
            _nameHelper = factory.GetNameHelperController(SelectedPeriod.Year, SelectedPeriod.Month);
            CodeMoCollection = new ObservableCollection<IMoProcessInfo>();
            RefreshCodeMoCollection(_settings.ProgramSettings.CodeMoCollection.ToList());
            DirectoryFilesInfo = new ObservableCollection<IFileDirectoryInfo>(_settings.ProgramSettings.FileDirectoriesInfo);
            UpdateDirectoryFilesInfo();
            UsingGroups = new ObservableCollection<ITypeGroupingSettings>(_settings.ProgramSettings.UsingGroups);
            SelectedGroup = _settings.ProgramSettings.UsingGroups.First(x => x.Id == _settings.ProgramSettings.DefaultGroupId);
        }
        private void RefreshCodeMoCollection(IEnumerable<string> codeMoCollection)
        {
            CodeMoCollection.Clear();
            foreach (var mo in codeMoCollection)
            {
                CodeMoCollection.Add(_factory.GetNewMoProcessInfo(mo));
            }
            OnPropertyChanged(nameof(CodeMoCollection));
        }
        // ReSharper disable once AsyncVoidLambda
        public ICommand BoxingMoFilesCommand => new RelayCommand(async obj =>
        {
            try
            {
                IsExecuting = true;
                ThrowExceptionIfNotSuitableParameters();
                var infoGetter = _factory.GetNewUserInfoGetter();
                AddToLogSafeThread("Начата работа по выгрузке файлов!");
                await Task.Run(HandlePackagingProcess);

                void ThrowExceptionIfNotSuitableParameters()
                {
                    var errors = CheckCollectionsErrors();
                    if (errors.Any())
                        AddErrorsToLogWithComment();

                    ICollection<string> CheckCollectionsErrors()
                    {
                        var result = new List<string>();
                        if (CodeMoCollection.All(x => !x.IsSelected))
                            result.Add("Не выбрана ни одна МО!");
                        if (!GetDirectoriesForRequireType().Any())
                            result.Add("Не выбрана ни один каталог для поиска файлов!");
                        return result;
                    }
                    void AddErrorsToLogWithComment()
                    {
                        foreach (var error in errors)
                            AddToLogSafeThread(error);
                    }
                }
                void HandlePackagingProcess()
                {
                    RefreshProgressInfoIfNeed();
                    var directoryOutput = infoGetter.GetDirectoryOutputPathByUserChoise(_settings.ProgramSettings.OutputDirectory.FullName);
                    if (!ChosenOutputDirectoryCorrect())
                        return;
                    var packagedFiles = GetPackagedMoFiles();
                    ThrowExceptionIfNotExistsAnyCreatedPackagedFiles();
                    AddToLogSafeThread("Файлы архивов для МО созданы!");
                    if (!CheckOutputPathExist())
                    {
                        AddToLogSafeThread(
                            $"Каталог для перемещения файлов не найден [{_settings.ProgramSettings.TempDirectory.Name}]. Файлы остались в каталоге Temp [{_settings.ProgramSettings.TempDirectory.FullName}]!");
                        return;
                    }
                    MovePackedFilesToOutboxDirectory(packagedFiles);
                    AddToLogSafeThread("Перемещение файлов завершено");
                    if (NeedShowResult())
                        OpenPathInExplorer(directoryOutput);

                    void RefreshProgressInfoIfNeed()
                    {
                        if (CodeMoCollection.All(x => x.IsPackageFileCreated == null && x.CountFiles == null)) return;
                        foreach (var mo in CodeMoCollection)
                        {
                            mo.IsPackageFileCreated = null;
                            mo.CountFiles = null;
                        }
                        OnPropertyChanged(nameof(CodeMoCollection));
                    }
                    bool ChosenOutputDirectoryCorrect()
                    {
                        return !string.IsNullOrEmpty(directoryOutput) && IsDirectoryExists(directoryOutput);
                    }
                    bool IsDirectoryExists(string path)
                    {
                        return Directory.Exists(path);
                    }
                    ICollection<FileInfo> GetPackagedMoFiles()
                    {
                        var selectedGroupNameArchive = _selectedGroup.FileNameArchive;
                        var filesCollectorToBoxingHandler = GetFilesCollectorToBoxingHandler();
                        //filesCollectorToBoxingHandler.CreatePackageFilesForMoFiles(CreateParameterForHandler());
                        var collectionMoWithArchiveName = GetCodeMoColection().Select(codeMo =>
                            _factory.GetNewMoWithName(codeMo, _nameHelper.GetTransformedValueForMo(selectedGroupNameArchive, codeMo))).ToList();
                        filesCollectorToBoxingHandler.CreatePackageFileForMoFiles(collectionMoWithArchiveName, CountHandleTasks);
                        return filesCollectorToBoxingHandler.SaveDirectory.GetFiles().ToList();

                        IFilesCollectorToPackingHandler GetFilesCollectorToBoxingHandler()
                        {
                            var handler = _factory.GetFilesCollectorToBoxingHandler(_settings.ProgramSettings.TempDirectory.FullName);
                            handler.OnFilesSearchComplite += OnFilesSearchComplite;
                            handler.OnMoPackingComplite += OnMoPackingComplite;
                            handler.DirectoryInfoCollection = GetDirectoriesForRequireType();
                            return handler;
                        }
                        IEnumerable<string> GetCodeMoColection()
                        {
                            return CodeMoCollection.Where(x => x.IsSelected).Select(x => x.CodeMo);
                        }
                    }
                    void ThrowExceptionIfNotExistsAnyCreatedPackagedFiles()
                    {
                        if (!packagedFiles.Any())
                            throw new ApplicationException("Не было создано ни одного архива! Проверьте настройки программы и существование файлов!");
                    }
                    bool CheckOutputPathExist()
                    {
                        return Directory.Exists(directoryOutput);
                    }
                    void MovePackedFilesToOutboxDirectory(IEnumerable<FileInfo> files)
                    {
                        foreach (var file in files)
                        {
                            var newPath = Path.Combine(directoryOutput, file.Name);
                            File.Move(file.FullName, newPath);
                        }
                    }
                    bool NeedShowResult()
                    {
                        return CreatedAnyPackages() && !IsOutputFolderDefault() &&
                               IsDirectoryExists(directoryOutput) && UserWantSeeFolder();

                        bool CreatedAnyPackages()
                        {
                            return CodeMoCollection.Any(x => x.IsPackageFileCreated == true);
                        }
                        bool IsOutputFolderDefault()
                        {
                            return directoryOutput == _settings.ProgramSettings.OutputDirectory.FullName;
                        }
                        bool UserWantSeeFolder()
                        {
                            return infoGetter.IsUserWantOpenInExplorerPath(directoryOutput);
                        }
                    }
                    void OpenPathInExplorer(string filePath)
                    {
                        Process.Start(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                AddToLogSafeThread($"Возникло исключение во формирования группированных сведений - {LibraryKurguzov.Class.Exception.ExceptionHanlder.GetAllMessages(ex)}");
            }
            finally
            {
                AddToLogSafeThread("Работа завершена");
                IsExecuting = false;
            }
        }, canExecute => !IsExecuting);
        private ICollection<IFileDirectoryInfo> GetDirectoriesForRequireType()
        {
            return DirectoryFilesInfo.Where(x => x.IdUsingGroups.Contains(SelectedGroup.Id)).ToList();
        }
        private void OnFilesSearchComplite(object sender, ISearchFileMoInfo searchFileMoInfo)
        {
            var mo = GetMoFromCollection(searchFileMoInfo.CodeMo);
            mo.CountFiles = searchFileMoInfo.CountFiles;
        }
        private void OnMoPackingComplite(object sender, IProcessHandleMoInfo processInfo)
        {
            var mo = GetMoFromCollection(processInfo.CodeMo);
            mo.IsPackageFileCreated = processInfo.IsPackageFileCreated;
            OnPropertyChanged(nameof(CodeMoCollection));
        }
        private IMoProcessInfo GetMoFromCollection(string codeMo)
        {
            return CodeMoCollection.First(x => x.CodeMo == codeMo);
        }
        private void AddToLogSafeThread(string text)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                LogCollection.Add(text);
            });
        }

        public ICommand OnOffCheckingMOCommand => new RelayCommand(obj =>
        {
            SetNewStatusForAllMo(!CodeMoCollection.Any(x => x.IsSelected));
            OnPropertyChanged(nameof(CodeMoCollection));

            void SetNewStatusForAllMo(bool newStatus)
            {
                foreach (var mo in CodeMoCollection)
                    mo.IsSelected = newStatus;
            }
        }, canExecute => !IsExecuting);
        public ICommand UpdateCollectionMoCommand => new RelayCommand(obj =>
        {
            try
            {
                var moCollection = _dataBaseController.GetCodesMo().OrderBy(x => x).ToList();
                _settings.ProgramSettings.CodeMoCollection = moCollection;
                RefreshCodeMoCollection(moCollection);
            }
            catch (Exception ex)
            {
                AddToLogSafeThread($"Возникло исключение во время обновления данных из БД - {LibraryKurguzov.Class.Exception.ExceptionHanlder.GetAllMessages(ex)}");
            }
            finally
            {
                AddToLogSafeThread("Обновление данных из БД завершено");
            }
        });
        public ICommand SaveSettingsFileCommand => new RelayCommand(obj =>
        {
            try
            {
                _settings.SaveCurrentSettingToFile();
            }
            catch (Exception ex)
            {
                AddToLogSafeThread($"Возникло исключение во время сохранения данных настроек - {LibraryKurguzov.Class.Exception.ExceptionHanlder.GetAllMessages(ex)}");
            }
            finally
            {
                AddToLogSafeThread("Сохранение данных настроек завершено");
            }
        });
    }
}