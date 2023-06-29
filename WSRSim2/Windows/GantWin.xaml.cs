using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using static WSRSim2.Classes.Helper;

namespace WSRSim2.Windows
{
    /// <summary>
    /// Логика взаимодействия для GantWin.xaml
    /// </summary>
    public partial class GantWin : Window
    {
        DateTime StartDate = DateTime.Now.Date;
        DateTime EndDate = DateTime.Now.Date;
        bool isFirst = true;
        public GantWin()
        {
            InitializeComponent();
            
        }
        private void loadGant()
        {

            try
            {
                if(GantGrid != null)
                {
                    GantGrid.RowDefinitions.Clear();
                    GantGrid.ColumnDefinitions.Clear();
                    GantGrid.Children.Clear();
                }
                else
                {
                    return;
                }
                for(int i = 0;i <= Db.Task.Count(); i++)
                {
                    GantGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                }
                for(int i = 0; i <= (EndDate.Date - StartDate.Date).Days; i++)
                {
                    GantGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SizwSl.Value) });
                    Label dateLb = new Label();
                    Grid.SetColumn(dateLb, i);
                    Grid.SetRow(dateLb, GantGrid.RowDefinitions.Count() -1);
                    DateTime thisDate = StartDate.AddDays(i);
                    dateLb.Width = double.NaN;
                    dateLb.Height = double.NaN;
                    dateLb.Content = thisDate.ToString("dd/MM ddd");
                    Border border = new Border();
                    border.BorderBrush = Brushes.Black;
                    border.BorderThickness = new Thickness(2);
                    Grid.SetColumn(border, i);
                    Grid.SetRowSpan(border, GantGrid.RowDefinitions.Count() - 1);

                    if(thisDate.DayOfWeek == DayOfWeek.Saturday || thisDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        border.Background = Brushes.Red;
                    }
                    if(thisDate.Date == DateTime.Now.Date)
                    {
                        border.Background = Brushes.LightBlue;
                    }
                    GantGrid.Children.Add(dateLb);
                    GantGrid.Children.Add(border);

                }

                
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void SizwSl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            loadGant();
            if (isFirst)
            {
                
                isFirst = false;
            }
            else
            {
                File.WriteAllText("Scale.txt", SizwSl.Value.ToString());
            }
           
        }

        private void IntervalCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                int index = IntervalCbx.SelectedIndex;
                if (index == 0 || index == 1)
                {
                    int weekDay = (int)EndDate.DayOfWeek;
                    if (weekDay != 0)
                    {
                        EndDate = EndDate.AddDays(7 - weekDay);
                    }
                    if (index == 0)
                    {
                        StartDate = EndDate.AddDays(-6);
                    }
                    if (index == 1)
                    {
                        StartDate = EndDate.AddDays(-13);
                    }
                }
                if (index == 2)
                {
                    StartDate = new DateTime(StartDate.Year, StartDate.Month, 1);
                    EndDate = StartDate.AddMonths(1).AddDays(-1);
                }
                if(index == 3)
                {
                    StartDate = new DateTime(StartDate.Year, 1, 1);
                    EndDate = StartDate.AddYears(1).AddDays(-1);
                }

                StartDateLb.Content = StartDate.ToString("dd MMMM yyyy");
                FinishDateLb.Content = EndDate.ToString("dd MMMM yyyy");
                loadGant();
                File.WriteAllText("Interval.txt", index.ToString());
                File.WriteAllText("Dates.txt", StartDate.ToString() + ";" + EndDate.ToString());
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
            
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int index = IntervalCbx.SelectedIndex;
                if(index == 0)
                {
                    StartDate = StartDate.AddDays(-7);
                    EndDate = EndDate.AddDays(-7);
                }
                if(index == 1)
                {
                    StartDate = StartDate.AddDays(-14);
                    EndDate = EndDate.AddDays(-14);
                }
                if(index == 2)
                {
                    StartDate = StartDate.AddMonths(-1);
                    EndDate = EndDate.AddMonths(-1);
                }
                if(index == 3)
                {
                    StartDate = StartDate.AddYears(-1);
                    EndDate = EndDate.AddYears(-1);
                }


                StartDateLb.Content = StartDate.ToString("dd MMMM yyyy");
                FinishDateLb.Content = EndDate.ToString("dd MMMM yyyy");
                loadGant();
                File.WriteAllText("Interval.txt", index.ToString());
                File.WriteAllText("Dates.txt", StartDate.ToString() + ";" + EndDate.ToString());
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = IntervalCbx.SelectedIndex;
                if (index == 0)
                {
                    StartDate = StartDate.AddDays(7);
                    EndDate = EndDate.AddDays(7);
                }
                if (index == 1)
                {
                    StartDate = StartDate.AddDays(14);
                    EndDate = EndDate.AddDays(14);
                }
                if (index == 2)
                {
                    StartDate = StartDate.AddMonths(1);
                    EndDate = EndDate.AddMonths(1);
                }
                if (index == 3)
                {
                    StartDate = StartDate.AddYears(1);
                    EndDate = EndDate.AddYears(1);
                }


                StartDateLb.Content = StartDate.ToString("dd MMMM yyyy");
                FinishDateLb.Content = EndDate.ToString("dd MMMM yyyy");
                loadGant();
                File.WriteAllText("Interval.txt", index.ToString());
                File.WriteAllText("Dates.txt", StartDate.ToString() + ";" + EndDate.ToString());
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("Dates.txt"))
            {
                StartDate = (DateTime)new DateTimeConverter().ConvertFromString(File.ReadAllText("Dates.txt").Split(';')[0]);
                EndDate = (DateTime)new DateTimeConverter().ConvertFromString(File.ReadAllText("Dates.txt").Split(';')[1]);
            }

            if (File.Exists("Interval.txt"))
            {
                IntervalCbx.SelectedIndex = Convert.ToInt32(File.ReadAllText("Interval.txt"));
            }
            else
            {
                IntervalCbx.SelectedIndex = 0;
            }
            if (File.Exists("Scale.txt"))
            {
                SizwSl.Value = Convert.ToDouble(File.ReadAllText("Scale.txt"));
            }
            
        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "CSV|*.csv";
            if(openFile.ShowDialog() == true)
            {

            }
        }
    }
}
