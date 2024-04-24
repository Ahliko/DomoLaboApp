using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DomoLabo.Components;

namespace DomoLabo.DataClass;

public class DataManager
{
    public static List<HUB> Hubs = new List<HUB>();
    public static List<FrameWifi> nowWifiList { get; set; } = new List<FrameWifi>();
}