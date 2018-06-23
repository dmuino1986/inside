using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Inside.Xamarin.Helpers;
using Inside.Xamarin.Models;
using Inside.Xamarin.Models.DomainModels;
using Inside.Xamarin.Services;
using Xamarin.Forms;

namespace Inside.Xamarin.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private string _newPassword;
        private string _oldPassword;

        public string NewPassword
        {
            get => _newPassword;
            set => SetValue(ref _newPassword, value);
        }

        public string OldPassword
        {
            get => _oldPassword;
            set => SetValue(ref _oldPassword, value);
        }

        public ICommand ChangeCommand => new RelayCommand(Change);

        private async void Change()
        {
            if (string.IsNullOrEmpty(OldPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.GeneralPasswordError,
                    "Your old password should not be empty",
                    Languages.GeneralAccept);
                return;
            }
            if (string.IsNullOrEmpty(NewPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.GeneralPasswordError,
                    "Your new password should not be empty",
                    Languages.GeneralAccept);
                return;
            }

            var userResponse = await InsideApi.GetUserByUserName(HostSetting.AuthEndPoint, Settings.UserName);

            var model = new ChangePasswordViewModel
            {
                Id = ((User) userResponse.Result).Id,
                OldPassword = OldPassword,
                NewPassword = NewPassword
            };

            var response = await InsideApi.Post<ChangePasswordViewModel, TokenResponse>(
                HostSetting.ChangePasswordEndPoint,
                model);

            if (response == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.GeneralError,
                    Languages.LoginErrorServerAlert +
                    "\n" +
                    Languages.GeneralCheckInternetConnection +
                    "\n",
                    Languages.GeneralAccept, Languages.GeneralCancel);
            }
            else
            {
                if (response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.GeneralSuccess,
                        Languages.ChangePasswordSuccessAlert,
                        Languages.GeneralAccept);
                    ResetLoginData();
                    NavigationService.GetInstance().SetMainPage(Pages.LoginView);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.GeneralError,
                        Languages.EditProfileOldPasswordAlert,
                        Languages.GeneralAccept);
                }
            }
        }

        public void ResetLoginData()
        {
            InsideApi.AuthToken = string.Empty;
            //CurrentUser = null;
            Settings.UserId = string.Empty;
            Settings.UserName = string.Empty;
            Settings.Token = string.Empty;
        }
    }
}