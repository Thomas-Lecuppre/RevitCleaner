
namespace RevitCleaner.Strings
{
    public class Lang_fr : ILanguage
    {
        public string Id => "fr";
        public string TranslateAuthor => "Traduit par Thomas Lecuppre";
        public string LanguageName => "Français";

        /// <summary>
        /// DEFAULT = Dossier de recherche :
        /// </summary>
        public string DirectoryTextBoxHeader
            => "Dossier de recherche :";

        /// <summary>
        /// DEFAULT = Indiquez le dossier dans lequel chercher les éléments à nettoyer.
        /// </summary>
        public string DirectoryTextBoxToolTip
            => "Indiquez le dossier dans lequel chercher les éléments à nettoyer.";

        /// <summary>
        /// DEFAULT = Ouvrir un explorateur de dossier.
        /// </summary>
        public string SearchButtonTooTip
            => "Ouvrir un explorateur de dossier.";

        /// <summary>
        /// DEFAULT = Rafraichir la liste des fichiers du dossier.
        /// </summary>
        public string RefreshButtonToolTip
            => "Rafraichir la liste des fichiers du dossier.";

        /// <summary>
        /// DEFAULT = Filtre de recherche :
        /// </summary>
        public string SearchTextBoxHeader
            => "Filtre de recherche :";

        /// <summary>
        /// DEFAULT = Project, Modèle générique, @Etudes
        /// </summary>
        public string SearchTextBoxPlaceHolder
            => "Project, Modèle générique, @Etudes";

        /// <summary>
        /// DEFAULT = 
        /// C'est dans cette zone que vous pouvez filtrer la liste des éléments trouvés.\nSéparez tous vos composants de filtre par \",\".\n\nPour obtenir plus d'informations sur les fonctionnalités et raccourcis possibles appuyez sur \"F1\".
        /// </summary>
        public string SearchTextBoxToolTip
            => "C'est dans cette zone que vous pouvez filtrer la liste des éléments trouvés.\nSéparez tous vos composants de filtre par \",\".\n\nPour obtenir plus d'informations sur les fonctionnalités et raccourcis possibles appuyez sur \"F1\".";

        /// <summary>
        /// DEFAULT = Recherche stricte activée
        /// </summary>
        public string CaseSensitiveToggleSwitchOn
            => "Recherche stricte activée";

        /// <summary>
        /// DEFAULT = Recherche stricte désactivée
        /// </summary>
        public string CaseSensitiveToggleSwitchOff
            => "Recherche stricte désactivée";

        /// <summary>
        /// DEFAULT = Ouvrir
        /// </summary>
        public string ListItemOpenButtonText
            => "Ouvrir";

        /// <summary>
        /// DEFAULT = Actions globales :
        /// </summary>
        public string GlobalActionTitle
            => "Actions globales :";

        /// <summary>
        /// DEFAULT = Tout cocher
        /// </summary>
        public string SelectAllText
            => "Tout cocher";

        /// <summary>
        /// DEFAULT = Coche tous les éléments sans tenir compte du filtre.
        /// </summary>
        public string SelectAllToolTip
            => "Coche tous les éléments sans tenir compte du filtre.";

        /// <summary>
        /// DEFAULT = Tout décocher
        /// </summary>
        public string UnSelectAllText
            => "Tout décocher";

        /// <summary>
        /// DEFAULT = Décoche tous les éléments sans tenir compte du filtre.
        /// </summary>
        public string UnSelectAllToolTip
            => "Décoche tous les éléments sans tenir compte du filtre.";

        /// <summary>
        /// DEFAULT = Tout inverser
        /// </summary>
        public string InvertAllText
            => "Tout inverser";

        /// <summary>
        /// DEFAULT = Inverse la sélection de tous les éléments sans tenir compte du filtre.
        /// </summary>
        public string InvertAllToolTip
            => "Inverse la sélection de tous les éléments sans tenir compte du filtre.";

        /// <summary>
        /// DEFAULT = Actions sur la sélection :
        /// </summary>
        public string FilteredActionTiTle
            => "Actions sur la sélection :";

        /// <summary>
        /// DEFAULT = Cocher filtrés
        /// </summary>
        public string FilteredSelectAllText
            => "Cocher filtrés";

        /// <summary>
        /// DEFAULT = Coche tout les éléments filtrés.
        /// </summary>
        public string FilteredSelectAllToolTip
            => "Coche tout les éléments filtrés.";

        /// <summary>
        /// DEFAULT = Décocher filtrés
        /// </summary>
        public string FilteredUnSelectAllText
            => "Décocher filtrés";

        /// <summary>
        /// DEFAULT = Décoche tout les éléments filtrés.
        /// </summary>
        public string FilteredUnSelectAllToolTip
            => "Décoche tout les éléments filtrés.";

        /// <summary>
        /// DEFAULT = Inverser filtrés
        /// </summary>
        public string FilteredInvertAllText
            => "Inverser filtrés";

        /// <summary>
        /// DEFAULT = Inverse la sélection de tout les éléments filtrés.
        /// </summary>
        public string FilteredInvertAllToolTip
            => "Inverse la sélection de tout les éléments filtrés.";

        /// <summary>
        /// DEFAULT = Générer un rapport de suppression
        /// </summary>
        public string DeleteReportSwitchOn
            => "Générer un rapport de suppression";

        /// <summary>
        /// DEFAULT = Ne pas générer de rapport de suppression
        /// </summary>
        public string DeleteReportSwitchOff
            => "Ne pas générer de rapport de suppression";

        /// <summary>
        /// DEFAULT = Les rapports de suppresion se trouve dans ..\\Mes Documents\\Revit Cleaner Reports
        /// </summary>
        public string DeleteReportSwitchToolTip
            => "Les rapports de suppresion se trouve dans ..\\Mes Documents\\Revit Cleaner Reports";

        /// <summary>
        /// DEFAULT = Cette action est irréversible. Réfléchissez avant de cliquer.
        /// </summary>
        public string DeleteButtonToolTip
            => "Cette action est irréversible. Réfléchissez avant de cliquer.";

        /// <summary>
        /// DEFAULT = Presser une seconde fois pour confirmer
        /// </summary>
        public string DeleteButtonConfirmMessage
            => "Presser une seconde fois pour confirmer";

        /// <summary>
        /// DEFAULT = Une erreur c'est produite. Veuillez redémarrer votre ordinateur puis retenter la mise à jour. Si le problème persiste, communiquez ce code erreur : $1
        /// </summary>
        public string UpdateErrorMessage
            => "Une erreur c'est produite. Veuillez redémarrer votre ordinateur puis retenter la mise à jour. Si le problème persiste, communiquez ce code erreur : $1";

        #region Common

        /// <summary>
        /// DEFAULT = Aucun fichier à afficher
        /// </summary>
        public string StrNoFile
            => "Aucun fichier à afficher";

        /// <summary>
        /// DEFAULT = Auncun fichier affiché
        /// </summary>
        public string StrNoShowedFile
            => "Auncun fichier affiché";

        /// <summary>
        /// DEFAULT = 1 fichier affiché
        /// </summary>
        public string StrOneShowedFile
            => "1 fichier affiché";

        /// <summary>
        /// DEFAULT = $1 fichiers affichés
        /// $1 représente le nombre de fichiers qui correspondent au tri
        /// </summary>
        public string StrMultipleShowedFile
            => "$1 fichiers affichés";

        /// <summary>
        /// DEFAULT = fichier
        /// </summary>
        public string StrFile
            => "fichier";

        /// <summary>
        /// DEFAULT = fichiers
        /// </summary>
        public string StrFiles
            => "fichiers";

        /// <summary>
        /// DEFAULT = Recherche dans :
        /// </summary>
        public string StrLookInto
            => "Recherche dans :";

        /// <summary>
        /// DEFAULT = Aucun fichier à nettoyer
        /// </summary>
        public string StrNoFileToClean
            => "Aucun fichier à nettoyer";

        /// <summary>
        /// DEFAULT = Aucun fichier à nettoyer
        /// $1 représente la taille du fichier à nettoyer
        /// </summary>
        public string StrOneFileToClean
            => "Nettoyer le fichier - $1";

        /// <summary>
        /// DEFAULT = Aucun fichier à nettoyer
        /// $1 représente la quantité de fichiers à nettoyer
        /// $2 représente la taille totale des fichiers à nettoyer
        /// </summary>
        public string StrMultipleToClean
            => "Nettoyer $1 fichiers - $2";

        /// <summary>
        /// DEFAULT = Mettre à jour Revit Cleaner $1 vers $2
        /// $1 représente la version actuelle de l'application
        /// $2 représente la dernière version disponible en ligne
        /// </summary>
        public string StrUpdateTo
            => "Mettre à jour Revit Cleaner $1 vers $2";

        /// <summary>
        /// DEFAULT = Installer
        /// </summary>
        public string StrInstall
            => "Installer";

        /// <summary>
        /// DEFAULT = Continuer
        /// </summary>
        public string StrContinue
            => "Continuer";

        /// <summary>
        /// DEFAULT = Ignorer cette mise à jour
        /// </summary>
        public string StrSkip
            => "Ignorer cette mise à jour";

        /// <summary>
        /// DEFAULT = Note de mise à jour :
        /// </summary>
        public string StrPatchNote
            => "Note de mise à jour :";

        #endregion
    }
}
