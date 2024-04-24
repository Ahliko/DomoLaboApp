using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomoLabo.Interfaces;

public interface IWifiScan
{
    Task<IEnumerable<string>> GetAvailableNetworksAsync();
}