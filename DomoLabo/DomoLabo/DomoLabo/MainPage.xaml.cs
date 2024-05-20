using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomoLabo.Components;
using DomoLabo.DataClass;
using DomoLabo.Page;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace DomoLabo
{
    public partial class MainPage : ContentPage
    {

        private HUB currentHub;
        private DateTime lastAdd = DateTime.Now;
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
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
                    MessagingCenter.Subscribe<ObjectListWidget, Objet>(this, "Hi", async (sender, arg) =>
                    {
                        MessagingCenter.Unsubscribe<ObjectListWidget, Objet>(this, "Hi");
                
                        await Navigation.PushAsync(new VentilationPage(arg));
                    });
            
                    MessagingCenter.Subscribe<BLE, String>(this, "addNewObject", async (sender, topic) =>
                    {
                        MessagingCenter.Unsubscribe<BLE, String>(this, "addNewObject");
                        if (!((lastAdd - DateTime.Now).TotalSeconds <= -1)) return;
                        lastAdd = DateTime.Now;
                        AddObj(topic);
                    });
                    
                    
                    
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


        private void ObjectChangeState(object sender, EventArgs e)
        {
            /*MQTT.SendDataObject();*/
        }

        private async void AddObj(string topic)
        {
            await MQTT.SendMessageToTopic(Request.Request.getRequest_ObjectAdd(topic));
        }

        private void showPopup(object sender, EventArgs e)
        {
            Navigation.ShowPopupAsync(new DevicesSelector());
        }

        private async void paramsPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ParamsPage());
        }
    }
}