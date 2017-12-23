using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using iSched3.Services;
using Microsoft.Practices.Unity.ObjectBuilder;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace iSched3.ViewModels
{
    public class LoginPageViewModel : BindableBase
    {
        private readonly IApiService _apiService;
        private readonly IPageDialogService _pageDialogService;
        private readonly INavigationService _navigationService;

        private string _username;

        public string UserName
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public LoginPageViewModel(IApiService apiService, IPageDialogService pageDialogService, INavigationService navigationService)
        {
            _apiService = apiService;
            _pageDialogService = pageDialogService;
            _navigationService = navigationService;
        }

        public ICommand LoginCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    IsLoading = true;

                    var isSuccess = await _apiService.LoginAsync(UserName, Password);

                    if (isSuccess)
                    {
                        IsLoading = false;
                        await _navigationService.NavigateAsync("DashboardPage");
                    }
                    else
                    {
                        IsLoading = false;
                        await _pageDialogService.DisplayAlertAsync("Error", "Invalid credentials or no internet connectivity", "Ok");
                    }
                });
            }
        }

        public ICommand RegisterCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await _navigationService.NavigateAsync("RegisterPage");
                });
            }
        }

    }
}
