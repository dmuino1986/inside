using System.Collections.Generic;

namespace Inside.Xamarin.Models.DomainModels
{
    public class ParkingType : BaseModel
    {
        public string Type { get; set; }
        public List<Parking> Parkings { get; set; }
    }
}