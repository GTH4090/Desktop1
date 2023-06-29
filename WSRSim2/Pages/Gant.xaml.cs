using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WSRSim2.Windows;

namespace WSRSim2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Gant.xaml
    /// </summary>
    public partial class Gant : Page
    {
        public Gant()
        {
            InitializeComponent();
            File.WriteAllText("Memory.txt", "3");
           
           

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GantWin gantWin = new GantWin();
            gantWin.ShowDialog();
            NavigationService.Navigate(new DashBoard());
        }
    }
}
