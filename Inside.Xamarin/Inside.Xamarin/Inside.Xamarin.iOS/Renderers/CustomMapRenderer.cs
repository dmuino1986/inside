using Inside.Xamarin.iOS.Renderers;
using Inside.Xamarin.CustomControls;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMapControl), typeof(CustomMapRenderer))]
namespace Inside.Xamarin.iOS.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        private readonly UITapGestureRecognizer _tapRecogniser;

        public CustomMapRenderer()
        {
            _tapRecogniser = new UITapGestureRecognizer(OnTap)
            {
                NumberOfTapsRequired = 1,
                NumberOfTouchesRequired = 1
            };
        }

        private void OnTap(UITapGestureRecognizer recognizer)
        {
            var cgPoint = recognizer.LocationInView(Control);

            var location = ((MKMapView)Control).ConvertPoint(cgPoint, Control);

            ((CustomMapControl)Element).OnTap(new Position(location.Latitude, location.Longitude));
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            if (Control != null)
                Control.RemoveGestureRecognizer(_tapRecogniser);

            base.OnElementChanged(e);

            if (Control != null)
                Control.AddGestureRecognizer(_tapRecogniser);
        }
    }
}
