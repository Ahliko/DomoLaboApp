using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DomoLabo.Components;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class FrameWifi : ViewCell
{
    public FrameWifi(String name, String password = "")
    {
        InitializeComponent();
        WifiName.Text = name;

        PasswordEntry.Text = password;
        PasswordEntry.IsVisible = false;
    }

    private void showPasswordEntry(object sender, EventArgs e)
    {
        PasswordEntry.IsVisible = true;
    }

    private void saveWifiPassword(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}