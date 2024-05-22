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

        private Objet objet;
        private DateTime lastSend = DateTime.Now;
        public VentilationPage(Objet objet)
        {
            this.objet = objet;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.objName.Text = this.objet.name;
            this.objValue.Text = this.objet.value + " %";
            this.sliderVentilo.Value = Convert.ToDouble(this.objet.value);
            this.sliderVisualizer.HeightRequest = sliderVentilo.Value/100*this.sliderContainer.Height+(sliderVentilo.Value == 0 ? 0 : 15);
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        async private void SpeedChange(object sender, ValueChangedEventArgs e)
        {
            this.objet.value = ((int)e.NewValue).ToString();
            this.objValue.Text = this.objet.value + " %";
            this.sliderVisualizer.HeightRequest = e.NewValue/100*this.sliderContainer.Height+(e.NewValue == 0 ? 0 : 15);
            
            if (!((lastSend - DateTime.Now).TotalMilliseconds <= -100)) return;
            
            lastSend = DateTime.Now;
            await MQTT.SendMessageToTopic(Request.Request.getRequest_ModifyObjetData(objet.topic, objet.state, ((int)e.NewValue).ToString()));
        }

        async private void SpeedChangeEnd(object sender, EventArgs e)
        {
            this.objet.value = ((int)sliderVentilo.Value).ToString();
            this.objValue.Text = this.objet.value + " %";
            this.sliderVisualizer.HeightRequest = sliderVentilo.Value/100*this.sliderContainer.Height+(sliderVentilo.Value == 0 ? 0 : 15);

            await MQTT.SendMessageToTopic(Request.Request.getRequest_ModifyObjetData(objet.topic, objet.state, ((int)sliderVentilo.Value).ToString()));
        }
    }
}