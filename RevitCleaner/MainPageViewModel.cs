using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Audio;

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
            long size = 0;
            foreach(ExplorerItem item in ExplorerItems)
            {
                if(item.IsSelected)
                {
                    count ++;
                    size += item.Size;
                }
            }

            ClearButtonState = count > 0;
            if (count <= 0)
            {
                FileCounter = $"Auncun fichier sélectionné";
            }
            else if (count == 1)
            {
                FileCounter = $"Nettoyer 1 fichier - {ReduceSize(size)}";
            }
            else
            {
                FileCounter = $"Nettoyer {count} fichiers - {ReduceSize(size)}";
            }
        }

        private string ReduceSize(long size)
        {
            int step = 0;
            long ajustedSize = size;

            while(ajustedSize > 1024 && step <= 4)
            {
                step++;
                ajustedSize = ajustedSize / 1024;
            }

            switch(step)
            {
                case 4:
                    return $"{ajustedSize} To";
                case 3:
                    return $"{ajustedSize} Go";
                case 2:
                    return $"{ajustedSize} Mo";
                case 1:
                    return $"{ajustedSize} Ko";
                default:
                    return $"{ajustedSize} o";
            }
        }
    }
}
