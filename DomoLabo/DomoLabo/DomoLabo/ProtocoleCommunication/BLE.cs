using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using Xamarin.Forms;

namespace DomoLabo;

public class BLE
{
    public List<IDevice> deviceList;
    public void BLEConnection()
    {
        var ble = CrossBluetoothLE.Current;
        var adapter = CrossBluetoothLE.Current.Adapter;
        adapter.ScanTimeout = 10000;

        deviceList = new List<IDevice>();

        var state = ble.State;

        ble.StateChanged += (s, e) =>
        {
            Debug.WriteLine($"The bluetooth state changed to {e.NewState}");
        };

        adapter.DeviceDiscovered += OnDeviceDiscovered;
        adapter.DeviceConnected += OnDeviceConnected;
        ScanDevi(adapter);
    }

    private async void OnDeviceConnected(object sender, DeviceEventArgs args)
    {
        Debug.WriteLine("On Connected");
        try
        {
            if (args.Device != null)
            {
                var service =
                    await args.Device.GetServiceAsync(Guid.Parse("3a13c4ec-4e06-49a2-8fa2-e189f0a9364a"));
                var characteristic =
                    await service.GetCharacteristicAsync(Guid.Parse("fd3b1289-4226-41fa-abe4-d9b6066a5b20"));
                var bytes = await characteristic.ReadAsync();
                string Topic = Encoding.UTF8.GetString(bytes);
                await MQTT.SendMessageToTopic(Request.Request.getRequest_ObjectAdd(Topic));
            }
            else
            {
                Debug.WriteLine("null");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private void OnDeviceDiscovered(object sender, DeviceEventArgs args)
    {
        
        if (args.Device.ToString() == null || args.Device.ToString().Split('|')[0] != "0df0032e")
            return;
        
        Debug.WriteLine(args.Device.ToString());
        deviceList.Add(args.Device);
    }

    private async void ScanDevi(IAdapter adapter)
    {
        deviceList.Clear();
        await adapter.StartScanningForDevicesAsync();
        
        Debug.WriteLine(deviceList.Count());
        if(deviceList.Count() != 0) Connect(adapter, deviceList[0]);
        if (adapter.IsScanning)
        {
            await adapter.StopScanningForDevicesAsync();
        }
        
    }

    private async void Connect(IAdapter adapter, IDevice device)
    {
        Device.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                var connectParameters = new ConnectParameters(forceBleTransport: true);
                await adapter.ConnectToDeviceAsync(device, connectParameters);
            }
            catch (DeviceConnectionException ex)
            {
                Debug.WriteLine(ex);
            }
        });
            
    }
}