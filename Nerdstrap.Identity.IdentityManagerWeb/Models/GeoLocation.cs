namespace Nerdstrap.Identity.IdentityManagerWeb.Models
{
    public class GeoLocation
    {
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int HorizontalAccuracy { get; set; }
        public int Altitude { get; set; }
        public int AltitudeAccuracy { get; set; }
        public int Heading { get; set; }
        public int Speed { get; set; }
        public string TimeStamp { get; set; }
        public int StatusCode { get; set; }
    }
}
