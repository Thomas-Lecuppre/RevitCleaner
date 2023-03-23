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
using System.Diagnostics;
using Windows.System;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Net;
using static RevitCleaner.ExplorerItem;
using Microsoft.WindowsAPICodePack.Dialogs;

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

        // Listes of different kind of filter.
        private List<string> ListDirectoryFilter { get; set; }
        public List<string> ListStrictDirectoryFilter { get; set; }
        private List<string> ListAntiDirectoryFilter { get; set; }
        public List<string> ListAntiStrictDirectoryFilter { get; set; }
        private List<string> ListFileFilter { get; set; }
        private List<string> ListAntiFileFilter { get; set; }
        private bool IsCaseSensitive { get; set; }
        private bool NeedConfirmation { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            // New ViewModel parse to Page DataContext
            ViewModel = new MainPageViewModel();
            this.DataContext = ViewModel;

            ListDirectoryFilter = new List<string>();
            ListStrictDirectoryFilter = new List<string>();
            ListFileFilter = new List<string>();

            ListAntiDirectoryFilter = new List<string>();
            ListAntiStrictDirectoryFilter = new List<string>();
            ListAntiFileFilter = new List<string>();

            CaseSensitiveToggleSwitch.IsOn = false;
            DeleteReportSwitch.IsOn = false;
            RefreshButton.IsEnabled = false;
            ViewModel.SearchToolTip = "C'est dans cette zone que vous pouvez filtrer la liste des éléments trouvés." +
                "\nSéparez tous vos composants de filtre par \",\"." +
                "\nLa recherche ne tient plus compte des majuscules et minuscules." +
                "\n\nPour obtenir plus d'informations sur les fonctionnalités et raccourcis possibles appuyez sur \"F1\".";

            NeedConfirmation = true;

            UpDateFilterData();
            DisplayFilteredElementsCount();
            ViewModel.CountSelected();

        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if(Environment.OSVersion.Version.Build >= 22000)
            {
                FolderPicker picker = new Windows.Storage.Pickers.FolderPicker();
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

                var hwnd = WindowNative.GetWindowHandle(MainWindowView);
                InitializeWithWindow.Initialize(picker, hwnd);

                StorageFolder dir = await picker.PickSingleFolderAsync();

                if (dir != null && Directory.Exists(dir.Path))
                {
                    DirectoryTextBox.Text = dir.Path;
                }
            }
            else
            {
                // Création de la boite de dialogue pour la recherche de dossier.
                CommonOpenFileDialog cofd = new CommonOpenFileDialog()
                {
                    Title = $"Dossier de recherche.",
                    IsFolderPicker = true,
                    Multiselect = false
                };

                CommonFileDialogResult r = cofd.ShowDialog();

                if (r == CommonFileDialogResult.Ok)
                {
                    if (cofd.FileName != null && Directory.Exists(cofd.FileName))
                    {
                        DirectoryTextBox.Text = cofd.FileName;
                    }
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpDateFilterData();
            ShowInUI();
        }

        private void CaseSensitiveToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ViewModel.SearchToolTip = CaseSensitiveToggleSwitch.IsOn ?
                "C'est dans cette zone que vous pouvez filtrer la liste des éléments trouvés." +
                "\nSéparez tous vos composants de filtre par \",\"." +
                "\nLa recherche ne tient compte des majuscules et minuscules." +
                "\n\nPour obtenir plus d'informations sur les fonctionnalités et raccourcis possibles appuyez sur \"F1\"."
                :
                "C'est dans cette zone que vous pouvez filtrer la liste des éléments trouvés." +
                "\nSéparez tous vos composants de filtre par \",\"." +
                "\nLa recherche ne tient plus compte des majuscules et minuscules." +
                "\n\nPour obtenir plus d'informations sur les fonctionnalités et raccourcis possibles appuyez sur \"F1\".";

            UpDateFilterData();
            ShowInUI();
        }

        private void DeleteFilesButton_Click(object sender, RoutedEventArgs e)
        {
            if(DeleteFilesButton.Content.ToString() == ViewModel.FileCounter && NeedConfirmation)
            {
                NeedConfirmation = false;
                ViewModel.FileCounter = "Presser une seconde fois pour confirmer";
            }
            else
            {
                NeedConfirmation = true;
                DeleteSelectedFiles(DeleteReportSwitch.IsOn);
            }
        }

        private void DirectoryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(DirectoryTextBox.Text))
            {
                // Looking for all the files in folder.
                UpDateFilterData();
                ParseFilesToUI(DirectoryTextBox.Text);
                RefreshButton.IsEnabled = true;
            }
            else
            {
                // Clearing existing UI items.
                ViewModel.ExplorerItems.Clear();
                ViewModel.ShowedExplorerItems.Clear();

                if(string.IsNullOrEmpty(DirectoryTextBox.Text)) SearchTextBox.Text = string.Empty;
                ViewModel.EnableControls = false;
                RefreshButton.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Process.Start("explorer.exe", Path.GetDirectoryName(b.Tag.ToString()));
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
                if (ListDirectoryFilter.Count <= 0 && ListFileFilter.Count <= 0)
                {
                    validFiles.Add(fi);
                    continue;
                }

                string filePath = IsCaseSensitive ? fi.DirectoryName : fi.DirectoryName.ToLower();
                string fileName = fi.Name.Replace(fi.Extension, "");
                fileName = IsCaseSensitive ? fileName : fileName.ToLower();
                List<bool> filterResult = new List<bool>();

                foreach(string d in ListDirectoryFilter)
                {
                    string str = IsCaseSensitive ? d : d.ToLower();
                    filterResult.Add(filePath.Contains(str));
                }

                // If all directory filter aren't correct then pass to the next file.
                if (filterResult.Where(x => x == true).Count() != ListDirectoryFilter.Count) continue;

                filterResult.Clear();

                foreach (string d in ListFileFilter)
                {
                    string str = IsCaseSensitive ? d : d.ToLower();
                    filterResult.Add(fileName.Contains(str));
                }

                // If all file name filter are correct then add the file to valid list.
                if (filterResult.Where(x => x == true).Count() == ListFileFilter.Count) validFiles.Add(fi);

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

            ListDirectoryFilter.Clear();
            ListStrictDirectoryFilter.Clear();
            ListFileFilter.Clear();
            ListAntiDirectoryFilter.Clear();
            ListAntiStrictDirectoryFilter.Clear();
            ListAntiFileFilter.Clear();

            if (string.IsNullOrEmpty(filter)) return;

            GetFolderFilter(filter);
            GetFileFilter(filter);
        }

        private void GetFolderFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter)) return;
            if (!(filter.Contains('@') || filter.Contains('#'))) return;

            if (filter.Contains(','))
            {
                string[] component = filter.Split(',');
                foreach (string s in component)
                {
                    ClassifyFolderFilter(s);
                }
            }
            else
            {
                ClassifyFolderFilter(filter);
            }
        }

        private void GetFileFilter(string filter)
        {
            if(string.IsNullOrEmpty(filter)) return;

            if (filter.Contains(','))
            {
                // Chop filter into list of component
                string[] component = filter.Split(',');
                foreach (string s in component)
                {
                    string txt = s.Trim();

                    if (!(txt.Contains('@') || txt.Contains('#')) && !string.IsNullOrEmpty(txt))
                    {
                        if (txt.StartsWith('!'))
                        {
                            txt = txt.Remove(0, 1);
                            txt = txt.Trim();
                            if (!string.IsNullOrEmpty(txt)) ListAntiFileFilter.Add(txt);
                        }
                        else
                        {
                            ListFileFilter.Add(txt);
                        }
                    }
                }
            }
            else if (!(filter.Contains('@') || filter.Contains('#')))
            {
                string txt = filter.Trim();
                if (txt.StartsWith('!'))
                {
                    txt = txt.Remove(0, 1);
                    txt = txt.Trim();
                    if (!string.IsNullOrEmpty(txt)) ListAntiFileFilter.Add(txt);
                }
                else
                {
                    ListFileFilter.Add(txt);
                }
            }

        }

        private void ClassifyFolderFilter(string filter)
        {
            string txt = filter.Trim();

            // If cleared filter component start with @ it's a strict folder filter.
            if (txt.StartsWith("@"))
            {
                string f = txt.Remove(0, 1);
                f = f.Trim();

                if (string.IsNullOrEmpty(f)) return;

                // If strict folder filter start with ! it's a reverse one.
                if (f.StartsWith("!"))
                {
                    f = f.Remove(0, 1);
                    f = f.Trim();
                    if (!string.IsNullOrEmpty(f)) ListAntiStrictDirectoryFilter.Add(f);
                    return;
                }
                else
                {
                    ListStrictDirectoryFilter.Add(f);
                    return;
                }
            }

            // If cleared filter component start with # it's a folder filter.
            if (txt.StartsWith("#"))
            {
                string f = txt.Remove(0, 1);
                f = f.Trim();

                if (string.IsNullOrEmpty(f)) return;

                // If folder filter start with ! it's a reverse one.
                if (f.StartsWith("!"))
                {
                    f = f.Remove(0, 1);
                    f = f.Trim();
                    if (!string.IsNullOrEmpty(f)) ListAntiDirectoryFilter.Add(f);
                    return;
                }
                else
                {
                    ListDirectoryFilter.Add(f);
                    return;
                }
            }
        }

        #endregion

        #region Data To UI

        /// <summary>
        /// Met à jour la liste des dossiers dans lesquels se trouvent des fichiers de sauvegarde.
        /// </summary>
        /// <param name="folderPath">Chemin du dossier inital.</param>
        /// <param name="filter">Filtre de recherche.</param>
        /// <param name="caseSensitive">La recherche est-elle sensible à la casse ?</param>
        public async void ParseFilesToUI(string folderPath)
        {
            // If directory doesn't exist then exit.
            if (!Directory.Exists(folderPath)) return;

            // Clearing existing UI items.
            ViewModel.ExplorerItems.Clear();
            ViewModel.ShowedExplorerItems.Clear();

            await Task.Run(() =>
            {
                FilesInFolder(folderPath);
            });

            ViewModel.EnableControls = ViewModel.ExplorerItems.Count > 0;
            ViewModel.CountSelected();
            DisplayFilteredElementsCount();
        }

        /// <summary>
        /// On recupère chaque dossier du dossier actif. On ajoute tous ces dossiers sur la page principale.
        /// </summary>
        /// <param name="folderPath">Chemin du dossier visé.</param>
        /// <param name="filter">Filtre de recherche.</param>
        /// <param name="caseSensitive">La recherche est-elle sensible à la casse ?</param>
        /// <param name="isExpanded">Le noeud doit-il être étendu ?</param>
        /// <returns>Une liste de noeuds.</returns>
        private void FilesInFolder(string folderPath)
        {
            FileInfo[] dirFiles = new FileInfo[] { };

            // Trying to get file in folder.
            // This was done like this because on the author pc, a direcotry was found whereas it doesn't exist on the pc.
            // If that occure again, application won't crash.
            try
            {
                dirFiles = new DirectoryInfo(folderPath).GetFiles();
            }
            catch
            {
                return;
            }

            // Get Files in current folder.
            this.DispatcherQueue.TryEnqueue(
            Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                () =>
                {
                    SelectionInformationBlock.Text = $"Recherche dans \n{folderPath}";
                });
            List<FileInfo> curDirFiles = GetFiles(folderPath, false);

            // Fill Explorer items list with files.
            foreach(FileInfo file in curDirFiles)
            {
                bool showed = true;
                if (!(ListDirectoryFilter.Count <= 0 
                    && ListStrictDirectoryFilter.Count <= 0 
                    && ListFileFilter.Count <= 0 
                    && ListAntiDirectoryFilter.Count <= 0 
                    && ListAntiStrictDirectoryFilter.Count <= 0 
                    && ListAntiFileFilter.Count <= 0))
                {
                    showed = IsFilterOk(file.FullName);
                }

                ExplorerItemType eit = GetUIFileType(file);

                ExplorerItem item = new ExplorerItem(this.ViewModel)
                {
                    Name = file.Name.Replace(file.Extension, string.Empty),
                    Path = file.FullName,
                    IsSelected = true,
                    IsShowed = showed,
                    Type= eit,
                    Size = file.Length / 8
                };

                ViewModel.ExplorerItems.Add(item);
                if(item.IsShowed) ViewModel.ShowedExplorerItems.Add(item);
            }

            // Initializing list
            string[] subDirs = new string[] { };

            // Trying to get sub directory.
            // This was done like this because on the author pc, a direcotry was found whereas it doesn't exist on the pc.
            // If that occure again, application won't crash.
            try
            {
                subDirs = Directory.GetDirectories(folderPath);
            }
            catch
            {
                return;
            }

            // If sub folder can be accessed then try to retrieve them files
            foreach(string dirPath in subDirs)
            {
                FilesInFolder(dirPath);
            }
        }

        private void ShowInUI()
        {
            ViewModel.CountSelected();
            // Add them in UI
            Task.Run(() =>
            {
                ViewModel.ShowedExplorerItems.Clear();

                // Inspect filter list before filtering
                if (ListDirectoryFilter.Count <= 0 && ListStrictDirectoryFilter.Count <= 0 && ListFileFilter.Count <= 0 && ListAntiDirectoryFilter.Count <= 0 && ListAntiStrictDirectoryFilter.Count <= 0 && ListAntiFileFilter.Count <= 0)
                {
                    // Set all file as showed in UI.
                    foreach (ExplorerItem explorerItem in ViewModel.ExplorerItems)
                    {
                        explorerItem.IsShowed = true;
                        ViewModel.ShowedExplorerItems.Add(explorerItem);
                    }
                }
                else
                {
                    foreach (ExplorerItem explorerItem in ViewModel.ExplorerItems)
                    {
                        explorerItem.IsShowed = IsFilterOk(explorerItem.Path);
                    }

                    List<ExplorerItem> list = ViewModel.ExplorerItems.Where(x => x.IsShowed).ToList();

                    foreach (ExplorerItem item in list)
                    {
                        ViewModel.ShowedExplorerItems.Add(item);
                    }
                }

                DisplayFilteredElementsCount();

            });
        }

        private void DisplayFilteredElementsCount()
        {
            this.DispatcherQueue.TryEnqueue(
                Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                () =>
                {
                    int fsc = ViewModel.ShowedExplorerItems.Count;
                    int ftc = ViewModel.ExplorerItems.Count;
                    string fs = ViewModel.ShowedExplorerItems.Count > 1 ? "s" : "";
                    string ft = ViewModel.ExplorerItems.Count > 1 ? "s" : "";

                    if (fsc <= 0)
                    {
                        SelectionInformationBlock.Text = $"Aucun fichier affiché / {ftc} fichier{ft}";
                    }
                    else
                    {
                        SelectionInformationBlock.Text = $"{fsc} fichier{fs} affiché{fs} / {ftc} fichier{ft}";
                    }
                });
        }

        private bool IsFilterOk(string filepath)
        {
            FileInfo file = new FileInfo(filepath);
            
            string name = file.Name.Replace(file.Extension, string.Empty);
            string path = Path.GetDirectoryName(file.FullName);
            string parentFolder = file.Directory.Name;

            name = IsCaseSensitive ? name : name.ToLower();
            path = IsCaseSensitive ? path : path.ToLower();
            parentFolder = IsCaseSensitive ? parentFolder : parentFolder.ToLower();

            bool fileNameTest = false, fileNameAntiTest = false, pathTest = false, pathAntiTest = false, parentTest = false, parentAntiTest = false;

            if (IsCaseSensitive)
            {
                fileNameTest = ListFileFilter.Where(x => name.Contains(x)).Count() == ListFileFilter.Count;
                fileNameAntiTest = ListAntiFileFilter.Where(x => name.Contains(x)).Count() == ListAntiFileFilter.Count;

                pathTest = ListDirectoryFilter.Where(x => path.Contains(x)).Count() == ListDirectoryFilter.Count;
                pathAntiTest = ListAntiDirectoryFilter.Where(x => !path.Contains(x)).Count() == ListAntiDirectoryFilter.Count;

                parentTest = ListStrictDirectoryFilter.Where(x => path.Contains(x)).Count() == ListStrictDirectoryFilter.Count;
                parentAntiTest = ListAntiStrictDirectoryFilter.Where(x => !path.Contains(x)).Count() == ListAntiStrictDirectoryFilter.Count;
            }
            else
            {
                fileNameTest = ListFileFilter.Where(x => name.Contains(x.ToLower())).Count() == ListFileFilter.Count;
                fileNameAntiTest = ListAntiFileFilter.Where(x => !name.Contains(x.ToLower())).Count() == ListAntiFileFilter.Count;

                pathTest = ListDirectoryFilter.Where(x => path.Contains(x.ToLower())).Count() == ListDirectoryFilter.Count;
                pathAntiTest = ListAntiDirectoryFilter.Where(x => !path.Contains(x.ToLower())).Count() == ListAntiDirectoryFilter.Count;

                parentTest = ListStrictDirectoryFilter.Where(x => path.Contains(x.ToLower())).Count() == ListStrictDirectoryFilter.Count;
                parentAntiTest = ListAntiStrictDirectoryFilter.Where(x => !path.Contains(x.ToLower())).Count() == ListAntiStrictDirectoryFilter.Count;
            }

            return fileNameTest && fileNameAntiTest && pathTest && pathAntiTest && parentTest && parentAntiTest;
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

        #endregion

        /// <summary>
        /// Fonction de teste qui permet de tester différents processus de manipulation des listes 
        /// et collections dans winui3.
        /// </summary>
        /// <param name="gCount"></param>
        private void DeleteSelectedFiles(bool report)
        {
            string docsPath = "";
            if (report)
            {
                docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Revit Cleaner Reports";
                if(!Directory.Exists(docsPath)) Directory.CreateDirectory(docsPath);
            }

            List<string> reportLines = new();
            List<ExplorerItem> selectedFiles = ViewModel.ExplorerItems.Where(x => x.IsSelected).ToList();

            foreach (ExplorerItem child in selectedFiles)
            {
                try
                {
                    File.Delete(child.Path);
                    if(report) reportLines.Add($"Suppression de {child.Path}");
                }
                catch
                {
                    if (report) reportLines.Add($"Une erreur s'est produite lors de la suppression de {child.Path}");
                }
            }

            if (report)
            {
                try
                {
                    string time = DateTime.Now.ToString("yyyyMMdd-HH-mm-ss");
                    string filePath = $"{docsPath}\\{time}_Rapport de suppression de fichiers.txt";
                    File.WriteAllLines(filePath, reportLines.ToArray());
                    Process.Start(docsPath);
                }
                catch { }
            }

            ParseFilesToUI(DirectoryTextBox.Text);
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach(ExplorerItem item in ViewModel.ExplorerItems)
            {
                item.IsSelected= true;
            }
        }

        private void UnselectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (ExplorerItem item in ViewModel.ExplorerItems)
            {
                item.IsSelected = false;
            }
        }

        private void InvertAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (ExplorerItem item in ViewModel.ExplorerItems)
            {
                item.IsSelected = !item.IsSelected;
            }
        }

        private void SelectSelection_Click(object sender, RoutedEventArgs e)
        {
            foreach (ExplorerItem item in ViewModel.ShowedExplorerItems)
            {
                item.IsSelected = true;
            }
        }

        private void UnselectSelection_Click(object sender, RoutedEventArgs e)
        {
            foreach (ExplorerItem item in ViewModel.ShowedExplorerItems)
            {
                item.IsSelected = false;
            }
        }

        private void InvertSelection_Click(object sender, RoutedEventArgs e)
        {
            foreach(ExplorerItem item in ViewModel.ShowedExplorerItems)
            {
                item.IsSelected = !item.IsSelected;
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            VirtualKey key= e.Key;
            if(key == VirtualKey.F1)
            {
                Process.Start(@"https://app.thomas-lecuppre.fr/application-pour-revit/revit-cleaner#filtrer-dans-revit-cleaner");
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if(Directory.Exists(DirectoryTextBox.Text))
            {
                // Looking for all the files in folder.
                UpDateFilterData();
                ParseFilesToUI(DirectoryTextBox.Text);
            }
        }
    }
}
