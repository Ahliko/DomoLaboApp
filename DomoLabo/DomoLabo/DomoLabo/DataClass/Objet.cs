using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace DomoLabo.DataClass;

public class Objet : INotifyPropertyChanged
{
    
    public event PropertyChangedEventHandler PropertyChanged;
    private string _name { get; set; }

    public string name
    {
        get { return _name; }
        set
        {
            _name = value;
            OnPropertyChanged("name");
        }
    }
    
    public string topic { get; set; }
    private string _state { get; set; }

    public string state
    {
        get { return _state; }
        set
        {
            _state = value;
            OnPropertyChanged("state");
        }
    }
    private string _value { get; set; }

    public string value
    {
        get { return _value; }
        set
        {
            _value = value;
            OnPropertyChanged("value");
        }
    }

    public Color activeColor
    {
        get
        {
            return this.state == "1" ? Color.Red:Color.Green;
        }
    }

    public Objet(string name, string topic, string state, string value)
    {
        this.name = name;
        this.topic = topic;
        this.state = state;
        this.value = value;
    }

    public View getView()
    {
        return new ObjectListWidget(this).View;
    }

    protected void OnPropertyChanged(string propertyName = null)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}