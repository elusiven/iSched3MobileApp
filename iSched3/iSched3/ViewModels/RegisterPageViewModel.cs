using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using iSched3.Services;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;

namespace iSched3.ViewModels
{
    public class RegisterPageViewModel : BindableBase
    {
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;

        #region Properties

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _confirmEmail;

        public string ConfirmEmail
        {
            get { return _confirmEmail; }
            set { SetProperty(ref _confirmEmail, value); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _confirmPassword;

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetProperty(ref _confirmPassword, value); }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        #endregion

        #region ValidationProperties

        private bool _isFirstNameValid;

        public bool IsFirstNameValid
        {
            get { return _isFirstNameValid; }
            set { SetProperty(ref _isFirstNameValid, value); }
        }

        private bool _isLastNameValid;

        public bool IsLastNameValid
        {
            get { return _isLastNameValid; }
            set { SetProperty(ref _isLastNameValid, value); }
        }

        private bool _isUserNameValid;

        public bool IsUserNameValid
        {
            get { return _isUserNameValid; }
            set { SetProperty(ref _isUserNameValid, value); }
        }

        private bool _isEmailValid;

        public bool IsEmailValid
        {
            get { return _isEmailValid; }
            set { SetProperty(ref _isEmailValid, value); }
        }

        private bool _isPasswordValid;

        public bool IsPasswordValid
        {
            get { return IsPasswordValid; }
            set { SetProperty(ref _isPasswordValid, value); }
        }

        // This has to return true, server side will handle errors until i fix behaviors 
        public bool IsFormValid { get; set; }

        #endregion

        public RegisterPageViewModel(IApiService apiService, INavigationService navigationService, IPageDialogService pageDialogService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
        }

        public ICommand RegisterCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {

                    if (Email == ConfirmEmail && Password == ConfirmPassword)
                    {
                        IsFormValid = true;
                        await ExecuteRegisterUser();
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Error",
                            "Confirmed email or password does not match.", "Ok");
                    }

                    
                    
                });
            }
        }

        private async Task ExecuteRegisterUser()
        {
            if (IsFormValid)
            {
                IsLoading = true;

                var isSucceded =
                    await _apiService.RegisterAsync(FirstName, LastName, UserName, Email, Password);
                if (isSucceded)
                {
                    IsLoading = false;
                    await _navigationService.NavigateAsync("LoginPage");
                }
                else
                {
                    IsLoading = false;
                    await _pageDialogService.DisplayAlertAsync("Error",
                        "Invalid input or no internet connectivity. Password needs at least 8 characters, including lowercase and special character", "Ok");
                }
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync("Invalid Input", "Correct all input marked red, Password needs at least 8 characters, including lowercase and special character", "Ok");
            }
        }

    }
}
