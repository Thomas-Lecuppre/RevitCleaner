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

        private string openText;

        public string OpenText
        {
            get { return openText; }
            set 
            { 
                openText = value; 
                NotifyPropertyChanged(nameof(OpenText));
            }
        }


        private ILanguage lang;

        public ILanguage Lang
        {
            get { return lang; }
            set 
            { 
                lang = value;
                OpenText = value.ListItemOpenButtonText;
            }
        }


        public ExplorerItem(MainPageViewModel mainPage, ILanguage lang)
        {
            _mainPage = mainPage;
            Lang = lang;
        }

        private MainPageViewModel _mainPage { get; set; }
    }
}
