using MoneyHoneyApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyHoneyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private EURMoneyService eur;

        public MainPage()
        {
            InitializeComponent();
            Entry entry = new Entry();
            Binding binding = new Binding { Source = entry, Path = "Text" };
            Current_EUR.SetBinding(Label.TextProperty, binding);
            eur = new EURMoneyService(() => entry.Text = eur.GetCurrentRate().ToString(), Current_Image);
            Target_EUR.Text = eur.GetCurrentRate().ToString();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(New_Target_EUR.Text))
                return;

            var targetEUR = Convert.ToDouble(New_Target_EUR.Text);
            eur.SetTargetRate(targetEUR);
            Target_EUR.Text = New_Target_EUR.Text;
            New_Target_EUR.Text = "";
            DisplayAlert("Уведомление", "Курс успешно задан", "ОK");
        }
    }
}