using Inside.Xamarin.ViewModels;

namespace Inside.Xamarin.Models
{
    public class ParkingCategoryModel : BaseModel
    {
        public string Category { get; set; }
        public double Price { get; set; }
        public double CoinPrice { get; set; }
    }
}