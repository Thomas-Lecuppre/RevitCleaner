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
using System.Security.Permissions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using System.Threading;
using System.Reflection.Metadata.Ecma335;
using Windows.Media.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RevitCleaner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainWindow MainWindowView { get; set; }
        public MainPageViewModel ViewModel { get; set; }
        private List<string> DirectoryFilter { get; set; }
        private List<string> FileFilter { get; set; }
        private bool IsCaseSensitive { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            // New ViewModel parse to Page DataContext
            ViewModel= new MainPageViewModel();
            this.DataContext = ViewModel;

            DirectoryFilter = new List<string>();
            FileFilter = new List<string>();

            CaseSensitiveToggleSwitch.IsOn = true;
            UpDateFilterData();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker picker = new();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            var hwnd = WindowNative.GetWindowHandle(MainWindowView);
            InitializeWithWindow.Initialize(picker, hwnd);

            StorageFolder dir = await picker.PickSingleFolderAsync();

            if(dir != null && Directory.Exists(dir.Path))
            {
                DirectoryTextBox.Text= dir.Path;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpDateFilterData();
            ParseFilesToUI(DirectoryTextBox.Text);
        }

        private void CaseSensitiveToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            UpDateFilterData();
            ParseFilesToUI(DirectoryTextBox.Text);
        }

        private void DeleteFilesButton_Click(object sender, RoutedEventArgs e)
        {
            if(DeleteFilesButton.Content.ToString() == "Nettoyer mes fichiers")
            {
                DeleteFilesButton.Content = "Presser une seconde fois pour confirmer";
            }
            else
            {
                ClearInfoMessage();
                DeleteSelectedFiles();
                DeleteFilesButton.Content = "Nettoyer mes fichiers";
            }
        }

        private void DirectoryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(DirectoryTextBox.Text))
            {
                // Looking for all the files in folder.
                UpDateFilterData();
                ParseFilesToUI(DirectoryTextBox.Text);
            }
            else
            {
                // Clearing existing UI items.
                ViewModel.ExplorerItems.Clear();
            }
        }

        public void CountSelected()
        {
            int count = FilesTreeView.SelectedItems.Count;
            AddInfoMessage("Debug", count.ToString(), InfoBarSeverity.Informational);
        }

        #region Files Analysis

        /// <summary>
        /// Return a list of FileInfo that are Revit files and saved files. All other file type will be ignored.
        /// </summary>
        /// <param name="folderPath">Path to folder where to search.</param>
        /// <returns>List of FileInfo that are Revit and saved files.</returns>
        private List<FileInfo> GetFiles(string folderPath, bool allDir)
        {
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            List<FileInfo> files = allDir ? dir.GetFiles("*", SearchOption.AllDirectories).ToList() : dir.GetFiles("*", SearchOption.TopDirectoryOnly).ToList();
            List<FileInfo> validFiles = new List<FileInfo>();

            foreach(FileInfo fi in files)
            {
                // If not revit file and not a saved file then go to the next.
                if (!IsRevitFile(fi.Extension)) continue;
                if (!IsSaveFile(fi.FullName)) continue;
                // There is no filter, simply add file to the list.
                if (DirectoryFilter.Count <= 0 && FileFilter.Count <= 0)
                {
                    validFiles.Add(fi);
                    continue;
                }

                string filePath = IsCaseSensitive ? fi.DirectoryName : fi.DirectoryName.ToLower();
                string fileName = fi.Name.Replace(fi.Extension, "");
                fileName = IsCaseSensitive ? fileName : fileName.ToLower();
                List<bool> filterResult = new List<bool>();

                foreach(string d in DirectoryFilter)
                {
                    string str = IsCaseSensitive ? d : d.ToLower();
                    filterResult.Add(filePath.Contains(str));
                }

                // If all directory filter aren't correct then pass to the next file.
                if (filterResult.Where(x => x == true).Count() != DirectoryFilter.Count) continue;

                filterResult.Clear();

                foreach (string d in FileFilter)
                {
                    string str = IsCaseSensitive ? d : d.ToLower();
                    filterResult.Add(fileName.Contains(str));
                }

                // If all file name filter are correct then add the file to valid list.
                if (filterResult.Where(x => x == true).Count() == FileFilter.Count) validFiles.Add(fi);

            }

            return validFiles;
        }

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

            string lastComponent = fileName.Substring(fileName.LastIndexOf(".") + 1);

            if (lastComponent.Length != 4) return false;
            return int.TryParse(lastComponent, out int i);
        }

        private void UpDateFilterData()
        {
            IsCaseSensitive = CaseSensitiveToggleSwitch.IsOn;
            string filter = SearchTextBox.Text;
            if (string.IsNullOrEmpty(filter))
            {
                DirectoryFilter.Clear();
                FileFilter.Clear();
            }
            else
            {
                DirectoryFilter = GetFolderFilter(filter);
                FileFilter = GetFileFilter(filter);
            }
        }

        private List<string> GetFolderFilter(string filter)
        {
            List<string> folders = new List<string>();
            if (string.IsNullOrWhiteSpace(filter)) return folders;
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
                    if (!s.StartsWith("@")) files.Add(s.Trim());
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
        public void ParseFilesToUI(string folderPath)
        {
            // If directory doesn't exist then exit.
            if (!Directory.Exists(folderPath)) return;

            // Clearing existing UI items.
            ViewModel.ExplorerItems.Clear();

            // Add main directory as root folder in treeview.
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
            foreach (ExplorerItem item in FilesInFolder(folderPath, false))
            {
                mainDir.Children.Add(item);
            }

            CountSelected();
        }

        /// <summary>
        /// On recupère chaque dossier du dossier actif. On ajoute tous ces dossiers sur la page principale.
        /// </summary>
        /// <param name="folderPath">Chemin du dossier visé.</param>
        /// <param name="filter">Filtre de recherche.</param>
        /// <param name="caseSensitive">La recherche est-elle sensible à la casse ?</param>
        /// <param name="isExpanded">Le noeud doit-il être étendu ?</param>
        /// <returns>Une liste de noeuds.</returns>
        private ObservableCollection<ExplorerItem> FilesInFolder(string folderPath, bool isExpanded)
        {
            // Initializing list
            ObservableCollection<ExplorerItem> list = new ObservableCollection<ExplorerItem>();
            string[] subDirs = new string[] { };

            // Trying to get sub directory.
            // This was done like this because on the author pc, a direcotry was found whereas it doesn't exist on the pc.
            // If that occure again, application won't crash.
            try
            {
                subDirs = Directory.GetDirectories(folderPath);
            }
            catch (DirectoryNotFoundException dnfex)
            {
                AddInfoMessage("Directory Not Found Exception", dnfex.Message, InfoBarSeverity.Warning);
            }
            catch (IOException ioex)
            {
                AddInfoMessage("IO Exception", ioex.Message, InfoBarSeverity.Error);
                return list;
            }
            catch (Exception ex)
            {
                AddInfoMessage("Exception", ex.Message, InfoBarSeverity.Error);
                return list;
            }
            finally { }

            // Add folder to treeview if it contain children.
            // Because this method turn in a loop from top directory to the farest one, if there is no child there is no file
            // so we don't need to show this.
            foreach (string dir in subDirs)
            {
                ObservableCollection<ExplorerItem> dirFiles = FilesInFolder(dir, false);

                // Directory and subdirectoies contain at least 1 save file so we add the folder.
                if (dirFiles != null && dirFiles.Count > 0)
                {
                    ExplorerItem d = new ExplorerItem()
                    {
                        Name = new DirectoryInfo(dir).Name,
                        Path = dir,
                        IsExpanded = isExpanded,
                        Type = ExplorerItem.ExplorerItemType.Folder
                    };

                    // Repeating the process.
                    foreach (ExplorerItem item in dirFiles)
                    {
                        d.Children.Add(item);
                    }

                    list.Add(d);
                }
            }

            // Get Files in current root folder.
            List<FileInfo> curDirFiles = GetFiles(folderPath, false);
            // Add them in UI
            foreach (FileInfo fi in curDirFiles)
            {
                ExplorerItem item = new ExplorerItem()
                {
                    Name = fi.Name.Replace(fi.Extension, ""),
                    Path = fi.FullName,
                };
                item.Type = GetUIFileType(fi);

                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// Return an Explorer Item Type according to the file extension.
        /// </summary>
        /// <param name="file">The target file.</param>
        /// <returns>An Explorer Item Type</returns>
        private ExplorerItem.ExplorerItemType GetUIFileType(FileInfo file)
        {
            // Choose icon in funtion of file type.
            switch (file.Extension)
            {
                case ".rfa":
                    {
                        return ExplorerItem.ExplorerItemType.RFAFile;
                    }
                case ".rft":
                    {
                        return ExplorerItem.ExplorerItemType.RFTFile;
                    }
                case ".rte":
                    {
                        return ExplorerItem.ExplorerItemType.RTEFile;
                    }
                case ".rvt":
                    {
                        return ExplorerItem.ExplorerItemType.RVTFile;
                    }
                default:
                    {
                        return ExplorerItem.ExplorerItemType.UnknowFile;
                    }
            }
        }

        /// <summary>
        /// Handle the error as an InfoBar direclty in UI.
        /// </summary>
        /// <param name="title">Title of message.</param>
        /// <param name="message">Message.</param>
        /// <param name="infoBarSeverity">The severity level.</param>
        private void AddInfoMessage(string title, string message, InfoBarSeverity infoBarSeverity)
        {
            InfoBar ib = new InfoBar()
            {
                Title = title,
                Message = message,
                Severity = infoBarSeverity,
                IsOpen = true
            };
            ProgressBar pb = new ProgressBar();

            ib.Content = pb;
            AlertPanel.Children.Add(ib);
            AutomaticallyCloseAlerte(ib, pb);
        }

        /// <summary>
        /// Clear ALL InfoBar in UI.
        /// </summary>
        private void ClearInfoMessage()
        {
            AlertPanel.Children.Clear();
        }

        /// <summary>
        /// Task that automatically close InfoBar after a certain time.
        /// </summary>
        /// <param name="infoBar">The InfoBar to close.</param>
        /// <param name="progressBar">The ProgressBar to update.</param>
        private void AutomaticallyCloseAlerte(InfoBar infoBar, ProgressBar progressBar)
        {
            Task.Run(() =>
            {
                // Determine cooldown before infobar disappear.
                int countMax = 0;
                InfoBarSeverity severity = InfoBarSeverity.Informational;
                // update progressbar values thread safely
                if (this.DispatcherQueue.HasThreadAccess)
                {
                    severity = infoBar.Severity;
                }
                else
                {
                    this.DispatcherQueue.TryEnqueue(
                        Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                        () =>
                        {
                            severity = infoBar.Severity;
                        });
                }

                switch (severity)
                {
                    case InfoBarSeverity.Success:
                        {
                            countMax = 2;
                            break;
                        }
                    case InfoBarSeverity.Informational:
                        {
                            countMax = 5;
                            break;
                        }
                    case InfoBarSeverity.Warning:
                        {
                            countMax = 10;
                            break;
                        }
                    case InfoBarSeverity.Error:
                        {
                            countMax = 15;
                            break;
                        }
                }

                // Converting second to millisecond
                countMax = countMax * 1000;
                int actualValue = 0;

                // update progressbar values thread safely
                if (this.DispatcherQueue.HasThreadAccess)
                {
                    progressBar.Maximum = countMax;
                    progressBar.Minimum = 0;
                    progressBar.Value = actualValue;
                }
                else
                {
                    this.DispatcherQueue.TryEnqueue(
                        Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                        () =>
                        {
                            progressBar.Maximum = countMax;
                            progressBar.Minimum = 0;
                            progressBar.Value = actualValue;
                        });
                }

                while (actualValue < countMax)
                {
                    // update progressbar value thread safely
                    if (this.DispatcherQueue.HasThreadAccess)
                    {
                        progressBar.Value = actualValue;
                    }
                    else
                    {
                        this.DispatcherQueue.TryEnqueue(
                            Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                            () =>
                            {
                                progressBar.Value = actualValue;
                            });
                    }

                    Thread.Sleep(100);
                    actualValue += 100;
                }
                if (this.DispatcherQueue.HasThreadAccess)
                {
                    infoBar.IsOpen = false;
                    AlertPanel.Children.Remove(infoBar);
                }
                else
                {
                    this.DispatcherQueue.TryEnqueue(
                        Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                        () =>
                        {
                            infoBar.IsOpen = false;
                            AlertPanel.Children.Remove(infoBar);
                        });
                }
            });
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
                    if (child.IsSelected && File.Exists(child.Path))
                    {
                        try
                        {
                            File.Delete(child.Path);
                        }
                        catch { }
                    }
                }
            }

            UpDateFilterData();
            ParseFilesToUI(DirectoryTextBox.Text);
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
                    if(child.IsSelected && File.Exists(child.Path))
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
}
