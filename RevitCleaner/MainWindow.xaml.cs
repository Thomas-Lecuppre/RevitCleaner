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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace RevitCleaner
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // Mise à jour de la barre de titre est de l'entête.
            PackageVersion pkgV = Package.Current.Id.Version;

            Title = $"Revit Cleaner - v{pkgV.Major}.{pkgV.Minor}.{pkgV.Build}";
            ExtendsContentIntoTitleBar = true;

            SetTitleBar(TitleBar);

            // Vérification des mises à jour.
            CheckUpdateAsync();
        }

        private async void CheckUpdateAsync()
        {
            WebClient client = new();
            Stream stream = client.OpenRead("https://update.thomas-lecuppre.fr/revitcleaner.txt");
            StreamReader reader = new StreamReader(stream);

            string updateInfo = await reader.ReadToEndAsync();
            string[] content = updateInfo.Split('\n');

            var newVersion = new Version(content[0]);
            Package package = Package.Current;
            PackageVersion packageVersion = package.Id.Version;
            var currentVersion = new Version(string.Format("{0}.{1}.{2}.{3}", packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision));

            //compare package versions
            if (newVersion.CompareTo(currentVersion) > 0)
            {
                UpdatePage up = new UpdatePage();
                up.MainWindowView = this;
                up.InfoUpdate = $"Mettre à jour Revit Cleaner {currentVersion} vers {newVersion} ?";
                up.ShowChangeLog(content.Skip(1).ToList());
                FilesListPage.Content = up;
            }
            else 
            { 
                ShowMainPage(); 
            }
            
        }

        public void ShowMainPage()
        {
            MainPage mp = new MainPage();
            mp.MainWindowView = this;
            FilesListPage.Content = mp;
        }

    }
}
