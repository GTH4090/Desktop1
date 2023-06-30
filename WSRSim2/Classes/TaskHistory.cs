using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WSRSim2.Classes.Helper;

namespace WSRSim2.Models
{
    public partial class TaskHistory
    {
        public string PreviousStatus { get
            {

                try
                {

                    return Db.TaskHistory.ToList().OrderBy(e => e.Id).Where(e => e.Id < this.Id).LastOrDefault().StatusId.ToString();
                }
                catch (Exception ex)
                {
                    return "создан";
                }
                
            } }
    }
}
