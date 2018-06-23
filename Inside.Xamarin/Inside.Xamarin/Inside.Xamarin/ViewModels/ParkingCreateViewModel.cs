using GalaSoft.MvvmLight.Command;
using Inside.Xamarin.Helpers;
using Inside.Xamarin.Models;
using Inside.Xamarin.Services;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Forms;
using Position = Xamarin.Forms.Maps.Position;

namespace Inside.Xamarin.ViewModels
{
    public class ParkingCreateViewModel : BaseViewModel
    {
        private readonly NavigationService _navigationService;
        private ObservableCollection<ParkingCategoryModel> _categories;
        private string _iconNameBasedOnCategory;
        private MediaFile _mediaFile;
        private ImageSource _parkingPhoto;
        private ObservableCollection<ParkingTypeModel> _parkingTypes;
        private ParkingCategoryModel _selectedCategory;
        private ParkingTypeModel _selectedParkingType;

        public DataService DataService; 

        public ParkingCreateViewModel()
        {
            DataService = new DataService();

        }

        public ParkingCreateViewModel(Position position)
        {
            DataService = new DataService();
            _iconNameBasedOnCategory = "ic_location_black";
            Coordenate = position;
            GetParkingCategoriesAsync();
            GetParkingTypesAsync();
            ParkingPhoto = "add_photo";
            _navigationService = NavigationService.GetInstance();
            MessagingCenter.Subscribe<EventModel>(this, Messages.ParkingEventCreated,
                parkingEvent => { ParkingEvent = parkingEvent; });
        }


        public Position Coordenate { get; set; }

        public ImageSource ParkingPhoto
        {
            get => _parkingPhoto;
            set => SetValue(ref _parkingPhoto, value);
        }

        public EventModel ParkingEvent { get; set; }

        public string IconNameBasedOnCategory
        {
            get => _iconNameBasedOnCategory;
            set => SetValue(ref _iconNameBasedOnCategory, value);
        }

        public Address Address { get; set; }

        public ObservableCollection<ParkingCategoryModel> Categories
        {
            get => _categories;
            set => SetValue(ref _categories, value);
        }

        public ParkingCategoryModel SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetValue(ref _selectedCategory, value);
                GetColorParkingIcon();
            }
        }

        public ObservableCollection<ParkingTypeModel> ParkingTypes
        {
            get => _parkingTypes;
            set => SetValue(ref _parkingTypes, value);
        }

        public ParkingTypeModel SelectedParkingType
        {
            get => _selectedParkingType;
            set => SetValue(ref _selectedParkingType, value);
        }

        public ICommand ParkingCreateCommand => new RelayCommand(ParkingCreate);
        public ICommand AddPhotoCommand => new RelayCommand(AddPhoto);
        public ICommand EventCommand => new RelayCommand(CreateParkingEvent);


        private async void AddPhoto()
        {
            await CrossMedia.Current.Initialize();
            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ParkingCreateAddPhotoHeaderAlert,
                Languages.GeneralCancel,
                null,
                Languages.ParkingCreateAddPhotoOption1,
                Languages.ParkingCreateAddPhotoOption2);
            if (source == null)
            {
                _mediaFile = null;
                return;
            }
            if (source == "Take A Photo From Camera.")
                TakeParkingPhoto();
            if (source == "Pick From Gallery.")
                PickParkingPhoto();
        }

        private async void TakeParkingPhoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.ParkingCreateTakePhotoHeaderAlert,
                    Languages.ParkingCreateTakePhotoAlert,
                    Languages.GeneralAccept);
                return;
            }
            _mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "parking_photo.jpg",
                PhotoSize = PhotoSize.Small
            });


            if (_mediaFile == null)
                return;
            ParkingPhoto = ImageSource.FromStream(() => { return _mediaFile.GetStream(); });
        }

        private async void PickParkingPhoto()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.ParkingCreatePickPhotoHeaderAlert,
                    Languages.ParkingCreatePickPhotoAlert,
                    Languages.GeneralAccept);
                return;
            }
            _mediaFile = await CrossMedia.Current.PickPhotoAsync();
            if (_mediaFile == null)
                return;
            ParkingPhoto = ImageSource.FromStream(() => { return _mediaFile.GetStream(); });
        }

        private async void ParkingCreate()
        {
            if (ParkingEvent == null)
            {
                await NotificationService.GetInstance().ShowDialogAlertOnMaster(
                    "Alert",
                    "You must set the parking availability");
                return;
            }
            if (SelectedCategory == null)
            {
                await NotificationService.GetInstance().ShowDialogAlertOnMaster(
                    "Alert",
                    "You must set the parking Category");
                return;
            }
            if (SelectedParkingType == null)
            {
                await NotificationService.GetInstance().ShowDialogAlertOnMaster(
                    "Alert",
                    "You must set the parking type");
                return;
            }
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var eventResponse =
                await InsideApi.Post<EventModel, EventModel>(HostSetting.EventEndPoint + "/addevent", ParkingEvent);
            if (eventResponse.IsSuccess)
            {
                ParkingEvent = eventResponse.Result as EventModel;
            }
            else
            {
                await NotificationService.GetInstance().ShowDialogAlertOnMaster(
                    "Error",
                    "Error trying create parking event.");
                return;
            }
            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(Coordenate.Latitude.ToString()), "Latitude");
                content.Add(new StringContent(Coordenate.Longitude.ToString()), "Longitude");
                content.Add(new StringContent(SelectedCategory.Id.ToString()), "ParkingCategoryId");
                content.Add(new StringContent(SelectedParkingType.Id.ToString()), "ParkingTypeId");
                content.Add(new StringContent(ParkingEvent.Id.ToString()), "ParkingEventId");
                content.Add(new StringContent(MainViewModel.GetInstance().CurrentUser.Id.ToString()), "UserId");
                if (_mediaFile != null)
                    content.Add(new StreamContent(_mediaFile.GetStream()),
                        "ImageUrl",
                        "parking.jpg");
                var response = await InsideApi.AddParking(HostSetting.ParkingEndPoint, content);
                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.GeneralError,
                        Languages.ParkingCreateAddAlert,
                        Languages.GeneralAccept);
                    ParkingPhoto = "add_photo";
                    SelectedCategory = null;
                    SelectedParkingType = null;
                    return;
                }

                MessagingCenter.Send(response.Result as ParkingModel, Messages.NewParkingCreated);
                await _navigationService.BackOnMaster();
            }
            catch (Exception e)
            {
                // TODO Esto luego hay que ver que será mejor en caso de capturar la exepcion 
                if (e is NullReferenceException)
                {
                  await  NotificationService.GetInstance().ShowDialogAlertOnMaster(
                        "Null error",
                        e.Message);
                }
               await NavigationService.GetInstance().BackOnMaster();
            }

           
        }

        private async void GetParkingTypesAsync()
        {
            //APIService se puede utilizar directamente si del backend llega ya el Model y no la entidad.
            //Cargar los datos del la api para cada pagina en el momento en el cual la misma lo necesite. 
            //Puede que paresca redundante pero evita problemas de ejecucion. Con mas experiancia se optimiza
            var typesModelResponse = await DataService.GetParkingTypesModel();

            if (!typesModelResponse.IsSuccess)
            {
                NotificationService.GetInstance().ShowInfoAlertOnMaster("Error message", typesModelResponse.Message);
                await NavigationService.GetInstance().BackOnMaster();
                return;
            }

            ParkingTypes = new ObservableCollection<ParkingTypeModel>(typesModelResponse.Result as List<ParkingTypeModel>); //TODO: Create generic Model Response
        }


        private async void GetParkingCategoriesAsync()
        {
            //APIService se puede utilizar directamente si del backend llega ya el Model y no la entidad.
            //Cargar los datos del la api para cada pagina en el momento en el cual la misma lo necesite. 
            //Puede que paresca redundante pero evita problemas de ejecucion. Con mas experiancia se optimiza
            var categoriesModelResponse = await DataService.GetParkingCategories();

            if (!categoriesModelResponse.IsSuccess) {
                NotificationService.GetInstance().ShowInfoAlertOnMaster("Error message", categoriesModelResponse.Message);
                await NavigationService.GetInstance().BackOnMaster();
                return;
            }

            Categories = new ObservableCollection<ParkingCategoryModel>(categoriesModelResponse.Result as List<ParkingCategoryModel>); //TODO: Create generic Model Response
        }

        private void GetColorParkingIcon()
        {
            if (SelectedCategory.Category == "Business")
                IconNameBasedOnCategory = "ic_location_green";
            else
                IconNameBasedOnCategory = "ic_location_black";
        }

        private async void CreateParkingEvent()
        {
            MainViewModel.GetInstance().Event = new EventViewModel();
            await _navigationService.NavigateOnMaster("EventView");
        }
    }
}