using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer_O
{
    public class DwFiles
    {
        public string nameFile { get; set; }
        public string pathFile { get; set; }
        public string linkSource { get; set; }
        public string pathCompleted { get { return pathFile + "\\" + nameFile;  } }
    }
}
