using System;
using System.Collections.Generic;
using System.Text;
using Inside.Domain.Core;
using Inside.Domain.Enum;

namespace Inside.Domain.Entities
{
   public class Order : BaseEntity
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
