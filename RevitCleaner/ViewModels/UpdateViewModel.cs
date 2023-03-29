using RevitCleaner.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitCleaner.ViewModels
{
    public class UpdateViewModel : BaseViewModel
    {
        private ILanguage lang;

        public ILanguage Lang 
		{
			get { return lang; }
			set
			{
				lang = value;
			} 
		}

        private string updateTitle;

		public string UpdateTitle
		{
			get { return updateTitle; }
			set 
			{ 
				updateTitle = value; 
				NotifyPropertyChanged(nameof(UpdateTitle));
			}
		}

		private string errorMessage;

		public string ErrorMessage
		{
			get { return errorMessage; }
			set 
			{ 
				errorMessage = Lang.UpdateErrorMessage.Replace("$1", value);
                NotifyPropertyChanged(nameof(ErrorMessage));
            }
		}

		public void UpdateInfos(Version newVersion)
		{
			UpdateTitle = Lang.StrUpdateTo.Replace("$1", "").Replace("$2", newVersion.ToString());
		}

	}
}
