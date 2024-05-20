using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace DomoLabo.Components;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DevicesSelector : Xamarin.CommunityToolkit.UI.Views.Popup
{
    public DevicesSelector()
    {
        InitializeComponent();
        
        
        MessagingCenter.Subscribe<BLE, IDevice>(this, "newDeviceFound", (ble, device) =>
        {
            bluetoothDevicesList.Children.Add(new FrameBluetooth(device).View);
        });
        
        MessagingCenter.Subscribe<BLE, String>(this, "addNewObject", async (sender, topic) =>
        {
            Debug.WriteLine(topic);
            Dismiss(true);
        });
        
        BLE.BLEConnection();
        
    }
}