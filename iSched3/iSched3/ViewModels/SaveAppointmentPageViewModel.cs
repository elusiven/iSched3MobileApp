using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using iSched3.Data;
using iSched3.Models;
using Prism.Navigation;
using Prism.Services;

namespace iSched3.ViewModels
{
    public class SaveAppointmentPageViewModel : BindableBase, INavigationAware
    {
        private readonly IAppointmentsRepository _appointmentsRepo;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        private bool isEditable;

        #region BindableProperties

        public int Id { get; private set; }

        private string _title = "Add New Appointment";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _comments;

        public string Comments
        {
            get { return _comments; }
            set { SetProperty(ref _comments, value); }
        }

        private DateTime _startTime = DateTime.UtcNow;

        public DateTime StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }

        private TimeSpan _startTime2;

        public TimeSpan StartTime2
        {
            get { return _startTime2; }
            set { SetProperty(ref _startTime2, value); }
        }

        private DateTime _endTime = DateTime.UtcNow;

        public DateTime EndTime
        {
            get { return _endTime; }
            set { SetProperty(ref _endTime, value); }
        }

        private TimeSpan _endTime2;

        public TimeSpan EndTime2
        {
            get { return _endTime2; }
            set { SetProperty(ref _endTime2, value); }
        }

        private bool _isAllDay;

        public bool IsAllDay
        {
            get { return _isAllDay; }
            set { SetProperty(ref _isAllDay, value); }
        }

        private bool _isRecurrence;

        public bool IsRecurrence
        {
            get { return _isRecurrence; }
            set { SetProperty(ref _isRecurrence, value); }
        }

        private string _recurrenceRule;

        public string RecurrenceRule
        {
            get { return _recurrenceRule; }
            set { SetProperty(ref _recurrenceRule, value); }
        }

        private string _commandText = "Create";

        public string CommandText
        {
            get { return _commandText; }
            set { SetProperty(ref _commandText, value); }
        }

        public DateTime CurrentDate => DateTime.UtcNow.AddDays(1);

        #endregion

        public SaveAppointmentPageViewModel(
            IAppointmentsRepository appointmentsRepo, 
            INavigationService navigationService, 
            IPageDialogService dialogService)
        {
            _appointmentsRepo = appointmentsRepo;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        public ICommand SaveAppointmentCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {

                    // Add time to dates
                    StartTime = StartTime.Add(StartTime2);
                    EndTime = EndTime.Add(EndTime2);

                    if (ValidateForm())
                    {
                        if (isEditable)
                        {
                            var result = await _appointmentsRepo.EditAsync(Id, Name, Comments, StartTime, EndTime, IsAllDay,
                                IsRecurrence);
                            if (result)
                            {
                                await _navigationService.NavigateAsync("DashboardPage");
                            }
                            else
                            {
                                await _dialogService.DisplayAlertAsync("Failed to edit",
                                    "Information is wrong or not internet connectivity", "Ok");
                            }
                        }
                        else
                        {
                            var result = await _appointmentsRepo.AddAsync(Name, Comments, StartTime, EndTime, IsAllDay,
                                IsRecurrence);
                            if (result)
                            {
                                await _navigationService.GoBackAsync();
                            }
                            else
                            {
                                await _dialogService.DisplayAlertAsync("Failed to create",
                                    "Information is wrong or no internet connectivity", "Ok");
                            }
                        }
                        
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync("Incorrect Input", "Title and Comments must be at least 5 character and end Date cannot be earlier than start date", "Ok");
                    }
                    
                });
            }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        // Fired when navigated to this view model
        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("appointment"))
            {
                var appointment = (Appointment) parameters["appointment"];
                isEditable = true;
                CommandText = "Edit";

                Id = appointment.Id;
                Title = appointment.Name;
                Name = appointment.Name;
                Comments = appointment.Comments;
                StartTime = appointment.StartTime.Add(_startTime2);
                EndTime = appointment.EndTime.Add(_endTime2);
                IsAllDay = appointment.IsAllDay;
                IsRecurrence = appointment.IsRecurrence;
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            
        }

        // This needs to be used until I fix behaviors
        private bool ValidateForm()
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Comments))
            {
                var isFormValid = Name.Length >= 5 && Comments.Length >= 5 && EndTime > StartTime && !DateTime.Equals(EndTime, StartTime);
                return isFormValid;
            }

            return false;
        }
    }
}
