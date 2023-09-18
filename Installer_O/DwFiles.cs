using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer_O
{
    public class DwFiles
    {
        //Zip File or name of source File
        public string nameFile { get; set; }
        
        //Location to save dowloaded file
        public string pathFile { get; set; }

        //URI to download File
        public string linkSource { get; set; }
        public string pathCompleted { get { return pathFile + "\\" + nameFile;  } }
    }
}
