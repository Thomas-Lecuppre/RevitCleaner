using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitCleaner
{
    public class ChangeLog
    {
        public enum ChangeLogType { Update = 1, Add = 2,  Remove = 3 }
        public ChangeLogType Type { get; set; }
        public string Change { get; set; }
    }
}
