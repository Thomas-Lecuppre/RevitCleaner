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
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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

            FilesListPage.mainWindow = this;

            Assembly a = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(a.Location);

            Title = $"Revit Cleaner - v{fvi.ProductMajorPart}.{fvi.ProductMinorPart}";
            ExtendsContentIntoTitleBar = true;

            SetTitleBar(TitleBar);

            FilesListPage.Visibility = Visibility.Visible;
            LoadingPage.Visibility = Visibility.Collapsed;
        }

        public void Process()
        {
            FilesListPage.Visibility = Visibility.Collapsed;
            LoadingPage.Visibility = Visibility.Visible;

            Task.Run(() => Test());
        }

        private void Test()
        {

            LoadingPage.UpdateLoadingRing(true, false, 100, 0);

            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(50);

                LoadingPage.UpdateLoadingRing(true, false, 100, i);
                LoadingPage.UpdateTextBox($"Suppression {i}/100");
            }


            LoadingPage.UpdateLoadingRing(true, true, 100, 0);

            Thread.Sleep(5000);

            if (this.DispatcherQueue.HasThreadAccess)
            {
                FilesListPage.Visibility = Visibility.Visible;
                LoadingPage.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.DispatcherQueue.TryEnqueue(
                    Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                    () =>
                    {
                        FilesListPage.Visibility = Visibility.Visible;
                        LoadingPage.Visibility = Visibility.Collapsed;
                    });
            }
        }

    }
}
