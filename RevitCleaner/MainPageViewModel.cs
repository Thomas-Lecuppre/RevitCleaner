using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitCleaner
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            ExplorerItems = new AsyncObservableCollection<ExplorerItem>();
            ShowedExplorerItems= new AsyncObservableCollection<ExplorerItem>();
        }

        private ObservableCollection<ExplorerItem>  explorerItems;

        public ObservableCollection<ExplorerItem> ExplorerItems
        {
            get { return explorerItems; }
            set 
            { 
                explorerItems = value;
                NotifyPropertyChanged(nameof(ExplorerItems));
            }
        }

        private ObservableCollection<ExplorerItem> showedExplorerItems;

        public ObservableCollection<ExplorerItem> ShowedExplorerItems
        {
            get { return showedExplorerItems; }
            set 
            { 
                showedExplorerItems = value; 
                NotifyPropertyChanged(nameof(ShowedExplorerItems));
            }
        }

        private string searchToolTip;

        public string SearchToolTip
        {
            get { return searchToolTip; }
            set 
            { 
                searchToolTip = value; 
                NotifyPropertyChanged(nameof(SearchToolTip));
            }
        }

        private string fileCounter;

        public string FileCounter
        {
            get 
            { 
                return fileCounter; 
            }
            set 
            { 
                fileCounter = value; 
                NotifyPropertyChanged(nameof(FileCounter));
            }
        }

        private bool enableControls;

        public bool EnableControls
        {
            get { return enableControls; }
            set 
            { 
                enableControls = value; 
                NotifyPropertyChanged(nameof(EnableControls));
            }
        }

        private bool clearButtonState;

        public bool ClearButtonState
        {
            get { return clearButtonState; }
            set 
            { 
                clearButtonState = value; 
                NotifyPropertyChanged(nameof(ClearButtonState));
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CountSelected()
        {
            int count = ExplorerItems.Where(x => x.IsSelected).Count();
            ClearButtonState = count > 0;
            if (count <= 0)
            {
                FileCounter = $"Auncun fichier sélectionné";
            }
            else if (count == 1)
            {
                FileCounter = "Nettoyer 1 fichier ";
            }
            else
            {
                FileCounter = $"Nettoyer {count} fichiers";
            }
        }
    }
}
