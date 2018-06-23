using System.Collections.Generic;

namespace Inside.Xamarin.Models.DomainModels
{
    public class Parking : BaseModel
    {
        public virtual ParkingCategory ParkingCategory { get; set; }
        public int ParkingCategoryId { get; set; }
        public Event ParkingEvent { get; set; }
        public int ParkingEventId { get; set; }
        public virtual ParkingType ParkingType{ get; set; }
        public int ParkingTypeId { get; set; }
        public bool IsRented { get; set; }
        public List<Order> Orders { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int Price { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}