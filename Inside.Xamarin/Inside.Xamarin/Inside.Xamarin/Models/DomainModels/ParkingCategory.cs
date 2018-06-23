using System.Collections.Generic;

namespace Inside.Xamarin.Models.DomainModels
{
    public class ParkingCategory : BaseModel
    {
        public string Category { get; set; }
        public double Price { get; set; }
        public double CoinPrice { get; set; }
        public List<Parking> Parkings { get; set; }
    }
}