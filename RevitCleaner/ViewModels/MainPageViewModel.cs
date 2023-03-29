using Microsoft.UI.Xaml.Automation.Peers;
using RevitCleaner.Strings;
using RevitCleaner.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Media.Audio;

namespace RevitCleaner
{
    public class MainPageViewModel : BaseViewModel
    {

        public MainPageViewModel(ILanguage lang)
        {
            ExplorerItems = new AsyncObservableCollection<ExplorerItem>();
            ShowedExplorerItems= new AsyncObservableCollection<ExplorerItem>();
            Lang = lang;
        }

        #region Propriétés

        private ILanguage lang;

        public ILanguage Lang
        {
            get { return lang; }
            set 
            { 
                lang = value;
                DisplayShowedCount();
                DisplaySelectedCount();
            }
        }


        private ObservableCollection<ExplorerItem>  explorerItems;

        /// <summary>
        /// Collection de l'entièreté des éléments.
        /// </summary>
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

        /// <summary>
        /// Collection des éléments visible dans l'interface.
        /// </summary>
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

        /// <summary>
        /// ToolTip de la boite de filtre.
        /// </summary>
        public string SearchToolTip
        {
            get { return searchToolTip; }
            set 
            { 
                searchToolTip = value; 
                NotifyPropertyChanged(nameof(SearchToolTip));
            }
        }

        private bool isCaseSensitiveOn;

        /// <summary>
        /// Détermine si la recherche sensible a la casse est active ou non.
        /// </summary>
        public bool IsCaseSensitiveOn
        {
            get { return isCaseSensitiveOn; }
            set 
            { 
                isCaseSensitiveOn = value; 
                NotifyPropertyChanged(nameof(IsCaseSensitiveOn));
            }
        }


        private string showedFilesCounter;

        /// <summary>
        /// Texte qui fait un résumé du nombre de fichiers affichés sur le nombre total de fichiers.
        /// </summary>
        public string ShowedFilesCounter
        {
            get { return showedFilesCounter; }
            set 
            { 
                showedFilesCounter = value; 
                NotifyPropertyChanged(nameof(ShowedFilesCounter));
            }
        }


        private string cleanButtonText;

        /// <summary>
        /// Texte du bouton pour nettoyer les fichiers.
        /// </summary>
        public string CleanButtonText
        {
            get 
            { 
                return cleanButtonText; 
            }
            set 
            { 
                cleanButtonText = value; 
                NotifyPropertyChanged(nameof(CleanButtonText));
            }
        }

        private bool enableControls;

        /// <summary>
        /// Déterminé l'état dan lequel se trouve les boutons relatif à la gestion de la liste de fichiers. Activé ou non.
        /// </summary>
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

        /// <summary>
        /// Détermine l'état dans lequel le bouton pour nettoyer se trouve. Activé ou non.
        /// </summary>
        public bool ClearButtonState
        {
            get { return clearButtonState; }
            set 
            { 
                clearButtonState = value; 
                NotifyPropertyChanged(nameof(ClearButtonState));
            }
        }

        #endregion

        public void DisplayShowedCount()
        {
            string totalCount = "";
            if(ExplorerItems.Count <= 0)
            {
                ShowedFilesCounter = Lang.StrNoFile;
            }
            else
            {
                totalCount = ExplorerItems.Count > 1 ? $" / {ExplorerItems.Count} {Lang.StrFiles}" : $" / 1 {Lang.StrFile}";

                int count = ShowedExplorerItems.Count;
                try
                {
                    ClearButtonState = count > 0;
                    if (count <= 0)
                    {
                        ShowedFilesCounter = Lang.StrNoFileToClean + totalCount;
                    }
                    else if (count == 1)
                    {
                        ShowedFilesCounter = Lang.StrOneShowedFile + totalCount;
                    }
                    else
                    {
                        ShowedFilesCounter = Lang.StrMultipleShowedFile + totalCount;
                        ShowedFilesCounter = ShowedFilesCounter.Replace("$1", count.ToString());
                    }
                }
                catch { }
            }
        }

        public void DisplaySelectedCount()
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

            try
            {
                ClearButtonState = count > 0;
                if (count <= 0)
                {
                    CleanButtonText = Lang.StrNoFileToClean;
                }
                else if (count == 1)
                {
                    CleanButtonText = Lang.StrOneFileToClean
                        .Replace("$1", ReduceSize(size));
                }
                else
                {
                    CleanButtonText = Lang.StrMultipleToClean;
                    CleanButtonText = CleanButtonText.Replace("$1", count.ToString());
                    CleanButtonText = CleanButtonText.Replace("$2", ReduceSize(size));
                }
            }
            catch { }
        }

        private static string ReduceSize(long size)
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
