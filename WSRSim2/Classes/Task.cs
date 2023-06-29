using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
