using System.Collections.Generic;
using Inside.Xamarin.Models;
using global::Xamarin.Forms.Maps;
using System;


namespace Inside.Xamarin.CustomControls
{
   
    public class CustomMapControl : Map
    {
        /// <summary>
        /// Event thrown when the user taps on the map
        /// </summary>
        public event EventHandler<MapTapEventArgs> Tapped;
        public event EventHandler<PinTapEventArgs> PinTapped;


        public List<CustomPin> CustomPins { get; set; }

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomMapControl()
        {

        }

        /// <summary>
        /// Constructor that takes a region
        /// </summary>
        /// <param name="region"></param>
        public CustomMapControl(MapSpan region)
            : base(region)
        {

        }

        #endregion

        public void OnTap(Position coordinate)
        {
            OnTap(new MapTapEventArgs { Position = coordinate });
        }

        public void OnPinTap(ParkingModel parking)
        {
            OnPinTap(new PinTapEventArgs{ Parking = parking });
        }

        protected virtual void OnTap(MapTapEventArgs e)
        {
            var handler = Tapped;

            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnPinTap(PinTapEventArgs e)
        {
            var handler = PinTapped;

            if (handler != null)
                handler(this, e);
        }
    }
    /// <summary>
    /// Event args used with maps, when the user tap on it
    /// </summary>
    public class MapTapEventArgs : EventArgs
    {
        public Position Position { get; set; }
    }
    public class PinTapEventArgs : EventArgs
    {
        public ParkingModel Parking { get; set; }
    }
}
