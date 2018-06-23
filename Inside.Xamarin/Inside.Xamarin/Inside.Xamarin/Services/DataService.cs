using Inside.Xamarin.Helpers;
using Inside.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inside.Xamarin.Services
{
    public class DataService
    {

        public ApiService InsideApi;

        public DataService()
        {
            this.InsideApi = new ApiService();
        }
        
        public async Task<Response> GetParkingTypesModel()
        {

            var parkingTypes = await InsideApi.GetAll<ParkingTypeModel>(HostSetting.ParkingTypeEndPoint);
            
            if (!parkingTypes.IsSuccess)
            {
                return parkingTypes;
            }
            else
            {
                return new Response
                {
                    IsSuccess = parkingTypes.IsSuccess,
                    Message = parkingTypes.Message,
                    Result = new List<ParkingTypeModel>(
                    parkingTypes.Result as List<ParkingTypeModel> ?? throw new InvalidOperationException("Parking Types are null."))
                };
            }
        }

        public async Task<Response> GetParkingCategories()
        {
            var parkingCategories = await InsideApi.GetAll<ParkingCategoryModel>(HostSetting.ParkingCategoryEndPoint);

            if (!parkingCategories.IsSuccess)
            {
               
                return parkingCategories;
            }
            else
            {
                return new Response
                {
                    IsSuccess = parkingCategories.IsSuccess,
                    Message = parkingCategories.Message,
                    Result = new List<ParkingCategoryModel>(
                    parkingCategories.Result as List<ParkingCategoryModel> ?? throw new InvalidOperationException("Parking Categories are null."))
                };

            }
        }
    }
}
