using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using iSched3.Data;
using iSched3.Models;
using iSched3.Views;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace iSched3.ViewModels
{
    public class AppointmentDetailPageViewModel : BindableBase, INavigatedAware
    {
        private readonly IAppointmentsRepository _appointmentsRepo;
        private readonly IPageDialogService _dialogService;
        private readonly INavigationService _navigationService;

        private Appointment appointment;

        #region BindableProperties

        private int _id;

        public int Id { get; set; }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _comments;

        public string Comments
        {
            get { return _comments; }
            set { SetProperty(ref _comments, value); }
        }

        private DateTime _startTime;

        public DateTime StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }

        private DateTime _endTime;

        public DateTime EndTime
        {
            get { return _endTime; }
            set { SetProperty(ref _endTime, value); }
        }

        #endregion

        public AppointmentDetailPageViewModel(
            IAppointmentsRepository appointmentsRepo, 
            IPageDialogService dialogService, 
            INavigationService navigationService)
        {
            _appointmentsRepo = appointmentsRepo;
            _dialogService = dialogService;
            _navigationService = navigationService;
        }

        public ICommand RemoveAppointmentCommand { get { return new DelegateCommand(async () =>
        {
            var result = await _appointmentsRepo.DeleteAsync(Id);
            if (result)
            {
                await _navigationService.GoBackAsync();
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Could not delete appointment", "Try Again later");
            }
        });} }

        public ICommand EditAppointmentCommand { get { return new DelegateCommand(async () =>
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("appointment", appointment);
            await _navigationService.NavigateAsync("SaveAppointmentPage", navigationParams);
        });} }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("appointment"))
            {
                appointment = (Appointment)parameters["appointment"];
                Id = appointment.Id;
                Title = appointment.Name;
                Comments = appointment.Comments;
                StartTime = appointment.StartTime;
                EndTime = appointment.EndTime;
            }
        }
    }
}
