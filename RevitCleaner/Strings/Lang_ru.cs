
namespace RevitCleaner.Strings
{
    public class Lang_ru : ILanguage
    {
        public string Id => "ru";
        public string TranslateAutor => "-";
        public string LanguageName => "Russian";

        /// <summary>
        /// DEFAULT = Dossier de recherche :
        /// </summary>
        public string DirectoryTextBoxHeader
            => "Research folder :";

        /// <summary>
        /// DEFAULT = Indiquez le dossier dans lequel chercher les éléments à nettoyer.
        /// </summary>
        public string DirectoryTextBoxToolTip
            => "Specify the folder in which to search for items to clean.";

        /// <summary>
        /// DEFAULT = Ouvrir un explorateur de dossier.
        /// </summary>
        public string SearchButtonTooTip
            => "Open a folder explorer.";

        /// <summary>
        /// DEFAULT = Rafraichir la liste des fichiers du dossier.
        /// </summary>
        public string RefreshButtonToolTip
            => "Refresh list of folder files.";

        /// <summary>
        /// DEFAULT = Filtre de recherche :
        /// </summary>
        public string SearchTextBoxHeader
            => "Search filter :";

        /// <summary>
        /// DEFAULT = Project, Modèle générique, @Etudes
        /// </summary>
        public string SearchTextBoxPlaceHolder
            => "Project, Generic model";

        /// <summary>
        /// DEFAULT = 
        /// C'est dans cette zone que vous pouvez filtrer la liste des éléments trouvés.\nSéparez tous vos composants de filtre par \",\".\n\nPour obtenir plus d'informations sur les fonctionnalités et raccourcis possibles appuyez sur \"F1\".
        /// </summary>
        public string SearchTextBoxToolTip
            => "It is in this area that you can filter the list of found items.\nSeparate all your filter components by \",\".\n\nTo obtain more information on the functions and possible shortcuts, press \"F1\".";

        /// <summary>
        /// DEFAULT = Recherche stricte activée
        /// </summary>
        public string CaseSensitiveToggleSwitchOn
            => "Case sensitive activated";

        /// <summary>
        /// DEFAULT = Recherche stricte désactivée
        /// </summary>
        public string CaseSensitiveToggleSwitchOff
            => "Case sensitive deactivated";

        /// <summary>
        /// DEFAULT = Ouvrir
        /// </summary>
        public string ListItemOpenButtonText
            => "Open";

        /// <summary>
        /// DEFAULT = Actions globales :
        /// </summary>
        public string GlobalActionTitle
            => "Global action :";

        /// <summary>
        /// DEFAULT = Tout cocher
        /// </summary>
        public string SelectAllText
            => "Check all";

        /// <summary>
        /// DEFAULT = Coche tous les éléments sans tenir compte du filtre.
        /// </summary>
        public string SelectAllToolTip
            => "Checks all items regardless of the filter.";

        /// <summary>
        /// DEFAULT = Tout décocher
        /// </summary>
        public string UnSelectAllText
            => "Uncheck all";

        /// <summary>
        /// DEFAULT = Décoche tous les éléments sans tenir compte du filtre.
        /// </summary>
        public string UnSelectAllToolTip
            => "Unchecks all items regardless of the filter.";

        /// <summary>
        /// DEFAULT = Tout inverser
        /// </summary>
        public string InvertAllText
            => "Invert all";

        /// <summary>
        /// DEFAULT = Inverse la sélection de tous les éléments sans tenir compte du filtre.
        /// </summary>
        public string InvertAllToolTip
            => "Invert all items regardless of the filter.";

        /// <summary>
        /// DEFAULT = Actions sur la sélection :
        /// </summary>
        public string FilteredActionTiTle
            => "Actions on filtered :";

        /// <summary>
        /// DEFAULT = Cocher filtrés
        /// </summary>
        public string FilteredSelectAllText
            => "Check filtered";

        /// <summary>
        /// DEFAULT = Coche tout les éléments filtrés.
        /// </summary>
        public string FilteredSelectAllToolTip
            => "Check all filtered items.";

        /// <summary>
        /// DEFAULT = Décocher filtrés
        /// </summary>
        public string FilteredUnSelectAllText
            => "Uncheck filtered";

        /// <summary>
        /// DEFAULT = Décoche tout les éléments filtrés.
        /// </summary>
        public string FilteredUnSelectAllToolTip
            => "Uncheck all filtered items.";

        /// <summary>
        /// DEFAULT = Inverser filtrés
        /// </summary>
        public string FilteredInvertAllText
            => "Invert filtered";

        /// <summary>
        /// DEFAULT = Inverse la sélection de tout les éléments filtrés.
        /// </summary>
        public string FilteredInvertAllToolTip
            => "Invert all filtered items.";

        /// <summary>
        /// DEFAULT = Générer un rapport de suppression
        /// </summary>
        public string DeleteReportSwitchOn
            => "Generate deletion report";

        /// <summary>
        /// DEFAULT = Ne pas générer de rapport de suppression
        /// </summary>
        public string DeleteReportSwitchOff
            => "Do not generate deletion report";

        /// <summary>
        /// DEFAULT = Les rapports de suppresion se trouve dans ..\\Mes Documents\\Revit Cleaner Reports
        /// </summary>
        public string DeleteReportSwitchToolTip
            => "Deletion reports can be found in ..\\My Document\\Revit Cleaner Reports";

        /// <summary>
        /// DEFAULT = Cette action est irréversible. Réfléchissez avant de cliquer.
        /// </summary>
        public string DeleteButtonToolTip
            => "This action is irreversible. Think twice before you click.";

        /// <summary>
        /// DEFAULT = Presser une seconde fois pour confirmer
        /// </summary>
        public string DeleteButtonConfirmMessage
            => "Press a second time to confirm";

        /// <summary>
        /// DEFAULT = Une erreur c'est produite. Veuillez redémarrer votre ordinateur puis retenter la mise à jour. Si le problème persiste, communiquez ce code erreur : $1
        /// </summary>
        public string UpdateErrorMessage
            => "An error has occured. Try to restart your computer then try again. If the problem remain, transmit this error code : $1";

        #region Common

        /// <summary>
        /// DEFAULT = Aucun fichier à afficher
        /// </summary>
        public string StrNoFile
            => "No file to show";

        /// <summary>
        /// DEFAULT = Auncun fichier affiché
        /// </summary>
        public string StrNoShowedFile
            => "No showed file";

        /// <summary>
        /// DEFAULT = 1 fichier affiché
        /// </summary>
        public string StrOneShowedFile
            => "1 showed file";

        /// <summary>
        /// DEFAULT = $1 fichiers affichés
        /// $1 représente le nombre de fichiers qui correspondent au tri
        /// </summary>
        public string StrMultipleShowedFile
            => "$1 showed files";

        /// <summary>
        /// DEFAULT = fichier
        /// </summary>
        public string StrFile
            => "file";

        /// <summary>
        /// DEFAULT = fichiers
        /// </summary>
        public string StrFiles
            => "files";

        /// <summary>
        /// DEFAULT = Recherche dans :
        /// </summary>
        public string StrLookInto
            => "Look into :";

        /// <summary>
        /// DEFAULT = Aucun fichier à nettoyer
        /// </summary>
        public string StrNoFileToClean
            => "No file to clean";

        /// <summary>
        /// DEFAULT = Aucun fichier à nettoyer
        /// $1 représente la taille du fichier à nettoyer
        /// </summary>
        public string StrOneFileToClean
            => "Clean the file - $1";

        /// <summary>
        /// DEFAULT = Aucun fichier à nettoyer
        /// $1 représente la quantité de fichiers à nettoyer
        /// $2 représente la taille totale des fichiers à nettoyer
        /// </summary>
        public string StrMultipleToClean
            => "Clean $1 files - $2";

        /// <summary>
        /// DEFAULT = Mettre à jour Revit Cleaner $1 vers $2
        /// $1 représente la version actuelle de l'application
        /// $2 représente la dernière version disponible en ligne
        /// </summary>
        public string StrUpdateTo
            => "Update Revit Cleaner $1 to $2";

        /// <summary>
        /// DEFAULT = Installer
        /// </summary>
        public string StrInstall
            => "Install";

        /// <summary>
        /// DEFAULT = Continuer
        /// </summary>
        public string StrContinue
            => "Continue";

        /// <summary>
        /// DEFAULT = Ignorer cette mise à jour
        /// </summary>
        public string StrSkip
            => "Skip this update";

        /// <summary>
        /// DEFAULT = Note de mise à jour :
        /// </summary>
        public string StrPatchNote
            => "Patch note :";

        #endregion
    }
}
