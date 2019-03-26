using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Essentials;
using System.Diagnostics;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        RestService _restService;

        public MainPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            _restService = new RestService();
        }

        async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
                else
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Default);
                    location = await Geolocation.GetLocationAsync(request);
                }

                WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestUri(Constants.OpenWeatherMapEndpoint, location.Latitude, location.Longitude));
                BindingContext = weatherData;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Boop beep: {ex}");
            }
           
        }

        string GenerateRequestUri(string endpoint, double lat, double lon)
        {
            // All of this hurts my soul
            string requestUri = endpoint;
            requestUri += $"?lat={lat}&lon={lon}";
            requestUri += "&units=metric"; // or units=metric
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;
        }
    }
}
