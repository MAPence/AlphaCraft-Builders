
using Newtonsoft.Json.Linq;
using System.Net.Http;
using ACB.Models;
using System.Net;

namespace ACB.Controllers

{
    public class GeoLocation
    {

        

        private const string GoogleMapsGeocodingApiUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        private readonly HttpClient _httpClient;

        public GeoLocation()
        {
            _httpClient = new HttpClient();
        }

        public async Task<LatLong> GetCoordinatesAsync(string? address, string city, string state, string zip)
        {
            var latlong = new LatLong();
            try
            {
                var fullAddress = $"{address}, {city}, {state} {zip}";
                var encodedAddress = WebUtility.UrlEncode(fullAddress);
                var requestUrl = $"{GoogleMapsGeocodingApiUrl}?address={encodedAddress}&key=AIzaSyChApDFEzmpCEBP6xfQPCHwucDuC5Z8Bzo";
                var response = await _httpClient.GetAsync(requestUrl);
                

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(content);
                    var location = json["results"][0]["geometry"]["location"];
                    var latitude = (double)location["lat"];
                    var longitude = (double)location["lng"];
                    latlong.Lat = latitude;
                    latlong.Long = longitude;
                    return latlong;
                }
                else
                {
                    // Handle unsuccessful response
                    return latlong;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return latlong;
            }
        }
    }
}
