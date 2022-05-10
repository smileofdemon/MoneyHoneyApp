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
        private double _less_EUR;
        private bool _isUpdate = true;
        private const int UPDATE_PERIOD = 1000; // update data every 1 second, add to setting

        public double UAH { get => _UAH; protected set => SetProperty(ref _UAH, value); }
        public double EUR { get => _EUR; protected set => SetProperty(ref _EUR, value); }
        public double Less_EUR { get => _less_EUR; protected set => SetProperty(ref _less_EUR, value); }
        public Command<object> SetEURCommand { get; }

        public MainViewModel()
        {
            Title = "Главная";
            _UAH = 1;
            SetEURCommand = new Command<object>(UpdateLessEUR);
            if (Application.Current.Properties.ContainsKey("Less_EUR"))
                Less_EUR = Convert.ToDouble(Application.Current.Properties["Less_EUR"]);

            _ = Task.Run(() => GetValuesAsync());
        }

        public void UpdateLessEUR(object newValue)
        {
            var textBox = (Entry)newValue;
            if (string.IsNullOrWhiteSpace(textBox.Text))
                return;

            Less_EUR = Convert.ToDouble(textBox.Text);
            Application.Current.Properties["Less_EUR"] = Less_EUR;
            textBox.Text = "";
        }

        private async Task GetValuesAsync()
        {
            while (_isUpdate)
            {
                var url = @"https://www.revolut.com/api/exchange/quote";
                var parameters = @"?amount=100&country=DE&fromCurrency=UAH&isRecipientAmount=false&toCurrency=EUR";

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("Accept-Language", "en");
                HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    RevolutResult myDeserializedClass = JsonConvert.DeserializeObject<RevolutResult>(jsonString);
                    EUR = myDeserializedClass.rate.rate;
                    CheckValue(myDeserializedClass.rate.rate);
                }

                Thread.Sleep(UPDATE_PERIOD);
            }
        }

        private void CheckValue(double newValue)
        {
            if (newValue <= Less_EUR)
            {

            }
        }
    }
}