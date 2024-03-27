using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DomoLabo.HUBConnectTemplate;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class FirstStepTemplate : ContentView
{
    public FirstStepTemplate()
    {
        InitializeComponent();
    }

    private void NextStep(object sender, EventArgs e)
    {
        this.ControlTemplate = new ControlTemplate(typeof(SecondStepTemplate));
    }
}