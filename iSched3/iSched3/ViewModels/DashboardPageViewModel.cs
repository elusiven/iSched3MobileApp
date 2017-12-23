using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using iSched3.Data;
using iSched3.Helpers;
using iSched3.Models;
using iSched3.Services;
using Prism.Navigation;
using Prism.Services;
using Syncfusion.SfSchedule.XForms;

namespace iSched3.ViewModels
{
    public class DashboardPageViewModel : BindableBase, INavigatedAware
    {
        private readonly IAppointmentsRepository _appointmentsRepo;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;

        private ScheduleView _calendarView = ScheduleView.MonthView;
        public ScheduleView CalendarView
        {
            get { return _calendarView; }
            set { SetProperty(ref _calendarView, value); }
        }

        public ObservableCollection<Appointment> Appointments { get; set; } = new ObservableCollection<Appointment>();

        public DashboardPageViewModel(IAppointmentsRepository appointmentsRepo, 
            INavigationService navigationService, 
            IPageDialogService dialogService,
            IApiService apiService)
        {
            _appointmentsRepo = appointmentsRepo;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _apiService = apiService;
        }

        private DelegateCommand<Appointment> _appointmentCommand;

        public DelegateCommand<Appointment> ShowAppointmentCommand => _appointmentCommand ?? (_appointmentCommand = new DelegateCommand<Appointment>((p) => ShowAppointmentDetail(p)));

        private async void ShowAppointmentDetail(Appointment appointment)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("appointment", appointment);
            if (appointment != null)
            {

                await _navigationService.NavigateAsync("AppointmentDetailPage", navigationParams);
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "This appointment is invalid", "Ok");
            }
        }


        // This will only work on Week view because cell tap event only goes into one event
        private DelegateCommand<Appointment> _appointmentCellCommand;

        public DelegateCommand<Appointment> ShowAppointmentCellCommand => _appointmentCellCommand ?? (_appointmentCellCommand = new DelegateCommand<Appointment>((p) => ShowAppointmentCellDetail(p)));

        private async void ShowAppointmentCellDetail(Appointment appointment)
        {
            if (CalendarView == ScheduleView.WeekView)
            {
                var navigationParams = new NavigationParameters();
                navigationParams.Add("appointment", appointment);
                if (appointment != null)
                {

                    await _navigationService.NavigateAsync("AppointmentDetailPage", navigationParams);
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("Error", "This appointment is invalid", "Ok");
                }
            }
        }

        public ICommand AddNewAppointmentCommand { get { return new DelegateCommand(async () =>
        {
            await _navigationService.NavigateAsync("SaveAppointmentPage");
        });} }

        public ICommand RefreshAppointmentsCommand { get { return new DelegateCommand(async () =>
        {
            await InitAppointments();
        });} }

        public ICommand ChangeViewsCommand { get { return new DelegateCommand(() =>
        {
            CalendarView = CalendarView == ScheduleView.MonthView ? ScheduleView.WeekView : ScheduleView.MonthView;
        });} }

        private async Task InitAppointments()
        {
            await RewnewAccessToken();

            Appointments.Clear();

            var result = await _appointmentsRepo.GetAllAsync();

            foreach (var e in result)
            {
                if (e != null)
                {
                    Appointments.Add(e);
                }
            }

            Debug.WriteLine("Appointment Count================== " + Appointments.Count);
        }

        private async Task RewnewAccessToken()
        {
            var timeLeft = (Settings.TokenExpiration - DateTime.UtcNow).TotalMinutes;
            Debug.WriteLine($"Token has {timeLeft} minutes left.");

            if (timeLeft < 0.5 || double.IsNaN(timeLeft))
            {
                var isSuccess = await _apiService.RenewAccessToken();
                if (!isSuccess)
                {
                    await _dialogService.DisplayAlertAsync("Error", "Failed to renew access, Please log in again",
                        "Ok");
                }
            }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
           await InitAppointments();
        }
    }
}
