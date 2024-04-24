using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DomoLabo.Components;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class FrameBluetooth : ViewCell
{
    private IDevice device;
    public FrameBluetooth(IDevice device)
    {
        this.device = device;
        InitializeComponent();
        DeviceName.Text = device.ToString();
        
    }

    private void ConnectDevice(object sender, EventArgs e)
    {
        BLE ble = new BLE();
        ble.Connect(adapter:CrossBluetoothLE.Current.Adapter, device:this.device);
    }
}