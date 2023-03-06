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
            ExplorerItems = new ObservableCollection<ExplorerItem>();
            ShowedExplorerItems= new ObservableCollection<ExplorerItem>();
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



        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
