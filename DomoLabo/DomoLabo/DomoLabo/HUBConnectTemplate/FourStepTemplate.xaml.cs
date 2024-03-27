using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DomoLabo.HUBConnectTemplate;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class FourStepTemplate : ContentView
{
    public FourStepTemplate()
    {
        InitializeComponent();


        if (MQTT.connection.ConnectionState == 2)
        {
            ConnectionSuccess();
        }
        
        MQTT.connection.PropertyChanged += (sender, args) =>
        {
            if (MQTT.connection.ConnectionState == 2)
            {
                try
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        ConnectionSuccess();
                    });
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    throw;
                }
            }
        };
    }

    private void ConnectionSuccess()
    {
        try
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushAsync(new MainPage());
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}