using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomoLabo.Components;
using DomoLabo.DataClass;
using DomoLabo.Interfaces;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace DomoLabo.Page;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ParamsPage : ContentPage
{
    public ParamsPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        
        try
        {
            string data = DependencyService.Get<IFileService>().ReadFile();
            if (data != "")
            {
                JsonConvert.DeserializeObject<DataManager>(data);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }

        foreach (var wifi in DataManager.nowWifiList)
        {
            WifiList.Children.Add(wifi.View);
        }
        
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        foreach (var wifi in DataManager.nowWifiList)
        {
            WifiList.Children.Add(wifi.View);
        }

        var listWifiView = await DependencyService.Get<IWifiScan>().GetAvailableNetworksAsync();
        foreach (var wifi in listWifiView)
        {
            Debug.WriteLine(wifi);
            UnknowWifiList.Children.Add(new FrameWifi(wifi).View);
        }
    }
}