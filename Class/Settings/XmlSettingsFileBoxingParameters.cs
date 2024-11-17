using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FilesBoxing.Interface;

namespace FilesBoxing.Class.Settings
{
    [XmlRoot(ElementName = "SettingsFileBoxing")]
    public class XmlSettingsFileBoxingParameters : ISettingsFileBoxingParameters
    {
        [XmlElement("ConnectionString")] 
        public string ConnectionString { get; set; }
        [XmlIgnore]
        public DirectoryInfo TempDirectory
        {
            get => new DirectoryInfo(TempDirectoryFullPath);
            set => TempDirectoryFullPath = value.FullName;
        }
        [XmlIgnore]
        public DirectoryInfo OutputDirectory
        {
            get => new DirectoryInfo(OutputDirectoryFullPath);
            set => OutputDirectoryFullPath = value.FullName;
        }

        [XmlElement(ElementName = "TempDirectory")]
        public string TempDirectoryFullPath { get; set; }
        [XmlElement(ElementName = "OutputDirectory")]
        public string OutputDirectoryFullPath { get; set; }

        [XmlIgnore]
        public IEnumerable<string> CodeMoCollection
        {
            get => XmlCodeMoCollection;
            set
            {
                if (value == null)
                    return;
                XmlCodeMoCollection = new List<string>(value);
            }
        }
        [XmlArray("CodeMoCollection")]
        [XmlArrayItem("CodeMo")]
        public List<string> XmlCodeMoCollection { get; set; }
        [XmlIgnore]
        public IEnumerable<IFileDirectoryInfo> FileDirectoriesInfo
        {
            get => XmlFileDirectoriesInfo;
            set
            {
                if (value == null)
                    return;
                XmlFileDirectoriesInfo = new List<XmlFileDirectoryInfo>(value.Select(x=> new XmlFileDirectoryInfo(x)));
            }
        }

        [XmlArray("FileDirectoriesInfo")]
        [XmlArrayItem("FD_Info", Type = typeof(XmlFileDirectoryInfo))]
        public List<XmlFileDirectoryInfo> XmlFileDirectoriesInfo { get; set; }
        [XmlElement(ElementName = "FileNameArchive")]
        public string FileNameArchive { get; set; }

        public XmlSettingsFileBoxingParameters(){}
        public XmlSettingsFileBoxingParameters(ISettingsFileBoxingParameters parameters)
        {
            ConnectionString = parameters.ConnectionString;
            TempDirectory = parameters.TempDirectory;
            OutputDirectory = parameters.OutputDirectory;
            CodeMoCollection = parameters.CodeMoCollection;
            FileDirectoriesInfo = parameters.FileDirectoriesInfo;
            FileNameArchive = parameters.FileNameArchive;
        }

        public class XmlFileDirectoryInfo : IFileDirectoryInfo
        {
            [XmlIgnore]
            public DirectoryInfo ParentFileDirectory
            {
                get => new DirectoryInfo(ParentFileDirectoryFullPath);
                set => ParentFileDirectoryFullPath = value.FullName;
            }
            [XmlElement(ElementName = "ParentFileDirectory")]
            public string ParentFileDirectoryFullPath;
            [XmlElement("IsEnabled")]
            public bool IsEnabled { get; set; }
            [XmlElement("ExtensionFile")]
            public string ExtensionFile { get; set; }
            public XmlFileDirectoryInfo()
            { }
            public XmlFileDirectoryInfo(IFileDirectoryInfo source)
            {
                ParentFileDirectory = source.ParentFileDirectory;
                IsEnabled = source.IsEnabled;
                ExtensionFile = source.ExtensionFile;
            }
        }
    }
}