using Inside.Xamarin.Models.DomainModels;

namespace Inside.Xamarin.Models
{
    public class ParkingModel : BaseModel
    {
        public ParkingCategoryModel ParkingCategory { get; set; }
        public int ParkingCategoryId { get; set; }
        public int ParkingTypeId { get; set; }
        public ParkingTypeModel ParkingType { get; set; }
        public EventModel ParkingEvent { get; set; }
        public int ParkingEventId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsRented { get; set; }
        public string Status { get; set; }
        public Order RentInfo { get; set; }
        public int UserId { get; set; }
        public string ImageUrl { get; set; }
    }
}