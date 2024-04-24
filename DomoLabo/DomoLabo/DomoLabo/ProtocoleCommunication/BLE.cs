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
                var characteristicTopic =
                    await service.GetCharacteristicAsync(Guid.Parse("fd3b1289-4226-41fa-abe4-d9b6066a5b20"));
                
                
                var characteristicWifiName =
                    await service.GetCharacteristicAsync(Guid.Parse("6716880a-6831-4689-8958-94e5a15a70d6"));
                
                var characteristicWifiPassword =
                    await service.GetCharacteristicAsync(Guid.Parse("3135cb97-c63b-49fd-a72c-29e2c347f647"));
                
                
                var bytes = await characteristicTopic.ReadAsync();
                string Topic = Encoding.UTF8.GetString(bytes);

                await characteristicWifiName.WriteAsync(Encoding.ASCII.GetBytes("Manon"));
                await characteristicWifiPassword.WriteAsync(Encoding.ASCII.GetBytes("oupslaco"));
                //await MQTT.SendMessageToTopic(Request.Request.getRequest_ObjectAdd(Topic));
                MessagingCenter.Send<BLE, String>(this, "addNewObject", Topic);
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
        MessagingCenter.Send<BLE, IDevice>(this, "newDeviceFound", args.Device);
        deviceList.Add(args.Device);
    }

    private async void ScanDevi(IAdapter adapter)
    {
        deviceList.Clear();
        await adapter.StartScanningForDevicesAsync();
        
        Debug.WriteLine(deviceList.Count());
        //if(deviceList.Count() != 0) Connect(adapter, deviceList[0]);
        if (adapter.IsScanning)
        {
            await adapter.StopScanningForDevicesAsync();
        }
        
    }

    public async void Connect(IAdapter adapter, IDevice device)
    {
        Device.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                if (adapter.IsScanning)
                {
                    await adapter.StopScanningForDevicesAsync();
                }
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