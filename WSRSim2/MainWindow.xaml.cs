using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WSRSim2.Models;
using WSRSim2.Pages;
using static WSRSim2.Classes.Helper;

namespace WSRSim2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadData()
        {

            try
            {
                ProjLv.ItemsSource = Db.Project.ToList();
                ProjLv.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
        }

        private void DashBoardBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashBoard());

        }

        private void TaskBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TaskList());

        }

        private void GantBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Gant());

        }

        private void ProjLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProject = ProjLv.SelectedItem as Project;
            if (File.Exists("Memory.Txt"))
            {
                if(File.ReadAllText("Memory.Txt") == "1")
                {
                    MainFrame.Navigate(new DashBoard());
                }
                if (File.ReadAllText("Memory.Txt") == "2")
                {
                    MainFrame.Navigate(new TaskList());
                }
                if (File.ReadAllText("Memory.Txt") == "3")
                {
                    MainFrame.Navigate(new Gant());
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            VersionLb.Content = version.Major + "." + version.Minor + version.Build.ToString() + version.Revision.ToString("D5");
            loadData();

        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            TitleLbl.Content = (e.Content as Page).Title;
            if (MainFrame.CanGoBack)
            {
                BackBtn.Visibility = Visibility.Visible;
            }
            else
            {
                BackBtn.Visibility = Visibility.Collapsed;
            }
        }
    }
}
