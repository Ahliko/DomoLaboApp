using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DomoLabo.HUBConnectTemplate;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ThirdStepTemplate : ContentView
{
    public ThirdStepTemplate()
    {
        InitializeComponent();
        MQTT.connection = new ConnectionRequestState();
        MQTT.SendMessageToTopic(Request.Request.getRequest_HubConnection());

        MQTT.connection.PropertyChanged += (sender, args) =>
        {
            if (MQTT.connection.ConnectionState == 1)
            {
                try
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Next(this, EventArgs.Empty);
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

    private void Next(object sender, EventArgs e)
    {
        ControlTemplate = new ControlTemplate(typeof(FourStepTemplate));
    }
}