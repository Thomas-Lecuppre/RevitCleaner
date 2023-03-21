using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace RevitCleaner
{
    public class ExplorerItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public enum ExplorerItemType { Folder, RFAFile, RFTFile, RTEFile, RVTFile, UnknowFile };
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public ExplorerItemType Type { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                NotifyPropertyChanged("IsSelected");
                _mainPage.CountSelected();
            }

        }

        public bool IsShowed { get; set; }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ExplorerItem(MainPageViewModel mainPage)
        {
            _mainPage = mainPage;
        }

        private MainPageViewModel _mainPage { get; set; }
    }
}
