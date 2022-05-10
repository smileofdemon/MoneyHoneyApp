using MoneyHoneyApp.Models;
using Newtonsoft.Json;
using Plugin.LocalNotification;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyHoneyApp.Services
{
    public class EURMoneyService : IMoneyService
    {
        private double _currentRate;
        private double _targetRate;
        private bool _isUpdate = true;
        private readonly int _updatePeriod = 1000; // update data every 1 second
        private readonly Action _onUpdateCurrentRate;
        private readonly Image _image;

        public EURMoneyService(Action onUpdateCurrentRate, Image image)
        {
            if (Application.Current.Properties.ContainsKey("TARGET_EUR"))
                _targetRate = Convert.ToDouble(Application.Current.Properties["TARGET_EUR"]);
            if (Application.Current.Properties.ContainsKey("UPDATE_PERIOD"))
                _updatePeriod = Convert.ToInt32(Application.Current.Properties["UPDATE_PERIOD"]) * 1000;

            _image = image;
            _onUpdateCurrentRate = onUpdateCurrentRate;
            _ = Task.Run(() => GetValuesAsync());
        }

        public double GetCurrentRate() => _currentRate;
        public void SetTargetRate(double newTargetRate)
        {
            _targetRate = newTargetRate;
            Application.Current.Properties["TARGET_EUR"] = _targetRate;
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
                    _currentRate = myDeserializedClass.rate.rate;
                    _onUpdateCurrentRate();
                    CheckValue(myDeserializedClass.rate.rate);
                }

                Thread.Sleep(_updatePeriod);
            }
        }

        private void CheckValue(double newValue)
        {
            if (newValue <= _targetRate)
            {
                if(_image.Source.ToString() != "File: success_icon.png")
                    _image.Source = "success_icon.png";
                var notification = new NotificationRequest
                {
                    BadgeNumber = 1,
                    Description = $"Текущий курс € {newValue}",
                    Title = "€ упал ниже заданного",
                    ReturningData = "",
                    NotificationId = 1
                };
                NotificationCenter.Current.Show(notification);
            }
            else
                if(_image.Source.ToString() != "File: fail_icon.png")
                    _image.Source = "fail_icon.png";
        }
    }
}
