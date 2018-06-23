using System;
using System.Collections.Generic;
using System.Text;
using Inside.Xamarin.Models;
using Xamarin.Forms.Maps;

namespace Inside.Xamarin.CustomControls
{
   public class CustomPin:Pin
    {
        public ParkingModel Parking { get; set; }
    }
}
