using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomoLabo.DataClass;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DomoLabo;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ObjectListWidget : ViewCell
{

    private Objet _objet;
    
    public ObjectListWidget(Objet objet)
    {
        InitializeComponent();
        this._objet = objet;
        name.Text = objet.name;
        stateIndicator.BackgroundColor =  objet.activeColor;
        Image.Source = objet.value != "0" ? "ventilateuron.png" :"ventilateur.png";

        objet.PropertyChanged += (sender, args) =>
        {
            name.Text = objet.name;
            stateIndicator.BackgroundColor =  objet.activeColor;
        };
    }

    private void DevelopObject(object sender, EventArgs e)
    {
        try
        {
            MessagingCenter.Send<ObjectListWidget, Objet>(this, "Hi", this._objet);
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
            throw;
        }
    }
}