using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FilesBoxing.Interface;

using LibraryKurguzov.Class.Command;
using System.Windows.Input;
using LibraryKurguzov.Class.Utility;
using FilesBoxing.Class;

namespace FilesBoxing.ViewModel
{
    public class MainViewModel : BaseWpfNotifyPropertyHandler
    {
        private readonly IFactory _factory;
        private readonly ISettings _settings;
        public ObservableCollection<string> LogCollection { get; set; }

        public MainViewModel(IFactory factory, ISettings settings)
        {
            _factory = factory;
            LogCollection = new ObservableCollection<string>();
            this._settings = settings;
        }

        public ICommand TestCommand => new RelayCommand(async obj =>
        {
            try
            {
                var tempDirectory = _settings.TempDirectory;
                GetClearExistingDirectory(tempDirectory);

                var filesCollector = _factory.GetFilesCollector();
                filesCollector.UniqueEntities = new List<string> { "750001", "750002" };
                filesCollector.FileDirectoryInfo = new List<IFileDirectoryInfo>
                {
                    new FileDirectoryInfo(new DirectoryInfo(@"C:\Users\Abakalamus\Desktop\tmpZIP"), ".TXT")
                };
                var dataMo = filesCollector.GetFilesForEntities();
                var boxingController = _factory.GetBoxingHandler(tempDirectory);
                var moWithData = dataMo.Where(x => x.Value.Any()).ToList();
                foreach (var mo in moWithData)
                    boxingController.BoxFiles(mo.Value.ToList(), mo.Key);
                var files = tempDirectory.GetFiles();
                LogCollection.Add(
                    $"Выгрузка данных завершена. Количество МО / всего МО - [{moWithData.Count}/{filesCollector.UniqueEntities.Count()}]. Количество архивов - {files.Length}");
                var outboxDirectory = _settings.OutputDirectory;
                if (!outboxDirectory.Exists)
                {
                    LogCollection.Add(
                        $"Каталог для финального расположения файлов не существует. Файлы хранятся во временном каталоге [{tempDirectory.FullName}]");
                    return;
                }

                foreach (var file in files)
                    File.Move(file.FullName, Path.Combine(outboxDirectory.FullName, file.Name));
                LogCollection.Add("Перемещение файлов завершено");
            }
            catch (Exception ex)
            {
                LogCollection.Add($"Возникло исключение во формирования группированных сведений - {LibraryKurguzov.Class.Exception.ExceptionHanlder.GetAllMessages(ex)}");
            }
            finally
            {
                LogCollection.Add("Работа завершена");
            }

            void GetClearExistingDirectory(DirectoryInfo tempDirectory)
            {
                if (tempDirectory.Exists)
                    tempDirectory.Delete(true);
                tempDirectory.Create();
            }
        });
    }
}