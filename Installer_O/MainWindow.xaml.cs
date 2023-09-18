using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Installer_O
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string AppNameKey = "EasyO";
        public static string zipFileName = "EasyO_v1.0800.zip";
        public static string exeFileName = "EasyOrchestra.exe";
        public static string linkDownload = "https://www.dropbox.com/scl/fi/es85xur3j176zjity3jpf/EasyO_v1.0800.zip?rlkey=5i2m5sgoz7xhw0kof77wrsf7n&dl=1";

        private string Appdata;
        private string LocationFiles;
        private string StartupPath;
        private string commonStartMenuPath;
        bool makeShortcut = false;

        private List<DwFiles> DwFiles = new List<DwFiles>();
        private WebClient webClient = new WebClient();

        public MainWindow()
        {
            InitializeComponent();
            Appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + AppNameKey;
            LocationFiles = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\" + AppNameKey;
            StartupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
            txtPath.Content = LocationFiles;

            DwFiles.Add(new DwFiles{ nameFile = zipFileName, pathFile = LocationFiles, linkSource = linkDownload });
        }

        private async void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            btnChooseFolder.IsEnabled = false;
            btnDownload.IsEnabled = false;

            //btnDownload.Content = "Cancelar";
            try
            {
                if (!Directory.Exists(txtPath.Content.ToString()))
                    Directory.CreateDirectory(txtPath.Content.ToString());

                //Descargar archivo
                progressBar.IsIndeterminate = true;
                await DownloadAsync(DwFiles.ElementAt(0));
                progressBar.IsIndeterminate = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                progressBar.IsIndeterminate = false;
                System.Windows.MessageBox.Show(Tags.ErrorD01, Tags.Error01, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            //Descomprimir archivo
            ZipFile.ExtractToDirectory(DwFiles.ElementAt(0).pathCompleted,LocationFiles);

            //Borrar Archivo Comprimido
            System.IO.File.Delete(DwFiles.ElementAt(0).pathCompleted);

            //Create Shortcut
            if(makeShortcut)
                CreateShortCut(StartupPath, AppNameKey, LocationFiles + "\\" + exeFileName);

            AddToStartMenu();

            progressBar.Value = 100;

            var p_info = new ProcessStartInfo
            {
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = LocationFiles,
                FileName = LocationFiles + "\\" + exeFileName
            };
            Process.Start(p_info);

            System.Windows.MessageBox.Show(Tags.CompleteDt, Tags.Completed, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            Environment.Exit(0);

        }

        private async Task DownloadAsync(DwFiles dwFile)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var s = await client.GetStreamAsync(dwFile.linkSource))
                    {
                        using (var fs = new FileStream(dwFile.pathCompleted, FileMode.OpenOrCreate))
                        {
                            await s.CopyToAsync(fs);
                        }
                    }
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        private void CreateShortCut(string exportPath, string nameShortcut, string appFile)
        {
            string shortcutLocation = System.IO.Path.Combine(exportPath, nameShortcut + ".lnk");

            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);
            string relativePath = System.IO.Path.GetDirectoryName(appFile);
            shortcut.WorkingDirectory = relativePath;

            shortcut.Description = AppNameKey + " by heleodoro.com";  
            shortcut.IconLocation = appFile;
            shortcut.TargetPath = appFile;
            shortcut.Save();

        }

        private void AddToStartMenu()
        {
            //Add to Program Star
            string appStartMenuPath = System.IO.Path.Combine(commonStartMenuPath, "Programs", AppNameKey);
            if (!Directory.Exists(appStartMenuPath))
                 Directory.CreateDirectory(appStartMenuPath);

            CreateShortCut(appStartMenuPath, AppNameKey, (LocationFiles + "\\" + exeFileName));
        }

        private void btnChooseFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LocationFiles = folderBrowserDialog1.SelectedPath;
                txtPath.Content = LocationFiles + "\\" + AppNameKey;
            }
            else
            {
                txtPath.Content = LocationFiles;
            }
        }

        private void checkToStart(object sender, RoutedEventArgs e)
        {
            makeShortcut = (bool)chbxStart.IsChecked;
        }

        private void btnConfigData_Click(object sender, RoutedEventArgs e)
        {
            ConfigDownload cd = new ConfigDownload();
            if( cd.ShowDialog() == true)
            {
                Appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + AppNameKey;
                txtPath.Content = LocationFiles + "\\" + AppNameKey;
                DwFiles.ElementAt(0).nameFile = zipFileName;
                DwFiles.ElementAt(0).linkSource = linkDownload;
                DwFiles.ElementAt(0).pathFile = LocationFiles + "\\" + AppNameKey;
            }
        }
    }
}
