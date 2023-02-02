using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using System.Threading;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RevitCleaner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainWindow mainWindow { get; set; }
        public MainPageViewModel ViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            // New ViewModel parse to Page DataContext
            ViewModel= new MainPageViewModel();
            this.DataContext = ViewModel;
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker picker = new();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            var hwnd = WindowNative.GetWindowHandle(mainWindow);
            InitializeWithWindow.Initialize(picker, hwnd);

            StorageFolder dir = await picker.PickSingleFolderAsync();

            if(dir != null && Directory.Exists(dir.Path))
            {
                DirectoryTextBox.Text= dir.Path;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Si le filtre est convenable.
            string s = SearchTextBox.Text;
            if (string.IsNullOrEmpty(s) && !s.Contains('\\'))
            {
                // On filtre ici la liste.
            }
        }

        private void CaseSensitiveToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteFilesButton_Click(object sender, RoutedEventArgs e)
        {
            if(DeleteFilesButton.Content.ToString() == "Nettoyer mes fichiers")
            {
                DeleteFilesButton.Content = "Presser une seconde fois pour confirmer";
            }
            else
            {
                DeleteSelectedFiles();
                DeleteFilesButton.Content = "Nettoyer mes fichiers";
            }
        }

        private void DirectoryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(DirectoryTextBox.Text))
            {
                // Looking for all the files in folder.

                ParseFilesToUI(DirectoryTextBox.Text, SearchTextBox.Text, CaseSensitiveToggleSwitch.IsOn);
            }
        }

        public int CountExtanded()
        {
            return FilesTreeView.SelectedItems.Count;
        }


        #region Files Analysis

        /// <summary>
        /// Détermine si l'extension est une extension Revit.
        /// </summary>
        /// <param name="file">EXTENSION du fichier.</param>
        /// <returns>True si le fichier est un fichier Revit, sinon False.</returns>
        private bool IsRevitFile(string file)
        {
            return file == ".rvt" || file == ".rte" || file == ".rfa" || file == ".rft";
        }

        /// <summary>
        /// Déterminé si le fichier est un fichier de sauvegarde.
        /// </summary>
        /// <param name="file">Chemin du fichier complet.</param>
        /// <returns>True si le fichier est un fichier de sauvegarde, sinon False.</returns>
        private bool IsSaveFile(string file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);

            if (string.IsNullOrEmpty(fileName)) return false;
            if (!fileName.Contains('.')) return false;

            string lastComponent = file.Substring(fileName.LastIndexOf("."));

            if (lastComponent.Length != 4) return false;
            if (int.TryParse(lastComponent, out int i)) return true;
            else return false;
        }

        private List<string> GetFolderFilter(string filter)
        {
            List<string> folders = new List<string>();
            if (!filter.Contains('@')) return folders;

            if (filter.Contains(','))
            {
                string[] component = filter.Split(',');
                foreach (string s in component)
                {
                    if (s.StartsWith("@"))
                    {
                        string f = s.Remove(0, 1);
                        f = f.Trim();
                        folders.Add(f);
                    }
                }
            }
            else
            {
                if (filter.StartsWith("@"))
                {
                    string f = filter.Remove(0, 1);
                    f = f.Trim();
                    folders.Add(f);
                }
            }

            return folders;
        }

        private List<string> GetFileFilter(string filter)
        {
            List<string> files = new List<string>();

            if (filter.Contains(','))
            {
                string[] component = filter.Split(',');
                foreach (string s in component)
                {
                    if (s.StartsWith("@")) files.Add(s.Trim());
                }
            }
            else if (!filter.StartsWith("@")) files.Add(filter.Trim());


            return files;
        }

        #endregion

        #region Data To UI

        /// <summary>
        /// Met à jour la liste des dossiers dans lesquels se trouvent des fichiers de sauvegarde.
        /// </summary>
        /// <param name="folderPath">Chemin du dossier inital.</param>
        /// <param name="filter">Filtre de recherche.</param>
        /// <param name="caseSensitive">La recherche est-elle sensible à la casse ?</param>
        public void ParseFilesToUI(string folderPath, string filter, bool caseSensitive)
        {
            // Si le dossier choisit n'existe pas alors on ne cherche pas.
            if (!Directory.Exists(folderPath))
            {
                return;
            }

            List<string> folderFilter = GetFolderFilter(filter);
            List<string> fileFilter = GetFileFilter(filter);

            // Réinitialisation des noeuds précédents.
            ViewModel.ExplorerItems.Clear();

            // On ajoute le dossier de recherche comme premier noeud.
            ExplorerItem mainDir = new ExplorerItem()
            {
                Name = new DirectoryInfo(folderPath).Name,
                Path = folderPath,
                IsExpanded = true,
                IsSelected = true,
                Type = ExplorerItem.ExplorerItemType.Folder
            };

            ViewModel.ExplorerItems.Add(mainDir);

            // Pour chaque dossier du dossier on l'affiche dans l'arbre de la page principale.
            foreach (ExplorerItem item in FilesInFolder(folderPath, folderFilter, fileFilter, caseSensitive, false))
            {
                mainDir.Children.Add(item);
            }
        }

        /// <summary>
        /// On recupère chaque dossier du dossier actif. On ajoute tous ces dossiers sur la page principale.
        /// </summary>
        /// <param name="folderPath">Chemin du dossier visé.</param>
        /// <param name="filter">Filtre de recherche.</param>
        /// <param name="caseSensitive">La recherche est-elle sensible à la casse ?</param>
        /// <param name="isExpanded">Le noeud doit-il être étendu ?</param>
        /// <returns>Une liste de noeuds.</returns>
        private ObservableCollection<ExplorerItem> FilesInFolder(string folderPath, List<string> foldersFilter, List<string> namesFilter, bool caseSensitive, bool isExpanded)
        {
            ObservableCollection<ExplorerItem> list = new ObservableCollection<ExplorerItem>();

            // Ajout des dossiers.
            foreach (string dir in Directory.GetDirectories(folderPath))
            {
                ExplorerItem d = new ExplorerItem()
                {
                    Name = new DirectoryInfo(dir).Name,
                    Path = dir,
                    IsExpanded = isExpanded,
                    Type = ExplorerItem.ExplorerItemType.Folder
                };


                foreach (ExplorerItem item in FilesInFolder(dir, foldersFilter, namesFilter, caseSensitive, false))
                {
                    //LoadingPage.UpdateTextBox($"Ajout de {item.Name}.");
                    d.Children.Add(item);
                }

                list.Add(d);
            }

            // Ajout des fichiers.
            foreach (string file in Directory.GetFiles(folderPath))
            {
                ExplorerItem item = new ExplorerItem()
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    Path = file,
                };

                string ext = Path.GetExtension(file);

                // Défini l'icone pour chaque fichier. Si le fichier ne correspond pas au cas indiqué alors on le passe.
                switch (ext)
                {
                    case ".rfa":
                        {
                            item.Type = ExplorerItem.ExplorerItemType.RFAFile;
                            break;
                        }
                    case ".rft":
                        {
                            item.Type = ExplorerItem.ExplorerItemType.RFTFile;
                            break;
                        }
                    case ".rte":
                        {
                            item.Type = ExplorerItem.ExplorerItemType.RTEFile;
                            break;
                        }
                    case ".rvt":
                        {
                            item.Type = ExplorerItem.ExplorerItemType.RVTFile;
                            break;
                        }
                    default:
                        {
                            continue;
                        }
                }

                list.Add(item);
            }

            return list;
        }

        #endregion


        /// <summary>
        /// Fonction de teste qui permet de tester différents processus de manipulation des listes 
        /// et collections dans winui3.
        /// </summary>
        /// <param name="gCount"></param>
        private void DeleteSelectedFiles()
        {
            foreach (ExplorerItem child in ViewModel.ExplorerItems)
            {
                if (child.Type == ExplorerItem.ExplorerItemType.Folder)
                {
                    DeleteSelectedFiles(child);
                }
                else
                {
                    try
                    {
                        File.Delete(child.Path);
                    }
                    catch { }
                }
            }
        }

        private void DeleteSelectedFiles(ExplorerItem item)
        {
            foreach (ExplorerItem child in item.Children)
            {
                if (child.Type == ExplorerItem.ExplorerItemType.Folder)
                {
                    DeleteSelectedFiles(child);
                }
                else
                {
                    try
                    {
                        File.Delete(child.Path);
                    }
                    catch { }
                }
            }
        }
    }
}
