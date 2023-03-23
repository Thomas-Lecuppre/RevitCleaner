// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using RevitCleaner.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Management.Deployment;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RevitCleaner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UpdatePage : Page
    {
        public string InfoUpdate { get; set; }
        public MainWindow MainWindowView { get; set; }

        public UpdatePage()
        {
            this.InitializeComponent();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowView.ShowMainPage();
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            PackageManager packagemanager = new PackageManager();
            uint res = RelaunchHelper.RegisterApplicationRestart(null, RelaunchHelper.RestartFlags.NONE);
            try
            {
                await packagemanager.AddPackageAsync(
                    new Uri("https://update.thomas-lecuppre.fr/RevitCleaner_lastest.msix"),
                    null,
                    DeploymentOptions.ForceApplicationShutdown
                );
            }
            catch (Exception ex)
            {
                UpdateErrorBlock.Text = $"{ex.Message}\n{ex.StackTrace}";
            }
        }

        public void ShowChangeLog(List<string> changes)
        {
            List<ChangeLog> changesList = new List<ChangeLog>();

            foreach(string change in changes)
            {
                if(string.IsNullOrEmpty(change)) continue;

                ChangeLog cl = new ChangeLog();

                if (char.IsDigit(change.First()) && change.Contains(":"))
                {
                    cl.Change = change.Remove(0,2);
                    int i = int.Parse(change.Substring(0,1));
                    cl.Type = (ChangeLog.ChangeLogType)i;
                }
                else
                {
                    cl.Change = change;
                    cl.Type = ChangeLog.ChangeLogType.Update;
                }

                changesList.Add(cl);
            }

            changesList = changesList.OrderBy(cl => cl.Type).ToList();

            foreach(ChangeLog cl in changesList)
            {
                ChangeLogList.Items.Add(cl);
            }
        }
    }
}
