using System;
using Inside.Xamarin.Models.DomainEnums;

namespace Inside.Xamarin.Models.DomainModels
{
    public class Order : BaseModel
    {
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public virtual Parking Parking { get; set; }
        public int ParkingId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}