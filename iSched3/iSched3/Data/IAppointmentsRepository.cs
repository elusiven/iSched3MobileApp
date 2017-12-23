using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSched3.Models;
using Syncfusion.SfSchedule.XForms;

namespace iSched3.Data
{
    public interface IAppointmentsRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<bool> AddAsync(string name, string comments, DateTime startTime, DateTime endTime, bool isAllDay, bool isRecurrence);
        Task<bool> EditAsync(int id, string name, string comments, DateTime startTime, DateTime endTime, bool isAllDay, bool isRecurrence);
        Task<bool> DeleteAsync(int id);
    }
}
