using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomoLabo.DataClass;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DomoLabo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VentilationPage : ContentPage
    {
        public VentilationPage(Objet objet)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Debug.WriteLine(objet.name);
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}