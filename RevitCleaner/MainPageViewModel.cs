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
        }

        private ObservableCollection<ExplorerItem>  explorerItems;

        public ObservableCollection<ExplorerItem> ExplorerItems
        {
            get { return explorerItems; }
            set 
            { 
                explorerItems = value;
                NotifyPropertyChanged("ExplorerItems");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
