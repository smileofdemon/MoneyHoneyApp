using MoneyHoneyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyHoneyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        //private double _less_EUR;
        public MainPage()
        {
            //if (Application.Current.Properties.ContainsKey("Less_EUR"))
            //    _less_EUR = Convert.ToDouble(Application.Current.Properties["Less_EUR"]);
            InitializeComponent();
            //Current_EUR.Text = $"{_less_EUR}";
        }

        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    New_EUR.Unfocus();
        //    _less_EUR = Convert.ToDouble(New_EUR.Text);
        //    Application.Current.Properties["Less_EUR"] = _less_EUR;
        //    Current_EUR.Text = New_EUR.Text;
        //    New_EUR.Text = "";
        //    DisplayAlert("Уведомление", "Курс успешно задан", "ОK");
        //}
    }
}