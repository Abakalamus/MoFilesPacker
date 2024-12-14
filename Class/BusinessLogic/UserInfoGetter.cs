using FilesBoxing.Interface.BusinessLogic;

using System;
using System.Windows;
using System.Windows.Forms;

namespace FilesBoxing.Class.BusinessLogic
{
    public class UserInfoGetter : IUserInfoGetter
    {
        public string DirectoryOutputPathByUserChoise(string defaultPath)
        {
            var resultChosingOutputDirectory = UserWantUseDefaultOutputDirectoryMessageResult();
            return OutputDirectoryChoiseCanceled() ? string.Empty : GetDirectoryByUserChoise();

            MessageBoxResult UserWantUseDefaultOutputDirectoryMessageResult()
            {
                return System.Windows.MessageBox.Show(
                    $"Сохранить файлы в каталог, указанный в настройках [{defaultPath}]?{Environment.NewLine}{Environment.NewLine}нет - выбрать каталог вручную",
                    "Выбор каталога для сохранения файлов", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);
            }
            string GetDirectoryByUserChoise()
            {
                var result = string.Empty;
                switch (resultChosingOutputDirectory)
                {
                    case MessageBoxResult.Yes:
                        result = defaultPath;
                        break;
                    case MessageBoxResult.No:
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            var choseFolderDialog = new FolderBrowserDialog();
                            if (choseFolderDialog.ShowDialog().ToString() == DialogResult.OK.ToString())
                                result = choseFolderDialog.SelectedPath;
                        });
                        break;
                    case MessageBoxResult.None:
                    case MessageBoxResult.OK:
                    case MessageBoxResult.Cancel:
                    default: break;
                }

                return result;
            }
            bool OutputDirectoryChoiseCanceled()
            {
                return resultChosingOutputDirectory == MessageBoxResult.Cancel;
            }
        }

        public bool IsUserWantOpenInExplorerPath(string path)
        {
            return System.Windows.MessageBox.Show($"Показать созданные файлы в указанном каталоге [{path}]?", "Показать файлы",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes;
        }
    }
}