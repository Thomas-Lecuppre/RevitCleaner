using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RevitCleaner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExecutionPage : Page
    {
        public ExecutionPage()
        {
            this.InitializeComponent();
        }

        public void UpdateTextBox(string s)
        {
            if (this.DispatcherQueue.HasThreadAccess)
            {
                ExecutionInfoTextBlock.Text = s;
            }
            else
            {
                this.DispatcherQueue.TryEnqueue(
                    Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                    () =>
                    {
                        ExecutionInfoTextBlock.Text = s;
                    });
            }
        }

        public void UpdateLoadingRing(bool isActive, bool isIndeterminate, double maximum, double value)
        {
            if (this.DispatcherQueue.HasThreadAccess)
            {
                ProcessRing.IsActive = isActive;
                ProcessRing.IsIndeterminate = isIndeterminate;
                ProcessRing.Maximum = maximum;
                ProcessRing.Value = value;
            }
            else
            {
                this.DispatcherQueue.TryEnqueue(
                    Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                    () =>
                    {
                        ProcessRing.IsActive = isActive;
                        ProcessRing.IsIndeterminate = isIndeterminate;
                        ProcessRing.Maximum = maximum;
                        ProcessRing.Value = value;
                    });
            }
        }
    }
}
