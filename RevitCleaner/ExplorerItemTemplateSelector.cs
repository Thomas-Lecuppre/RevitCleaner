using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitCleaner
{
    class ExplorerItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RFAFileTemplate { get; set; }
        public DataTemplate RFTFileTemplate { get; set; }
        public DataTemplate RTEFileTemplate { get; set; }
        public DataTemplate RVTFileTemplate { get; set; }
        public DataTemplate UnknowFileTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var explorerItem = (ExplorerItem)item;
            if (explorerItem.Type == ExplorerItem.ExplorerItemType.RFAFile)
                return RFAFileTemplate;
            if (explorerItem.Type == ExplorerItem.ExplorerItemType.RFTFile)
                return RFTFileTemplate;
            if (explorerItem.Type == ExplorerItem.ExplorerItemType.RTEFile)
                return RTEFileTemplate;
            if (explorerItem.Type == ExplorerItem.ExplorerItemType.RVTFile)
                return RVTFileTemplate;

            return UnknowFileTemplate;
        }
    }
}
