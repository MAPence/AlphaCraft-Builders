using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ACB.Models
{
    public class GeocodingService
    {
        private readonly HttpClient _httpClient;

        public GeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(double, double)> GeocodeAddress(string address)
        {
            string apiKey = "AIzaSyChApDFEzmpCEBP6xfQPCHwucDuC5Z8Bzo";
            string baseUrl = "https://maps.googleapis.com/maps/api/geocode/json";
            string url = $"{baseUrl}?address={Uri.EscapeDataString(address)}&key={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                if (result.status == "OK" && result.results.Count > 0)
                {
                    double lat = result.results[0].geometry.location.lat;
                    double lng = result.results[0].geometry.location.lng;
                    return (lat, lng);
                }
            }

            // Return default values if geocoding fails
            return (0.0, 0.0);
        }
    }
}
