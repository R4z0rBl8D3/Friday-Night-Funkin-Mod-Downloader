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
using System.IO;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;

namespace FnfModDownloaderWPF
{
    /// <summary>
    /// Interaction logic for ManageWindow.xaml
    /// </summary>
    public partial class ManageWindow : Window
    {
        public static List<string> DownloadData = new List<string>();
        public ManageWindow()
        {
            InitializeComponent();
            Loaded += MyWindow_Loaded;
            
            //image.Source = imageSource;
            //string Selected = ListBox.SelectedValue.ToString();
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadList();
        }
        private void ReloadList()
        {
            //StatusLbl.Content = "Reloading...";
            //List<string> deleteList = new List<string>();
            //deleteList.Clear();


            //ListBox.Items.Clear();
            //foreach (string text in DownloadData)
            //{
            //if (Directory.Exists(text.Split('|')[0])) {}
            //else { deleteList.Add(text); }
            //}
            //foreach (string temp in deleteList)
            //{
            //DeleteLineFromDownloadstxt(temp);
            //}
            //foreach (string text in DownloadData)
            //{
            //ListBox.Items.Add(text.Split('|')[2] + "  Path: " + text.Split('|')[0]);
            //}
            //StatusLbl.Content = "Ready";

            List<string> deleteList = new List<string>();
            if (!File.Exists("Downloads.txt")) { File.Create("Downloads.txt"); }
            StreamReader sr = new StreamReader("Downloads.txt");
            ListBox.Items.Clear();
            while (!sr.EndOfStream)
            {
                //lol help idk what to name the variables anymore 
                string temp = sr.ReadLine();
                if (Directory.Exists(temp.Split('|')[0]))
                {
                    DownloadData.Add(temp);
                    ListBox.Items.Add(temp.Split('|')[2] + "  Path: " + temp.Split('|')[0]);
                }
                else
                {
                    deleteList.Add(temp);
                }

            }
            sr.Close();
            foreach (string temp in deleteList)
            {
                DeleteLineFromDownloadstxt(temp);
            }
            if (DownloadData.Count == 0) { ListBox.Items.Add("There are no mods downloaded, or it is not detected."); }
            StatusLbl.Content = "Ready";
        }

        private void DeleteLineFromDownloadstxt(string remove)
        {
            StatusLbl.Content = "Editing file...";
            string tempFile = Path.GetTempFileName() ;

            using (var sr = new StreamReader("Downloads.txt"))
            using (var sw = new StreamWriter(tempFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line != remove)
                        sw.WriteLine(line);
                }
            }

            File.Delete("Downloads.txt");
            File.Move(tempFile, "Downloads.txt");
            StatusLbl.Content = "Ready";
        }
        
        private void ToolBarOpenBtn_Click(object sender, RoutedEventArgs e)
        {
            int Selected = ListBox.SelectedIndex;
            if (Selected == -1)
            {
                MessageBox.Show("You need to select something to continue", "Can't continue!", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusLbl.Content = "Ready";
                return;
            }
            Process.Start("explorer.exe", DownloadData[Selected].Split('|')[0]);
        }

        private void ToolBarDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int Selected = ListBox.SelectedIndex;
            if (Selected == -1)
            {
                MessageBox.Show("You need to select something to continue", "Can't continue!", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusLbl.Content = "Ready";
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete the mod?", "My App", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Directory.Delete(DownloadData[Selected].Split('|')[0], true);
                    ReloadList();
                    DeleteLineFromDownloadstxt(DownloadData[Selected]);
                    DownloadData.Remove(DownloadData[Selected]);
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void ToolBarReinstallBtn_Click(object sender, RoutedEventArgs e)
        {
            int Selected = ListBox.SelectedIndex;
            if (Selected == -1)
            {
                MessageBox.Show("You need to select something to continue", "Can't continue!", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusLbl.Content = "Ready";
                return;
            }
            StatusLbl.Content = "Deleting mod...";
            MessageBoxResult result = MessageBox.Show("Are you sure you want to reinstall the mod?", "My App", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Directory.Delete(DownloadData[Selected].Split('|')[0], true);
                    ReloadList();
                    DeleteLineFromDownloadstxt(DownloadData[Selected]);
                    StatusLbl.Content = "Downloading mod...";
                    MainWindow.LinkValue = DownloadData[Selected].Split('|')[1];
                    MainWindow.NameValue = DownloadData[Selected].Split('|')[2];
                    MainWindow.DirValue = DownloadData[Selected].Split('|')[0];
                    MainWindow.Reinstall = true;
                    if (DownloadData[Selected].Split('|')[3] == "zip") { MainWindow.ZipValue = true; }
                    if (DownloadData[Selected].Split('|')[3] == "7z") { MainWindow.SevenZValue = true; }
                    DownloadData.Remove(DownloadData[Selected]);
                    DownloadWindow downloadWindow = new DownloadWindow();
                    downloadWindow.ShowDialog();
                    break;
                case MessageBoxResult.No:
                    break;
            }
            StatusLbl.Content = "Ready";
        }

        private void ToolBarReloadBtn_Click(object sender, RoutedEventArgs e)
        {
            ReloadList();
        }
    }
}
