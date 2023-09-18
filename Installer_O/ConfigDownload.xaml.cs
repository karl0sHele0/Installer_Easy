using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Installer_O
{
    /// <summary>
    /// Lógica de interacción para ConfigDownload.xaml
    /// </summary>
    public partial class ConfigDownload : Window
    {
        public ConfigDownload()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AppNameKey = txtNameKey.Text;
            MainWindow.linkDownload = txtDownloadLink.Text;
            MainWindow.zipFileName = txtZipName.Text;
            MainWindow.exeFileName = txtExeName.Text;

            this.DialogResult = true;
            this.Close();
        }
    }
}
