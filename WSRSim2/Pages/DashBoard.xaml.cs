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
using static WSRSim2.Classes.Helper;
using WSRSim2.Models;
using System.Windows.Threading;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Win32;

namespace WSRSim2.Pages
{
    /// <summary>
    /// Логика взаимодействия для DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Page
    {
        DispatcherTimer Timer = new DispatcherTimer();
        public DashBoard()
        {
            InitializeComponent();
            File.WriteAllText("Memory.txt", "1");
            Timer.Interval = new TimeSpan(0, 0, 30);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            loadData();
        }
        private void CheckSize()
        {
            if(MainGrid.ActualWidth < 1440)
            {
                Grid.SetRow(OpenPerWeedSp, 1);
                Grid.SetColumn(OpenPerWeedSp, 0);
                Grid.SetColumn(Top5BestSp, 1);
                Grid.SetColumn(Top5WorstSp, 2);
            }
            else
            {
                Grid.SetRow(OpenPerWeedSp, 0);
                Grid.SetColumn(OpenPerWeedSp, 3);
                Grid.SetColumn(Top5BestSp, 0);
                Grid.SetColumn(Top5WorstSp, 1);
            }
        }

        private void loadData()
        {

            try
            {
                List<Models.Task> openedTasks = Db.Task.Where(el => el.ProjectId == SelectedProject.Id && el.StatusId == 1).ToList();
                openedtaskDataGrid.ItemsSource = openedTasks;
                OpenedCountLbl.Content = openedTasks.Count();

                List<Models.Task> deadlinedTasks = Db.Task.Where(el => el.ProjectId == SelectedProject.Id && el.Deadline < DateTime.Now).ToList();
                if(deadlinedTasks.Count() > 2)
                {
                    DeadlinedSp.Background = Brushes.Red;
                }
                deadlinedtaskDataGrid.ItemsSource = deadlinedTasks.ToList();
                DeadlinedCountLbl.Content = deadlinedTasks.Count();

                List<Models.Task> activeTasks = Db.Task.Where(el => el.ProjectId == SelectedProject.Id && el.StatusId == 2 && (el.StartActualTime <= DateTime.Now || el.CreatedTime <= DateTime.Now) &&
                (el.FinishActualTime >= DateTime.Now || el.Deadline >= DateTime.Now)).ToList();
                ActiveCountLbl.Content = activeTasks.Count();
                activetaskDataGrid.ItemsSource = activeTasks;
                if(activeTasks.Count() == 0 && DateTime.Now.Hour >= 9 && DateTime.Now.Hour <= 18)
                {
                    ActiveSp.Background = Brushes.Red;
                }

                DateTime weekstart = DateTime.Now.Date;
                DateTime weekend = DateTime.Now.Date;
                int weekDay = (int)weekend.DayOfWeek;
                if (weekDay != 0)
                {
                    weekend = weekend.AddDays(7 - weekDay);
                }
                weekstart = weekend.AddDays(-6);
                List<Models.Task> openedperWeekTasks = Db.Task.Where(el => el.ProjectId == SelectedProject.Id && el.StatusId == 1 && ((el.StartActualTime <= weekstart || el.CreatedTime <= weekstart)
                && (el.FinishActualTime >= weekstart || el.Deadline >= weekstart) || (el.StartActualTime >= weekstart || el.CreatedTime >= weekstart)
                && (el.StartActualTime <= weekend || el.CreatedTime <= weekend))).ToList();
                OpenPerWeekCountLbl.Content = openedperWeekTasks.Count();
                OpenPerWeekDataGrid.ItemsSource = openedperWeekTasks;

                List<Employee> bestEmployee = Db.Employee.Where(el => el.Task.FirstOrDefault(e => e.ProjectId == SelectedProject.Id) != null
                ).ToList().OrderByDescending(el => el.ClosedPerMonth).Take(5).ToList();
                BestemployeeDataGrid.ItemsSource = bestEmployee;

                List<Employee> worstEmployee = Db.Employee.Where(el => el.Task.FirstOrDefault(e => e.ProjectId == SelectedProject.Id) != null
                ).ToList().OrderByDescending(el => el.DeadlinedPerMonth).Take(5).ToList();
                WorstemployeeDataGrid.ItemsSource = worstEmployee;

            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
            
        }

        private void EmployeeExportBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string csvList = "Id;Name;\n";
                SaveFileDialog saveFile = new SaveFileDialog();
                if(saveFile.ShowDialog() == true)
                {
                    foreach (var item in Db.Employee.Where(el => el.Task.FirstOrDefault(l => l.ProjectId == SelectedProject.Id) != null))
                    {
                        csvList += $"{item.Id};{item.Name};\n";
                    }
                    if (!(saveFile.FileName + " ").Contains(".csv "))
                    {
                        saveFile.FileName = saveFile.FileName + ".csv";
                    }
                    File.WriteAllText(saveFile.FileName, csvList, UTF8Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void TasksExportBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string csvList = "Id;Project;FullTitle;ShortTitle;Deadline;Status;Description;ExecutiveEmployee;CreatedTime;UpdatedTime;DeletedTime;StartActualTime;FinishActualTime;PreviousTask;\n";
                SaveFileDialog saveFile = new SaveFileDialog();
                if (saveFile.ShowDialog() == true)
                {
                    foreach (var item in Db.Task.Where(el => el.ProjectId == SelectedProject.Id))
                    {
                        csvList += $"{item.Id};{item.Project.FullTitle};{item.FullTitle};{item.ShortTitle};" +
                            $"{item.Deadline};{item.TaskStatus.Name};{(item.Description != null ? item.Description : "")};{item.Employee.Name};" +
                            $"{(item.CreatedTime != null ? item.CreatedTime.ToString() : "")};{(item.UpdatedTime != null ? item.UpdatedTime.ToString() : "")};" +
                            $"{(item.DeletedTime != null ? item.DeletedTime.ToString() : "")};{(item.StartActualTime != null ? item.StartActualTime.ToString() : "")};" +
                            $"{(item.FinishActualTime != null ? item.FinishActualTime.ToString() : "")};{(item.PreviousTaskId != null ? item.Task2.FullTitle: "")};\n";
                    }
                    if (!(saveFile.FileName + " ").Contains(".csv "))
                    {
                        saveFile.FileName = saveFile.FileName + ".csv";
                    }
                    File.WriteAllText(saveFile.FileName, csvList, UTF8Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void OpenedNextMonthBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                DateTime StartDate = DateTime.Now.Date;
                DateTime EndDate = DateTime.Now.Date;
                StartDate = new DateTime(StartDate.Year, StartDate.Month +1, 1);
                EndDate = StartDate.AddMonths(1).AddDays(-1);
                DateTime start = new DateTime();
                DateTime end = new DateTime();
                string csvList = "Id;Project;FullTitle;ShortTitle;Deadline;Status;Description;ExecutiveEmployee;CreatedTime;UpdatedTime;DeletedTime;StartActualTime;FinishActualTime;PreviousTask;\n";
                SaveFileDialog saveFile = new SaveFileDialog();
                if (saveFile.ShowDialog() == true)
                {
                    foreach (var item in Db.Task.Where(el => el.ProjectId == SelectedProject.Id && el.StatusId == 1 && ((el.StartActualTime <= StartDate || el.CreatedTime <= StartDate)
                && (el.FinishActualTime >= StartDate || el.Deadline >= StartDate) || (el.StartActualTime >= StartDate || el.CreatedTime >= StartDate)
                && (el.StartActualTime <= EndDate || el.CreatedTime <= EndDate))))
                    {
                        csvList += $"{item.Id};{item.Project.FullTitle};{item.FullTitle};{item.ShortTitle};" +
                            $"{item.Deadline};{item.TaskStatus.Name};{(item.Description != null ? item.Description : "")};{item.Employee.Name};" +
                            $"{(item.CreatedTime != null ? item.CreatedTime.ToString() : "")};{(item.UpdatedTime != null ? item.UpdatedTime.ToString() : "")};" +
                            $"{(item.DeletedTime != null ? item.DeletedTime.ToString() : "")};{(item.StartActualTime != null ? item.StartActualTime.ToString() : "")};" +
                            $"{(item.FinishActualTime != null ? item.FinishActualTime.ToString() : "")};{(item.PreviousTaskId != null ? item.Task2.FullTitle : "")};\n";
                    }
                    if (!(saveFile.FileName + " ").Contains(".csv "))
                    {
                        saveFile.FileName = saveFile.FileName + ".csv";
                    }
                    File.WriteAllText(saveFile.FileName, csvList, UTF8Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void ClosedOnMonthBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                DateTime StartDate = DateTime.Now.Date;
                DateTime EndDate = DateTime.Now.Date;
                StartDate = new DateTime(StartDate.Year, StartDate.Month, 1);
                EndDate = StartDate.AddMonths(1).AddDays(-1);
                DateTime start = new DateTime();
                DateTime end = new DateTime();
                string csvList = "Id;Project;FullTitle;ShortTitle;Deadline;Status;Description;ExecutiveEmployee;CreatedTime;UpdatedTime;DeletedTime;StartActualTime;FinishActualTime;PreviousTask;\n";
                SaveFileDialog saveFile = new SaveFileDialog();
                if (saveFile.ShowDialog() == true)
                {
                    foreach (var item in Db.Task.Where(el => el.ProjectId == SelectedProject.Id && el.StatusId == 3 && ((el.StartActualTime <= StartDate || el.CreatedTime <= StartDate)
                && (el.FinishActualTime >= StartDate || el.Deadline >= StartDate) || (el.StartActualTime >= StartDate || el.CreatedTime >= StartDate)
                && (el.StartActualTime <= EndDate || el.CreatedTime <= EndDate))))
                    {
                        csvList += $"{item.Id};{item.Project.FullTitle};{item.FullTitle};{item.ShortTitle};" +
                            $"{item.Deadline};{item.TaskStatus.Name};{(item.Description != null ? item.Description : "")};{item.Employee.Name};" +
                            $"{(item.CreatedTime != null ? item.CreatedTime.ToString() : "")};{(item.UpdatedTime != null ? item.UpdatedTime.ToString() : "")};" +
                            $"{(item.DeletedTime != null ? item.DeletedTime.ToString() : "")};{(item.StartActualTime != null ? item.StartActualTime.ToString() : "")};" +
                            $"{(item.FinishActualTime != null ? item.FinishActualTime.ToString() : "")};{(item.PreviousTaskId != null ? item.Task2.FullTitle : "")};\n";
                    }
                    if (!(saveFile.FileName + " ").Contains(".csv "))
                    {
                        saveFile.FileName = saveFile.FileName + ".csv";
                    }
                    File.WriteAllText(saveFile.FileName, csvList, UTF8Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }
    }
}
