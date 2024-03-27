using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DomoLabo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StationPage : ContentPage
    {
        public StationPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}