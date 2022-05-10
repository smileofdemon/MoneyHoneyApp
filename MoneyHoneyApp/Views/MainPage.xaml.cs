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
            CreateNewService();
        }

        public void CreateNewService()
        {
            Entry entry = new Entry();
            Binding binding = new Binding { Source = entry, Path = "Text" };
            Binding bindingCheckBox = new Binding { Source = entry, Path = "IsEnabled" };

            Current_EUR.SetBinding(Label.TextProperty, binding);
            IsMatchedBox.SetBinding(CheckBox.IsCheckedProperty, bindingCheckBox);

            eur = new EURMoneyService(() => UpdateData(entry));
            Target_EUR.Text = eur.GetTargetRate().ToString();
        }

        public void UpdateData(Entry entry)
        {
            entry.Text = eur.GetCurrentRate().ToString();
            entry.IsEnabled = eur.IsMatched();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(New_Target_EUR.Text))
                return;

            var targetEUR = Convert.ToDouble(New_Target_EUR.Text);
            eur.SetTargetRate(targetEUR);
            Target_EUR.Text = New_Target_EUR.Text;
            New_Target_EUR.Text = "";
            //DisplayAlert("Уведомление", "Курс успешно задан", "ОK");
        }
    }
}