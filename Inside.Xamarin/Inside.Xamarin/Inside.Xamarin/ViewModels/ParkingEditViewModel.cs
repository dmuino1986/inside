using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Inside.Xamarin.Helpers;
using Inside.Xamarin.Models;
using Inside.Xamarin.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Inside.Xamarin.ViewModels
{
    public class ParkingEditViewModel : BaseViewModel
    {
        #region Constructor

        public ParkingEditViewModel(ParkingModel parking)
        {
            _navigationService = NavigationService.GetInstance();
            _parkingEdited = parking;
            // this.ConvertUmageUrlToImageSource(parking.ImageUrl);
            DataService = new DataService();
            GetParkingCategories();
            GetParkingTypes();

            GetColorParkingIcon();
        }

        #endregion

        #region Attributes

        private readonly NavigationService _navigationService;
        private MediaFile _mediaFile;
        private ImageSource _parkingPhoto;
        private ParkingModel _parkingEdited;
        private ObservableCollection<ParkingCategoryModel> _categories;
        private ObservableCollection<ParkingTypeModel> _parkingTypes;
        private ParkingCategoryModel _selectedCategory;
        private ParkingTypeModel _selectedParkingType;
        private string _iconNameBasedOnCategory;
        public DataService DataService;

        #endregion

        #region Properties

        public ParkingModel ParkingEdited
        {
            get => _parkingEdited;
            set => SetValue(ref _parkingEdited, value);
        }

        public ParkingCategoryModel SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetValue(ref _selectedCategory, value);
                ParkingEdited.ParkingCategoryId = value.Id;
                GetColorParkingIcon();
            }
        }

        public string IconNameBasedOnCategory
        {
            get => _iconNameBasedOnCategory;
            set => SetValue(ref _iconNameBasedOnCategory, value);
        }

        public ObservableCollection<ParkingCategoryModel> Categories
        {
            get => _categories;
            set => SetValue(ref _categories, value);
        }

        public ObservableCollection<ParkingTypeModel> ParkingTypes
        {
            get => _parkingTypes;
            set => SetValue(ref _parkingTypes, value);
        }

        public ParkingTypeModel SelectedParkingType
        {
            get => _selectedParkingType;
            set
            {
                SetValue(ref _selectedParkingType, value);
                ParkingEdited.ParkingTypeId = value.Id;
            }
        }

        public ImageSource ParkingPhoto
        {
            get => _parkingPhoto;
            set => SetValue(ref _parkingPhoto, value);
        }

        #endregion

        #region Commands

        public ICommand ParkingEditCommand => new RelayCommand(ParkingEdit);
        public ICommand ChangePhotoCommand => new RelayCommand(ChangePhoto);
        public ICommand EventCommand => new RelayCommand(EditParkingEvent);

        #endregion


        #region Methods

        private void ConvertUmageUrlToImageSource(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var imageSource = new UriImageSource();
            imageSource.Uri = uri;
            ParkingPhoto = imageSource;
        }

        private async void EditParkingEvent()
        {
            MainViewModel.GetInstance().Event = new EventViewModel(ParkingEdited.ParkingEvent);
            await _navigationService.NavigateOnMaster("EventView");
        }

        private async void ChangePhoto()
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

        private async void ParkingEdit()
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(ParkingEdited.Id.ToString()), "Id");
            
            // Esta propiedad siempre llega vacia al servidor , no se porque sucede esto
            //con lo cual editas el parking pero la imagen se queda null
            content.Add(new StringContent(ParkingEdited.ImageUrl), "ImageUrl");

            content.Add(new StringContent(ParkingEdited.ParkingCategoryId.ToString()), "ParkingCategoryId");
            content.Add(new StringContent(ParkingEdited.ParkingTypeId.ToString()), "ParkingTypeId");
            content.Add(new StringContent(ParkingEdited.ParkingEventId.ToString()), "ParkingEventId");
            content.Add(new StringContent(ParkingEdited.Latitude), "Latitude");
            content.Add(new StringContent(ParkingEdited.Longitude), "Longitude");
            content.Add(new StringContent(ParkingEdited.UserId.ToString()), "UserId");
            if (_mediaFile != null)
                content.Add(new StreamContent(_mediaFile.GetStream()),
                    "ImageUrl",
                    "parking.jpg");
            var response = await InsideApi.EditParking(HostSetting.ParkingEndPoint, content);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.GeneralError,
                    Languages.ParkingEditAlert,
                    Languages.GeneralAccept);
                ParkingPhoto = "add_photo";
                return;
            }
            var parkingEdited = response.Result as ParkingModel;
        }

        private async void GetParkingTypes()
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

            ParkingTypes =
                new ObservableCollection<ParkingTypeModel>(
                    typesModelResponse.Result as List<ParkingTypeModel>); //TODO: Create generic Model Response
            SelectedParkingType = ParkingTypes.FirstOrDefault(type => type.Id == ParkingEdited.ParkingTypeId);
        }


        private async void GetParkingCategories()
        {
            //APIService se puede utilizar directamente si del backend llega ya el Model y no la entidad.
            //Cargar los datos del la api para cada pagina en el momento en el cual la misma lo necesite.
            //Puede que paresca redundante pero evita problemas de ejecucion. Con mas experiancia se optimiza
            var categoriesModelResponse = await DataService.GetParkingCategories();
            if (!categoriesModelResponse.IsSuccess)
            {
                NotificationService.GetInstance()
                    .ShowInfoAlertOnMaster("Error message", categoriesModelResponse.Message);
                await NavigationService.GetInstance().BackOnMaster();
                return;
            }

            Categories =
                new ObservableCollection<ParkingCategoryModel>(
                    categoriesModelResponse.Result as List<ParkingCategoryModel>); //TODO: Create generic Model Response
            SelectedCategory = Categories.FirstOrDefault(cat => cat.Id == ParkingEdited.ParkingCategoryId);
        }

        private void GetColorParkingIcon()
        {
            if (SelectedCategory.Category == "Business")
                IconNameBasedOnCategory = "ic_location_green";
            else
                IconNameBasedOnCategory = "ic_location_black";
        }

        #endregion
    }
}