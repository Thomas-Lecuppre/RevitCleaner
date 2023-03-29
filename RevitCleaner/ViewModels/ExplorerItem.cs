using RevitCleaner.Strings;
using RevitCleaner.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace RevitCleaner
{
    public class ExplorerItem : BaseViewModel
    {
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
                _mainPage.DisplaySelectedCount();
            }

        }

        public bool IsShowed { get; set; }

        public ILanguage Lang { get; set; }

        public ExplorerItem(MainPageViewModel mainPage, ILanguage lang)
        {
            _mainPage = mainPage;
            Lang = lang;
        }

        private MainPageViewModel _mainPage { get; set; }
    }
}
