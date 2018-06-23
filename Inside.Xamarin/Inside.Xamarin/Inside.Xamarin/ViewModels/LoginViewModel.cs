using GalaSoft.MvvmLight.Command;
using Inside.Xamarin.Helpers;
using Inside.Xamarin.Models;
using Inside.Xamarin.Services;
using System.Windows.Input;
using Inside.Xamarin.Models.DomainModels;
using Xamarin.Forms;

namespace Inside.Xamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private bool _isEnable;
        private bool _isRunning;
        private string _password;
        private string _username;

        private NavigationService navigationService;

        public LoginViewModel()
        {
            IsRemembered = true;
            IsEnable = true;
            navigationService = NavigationService.GetInstance();
        }

        public string UserName
        {
            get => _username;
            set => SetValue(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetValue(ref _password, value);
        }

        public bool IsRemembered { get; set; }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetValue(ref _isRunning, value);
        }

        public bool IsEnable
        {
            get => _isEnable;
            set => SetValue(ref _isEnable, value);
        }

        public ICommand LoginCommand => new RelayCommand(Login);
        public ICommand GoToRegisterPageCommand => new RelayCommand(GoToRegisterPage);


        private async void Login()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.GeneralError,
                    Languages.LoginUserNameAlert,
                    Languages.GeneralAccept);
                return;
            }


            if (string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                   Languages.GeneralError,
                    Languages.LoginPasswordAlert,
                    Languages.GeneralAccept);
                return;
            }


            LoginModel model = new LoginModel
            {
                UserName = this.UserName,
                Password = this.Password
            };

            var tokenResponse = await InsideApi.Post<LoginModel, TokenResponse>(HostSetting.LoginEndpoint, model);
            if (tokenResponse == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.GeneralError,
                    Languages.LoginErrorServerAlert +
                    "\n" +
                    Languages.GeneralCheckInternetConnection +
                    "\n",
                    Languages.GeneralAccept);
            }
            else
            {
                if (tokenResponse.IsSuccess)
                {
                    try
                    {
                        var loggerUserResponse = await this.InsideApi.GetUserByUserName(HostSetting.AuthEndPoint, UserName);
                        MainViewModel.GetInstance().SetUpLoginData(loggerUserResponse.Result as User, (tokenResponse.Result as TokenResponse).AuthToken);
                    }
                    catch { }
                    NavigationService.GetInstance().SetMainPage(Pages.MasterView);
                }
                else
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.GeneralError,
                        Languages.LoginUserPasswordIncorrectAlert,
                        Languages.GeneralAccept);
                this.Refresh();
            }
        }

        private void Refresh()
        {
            this.UserName = string.Empty;
            this.Password = string.Empty;
        }

        private async void GoToRegisterPage()
        {
            await navigationService.NavigateOnLogin(Pages.RegisterView);
            Refresh();
        }
    }
}