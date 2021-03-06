﻿using System.Collections.Generic;
using Inside.Domain.Core;

namespace Inside.Domain.Entities
{
    public class Parking : BaseEntity
    {
        public ParkingCategory ParkingCategory { get; set; }
        public int ParkingCategoryId { get; set; }
        public Event ParkingEvent { get; set; }
        public int ParkingEventId { get; set; }
        public ParkingType ParkingType { get; set; }
        public int ParkingTypeId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsRented { get; set; }
        public List<Order> Orders { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}