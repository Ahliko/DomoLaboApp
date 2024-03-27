using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomoLabo.DataClass;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace DomoLabo
{
    public partial class MainPage : ContentPage
    {

        private HUB currentHub;
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            
            MessagingCenter.Subscribe<ObjectListWidget, Objet>(this, "Hi", async (sender, arg) =>
            {
                Navigation.PushAsync(new VentilationPage(arg));
            });
        }
        protected override void OnAppearing()
        {
            try
            {
                if (!DataManager.Hubs.Any())
                {
                    Navigation.PushAsync(new PopupHub());
                }
                else
                {
                    currentHub = DataManager.Hubs[0];
                    currentHub.PropertyChanged += (sender, args) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                test.Children.Clear();
                                foreach (var objects in currentHub.GetObject().Values.ToArray())
                                {
                                    test.Children.Add(objects.getView());
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex);
                            }
                        });
                    };
                    Header.Text = currentHub.Name;
                    try
                    {
                        test.Children.Clear();
                        foreach (var objects in currentHub.GetObject().Values.ToArray())
                        {
                            test.Children.Add(objects.getView());
                        }
            
                        
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
            
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            
        }


        private void StationPage_OnTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StationPage());
        }

        private void ObjectChangeState(object sender, EventArgs e)
        {
            /*MQTT.SendDataObject();*/
        }

        private async void AddObj(object sender, EventArgs e)
        {
            /*BLE v = new BLE();
            v.BLEConnection();*/
            await MQTT.SendMessageToTopic(Request.Request.getRequest_ObjectAdd("12345"));
        }

        private void StopTest(object sender, EventArgs e)
        {
            MQTT.SendDataObject("0");
        }

        private void StartTest(object sender, EventArgs e)
        {
            MQTT.SendDataObject("1");
        }
    }
}