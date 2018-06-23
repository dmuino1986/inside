using Android.Content;
using Android.Gms.Maps;
using Inside.Xamarin.CustomControls;
using Inside.Xamarin.Droid.Renderers;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMapControl), typeof(CustomMapRenderer))]

namespace Inside.Xamarin.Droid.Renderers
{
    /// <summary>
    ///     Renderer for the xamarin map.
    ///     Enable user to get a position by taping on the map.
    /// </summary>
    public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
    {
        private CustomMapControl _formsMap;

        // We use a native google map for Android
        private GoogleMap _map;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;

            if (_map != null)
            {
                _map.MapClick += googleMap_MapClick;
                _map.MarkerClick += OnMarkerClick;
            }
        }

        private void OnMarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            var pos = new Position(e.Marker.Position.Latitude, e.Marker.Position.Longitude);
            var pin = _formsMap.Pins.First(p => p.Position == pos);
            var customPin = pin as CustomPin;
            if (customPin != null) ((CustomMapControl) Element).OnPinTap(customPin.Parking);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
                _map.MapClick -= googleMap_MapClick;

            if (e.NewElement != null)
            {
                _formsMap = (CustomMapControl) e.NewElement;
                Control.GetMapAsync(this);
            }
        }

        /* private void CustomPins_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
         {
             _customPins = (ObservableCollection<CustomPin>)sender;
             _map.Clear();
             foreach (var pin in _customPins)
             {
                 var marker = new MarkerOptions();
                 marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
                 marker.SetTitle(pin.Label);
                 marker.SetSnippet(pin.Address);
                // marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));

                 _map.AddMarker(marker);
             }
         }*/

        private void googleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            ((CustomMapControl) Element).OnTap(new Position(e.Point.Latitude, e.Point.Longitude));
        }
    }
}