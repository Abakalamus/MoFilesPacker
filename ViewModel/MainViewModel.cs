using System;
using System.Collections.ObjectModel;
using System.Linq;

using LibraryKurguzov.Class.Command;
using System.Windows.Input;
using FilesBoxing.Interface.DataBase;
using FilesBoxing.Interface.Factory;
using LibraryKurguzov.Class.Utility;
using System.Windows;
using FilesBoxing.Interface.Settings;
using FilesBoxing.Interface.BusinessLogic;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using FilesBoxing.Interface.Visual;

namespace FilesBoxing.ViewModel
{
    public class MainViewModel : BaseWpfNotifyPropertyHandler
    {
        private readonly ISettingsFileBoxing _settings;
        private readonly IDataBaseController _dataBaseController;
        private readonly IFullFactoryFileBoxingHandler _factory;
        private readonly IFileDirectoryInfoUpdater _updater;
        private bool _isExecuting;
        private int _year;
        private int _month;

        public ObservableCollection<string> LogCollection { get; set; }
        public ObservableCollection<IMoProcessInfo> CodeMoCollection { get; set; }
        public ObservableCollection<IFileDirectoryInfo> DirectoryFilesInfo { get; set; }
        public ObservableCollection<ITypeGroupingSettings> UsingGroups { get; set; }
        public ITypeGroupingSettings SelectedGroup { get; set; }
        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                if (value == _isExecuting) return;
                _isExecuting = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BoxingMoFilesCommand));
            }
        }
        public int Year
        {
            get => _year;
            set
            {
                if (value == _year) return;
                _year = value;
                if (_updater != null)
                {
                    _updater.Year = value;
                    UpdateDirectoryFilesInfo();
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(BoxingMoFilesCommand));
            }
        }
        public int Month
        {
            get => _month;
            set
            {
                if (value == _month) return;
                _month = value;
                if (_updater != null)
                {
                    _updater.Month = value;
                    UpdateDirectoryFilesInfo();
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(BoxingMoFilesCommand));
            }
        }

        private void UpdateDirectoryFilesInfo()
        {
            DirectoryFilesInfo?.Clear();
            var newInfo = _updater.GetUpdatedByPeriod(_settings.ProgramSettings.FileDirectoriesInfo);
            DirectoryFilesInfo = new ObservableCollection<IFileDirectoryInfo>(newInfo);
            OnPropertyChanged(nameof(DirectoryFilesInfo));
        }

        public MainViewModel(IFullFactoryFileBoxingHandler factory, ISettingsFileBoxing settings)
        {
            _factory = factory;
            _settings = settings;
            var dateTimeMonthBefore = DateTime.Now.AddMonths(-1);
            Year = dateTimeMonthBefore.Year;
            Month = dateTimeMonthBefore.Month;
            LogCollection = new ObservableCollection<string>();
            try
            {
                _settings.ResreshDataFromFile();
            }
            catch (Exception ex)
            {
                LogCollection.Add($"Ошибка получения данных из файла настроек!{Environment.NewLine}{LibraryKurguzov.Class.Exception.ExceptionHanlder.GetAllMessages(ex)}");
                return;
            }
            _dataBaseController = factory.GetDataBaseController(settings.ProgramSettings.ConnectionString);
            _updater = _factory.GetFileDirectoryInfoUpdater(Year, Month);
            UpdateDirectoryFilesInfo();
            CodeMoCollection = new ObservableCollection<IMoProcessInfo>();
            RefreshCodeMoCollection(_settings.ProgramSettings.CodeMoCollection.ToList());
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
        }

        // ReSharper disable once AsyncVoidLambda
        public ICommand BoxingMoFilesCommand => new RelayCommand(async obj =>
        {
            try
            {
                IsExecuting = true;
                // RefreshCodeMoCollection(CodeMoCollection.Select(x=>x.CodeMo).ToList());
                var filesCollectorToBoxingHandler = GetFilesCollectorToBoxingHandler();
                await Task.Run(() =>
                {
                    filesCollectorToBoxingHandler.CreatePackageFileForMoFiles(CreateParameterForHandler());
                    AddToLogSafeThread("Файлы архивов для МО созданы!");
                    if (!CheckOutputPathExist())
                    {
                        AddToLogSafeThread($"Каталог для перемещения файлов не найден [{_settings.ProgramSettings.TempDirectory.Name}]. Файлы остались в каталоге Temp [{_settings.ProgramSettings.TempDirectory.FullName}]!");
                        return;
                    }
                    MovePackedFilesToOutboxDirectory(filesCollectorToBoxingHandler.SaveFilesDirectory.GetFiles());
                });
                AddToLogSafeThread("Перемещение файлов завершено");

                IEnumerable<IFileDirectoryInfo> GetDirectoriesForRequireType()
                {
                    return DirectoryFilesInfo.Where(x => x.IdUsingGroups.Contains(SelectedGroup.Id));//x.IsEnabled &&
                }
                IFilesCollectorToPackingHandler GetFilesCollectorToBoxingHandler()
                {
                    var handler = _factory.GetFilesCollectorToBoxingHandler(_settings.ProgramSettings.TempDirectory.FullName);
                    handler.OnFilesSearchComplite += OnFilesSearchComplite;
                    handler.OnMoPackingComplite += OnMoPackingComplite;
                    return handler;
                }
                IFilesCollectorHandlerParameter CreateParameterForHandler()
                {
                    return _factory.CreateFilesCollectorHandlerParameter(Year, Month, CodeMoCollection.Select(x => x.CodeMo), GetDirectoriesForRequireType(), SelectedGroup.FileNameArchive);
                }

                bool CheckOutputPathExist()
                {
                    return _settings.ProgramSettings.OutputDirectory.Exists;
                }
                void MovePackedFilesToOutboxDirectory(IEnumerable<FileInfo> files)
                {
                    foreach (var file in files)
                    {
                        var newPath = Path.Combine(_settings.ProgramSettings.OutputDirectory.FullName, file.Name);
                        File.Move(file.FullName, newPath);
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

        private void OnFilesSearchComplite(object sender, ISearchFileMoInfo searchFileMoInfo)
        {
            var mo = GetMoFromCollection(searchFileMoInfo.CodeMo);
            mo.CountFiles = searchFileMoInfo.CountFiles;
        }
        private IMoProcessInfo GetMoFromCollection(string codeMo)
        {
            return CodeMoCollection.First(x => x.CodeMo == codeMo);
        }
        private void OnMoPackingComplite(object sender, IProcessHandleMoInfo processInfo)
        {
            var mo = GetMoFromCollection(processInfo.CodeMo);
            mo.IsPackageFileCreated = processInfo.IsPackageFileCreated;
        }
        private void AddToLogSafeThread(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                LogCollection.Add(text);
            });
        }
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
        //private void BoxMoFilesIntoSingleFile()
        //{

        //    var tempDirectory = _settings.ProgramSettings.TempDirectory;
        //    var outboxDirectory = _settings.ProgramSettings.OutputDirectory;
        //    var defaultGroup = _settings.ProgramSettings.UsingGroups.First(x => x.Id == _settings.ProgramSettings.DefaultGroupId);
        //    var boxingController = _factoryFileBoxingHandler.GetBoxingHandler(tempDirectory);
        //    GetClearExistingDirectory(tempDirectory);
        //    var filesCollector = GetPreparedFilesCollector();
        //    var dataMo = filesCollector.GetFilesForEntities();

        //    var moWithData = dataMo.Where(x => x.Value.Any()).ToList();
        //    foreach (var mo in moWithData)
        //    {
        //        var nameArchive = _settings.ProgramSettings.BoxAllGroupTypes ? $"Данные реестра {Year} {Month} для {mo.Key}" : FinalDefaultGroupName(mo.Key);
        //        boxingController.BoxFiles(mo.Value.ToList(), nameArchive);
        //    }

        //    var files = tempDirectory.GetFiles();
        //    OnLoggingFileCollector(this,
        //        $"Выгрузка данных завершена. Количество МО / всего МО - [{moWithData.Count}/{filesCollector.UniqueEntities.Count()}]. Количество архивов - {files.Length}");
        //    TryMoveFilesToFinalPlace();

        //    void GetClearExistingDirectory(DirectoryInfo directory)
        //    {
        //        if (directory.Exists)
        //            directory.Delete(true);
        //        directory.Create();
        //    }
        //    IFilesCollector GetPreparedFilesCollector()
        //    {
        //        var collector = _factoryFileBoxingHandler.GetFilesCollector();
        //        collector.UniqueEntities = CodeMoCollection;
        //        collector.FileDirectoryInfo = _settings.ProgramSettings.BoxAllGroupTypes ? DirectoryFilesInfo.Where(x => x.IsEnabled) : DirectoryFilesInfo.Where(x => x.IsEnabled && x.IdUsingGroups.Contains(defaultGroup.Id));
        //        collector.OnLogging += OnLoggingFileCollector;
        //        return collector;
        //    }
        //    void OnLoggingFileCollector(object @object, string text)
        //    {
        //        AddToLogSafeThread(text);
        //    }
        //    string FinalDefaultGroupName(string codeMO)
        //    {
        //        return defaultGroup.FileNameArchive.Replace("![YEAR]!", $"{Year}").Replace("![MONTH]!", $"{Month}").Replace("![CODE_MO]!", codeMO);
        //    }

        //    void TryMoveFilesToFinalPlace()
        //    {
        //        if (!outboxDirectory.Exists)
        //        {
        //            AddToLogSafeThread(
        //                $"Каталог для финального расположения файлов не существует. Файлы хранятся во временном каталоге [{tempDirectory.FullName}]");
        //            return;
        //        }
        //        MovingFilesToFinalPlace();

        //    }
        //}
    }
}