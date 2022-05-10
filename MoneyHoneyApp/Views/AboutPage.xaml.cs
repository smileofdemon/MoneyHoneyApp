using System;
using Xamarin.Forms;

namespace MoneyHoneyApp.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            if (Application.Current.Properties.ContainsKey("UPDATE_PERIOD"))
            {
                SliderMain.Value = Convert.ToDouble(Application.Current.Properties["UPDATE_PERIOD"]);
                displayLabel.Text = $"Период обновления данных: {Application.Current.Properties["UPDATE_PERIOD"]} сек.";
            }
            else
            {
                SliderMain.Value = 1;
                displayLabel.Text = "Период обновления данных: 1 сек.";
            }
        }
        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            var newStep = Math.Round(args.NewValue / 1.0);
            SliderMain.Value = newStep * 1.0;

            double value = args.NewValue;
            displayLabel.Text = $"Период обновления данных: {value} сек.";
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["UPDATE_PERIOD"] = SliderMain.Value;
            DisplayAlert("Уведомление", "Сохранено", "ОK");
            Context.OnSettingUpdated();
        }
    }
}