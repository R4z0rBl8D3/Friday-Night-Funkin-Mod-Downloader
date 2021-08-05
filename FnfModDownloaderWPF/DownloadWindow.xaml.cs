using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.CompilerServices;
using System.Threading;

namespace FnfModDownloaderWPF
{
    /// <summary>
    /// Interaction logic for DownloadWindow.xaml
    /// </summary>
    public partial class DownloadWindow : Window
    {
        public static bool ShowBytes = false;
        public static bool ShowETA = false;
        public static string path = "";
        public static string name = "";
        public DownloadWindow()
        {
            InitializeComponent();
            Loaded += StartDownload;
        }

        private void StartDownload(object sender, RoutedEventArgs e)
        {
            StatusLbl.Content = "Downloading...";
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                path = "";
                name = MainWindow.NameValue;
                string extension = "";
                path = path + MainWindow.DirValue;
                if (MainWindow.ZipValue == true) { name = name + ".zip"; extension = "zip"; }
                if (MainWindow.SevenZValue == true) { name = name + ".7z"; extension = "7z"; }
                if (Directory.Exists("temp"))
                {
                    //Path exists already
                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory("temp");
                }
                if (MainWindow.ZipValue == true && MainWindow.SevenZValue == true)
                {
                    DirectoryInfo di = Directory.CreateDirectory(@"temp\" + MainWindow.NameValue);
                    wc.DownloadFileAsync(new System.Uri(MainWindow.LinkValue), @"temp\" + MainWindow.NameValue);
                }
                else { wc.DownloadFileAsync(new System.Uri(MainWindow.LinkValue), @"temp\" + name); }
                StreamWriter sw = new StreamWriter("Downloads.txt");
                sw.WriteLine(MainWindow.DirValue + MainWindow.NameValue + "|" + MainWindow.LinkValue + "|" + MainWindow.NameValue + "|" + extension);
                sw.Close();
            }
            
            
        }
        // Event to track the progress
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
            PercentLbl.Content = e.ProgressPercentage.ToString() + "%";
            StatusLbl.Content = "Downloading...";
            if (e.ProgressPercentage == 100)
            {
                StatusLbl.Content = "Creating directory...";
                if (Directory.Exists(MainWindow.DirValue + MainWindow.NameValue))
                {
                    //Path already exists
                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory(MainWindow.DirValue + name);
                }
                PercentLbl.Content = "???";
                if (MainWindow.ZipValue == true)
                {
                    if (MainWindow.Reinstall == true)
                    {
                        StatusLbl.Content = "Extracting...";
                        Thread.Sleep(1000);
                        ZipFile.ExtractToDirectory(@"temp\" + name, MainWindow.DirValue);
                        StatusLbl.Content = "Deleting Zip file...";
                        File.Delete(@"temp\" + name);
                        Directory.Delete(MainWindow.DirValue + name);
                        Process.Start("explorer.exe", MainWindow.DirValue);
                        this.Close();
                    }
                    else
                    {
                         StatusLbl.Content = "Extracting...";
                         Thread.Sleep(1000);
                         ZipFile.ExtractToDirectory(@"temp\" + name, MainWindow.DirValue + MainWindow.NameValue);
                         StatusLbl.Content = "Deleting Zip file...";
                         File.Delete(@"temp\" + name);
                         Directory.Delete(MainWindow.DirValue + name);
                         Process.Start("explorer.exe", MainWindow.DirValue + MainWindow.NameValue);
                         this.Close();
                    }
                   
                }
                if (MainWindow.SevenZValue == true)
                {
                    if (MainWindow.Reinstall == true)
                    {
                        StatusLbl.Content = "Extracting...";
                        ExtractFile(@"temp\" + name, MainWindow.DirValue);
                        StatusLbl.Content = "Deleting 7z file...";
                        File.Delete(@"temp\" + name);
                        Directory.Delete(MainWindow.DirValue + name);
                        Process.Start("explorer.exe", MainWindow.DirValue);
                        this.Close();
                    }
                    else
                    {
                        StatusLbl.Content = "Extracting...";
                        ExtractFile(@"temp\" + name, MainWindow.DirValue + MainWindow.NameValue);
                        StatusLbl.Content = "Deleting 7z file...";
                        File.Delete(@"temp\" + name);
                        Directory.Delete(MainWindow.DirValue + name);
                        Process.Start("explorer.exe", MainWindow.DirValue + MainWindow.NameValue);
                        this.Close();
                    }
                }
                MainWindow.Reinstall = false;
            }
        }

        public void ExtractFile(string sourceArchive, string destination)
        {
            string zPath = "7za.exe"; //add to proj and set CopyToOuputDir
            try
            {
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.WindowStyle = ProcessWindowStyle.Hidden;
                pro.FileName = zPath;
                pro.Arguments = string.Format("x \"{0}\" -y -o\"{1}\"", sourceArchive, destination);
                Process x = Process.Start(pro);
                x.WaitForExit();
            }
            catch (System.Exception Ex)
            {
                //handle error
            }
        }
    }

}

