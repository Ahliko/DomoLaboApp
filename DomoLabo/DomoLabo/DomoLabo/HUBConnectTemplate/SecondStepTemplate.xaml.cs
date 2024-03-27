using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DomoLabo.HUBConnectTemplate;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SecondStepTemplate : ContentView
{
    public SecondStepTemplate()
    {
        InitializeComponent();
    }

    private void NextStep(object sender, EventArgs e)
    {
        this.ControlTemplate = new ControlTemplate(typeof(ThirdStepTemplate));
    }
}