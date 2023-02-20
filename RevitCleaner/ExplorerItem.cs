using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RevitCleaner
{
    public class ExplorerItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public enum ExplorerItemType { Folder, RFAFile, RFTFile, RTEFile, RVTFile, UnknowFile };
        public string Name { get; set; }
        public string Path { get; set; }
        public ExplorerItemType Type { get; set; }

        private bool m_isSelected;
        public bool IsSelected
        {
            get { return m_isSelected; }

            set
            {
                if (m_isSelected != value)
                {
                    m_isSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }

        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
