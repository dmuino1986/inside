using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Inside.Xamarin.Helpers;
using Inside.Xamarin.Models;
using Inside.Xamarin.Models.DomainModels;
using Inside.Xamarin.Services;
using Xamarin.Forms;

namespace Inside.Xamarin.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        //private async void GetParkingTypes()
        //{
        //    var parkingTypes = await InsideApi.GetAll<ParkingTypeModel>(HostSetting.ParkingTypeEndPoint);

        //    if (!parkingTypes.IsSuccess)
        //    {
        //        //Errore da correggere
        //        await Application.Current.MainPage.DisplayAlert("Error Message",parkingTypes.Message,"Accept");
        //        NavigationService.GetInstance().SetMainPage(Pages.LoginView);

        //    }
        //    else
        //    {
        //        ParkingTypes = new List<ParkingTypeModel>(
        //            parkingTypes.Result as List<ParkingTypeModel> ?? throw new InvalidOperationException("Parking Types are null."));
        //    }
        //}

        //private async void GetParkingCategories()
        //{
        //    var parkingCategories = await InsideApi.GetAll<ParkingCategoryModel>(HostSetting.ParkingCategoryEndPoint);
        //    if (!parkingCategories.IsSuccess)
        //    {
        //        //Errore da correggere
        //        await Application.Current.MainPage.DisplayAlert("Error Message", parkingCategories.Message, "Accept");
        //        NavigationService.GetInstance().SetMainPage(Pages.LoginView);
        //    }
        //    else
        //    {
        //        this.Categories = new List<ParkingCategoryModel>(
        //            parkingCategories.Result as List<ParkingCategoryModel> ?? throw new InvalidOperationException("Parking Categories are null."));
        //    }
        //}

        private static MainViewModel _instance;

        #region Attributes

        private User _currentUser;

        #endregion

        public MainViewModel()
        {
            _instance = this;
            LoadMenu();
            Login = new LoginViewModel();
            Coins = new CoinPageViewModel();
            RegisterUser = new RegisterUserViewModel();
            Menu = new MenuViewModel();
            Tabs = new TabsPageViewModel();
            EditProfile = new EditProfileViewModel();
            ChangePassword = new ChangePasswordViewModel();
        }

        public void SetUpLoginData(User user, string authToken)
        {
            InsideApi.AuthToken = authToken;
            CurrentUser = user;

            //TODO Ver la mejor forma donde debe estar la llave
            var key = "wisegar";
            var salt = CryptoHelper.CreateSalt(16);
            var id = CryptoHelper.EncryptAes(CurrentUser.Id.ToString(), key, salt);
            var username = CryptoHelper.EncryptAes(CurrentUser.UserName, key, salt);

            Settings.UserId = id;
            Settings.UserName = username;
            Settings.Token = authToken;
        }

        public async Task RestoreLoginData()
        {
            if (CurrentUser != null) return;

            if (string.IsNullOrEmpty(Settings.Token) || string.IsNullOrEmpty(Settings.UserName))
            {
                NavigationService.GetInstance().SetMainPage(Pages.LoginView);
                return;
            }

            try
            {
                var key = "wisegar";
                var salt = CryptoHelper.CreateSalt(16);
                var username = CryptoHelper.DecryptAes(Settings.UserName, key, salt);
                InsideApi.AuthToken = Settings.Token;
                var loggerUserResponse = await InsideApi.GetUserByUserName(HostSetting.AuthEndPoint, username);

                if (loggerUserResponse == null ||
                    !loggerUserResponse.IsSuccess ||
                    loggerUserResponse.Result == null)
                {
                    NavigationService.GetInstance().SetMainPage(Pages.LoginView);
                    return;
                }
                CurrentUser = loggerUserResponse.Result as User;
            }
            catch
            {
                NavigationService.GetInstance().SetMainPage(Pages.LoginView);
            }
        }

        public void ResetLoginData()
        {
            InsideApi.AuthToken = string.Empty;
            CurrentUser = null;
            Settings.UserId = string.Empty;
            Settings.UserName = string.Empty;
            Settings.Token = string.Empty;
        }

        private void LoadMenu()
        {
            Menus = new ObservableCollection<MenuModel>
            {
                new MenuModel
                {
                    Title = Languages.MasterItemEditProfile,
                    Icon = "ic_settings",
                    PageName = "EditProfilePage",
                    OnTabActionCommand = new RelayCommand(EditProfileNavigate)
                },
                new MenuModel
                {
                    Title = Languages.MasterItemChangePassword,
                    Icon = "ic_settings",
                    PageName = "ChangePasswordPage",
                    OnTabActionCommand = new RelayCommand(ChangePasswordNavigate)
                },
                new MenuModel
                {
                    Title = Languages.MasterItemLogout,
                    Icon = "ic_exit_to_app",
                    PageName = "LoginPage",
                    OnTabActionCommand = new RelayCommand(Logout)
                }
            };
        }

        private async void Logout()
        {
            await Application.Current.MainPage.DisplayAlert(
                Languages.MasterItemLogoutHeaderAlert,
                Languages.MasterItemLogoutAlert,
                Languages.GeneralAccept);

            //TODO: Implement backend logout call

            ResetLoginData();
            NavigationService.GetInstance().SetMainPage(Pages.LoginView);
        }

        private async void ChangePasswordNavigate()
        {
            await Application.Current.MainPage.DisplayAlert(
                Languages.MasterItemChangePasswordHeaderAlert,
                Languages.MasterItemChangePasswordAlert,
                Languages.GeneralAccept);
            await NavigationService.GetInstance().NavigateOnMaster("ChangePassword");
        }

        private async void EditProfileNavigate()
        {
            await Application.Current.MainPage.DisplayAlert(
                Languages.MasterItemEditProfileHeaderAlert,
                Languages.MasterItemEditProfileAlert,
                Languages.GeneralAccept, Languages.GeneralCancel);
            await NavigationService.GetInstance().NavigateOnMaster("EditProfile");
        }

        public static MainViewModel GetInstance()
        {
            return _instance ?? new MainViewModel();
        }

        #region Properties

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                if (value == null) return;
                Menu.UserName = value.UserName;
                Menu.Coins = value.Coins.ToString();
            }
        }

        //public List<ParkingCategoryModel> Categories { get; set; }
        //public List<ParkingTypeModel> ParkingTypes { get; set; }
        public ObservableCollection<MenuModel> Menus { get; set; }

        public CoinPageViewModel Coins { get; set; }
        public LoginViewModel Login { get; set; }
        public RegisterUserViewModel RegisterUser { get; set; }
        public ParkingCreateViewModel ParkingCreate { get; set; }
        public ParkingEditViewModel ParkingEdit { get; set; }
        public ParkingInfoViewModel ParkingInfo { get; set; }
        public MenuViewModel Menu { get; set; }
        public TabsPageViewModel Tabs { get; set; }
        public EventViewModel Event { get; set; }
        public EditProfileViewModel EditProfile { get; set; }
        public ChangePasswordViewModel ChangePassword { get; set; }

        #endregion
    }
}