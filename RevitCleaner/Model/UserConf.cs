using Microsoft.Win32;
using RevitCleaner.Strings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RevitCleaner.Model
{
    public class UserConf
    {
        public string SavePath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) 
            + $"\\{Environment.UserName}.json";
        public string LangId { get; set; }
        public string LastDirectory { get; set; }
        public Version SkipVersion { get; set; }
        public void Save()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string stockContent = JsonSerializer.Serialize<UserConf>(this, jso);
            File.WriteAllText(SavePath, stockContent);
        }
    }
}
