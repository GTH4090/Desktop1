using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace WSRSim2.Models
{
    public partial class Task
    {
        public int SortNum { get
            {
                if(StatusId == 2 && Deadline < DateTime.Now)
                {
                    return 1;
                }
                if (StatusId == 1 && Deadline < DateTime.Now)
                {
                    return 2;
                }
                if (StatusId == 2 && Deadline >= DateTime.Now)
                {
                    return 3;
                }
                if (StatusId == 1 && Deadline >= DateTime.Now)
                {
                    return 4;
                }
                else
                {
                    return 5;
                }

            } }

        public string SpecialTime { get
            {
                if(StatusId == 3)
                {
                    DateTime start = new DateTime();
                    DateTime end = new DateTime();
                    
                    if (this.StartActualTime == null)
                    {
                        start = (DateTime)this.CreatedTime;
                    }
                    else
                    {
                        start = (DateTime)this.StartActualTime;
                    }
                    if (this.FinishActualTime == null)
                    {
                        end = (DateTime)this.Deadline;
                    }
                    else
                    {
                        end = (DateTime)this.FinishActualTime;
                    }
                    return "потраченное время: " + (end - start).ToString();
                }
                if(StatusId == 2)
                {
                    return "время до дедлайна: " + (Deadline - DateTime.Now).ToString();
                }
                if (StatusId == 1)
                {
                    DateTime start = new DateTime();
                    DateTime end = new DateTime();

                    if (this.StartActualTime == null)
                    {
                        start = (DateTime)this.CreatedTime;
                    }
                    else
                    {
                        start = (DateTime)this.StartActualTime;
                    }
                   
                        end = (DateTime)this.Deadline;
                    
                    return "Планируемое время на выполнение: " + (end - start).ToString();
                }
                else
                {
                    return ";o";
                }
            } }
    }
}
