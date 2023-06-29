using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WSRSim2.Models;

namespace WSRSim2.Classes
{
    public static class Helper
    {
        public static WSRSim2Entities1 Db = new WSRSim2Entities1();
        public static Project SelectedProject = null;

        public static void Error(string message="Ошибка подключения к БД")
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static void Info(string message)
        {
            MessageBox.Show(message, "Инфо", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
