using MoneyHoneyApp.Models;
using Newtonsoft.Json;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
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
        private List<bool> _isUpdateList = new List<bool>() { true };
        private int _lastIndex = 0;
        private bool _isMatched = false;
        private int _updatePeriod = 1000; // update data every 1 second
        private readonly Action _onUpdateCurrentRate;

        public EURMoneyService(Action onUpdateCurrentRate)
        {
            if (Application.Current.Properties.ContainsKey("TARGET_EUR"))
                _targetRate = Convert.ToDouble(Application.Current.Properties["TARGET_EUR"]);

            _onUpdateCurrentRate = onUpdateCurrentRate;
            Context.OnSettingUpdated = NewWork;
            NewWork();
        }

        private void NewWork()
        {
            if (Application.Current.Properties.ContainsKey("UPDATE_PERIOD"))
                _updatePeriod = Convert.ToInt32(Application.Current.Properties["UPDATE_PERIOD"]) * 1000;

            if (_lastIndex > 1000)
            {
                _isUpdateList[_lastIndex] = false;
                _isUpdateList = new List<bool>() { true };
                _lastIndex = 0;
            }
            _isUpdateList[_lastIndex] = false;
            _isUpdateList.Add(true);
            _lastIndex++;

            _ = Task.Run(() => GetValuesAsync(_lastIndex));
        }

        public bool IsMatched() => _isMatched;
        public double GetCurrentRate() => _currentRate;
        public double GetTargetRate() => _targetRate;
        public void SetTargetRate(double newTargetRate)
        {
            _targetRate = newTargetRate;
            Application.Current.Properties["TARGET_EUR"] = _targetRate;
        }

        private async Task GetValuesAsync(int index)
        {
            while (_isUpdateList[index])
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
                    CheckValue(myDeserializedClass.rate.rate);
                    _onUpdateCurrentRate();
                }

                Thread.Sleep(_updatePeriod);
            }
        }

        private void CheckValue(double newValue)
        {
            _isMatched = newValue <= _targetRate;
            if (_isMatched)
            {
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
        }
    }
}
