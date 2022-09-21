using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

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

        public MainPage()
        {
            this.InitializeComponent();

            FilesTreeView.ItemsSource = GetData();
        }

        private ObservableCollection<ExplorerItem> GetData()
        {
            var list = new ObservableCollection<ExplorerItem>();
            ExplorerItem folder1 = new ExplorerItem()
            {
                Name = "Familles Revit",
                Type = ExplorerItem.ExplorerItemType.Folder,
                Children =
                {
                    new ExplorerItem()
                    {
                        Name = "MR Type 1",
                        Type = ExplorerItem.ExplorerItemType.Folder,
                        Children =
                        {
                            new ExplorerItem()
                            {
                                Name = "MGMA_10156 Capot 13mm MECANO52_PP",
                                Type = ExplorerItem.ExplorerItemType.RFAFile,
                            },
                            new ExplorerItem()
                            {
                                Name = "MGMA_10156 Capot 13mm MECANO52_PP.0001",
                                Type = ExplorerItem.ExplorerItemType.RFAFile,
                            },
                            new ExplorerItem()
                            {
                                Name = "MGMA_10156 Capot 13mm MECANO52_PP.0001",
                                Type = ExplorerItem.ExplorerItemType.RFAFile,
                            },
                            new ExplorerItem()
                            {
                                Name = "MGMA_10156 Capot 13mm MECANO52_PP.0001",
                                Type = ExplorerItem.ExplorerItemType.RFAFile,
                            },
                            new ExplorerItem()
                            {
                                Name = "MGMA_10156 Capot 13mm MECANO52_PP.0001",
                                Type = ExplorerItem.ExplorerItemType.RFAFile,
                            },
                            new ExplorerItem()
                            {
                                Name = "MGMA_10156 Capot 13mm MECANO52_PP.0001",
                                Type = ExplorerItem.ExplorerItemType.RFAFile,
                            },
                            new ExplorerItem()
                            {
                                Name = "MGMA_10156 Capot 13mm MECANO52_PP.0001",
                                Type = ExplorerItem.ExplorerItemType.RFAFile,
                            },
                            new ExplorerItem()
                            {
                                Name = "MGMA_10156 Capot 13mm MECANO52_PP.0001",
                                Type = ExplorerItem.ExplorerItemType.RFAFile,
                            },
                            new ExplorerItem()
                            {
                                Name = "MGMA_10156 Capot 13mm MECANO52_PP.0001",
                                Type = ExplorerItem.ExplorerItemType.RFAFile,
                            },
                            new ExplorerItem()
                            {
                                Name = "MGMA_10156 Capot 13mm MECANO52_PP.0001",
                                Type = ExplorerItem.ExplorerItemType.RFAFile,
                            }
                        }
                    },
                    new ExplorerItem()
                    {
                        Name = "DETA_Parclose et serreur ponctuel MECANO52",
                        Type = ExplorerItem.ExplorerItemType.RFAFile,
                    },
                    new ExplorerItem()
                    {
                        Name = "DETA_Jonction MR GO",
                        Type = ExplorerItem.ExplorerItemType.RFAFile,
                    },
                    new ExplorerItem()
                    {
                        Name = "MGMF_Sabot de fixation Type A",
                        Type = ExplorerItem.ExplorerItemType.RFAFile,
                    }
                }
            };
            ExplorerItem folder2 = new ExplorerItem()
            {
                Name = "Dossier projet",
                Type = ExplorerItem.ExplorerItemType.Folder,
                Children =
                        {
                            new ExplorerItem()
                            {
                                Name = "0001-CLI-Projet",
                                Type = ExplorerItem.ExplorerItemType.Folder,
                                Children =
                                {
                                    new ExplorerItem()
                                    {
                                        Name = "MEX-EXE-TZ-TN-IND-0",
                                        Type = ExplorerItem.ExplorerItemType.RVTFile,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Gabarit_BIM_MEX",
                                        Type = ExplorerItem.ExplorerItemType.RTEFile,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Gabarit_BIM_MEX",
                                        Type = ExplorerItem.ExplorerItemType.RTEFile,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Gabarit_BIM_MEX",
                                        Type = ExplorerItem.ExplorerItemType.RTEFile,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Gabarit_BIM_MEX",
                                        Type = ExplorerItem.ExplorerItemType.RTEFile,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Gabarit_BIM_MEX",
                                        Type = ExplorerItem.ExplorerItemType.RTEFile,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Gabarit_BIM_MEX",
                                        Type = ExplorerItem.ExplorerItemType.RTEFile,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Gabarit_BIM_MEX",
                                        Type = ExplorerItem.ExplorerItemType.RTEFile,
                                    }
                                }
                            }
                        }
            };

            list.Add(folder1);
            list.Add(folder2);
            return list;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CaseSensitiveToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteFilesButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Process();
        }
    }
}
