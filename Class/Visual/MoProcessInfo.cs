using FilesBoxing.Interface.Visual;

using LibraryKurguzov.Class.Utility;

namespace FilesBoxing.Class.Visual
{
    public class MoProcessInfo : BaseWpfNotifyPropertyHandler, IMoProcessInfo
    {
        private string _codeMo;
        private byte? _countFiles;
        private bool? _isPackageFileCreated;

        public string CodeMo
        {
            get => _codeMo;
            set
            {
                if (value == _codeMo) return;
                _codeMo = value;
                OnPropertyChanged();
            }
        }
        public byte? CountFiles
        {
            get => _countFiles;
            set
            {
                if (value == _countFiles) return;
                _countFiles = value;
                OnPropertyChanged();
            }
        }

        public bool? IsPackageFileCreated
        {
            get => _isPackageFileCreated;
            set
            {
                if (value == _isPackageFileCreated) return;
                _isPackageFileCreated = value;
                OnPropertyChanged();
            }
        }

        public MoProcessInfo(string codeMo)
        {
            CodeMo = codeMo;
            CountFiles = null;
            IsPackageFileCreated = false;
        }
    }
}