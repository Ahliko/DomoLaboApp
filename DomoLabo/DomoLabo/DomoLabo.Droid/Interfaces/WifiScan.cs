using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using DomoLabo.Droid;
using DomoLabo.Interfaces;


[assembly: Xamarin.Forms.Dependency(typeof(WifiScan))]
namespace DomoLabo.Droid
{
    public class WifiScan : IWifiScan
    {
        private Context context = null;

        public WifiScan()
        {
            this.context = Android.App.Application.Context;
        }

        public async Task<IEnumerable<string>> GetAvailableNetworksAsync()
        {
            IEnumerable<string> availableNetworks = null;

            // Get a handle to the Wifi
            var wifiMgr = (WifiManager)context.GetSystemService(Context.WifiService);
            var wifiReceiver = new WifiReceiver(wifiMgr);

            await Task.Run(() =>
            {
                // Start a scan and register the Broadcast receiver to get the list of Wifi Networks
                context.RegisterReceiver(wifiReceiver, new IntentFilter(WifiManager.ScanResultsAvailableAction));
                availableNetworks = wifiReceiver.Scan();
            });

            return availableNetworks;
        }

        class WifiReceiver : BroadcastReceiver
        {
            private WifiManager wifi;
            private List<string> wifiNetworks;
            private AutoResetEvent receiverARE;
            private Timer tmr;
            private const int TIMEOUT_MILLIS = 20000; // 20 seconds timeout

            public WifiReceiver(WifiManager wifi)
            {
                this.wifi = wifi;
                wifiNetworks = new List<string>();
                receiverARE = new AutoResetEvent(false);
            }

            public IEnumerable<string> Scan()
            {
                tmr = new Timer(Timeout, null, TIMEOUT_MILLIS, System.Threading.Timeout.Infinite);
                wifi.StartScan();
                receiverARE.WaitOne();
                return wifiNetworks;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                Debug.WriteLine("receive");
                IList<ScanResult> scanwifinetworks = wifi.ScanResults;
                foreach (ScanResult wifinetwork in scanwifinetworks)
                {
                    wifiNetworks.Add(wifinetwork.Ssid);
                }

                receiverARE.Set();
            }

            private void Timeout(object sender)
            {
                // NOTE release scan, which we are using now, or we throw an error?
                receiverARE.Set();
            }
        }
    }
}