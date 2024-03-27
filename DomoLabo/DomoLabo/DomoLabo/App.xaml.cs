using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Plugin.FirebasePushNotification;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace DomoLabo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            };
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {

                System.Diagnostics.Debug.WriteLine("Received");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }


            }; 
            
            
            
            
        }

        protected override void OnStart()
        {
            Thread t = new Thread(async () =>
            {
                await MQTT.MQTTListener();
            });
            t.IsBackground = true;
            t.Start();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        
       
    }
}