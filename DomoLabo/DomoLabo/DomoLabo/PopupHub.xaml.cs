using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DomoLabo.HUBConnectTemplate;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DomoLabo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class PopupHub : ContentPage
    {
        public TemplatedView view;
        public PopupHub()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            
            view = templatedView;
            ChangeTemplate(new ControlTemplate(typeof(FirstStepTemplate)));

        }

        public void ChangeTemplate(ControlTemplate template)
        {
            view.ControlTemplate = template;
        }
    }
    
}