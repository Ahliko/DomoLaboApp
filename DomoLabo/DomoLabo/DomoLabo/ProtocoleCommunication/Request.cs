using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text.Json;
using DomoLabo.DataClass;

namespace DomoLabo.Request;

public class Request
{
    public static Dictionary<string,string> identity
    {
        get
        {
            return new Dictionary<string, string>(){
                { "name", "App"},
                {"mac", GetDeviceMacAddress()}
            };
        }
    }
    
    public static string GetDeviceMacAddress()
    {
        foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces()) 
        {
            if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet) 
            {
                var address = netInterface.GetPhysicalAddress();
                return BitConverter.ToString(address.GetAddressBytes());

            }
        }

        return "NoMac";
    }

    public static string getRequest_HubConnection()
    {
        return JsonSerializer.Serialize(new Dictionary<string, Dictionary<string, string>>()
            {
                {
                    "identity", identity
                },
                {
                    "content", new Dictionary<string, string>()
                    {
                        { "type", "connection" }
                    }
                }
            });
    }

    public static string getRequest_ObjectAdd(string objTopic)
    {
        return JsonSerializer.Serialize(new Dictionary<string, Dictionary<string, string>>()
        {
            {
                "identity", identity
            },
            {
                "content", new Dictionary<string, string>()
                {
                    { "type", "add" },
                    { "topic", objTopic }
                }
            }
        });
    }

    public static void processRequest(string rqt)
    {
        Dictionary<string, Dictionary<string, string>> r = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(rqt);

        if (r["identity"] == identity) return;

        switch (r["content"]["type"])
        {
            case "response":

                Debug.WriteLine("Reponse");
                switch (r["content"]["what"])
                {
                    case "connection":
                        Debug.WriteLine("Connection");

                        switch (r["content"]["response"])
                        {
                            case "disponible":
                                MQTT.connection.ConnectionState = 1;
                                break;
                            case "ok":
                                MQTT.HubConnection(r);
                                MQTT.connection.ConnectionState = 2;
                                break;
                        }
                        
                        break;
                    case "add":
                        Debug.WriteLine("Add");
                        break;
                    default:
                        break;
                }
                
                if (r["content"]["what"] == "add")
                {
                    Debug.WriteLine("Ajout");
                    if (r["content"]["response"] == "ok")
                    {
                        Debug.WriteLine("Ok");
                        Dictionary<string, string> id =
                            JsonSerializer.Deserialize<Dictionary<string, string>>(r["content"]["identity"]);
                        Objet nObj = new Objet(id["name"], "1234", "0", "0");
                                    
                        DataManager.Hubs[0].AddObject(nObj);
                    }
                }
                break;
            default:
                break;
        }
    }
}