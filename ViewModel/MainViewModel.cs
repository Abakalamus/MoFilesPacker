using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FilesBoxing.Interface;

using LibraryKurguzov.Class.Command;
using System.Windows.Input;
using FilesBoxing.Interface.DataBase;
using FilesBoxing.Interface.Factory;
using LibraryKurguzov.Class.Utility;
using System.Threading.Tasks;
using System.Windows;
using FilesBoxing.Interface.Settings;
using FilesBoxing.Class;
using FilesBoxing.Interface.BusinessLogic;

namespace FilesBoxing.ViewModel
{
    public class MainViewModel : BaseWpfNotifyPropertyHandler
    {
        private readonly ISettingsFileBoxing _settings;
        private readonly IFactoryFileBoxingHandler _factoryFileBoxingHandler;
        private readonly IDataBaseController _dataBaseController;
        public ObservableCollection<string> LogCollection { get; set; }
        public ObservableCollection<string> CodeMoCollection { get; set; }
        public ObservableCollection<IFileDirectoryInfo> DirectoryFilesInfo { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public MainViewModel(IFullFactoryFileBoxingHandler factory, ISettingsFileBoxing settings)
        {
            _settings = settings;
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
            _factoryFileBoxingHandler = factory.GetFileBoxingFactory();
            _dataBaseController = factory.GetDataBaseController(settings.ProgramSettings.ConnectionString);
            DirectoryFilesInfo = new ObservableCollection<IFileDirectoryInfo>(_settings.ProgramSettings.FileDirectoriesInfo);
            CodeMoCollection = new ObservableCollection<string>(_settings.ProgramSettings.CodeMoCollection);
            var dateTimeMonthBefore = DateTime.Now.AddMonths(-1);
            Year = dateTimeMonthBefore.Year;
            Month = dateTimeMonthBefore.Month;
        }

        public ICommand BoxingMoFilesCommand => new RelayCommand(async obj =>
        {
            try
            {
                var defaultGroup = _settings.ProgramSettings.UsingGroups.First(x => x.Id == _settings.ProgramSettings.DefaultGroupId);
                var directoryInfoCollection = _settings.ProgramSettings.BoxAllGroupTypes
                    ? DirectoryFilesInfo.Where(x => x.IsEnabled)
                    : DirectoryFilesInfo.Where(x => x.IsEnabled && x.IdUsingGroups.Contains(defaultGroup.Id));
                var nameArchive = _settings.ProgramSettings.BoxAllGroupTypes ? $"Данные реестра {Year} {Month} для [!CODE_MO!]" : defaultGroup.FileNameArchive;
               // await Task.Run(() => BoxMoFilesIntoSingleFileNew(CodeMoCollection, directoryInfoCollection, nameArchive));
            }
            catch (Exception ex)
            {
                AddToLogSafeThread($"Возникло исключение во формирования группированных сведений - {LibraryKurguzov.Class.Exception.ExceptionHanlder.GetAllMessages(ex)}");
            }
            finally
            {
                AddToLogSafeThread("Работа завершена");
            }
        });

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
                var mo = _dataBaseController.GetCodesMo().OrderBy(x => x).ToList();
                _settings.ProgramSettings.CodeMoCollection = mo;
                RefreshCodeMoCollection();

                void RefreshCodeMoCollection()
                {
                    CodeMoCollection.Clear();
                    foreach (var record in mo)
                    {
                        CodeMoCollection.Add(record);
                    }
                }
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
        private void BoxMoFilesIntoSingleFile()
        {

            var tempDirectory = _settings.ProgramSettings.TempDirectory;
            var outboxDirectory = _settings.ProgramSettings.OutputDirectory;
            var defaultGroup = _settings.ProgramSettings.UsingGroups.First(x => x.Id == _settings.ProgramSettings.DefaultGroupId);
            var boxingController = _factoryFileBoxingHandler.GetBoxingHandler(tempDirectory);
            GetClearExistingDirectory(tempDirectory);
            var filesCollector = GetPreparedFilesCollector();
            var dataMo = filesCollector.GetFilesForEntities();

            var moWithData = dataMo.Where(x => x.Value.Any()).ToList();
            foreach (var mo in moWithData)
            {
                var nameArchive = _settings.ProgramSettings.BoxAllGroupTypes ? $"Данные реестра {Year} {Month} для {mo.Key}" : FinalDefaultGroupName(mo.Key);
                boxingController.BoxFiles(mo.Value.ToList(), nameArchive);
            }

            var files = tempDirectory.GetFiles();
            OnLoggingFileCollector(this,
                $"Выгрузка данных завершена. Количество МО / всего МО - [{moWithData.Count}/{filesCollector.UniqueEntities.Count()}]. Количество архивов - {files.Length}");
            TryMoveFilesToFinalPlace();

            void GetClearExistingDirectory(DirectoryInfo directory)
            {
                if (directory.Exists)
                    directory.Delete(true);
                directory.Create();
            }
            IFilesCollector GetPreparedFilesCollector()
            {
                var collector = _factoryFileBoxingHandler.GetFilesCollector();
                collector.UniqueEntities = CodeMoCollection;
                collector.FileDirectoryInfo = _settings.ProgramSettings.BoxAllGroupTypes ? DirectoryFilesInfo.Where(x => x.IsEnabled) : DirectoryFilesInfo.Where(x => x.IsEnabled && x.IdUsingGroups.Contains(defaultGroup.Id));
                collector.OnLogging += OnLoggingFileCollector;
                return collector;
            }
            void OnLoggingFileCollector(object @object, string text)
            {
                AddToLogSafeThread(text);
            }
            string FinalDefaultGroupName(string codeMO)
            {
                return defaultGroup.FileNameArchive.Replace("![YEAR]!", $"{Year}").Replace("![MONTH]!", $"{Month}").Replace("![CODE_MO]!", codeMO);
            }

            void TryMoveFilesToFinalPlace()
            {
                if (!outboxDirectory.Exists)
                {
                    AddToLogSafeThread(
                        $"Каталог для финального расположения файлов не существует. Файлы хранятся во временном каталоге [{tempDirectory.FullName}]");
                    return;
                }
                MovingFilesToFinalPlace();

                void MovingFilesToFinalPlace()
                {
                    foreach (var file in files)
                        File.Move(file.FullName, Path.Combine(outboxDirectory.FullName, file.Name));
                    AddToLogSafeThread("Перемещение файлов завершено");
                }
            }
        }
    }
}