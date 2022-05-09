using MoneyHoneyApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MoneyHoneyApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}