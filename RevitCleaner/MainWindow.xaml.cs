using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using RevitCleaner.Model;
using RevitCleaner.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Text.Json;
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
        public UserConf UserConf { get; set; }
        public ILanguage Lang { get; set; }

        public MainWindow()
        {
            this.InitializeComponent();

            // Mise à jour de la barre de titre est de l'entête.
            PackageVersion pkgV = Package.Current.Id.Version;

            Title = $"Revit Cleaner - v{pkgV.Major}.{pkgV.Minor}.{pkgV.Build}";
            ExtendsContentIntoTitleBar = true;

            SetTitleBar(TitleBar);

            // Vérification configuration utilisateur
            string localConfPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + $"\\{Environment.UserName}.json";

            if (File.Exists(localConfPath))
            {
                string content = File.ReadAllText(localConfPath);

                if(content != null)
                {
                    UserConf = JsonSerializer.Deserialize<UserConf>(content);
                    SetLanguage(UserConf.LangId);
                }
                else
                {
                    InitUserConf();
                }
            }
            else
            {
                InitUserConf();
            }

            // Vérification des mises à jour.
            CheckUpdate();
        }

        private void CheckUpdate()
        {
            try
            {
                string updateInfo = ""; 
                // On contact la page qui contient les informations de mise à jour.
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = client.GetAsync("http://update.thomas-lecuppre.fr/revitcleaner.txt").Result)
                    {
                        using (HttpContent content = response.Content)
                        {
                            updateInfo = content.ReadAsStringAsync().Result;
                        }
                    }
                }
                string[] contentAr = updateInfo.Split('\n');

                var newVersion = new Version(contentAr[0]);
                Package package = Package.Current;
                PackageVersion packageVersion = package.Id.Version;
                var currentVersion = new Version(string.Format("{0}.{1}.{2}.{3}", packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision));

                //Comparaison des versions d'application.
                //Si la version nouvelle est au dessus de la version skipper alors on l'affiche.
                if (newVersion.CompareTo(currentVersion) > 0 && newVersion.CompareTo(UserConf.SkipVersion) > 0)
                {
                    ShowUpdatePage(newVersion);
                }
                else
                {
                    ShowMainPage();
                }
            }
            catch
            {
                // En cas d'erreur on affiche la page normale.
                ShowMainPage();
            }
            
        }

        /// <summary>
        /// Affiche la page qui traite des mises à jour de l'application.
        /// </summary>
        /// <param name="newVersion">La valeur de la version de la nouvelle mise à jour.</param>
        private void ShowUpdatePage(Version newVersion)
        {
            UpdatePage up = new UpdatePage(Lang, newVersion)
            {
                MainWindowView = this
            };
            FilesListPage.Content = up;
        }

        /// <summary>
        /// Affiche la page normale.
        /// </summary>
        public void ShowMainPage()
        {
            MainPage mp = new MainPage(Lang)
            {
                MainWindowView = this
            };
            FilesListPage.Content = mp;
        }

        /// <summary>
        /// Initialise pour la première fois le fichier de configuration utilisateur.
        /// </summary>
        /// <param name="path">Chemin du fichier de configuration utilisateur.</param>
        private void InitUserConf()
        {
            UserConf = new UserConf()
            {
                LangId = "fr",
                LastDirectory = "",
                SkipVersion = new Version(0,0,0,0)
            };

            UserConf.Save();
        }

        private void SetLanguage(string langId)
        {
            switch (langId)
            {
                case "en":
                    {
                        Lang = new Lang_en();
                        break;
                    }
                case "pt":
                    {
                        Lang = new Lang_pt();
                        break;
                    }
                default:
                    {
                        Lang = new Lang_fr();
                        break;
                    }
            }
        }

    }
}
