
namespace RevitCleaner.Strings
{
    public class Lang_pt : ILanguage
    {
        public string Id => "pt";
        public string TranslateAuthor => "Traduzido por Carina Maccari Blazius";
        public string LanguageName => "Português Brasil";

        /// <summary>
        /// DEFAULT = Dossier de recherche :
        /// </summary>
        public string DirectoryTextBoxHeader
            => "Diretório de pesquisa:";

        /// <summary>
        /// DEFAULT = Indiquez le dossier dans lequel chercher les éléments à nettoyer.
        /// </summary>
        public string DirectoryTextBoxToolTip
            => "Escolha a pasta que contêm os arquivos a serem excluídos.";

        /// <summary>
        /// DEFAULT = Ouvrir un explorateur de dossier.
        /// </summary>
        public string SearchButtonTooTip
            => "Abrir o explorador de arquivos.";

        /// <summary>
        /// DEFAULT = Rafraichir la liste des fichiers du dossier.
        /// </summary>
        public string RefreshButtonToolTip
            => "Atualizar a lista de arquivos na pasta.";

        /// <summary>
        /// DEFAULT = Filtre de recherche :
        /// </summary>
        public string SearchTextBoxHeader
            => "Filtro de pesquisa:";

        /// <summary>
        /// DEFAULT = Project, Modèle générique, @Etudes
        /// </summary>
        public string SearchTextBoxPlaceHolder
            => "Projeto, Modelo genérico, @Equipe";

        /// <summary>
        /// DEFAULT = 
        /// C'est dans cette zone que vous pouvez filtrer la liste des éléments trouvés.\nSéparez tous vos composants de filtre par \",\".\n\nPour obtenir plus d'informations sur les fonctionnalités et raccourcis possibles appuyez sur \"F1\".
        /// </summary>
        public string SearchTextBoxToolTip
            => "Nessa sessão você tem a possibilidade de filtrar a lista de arquivos encontrados.\n " +
            "Separe todos os arquivos a serem procurados por \",\".\n\n Para obter ajuda e saber mais sobre os atalhos disponíveis, clique em \"F1\".";

        /// <summary>
        /// DEFAULT = Recherche stricte activée
        /// </summary>
        public string CaseSensitiveToggleSwitchOn
            => "Procura precisa ativada";

        /// <summary>
        /// DEFAULT = Recherche stricte désactivée
        /// </summary>
        public string CaseSensitiveToggleSwitchOff
            => "Procura precisa desativada";

        /// <summary>
        /// DEFAULT = Ouvrir
        /// </summary>
        public string ListItemOpenButtonText
            => "Abrir";

        /// <summary>
        /// DEFAULT = Actions globales :
        /// </summary>
        public string GlobalActionTitle
            => "Opções gerais :";

        /// <summary>
        /// DEFAULT = Tout cocher
        /// </summary>
        public string SelectAllText
            => "Marcar tudo";

        /// <summary>
        /// DEFAULT = Coche tous les éléments sans tenir compte du filtre.
        /// </summary>
        public string SelectAllToolTip
            => "Marcar todos os arquivos sem levar em consideração o filtro escolhido.";

        /// <summary>
        /// DEFAULT = Tout décocher
        /// </summary>
        public string UnSelectAllText
            => "Desmarcar tudo";

        /// <summary>
        /// DEFAULT = Décoche tous les éléments sans tenir compte du filtre.
        /// </summary>
        public string UnSelectAllToolTip
            => "Desmarcar todos os arquivos sem levar em consideração o filtro escolhido.";

        /// <summary>
        /// DEFAULT = Tout inverser
        /// </summary>
        public string InvertAllText
            => "Inverter tudo";

        /// <summary>
        /// DEFAULT = Inverse la sélection de tous les éléments sans tenir compte du filtre.
        /// </summary>
        public string InvertAllToolTip
            => "Inverter todos os arquivos sem levar em consideração o filtro escolhido.";

        /// <summary>
        /// DEFAULT = Actions sur la sélection :
        /// </summary>
        public string FilteredActionTiTle
            => "Opções sobre a seleção :";

        /// <summary>
        /// DEFAULT = Cocher filtrés
        /// </summary>
        public string FilteredSelectAllText
            => "Marcar filtrados";

        /// <summary>
        /// DEFAULT = Coche tout les éléments filtrés.
        /// </summary>
        public string FilteredSelectAllToolTip
            => "Marcar todos os arquivos filtrados.";

        /// <summary>
        /// DEFAULT = Décocher filtrés
        /// </summary>
        public string FilteredUnSelectAllText
            => "Desmarcar filtrados";

        /// <summary>
        /// DEFAULT = Décoche tout les éléments filtrés.
        /// </summary>
        public string FilteredUnSelectAllToolTip
            => "Desmarcar todos os arquivos filtrados.";

        /// <summary>
        /// DEFAULT = Inverser filtrés
        /// </summary>
        public string FilteredInvertAllText
            => "Inverter filtrados";

        /// <summary>
        /// DEFAULT = Inverse la sélection de tout les éléments filtrés.
        /// </summary>
        public string FilteredInvertAllToolTip
            => "Inverter todos os arquivos filtrados.";

        /// <summary>
        /// DEFAULT = Générer un rapport de suppression
        /// </summary>
        public string DeleteReportSwitchOn
            => "Gerar um relatório de arquivos excluídos";

        /// <summary>
        /// DEFAULT = Ne pas générer de rapport de suppression
        /// </summary>
        public string DeleteReportSwitchOff
            => "Não gerar um relatório de arquivos excluídos";

        /// <summary>
        /// DEFAULT = Les rapports de suppresion se trouve dans ..\\Mes Documents\\Revit Cleaner Reports
        /// </summary>
        public string DeleteReportSwitchToolTip
            => "Os relatório de arquivos excluídos se encontram em ..\\Meus Documentos\\Revit Cleaner Reports";

        /// <summary>
        /// DEFAULT = Cette action est irréversible. Réfléchissez avant de cliquer.
        /// </summary>
        public string DeleteButtonToolTip
            => "Essa ação é irreversível. Você tem certeza que deseja continuar?";

        /// <summary>
        /// DEFAULT = Presser une seconde fois pour confirmer
        /// </summary>
        public string DeleteButtonConfirmMessage
            => "Clique mais uma vez para confirmar";

        /// <summary>
        /// DEFAULT = Une erreur c'est produite. Veuillez redémarrer votre ordinateur puis retenter la mise à jour. Si le problème persiste, communiquez ce code erreur : $1
        /// </summary>
        public string UpdateErrorMessage
            => "Houve um erro. Reinicie seu computador e verifique se existe uma atualização. Se o problema persistir, comunique esse código de erro : $1";

        #region Common

        /// <summary>
        /// DEFAULT = Aucun fichier à afficher
        /// </summary>
        public string StrNoFile
            => "Nenhum arquivo a ser apresentado na pasta";

        /// <summary>
        /// DEFAULT = Auncun fichier affiché
        /// </summary>
        public string StrNoShowedFile
            => "Nenhum arquivo encontrado";

        /// <summary>
        /// DEFAULT = 1 fichier affiché
        /// </summary>
        public string StrOneShowedFile
            => "1 arquivo encontrado";

        /// <summary>
        /// DEFAULT = $1 fichiers affichés
        /// $1 représente le nombre de fichiers qui correspondent au tri
        /// </summary>
        public string StrMultipleShowedFile
            => "$1 arquivos encontrados";

        /// <summary>
        /// DEFAULT = fichier
        /// </summary>
        public string StrFile
            => "arquivo";

        /// <summary>
        /// DEFAULT = fichiers
        /// </summary>
        public string StrFiles
            => "arquivos";

        /// <summary>
        /// DEFAULT = Recherche dans :
        /// </summary>
        public string StrLookInto
            => "Pesquisar em :";

        /// <summary>
        /// DEFAULT = Aucun fichier à nettoyer
        /// </summary>
        public string StrNoFileToClean
            => "Nenhum arquivo a ser excluído";

        /// <summary>
        /// DEFAULT = Aucun fichier à nettoyer
        /// $1 représente la taille du fichier à nettoyer
        /// </summary>
        public string StrOneFileToClean
            => "Excluir o arquivo - $1";

        /// <summary>
        /// DEFAULT = Aucun fichier à nettoyer
        /// $1 représente la quantité de fichiers à nettoyer
        /// $2 représente la taille totale des fichiers à nettoyer
        /// </summary>
        public string StrMultipleToClean
            => "Excluir $1 arquivos - $2";

        /// <summary>
        /// DEFAULT = Mettre à jour Revit Cleaner $1 vers $2
        /// $1 représente la version actuelle de l'application
        /// $2 représente la dernière version disponible en ligne
        /// </summary>
        public string StrUpdateTo
            => "Atualizae Revit Cleaner $1 para a versão $2";

        /// <summary>
        /// DEFAULT = Installer
        /// </summary>
        public string StrInstall
            => "Instalar";

        /// <summary>
        /// DEFAULT = Continuer
        /// </summary>
        public string StrContinue
            => "Continuar";

        /// <summary>
        /// DEFAULT = Ignorer cette mise à jour
        /// </summary>
        public string StrSkip
            => "Ignorar essa atualização";

        /// <summary>
        /// DEFAULT = Note de mise à jour :
        /// </summary>
        public string StrPatchNote
            => "Nota da atualização :";

        #endregion
    }
}
