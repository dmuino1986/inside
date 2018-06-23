using System.Collections.Generic;

namespace Inside.Xamarin.Models.DomainModels
{
    public class User : BaseModel {
        public string UserName { get; set; }
        public int Coins { get; set; }
        public string Email { get; set; }
        public List<Parking> Parkings { get; set; }
    }
}
