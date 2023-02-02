namespace RevitCleaner.Core
{
    public class Functions
    {
        /// <summary>
        /// Recupère la liste des fichiers dans le dossier de type ".rvt", ".rte", ".rfa", ".rft".
        /// </summary>
        /// <param name="dirPath">Dossier cible.</param>
        /// <returns>Une liste de fichiers.</returns>
        public static List<FileInfo>? GetFiles (string dirPath)
        {
            DirectoryInfo directoryInfo= new DirectoryInfo(dirPath);
            List<FileInfo> files = new List<FileInfo>();
            try
            {
                files.AddRange(directoryInfo.GetFiles("*", SearchOption.AllDirectories)
                    .Where(x => IsRevitFile(x.Extension) && IsSaveFile(x.FullName))
                    .ToList());

                return files;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Détermine si l'extension est une extension Revit.
        /// </summary>
        /// <param name="file">EXTENSION du fichier.</param>
        /// <returns>True si le fichier est un fichier Revit, sinon False.</returns>
        private static bool IsRevitFile(string file)
        {
            return file == ".rvt" || file == ".rte" || file == ".rfa" || file == ".rft";
        }

        /// <summary>
        /// Déterminé si le fichier est un fichier de sauvegarde.
        /// </summary>
        /// <param name="file">Chemin du fichier complet.</param>
        /// <returns>True si le fichier est un fichier de sauvegarde, sinon False.</returns>
        private static bool IsSaveFile(string file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);

            if (string.IsNullOrEmpty(fileName)) return false;
            if (!fileName.Contains('.')) return false;

            string lastComponent = file.Substring(fileName.LastIndexOf("."));

            if (lastComponent.Length != 4) return false;
            if (int.TryParse(lastComponent, out int i)) return true;
            else return false;
        }

        /// <summary>
        /// Filtre une liste de fichiers selon le filtre indiqué.
        /// </summary>
        /// <param name="files">La liste de fichiers à filtrer.</param>
        /// <param name="filter">Le filtre dont chaque particule de filtre est séparée par une virgule.</param>
        /// <returns>Une liste de fichiers filtrés.</returns>
        public static List<FileInfo>? FilterFiles (List<FileInfo> files, string filter)
        {
            if (files == null) return null;
            if (string.IsNullOrWhiteSpace(filter)) return files;

            List<string> foldersFilter = GetFolderFilter(filter);
            List<string> filesFilter = GetFileFilter(filter);

            List<FileInfo> filteredFiles = new List<FileInfo>();

            foreach (FileInfo file in files)
            {

            }

            return filteredFiles;
        }

        private static List<string> GetFolderFilter(string filter)
        {
            List<string> folders = new List<string>();
            if (!filter.Contains('@')) return folders;

            if (filter.Contains(','))
            {
                string[] component = filter.Split(',');
                foreach(string s in component)
                {
                    if (s.StartsWith("@"))
                    {
                        string f = s.Remove(0, 1);
                        f = f.Trim();
                        folders.Add(f);
                    }
                }
            }
            else
            {
                if (filter.StartsWith("@"))
                {
                    string f = filter.Remove(0, 1);
                    f = f.Trim();
                    folders.Add(f);
                }
            }

            return folders;
        }

        private static List<string> GetFileFilter(string filter)
        {
            List<string> files = new List<string>();

            if (filter.Contains(','))
            {
                string[] component = filter.Split(',');
                foreach (string s in component)
                {
                    if (s.StartsWith("@")) files.Add(s.Trim());
                }
            }
            else if (!filter.StartsWith("@")) files.Add(filter.Trim());
            

            return files;
        }
    }
}