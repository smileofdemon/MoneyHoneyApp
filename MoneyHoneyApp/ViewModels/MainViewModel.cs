using MoneyHoneyApp.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MoneyHoneyApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private double _UAH;
        private double _EUR;
        private bool _isUpdate = true;
        private const int UPDATE_PERIOD = 1000; // update data every 1 second, add to setting

        public double UAH { get => _UAH; }
        public double EUR { get => _EUR; }

        public MainViewModel()
        {
            Title = "Главная";
            _UAH = 1;
            _ = Task.Run(() => GetValuesAsync());
        }

        private async Task GetValuesAsync()
        {
            while (_isUpdate)
            {
                var url = @"https://www.revolut.com/api/exchange/quote";
                var parameters = @"?amount=100000&country=DE&fromCurrency=UAH&isRecipientAmount=false&toCurrency=EUR";

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("Accept-Language", "en");
                HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    RevolutResult myDeserializedClass = JsonConvert.DeserializeObject<RevolutResult>(jsonString);
                    _EUR = myDeserializedClass.rate.rate;
                }

                Thread.Sleep(UPDATE_PERIOD);
            }
        }
    }
}