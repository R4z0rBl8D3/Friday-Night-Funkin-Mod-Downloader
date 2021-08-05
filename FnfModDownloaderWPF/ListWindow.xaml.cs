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

namespace FnfModDownloaderWPF
{
    /// <summary>
    /// Interaction logic for ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        public static string[] mods =
        {
            "The Full-Ass Tricky Mod|https://gamebanana.com/dl/583960|zip",
            "B-Side Remixes|https://gamebanana.com/dl/554959|zip",
            "Hatsune Miku|https://gamebanana.com/dl/534884|zip",
            "Smoke 'Em Out Struggle (Garcello)|https://gamebanana.com/dl/563495|zip",
            "Hex Mod|https://gamebanana.com/dl/535849|zip",
            "Arcade Showdown (Kapi)|https://gamebanana.com/dl/580564|zip",
            "Friday Night Funkin' Multiplayer|https://gamebanana.com/dl/542560|zip",
            "Neo|https://gamebanana.com/dl/526052|zip",
            "Tabi|https://gamebanana.com/dl/577675|zip",
            "Agoti|https://gamebanana.com/dl/598687|zip",
            "Zardy|https://gamebanana.com/dl/537613|zip",
            "Shaggy|https://gamebanana.com/dl/579603|zip",
            "Matt|https://gamebanana.com/dl/587470|zip",
            "literally every fnf mod ever (Bob)|https://gamebanana.com/dl/611533|zip",
            "Friday Night Funkin’ HD|https://gamebanana.com/dl/621798|zip",
            "Friday Night Funkin, but bad|https://gamebanana.com/dl/529588|zip",
            "Monika|https://gamebanana.com/dl/603297|zip",
            "Imposter V2|https://gamebanana.com/dl/576560|zip",
            "Carol V2|https://gamebanana.com/dl/541805|zip",
            "Tord|https://gamebanana.com/dl/540211|zip",
            "Starlight Mayhem (Cj)|https://gamebanana.com/dl/575291|zip",
            "Fnf|https://www.mcpe117.com/funkin-windows-64bit.zip|zip",
            "Autoplay Mod|https://gamebanana.com/dl/570755|7z"
        };
        public ListWindow()
        {
            InitializeComponent();
            Loaded += MyWindow_Loaded;
        }
        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StatusLbl.Content = "Loading...";
            foreach(string text in mods)
            {
                ListBox.Items.Add(text.Split('|')[0]);
            }
            StatusLbl.Content = "Ready";
        }
        private void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            StatusLbl.Content = "Downloading...";
            int Selected = ListBox.SelectedIndex;
            if (Selected == -1)
            {
                MessageBox.Show("You need to select something to continue", "Can't continue!", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusLbl.Content = "Ready";
                return;
            }
            MainWindow.NameValue = mods[Selected].Split('|')[0];
            MainWindow.LinkValue = mods[Selected].Split('|')[1];
            if (mods[Selected].Split('|')[2] == "zip") { MainWindow.ZipValue = true; }
            if (mods[Selected].Split('|')[2] == "7z") { MainWindow.SevenZValue = true; }
            DownloadWindow downloadWindow = new DownloadWindow();
            downloadWindow.ShowDialog();
            StatusLbl.Content = "Ready";
        }
    }
}
