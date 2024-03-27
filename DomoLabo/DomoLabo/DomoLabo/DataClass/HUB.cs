using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace DomoLabo.DataClass;

public class HUB : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public string Name { get; set; }
    private Dictionary<string, Objet> Objects  { get; set; } //  Key : Objet mac(topic) Value : Objet
    public string Topic { get; set; }

    public HUB(string name, Dictionary<string,Objet> objects, string topic)
    {
        this.Name = name;
        this.Objects = objects;
        this.Topic = topic;
    }


    protected void OnPropertyChanged(string propertyName = null)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public Dictionary<string, Objet> GetObject()
    {
        return Objects;
    }
    
    public void AddObject(Objet obj)
    {
        if (Objects.ContainsKey(obj.topic)) return;
        Objects.Add(obj.topic, obj);

        OnPropertyChanged("Objects");
    }
    
    
}