using Microsoft.Win32;
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
using WSRSim2.Models;
using static WSRSim2.Classes.Helper;

namespace WSRSim2.Pages
{
    /// <summary>
    /// Логика взаимодействия для TaskList.xaml
    /// </summary>
    public partial class TaskList : Page
    {
        bool IsAdd = false;
        public TaskList()
        {
            InitializeComponent();
            File.WriteAllText("Memory.txt", "2");
        }

        private void LoadData()
        {

            try
            {
                List<Models.Task> tasks = Db.Task.Where(el => el.ProjectId == SelectedProject.Id && el.StatusId != 3 && el.StatusId != 4).ToList().OrderBy(el => el.SortNum).ToList();
                if (SearchTbx.Text != "")
                {
                    tasks = tasks.Where(el => el.FullTitle.Contains(SearchTbx.Text) || el.Description.Contains(SearchTbx.Text)).ToList();
                }
                taskDataGrid.ItemsSource = tasks;

            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            executiveEmployeeIdComboBox.ItemsSource = Db.Employee.ToList();
            statusIdComboBox.ItemsSource = Db.TaskStatus.ToList();
        }

        private void SearchTbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (IsAdd)
                {
                    Db.Task.Add(grid1.DataContext as Models.Task);
                }
                Db.SaveChanges();
                LoadData();
                SpectatorSp.Visibility = Visibility.Visible;
                AddAttachment.Visibility = Visibility.Visible;
                taskAttachmentDataGrid.Visibility = Visibility.Visible;
                taskSpectatorDataGrid.Visibility = Visibility.Visible;
                IsAdd = false;
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            TaskSp.Visibility = Visibility.Collapsed;
            Grid.SetColumnSpan(taskDataGrid, 2);
            grid1.DataContext = null;
            SpectatorCbx.ItemsSource = null;
            taskDataGrid.SelectedItem = null;
            IsAdd = false;
        }

        private void DownLoadBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            if (saveFile.ShowDialog() == true)
            {
                var item = (sender as Button).DataContext as TaskAttachment;
                File.WriteAllBytes(saveFile.FileName, item.Attachment);
            }
        }

        private void AddSpectatorBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (SpectatorCbx.SelectedItem != null)
                {
                    var item = SpectatorCbx.SelectedItem as Employee;
                    TaskSpectator taskSpectator = new TaskSpectator();
                    taskSpectator.Task = (grid1.DataContext as Models.Task);
                    taskSpectator.EmployeeId = item.Id;
                    Db.TaskSpectator.Add(taskSpectator);
                    Db.SaveChanges();

                    var task = (grid1.DataContext as Models.Task);

                    SpectatorCbx.ItemsSource = Db.Employee.Where(el => el.TaskSpectator.FirstOrDefault(j => j.Task.Id == task.Id) == null).ToList();
                    SpectatorCbx.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }

        }

        private void AddAttachment_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                if (openFile.ShowDialog() == true)
                {
                    TaskAttachment attachment = new TaskAttachment();
                    attachment.Attachment = File.ReadAllBytes(openFile.FileName);
                    attachment.TaskId = (grid1.DataContext as Models.Task).Id;
                    Db.TaskAttachment.Add(attachment);

                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void taskDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                if (taskDataGrid.SelectedItem != null)
                {
                    var item = taskDataGrid.SelectedItem as Models.Task;
                    TaskSp.Visibility = Visibility.Visible;
                    Grid.SetColumnSpan(taskDataGrid, 1);
                    grid1.DataContext = item;
                    
                    SpectatorCbx.ItemsSource = Db.Employee.Where(el => el.TaskSpectator.FirstOrDefault(j => j.TaskId == item.Id) == null).ToList();
                    SpectatorCbx.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {


                TaskSp.Visibility = Visibility.Visible;
                Grid.SetColumnSpan(taskDataGrid, 1);
                grid1.DataContext = new Models.Task();
                SpectatorCbx.ItemsSource = Db.Employee.ToList();
                SpectatorCbx.SelectedIndex = 0;
                IsAdd = true;
                executiveEmployeeIdComboBox.SelectedIndex = 0;
                statusIdComboBox.SelectedIndex = 0;
                deadlineDatePicker.SelectedDate = DateTime.Now;
                (grid1.DataContext as Models.Task).ShortTitle = SelectedProject.ShortTitle + (SelectedProject.Task.Count() + 1).ToString();
                (grid1.DataContext as Models.Task).ProjectId = SelectedProject.Id;
                SpectatorSp.Visibility = Visibility.Collapsed;
                AddAttachment.Visibility = Visibility.Collapsed;
                taskAttachmentDataGrid.Visibility = Visibility.Collapsed;
                taskSpectatorDataGrid.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var item = (sender as Button).DataContext as Models.Task;
                if(MessageBox.Show("Точно?", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if(MessageBox.Show("Будут удалены все ссылки на эту задачу", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        item.StatusId = 4;
                        foreach(var i in item.Task1)
                        {
                            i.Task2 = null;
                        }
                        Db.SaveChanges();
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }
    }
}
