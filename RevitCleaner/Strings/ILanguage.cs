using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitCleaner.Strings
{
    public interface ILanguage
    {
        public string Id => "";
        public string TranslateAutor => "";
        public string LanguageName => "";
        public string DirectoryTextBoxHeader => "";
        public string DirectoryTextBoxToolTip => "";
        public string SearchButtonTooTip => "";
        public string RefreshButtonToolTip => "";
        public string SearchTextBoxHeader => "";
        public string SearchTextBoxPlaceHolder => "";
        public string SearchTextBoxToolTip => "";
        public string CaseSensitiveToggleSwitchOn => "";
        public string CaseSensitiveToggleSwitchOff => "";
        public string ListItemOpenButtonText => "";
        public string GlobalActionTitle => "";
        public string SelectAllText => "";
        public string SelectAllToolTip => "";
        public string UnSelectAllText => "";
        public string UnSelectAllToolTip => "";
        public string InvertAllText => "";
        public string InvertAllToolTip => "";
        public string FilteredActionTiTle => "";
        public string FilteredSelectAllText => "";
        public string FilteredSelectAllToolTip => "";
        public string FilteredUnSelectAllText => "";
        public string FilteredUnSelectAllToolTip => "";
        public string FilteredInvertAllText => "";
        public string FilteredInvertAllToolTip => "";
        public string DeleteReportSwitchOn => "";
        public string DeleteReportSwitchOff => "";
        public string DeleteReportSwitchToolTip => "";
        public string DeleteButtonToolTip => "";
        public string DeleteButtonConfirmMessage => "";
        public string UpdateErrorMessage => "";

        #region Common

        public string StrNoFile => "";
        public string StrNoShowedFile => "";
        public string StrOneShowedFile => "";
        public string StrMultipleShowedFile => "";
        public string StrFile => "";
        public string StrFiles => "";
        public string StrLookInto => "";
        public string StrNoFileToClean => "";
        public string StrOneFileToClean => "";
        public string StrMultipleToClean => "";
        public string StrUpdateTo => "";
        public string StrInstall => "";
        public string StrContinue => "";
        public string StrSkip => "";
        public string StrPatchNote => "";

        #endregion
    }
}
