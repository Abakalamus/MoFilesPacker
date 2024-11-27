using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FilesBoxing.Interface.BusinessLogic;
using FilesBoxing.Interface.Settings;

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
        public IEnumerable<ITypeGroupingSettings> UsingGroups
        {
            get => XmlUsingGroups;
            set
            {
                if (value == null)
                    return;
                XmlUsingGroups = new List<XmlTypeGroupingSettings>(value.Select(x => new XmlTypeGroupingSettings(x)));
            }
        }
        [XmlArray("UsingArchiveGroups")]
        [XmlArrayItem("GroupInfo", Type = typeof(XmlTypeGroupingSettings))]
        public List<XmlTypeGroupingSettings> XmlUsingGroups { get; set; }
        [XmlElement(ElementName = "DefaultGroupId")]
        public int DefaultGroupId { get; set; }
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

        public XmlSettingsFileBoxingParameters(){}
        public XmlSettingsFileBoxingParameters(ISettingsFileBoxingParameters parameters)
        {
            ConnectionString = parameters.ConnectionString;
            TempDirectory = parameters.TempDirectory;
            OutputDirectory = parameters.OutputDirectory;
            CodeMoCollection = parameters.CodeMoCollection;
            FileDirectoriesInfo = parameters.FileDirectoriesInfo;
            UsingGroups = parameters.UsingGroups;
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
            //[XmlElement("IsEnabled")]
            //public bool IsEnabled { get; set; }
            [XmlElement("ExtensionFile")]
            public string ExtensionFile { get; set; }
            [XmlIgnore]
            public IEnumerable<int> IdUsingGroups
            {
                get => XmlIdUsingGroups;
                set
                {
                    if (value == null)
                        return;
                    XmlIdUsingGroups = new List<int>(value);
                }
            }
            [XmlArray("UsingGroups")]
            [XmlArrayItem("IdGroup")]
            public List<int> XmlIdUsingGroups { get; set; }
            public XmlFileDirectoryInfo()
            { }
            public XmlFileDirectoryInfo(IFileDirectoryInfo source)
            {
                ParentFileDirectory = source.ParentFileDirectory;
              //  IsEnabled = source.IsEnabled;
                ExtensionFile = source.ExtensionFile;
                IdUsingGroups = source.IdUsingGroups;
            }
        }

        public class XmlTypeGroupingSettings : ITypeGroupingSettings
        {
            [XmlElement(ElementName = "Id")]
            public int Id { get; set; }
            [XmlElement(ElementName = "Name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "FileNameArchive")]
            public string FileNameArchive { get; set; }

            public XmlTypeGroupingSettings()
            {
            }

            public XmlTypeGroupingSettings(ITypeGroupingSettings source)
            {
                Id = source.Id;
                Name = source.Name;
                FileNameArchive = source.FileNameArchive;
            }
        }
    }
}