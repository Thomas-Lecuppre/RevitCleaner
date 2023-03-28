using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitCleaner
{
    internal class ChangeLogSelector : DataTemplateSelector
    {
        public DataTemplate AddChangeLog { get; set; }
        public DataTemplate UpdateChangeLog { get; set; }
        public DataTemplate RemoveChangeLog { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var changeLog = (ChangeLog)item;

            if (changeLog.Type == ChangeLog.ChangeLogType.Add)
                return AddChangeLog;

            if (changeLog.Type == ChangeLog.ChangeLogType.Remove)
                return RemoveChangeLog;

            return UpdateChangeLog;
        }
    }
}
