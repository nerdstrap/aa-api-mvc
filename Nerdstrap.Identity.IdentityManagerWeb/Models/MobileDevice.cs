namespace Nerdstrap.Identity.IdentityManagerWeb.Models
{
    public class MobileDevice
    {
        public string SimId { get; set; }
        public string OtherId { get; set; }
        public string HardwareId { get; set; }
        public GeoLocation GeoLocation { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceMultiTaskingSupported { get; set; }
        public string DeviceName { get; set; }
        public string DeviceSystemName { get; set; }
        public string DeviceSystemVersion { get; set; }
        public string Languages { get; set; }
        public string WiFiMacAddress { get; set; }
        public string WiFiNetworksDataBbsid { get; set; }
        public string WiFiNetworksDataStationName { get; set; }
        public int WiFiNetworksDataSignalStrength { get; set; }
        public string WiFiNetworksDataChannel { get; set; }
        public string WiFiNetworksDataSsid { get; set; }
        public string CellTowerId { get; set; }
        public string LocationAreaCode { get; set; }
        public string ScreenSize { get; set; }
        public int NumberOfAddressBookEntries { get; set; }
        public string RsaApplicationKey { get; set; }
        public string WapClientId { get; set; }
        public string VendorClientId { get; set; }
        public string MobileCountryCode { get; set; }
        public string MobileCarrierCode { get; set; }
        public string OsId { get; set; }
        public string MobileSdkData { get; set; }
    }
}
