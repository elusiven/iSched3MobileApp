using System.Diagnostics;
using iSched3.Models;
using Prism.Navigation;
using Syncfusion.SfSchedule.XForms;
using Xamarin.Forms;

namespace iSched3.Views
{
    public partial class DashboardPage : ContentPage
    {

        public DashboardPage()
        {
            InitializeComponent();
            ScheduleAppointmentMapping dataMapping = new ScheduleAppointmentMapping();
            dataMapping.SubjectMapping = "Name";
            dataMapping.NotesMapping = "Comments";
            dataMapping.StartTimeMapping = "StartTime";
            dataMapping.EndTimeMapping = "EndTime";
            dataMapping.LocationMapping = "UserName";
            dataMapping.IsAllDayMapping = "IsAllDay";
            Sfschedule.AppointmentMapping = dataMapping;

            AppointmentStyle appointmentStyle = new AppointmentStyle();
            appointmentStyle.TextColor = Color.Red;
            appointmentStyle.TextStyle = Font.SystemFontOfSize(15, FontAttributes.Bold);
            appointmentStyle.BorderColor = Color.Blue;
            appointmentStyle.BorderWidth = 5;
            appointmentStyle.SelectionBorderColor = Color.Yellow;
            appointmentStyle.SelectionTextColor = Color.Yellow;
            Sfschedule.AppointmentStyle = appointmentStyle;
        }
    }
}
