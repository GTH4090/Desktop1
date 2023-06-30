using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace WSRSim2.Models
{
    public partial class Employee
    {
        public int ClosedPerMonth { get
            {
                DateTime StartDate = DateTime.Now.Date;
                DateTime EndDate = DateTime.Now.Date;
                StartDate = new DateTime(StartDate.Year, StartDate.Month, 1);
                EndDate = StartDate.AddMonths(1).AddDays(-1);
                DateTime start = new DateTime();
                DateTime end = new DateTime();
                int res = 0;
                foreach(var  item in this.Task)
                {
                    if (item.StartActualTime == null)
                    {
                        start = (DateTime)item.CreatedTime;
                    }
                    else
                    {
                        start = (DateTime)item.StartActualTime;
                    }
                    if (item.FinishActualTime == null)
                    {
                        end = (DateTime)item.Deadline;
                    }
                    else
                    {
                        end = (DateTime)item.FinishActualTime;
                    }
                    if (((start.Date < StartDate && end.Date >= StartDate.Date) || (start.Date >= StartDate)) && item.StatusId == 3)
                    {
                        res++;
                    }
                }
                return res;
                
            } }
        public int DeadlinedPerMonth
        {
            get
            {
                DateTime StartDate = DateTime.Now.Date;
                DateTime EndDate = DateTime.Now.Date;
                StartDate = new DateTime(StartDate.Year, StartDate.Month, 1);
                EndDate = StartDate.AddMonths(1).AddDays(-1);
                DateTime start = new DateTime();
                DateTime end = new DateTime();
                int res = 0;
                foreach (var item in this.Task)
                {
                    if (item.StartActualTime == null)
                    {
                        start = (DateTime)item.CreatedTime;
                    }
                    else
                    {
                        start = (DateTime)item.StartActualTime;
                    }
                    if (item.FinishActualTime == null)
                    {
                        end = (DateTime)item.Deadline;
                    }
                    else
                    {
                        end = (DateTime)item.FinishActualTime;
                    }
                    if (((start.Date < StartDate && end.Date >= StartDate.Date) || (start.Date >= StartDate)) && item.Deadline < DateTime.Now)
                    {
                        res++;
                    }
                }
                return res;

            }
        }
    }
}
