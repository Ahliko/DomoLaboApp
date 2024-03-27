using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DomoLabo.DataClass;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Xamarin.Forms.Xaml;
using DomoLabo.Request;

namespace DomoLabo
{
    public class MQTT
    {
        private static IMqttClient? mqttClient;
        public static ConnectionRequestState connection = new ConnectionRequestState();
        private static string appId
        {
            get
            {
                string tmp;
                Request.Request.identity.TryGetValue("mac", out tmp);
                return tmp;
            }
        }
        static string topicApp = "ac863f346e618f9a959b5c95d5d28941/"+appId;


        public static async Task MQTTListener()
        {
            string broker = "test.mosquitto.org";
            int port = 1883;
            string clientId = Guid.NewGuid().ToString();

            // Create a MQTT client factory
            var factory = new MqttFactory();

            // Create a MQTT client instance
            mqttClient = factory.CreateMqttClient();

            // Create MQTT client options
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(broker, port) // MQTT broker address and port
                .WithClientId(clientId)
                .WithCleanSession()
                .Build();

            // Connect to MQTT broker
            var connectResult = await mqttClient.ConnectAsync(options);

            if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
            {
                Debug.WriteLine("Local |    Connected to MQTT broker successfully.");

                await ConnectToTopic(topicApp);

                // Callback function when a message is received
                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    string payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.ToArray());
                    Debug.WriteLine($"MQTT  |    {payload}");

                    try
                    {
                        Request.Request.processRequest(payload);
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                        throw;
                    }
                    
                    return Task.CompletedTask;
                };

            }
            else
            {
                Debug.WriteLine($"Local |    Failed to connect to MQTT broker: {connectResult.ResultCode}");
            }
        }

        public static void HubConnection(Dictionary<string, Dictionary<string, string>> request)
        {

            Dictionary<string, Dictionary<string, string>> objects = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(request["content"]["data"]);
            Dictionary<string, Objet> HubObj = new Dictionary<string, Objet>();
            foreach (var obj in objects)
            {
                Objet nObj = new Objet(obj.Value["name"], obj.Value["topic"], obj.Value["state"], obj.Value["value"]);
                HubObj.Add(obj.Key, nObj);
            }

            DataManager.Hubs.Add(new HUB(request["identity"]["name"], HubObj, topicApp));
        }

        async public static Task SendDataObject(string value)
        {
            Debug.WriteLine("send data");
            string broker = "test.mosquitto.org";
            int port = 1883;
            string clientId = Guid.NewGuid().ToString();

            // HASH MD5 de App
            string topicObjet = "1234";

            // Create a MQTT client factory
            var factory = new MqttFactory();

            // Create a MQTT client instance
            mqttClient = factory.CreateMqttClient();

            // Create MQTT client options
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(broker, port) // MQTT broker address and port
                .WithClientId(clientId)
                .WithCleanSession()
                .Build();

            // Connect to MQTT broker
            var connectResult = await mqttClient.ConnectAsync(options);

            if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
            {
                Debug.WriteLine("Local |    Connected to MQTT broker successfully.");

                // Subscribe to a topic
                await ConnectToTopic(topicObjet);

                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    string payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.ToArray());


                    Debug.WriteLine($"MQTT  |    {payload}");

                    return Task.CompletedTask;
                };


                await SendMessageToO(value, topicObjet);


                //await StopMqtt();
            }
            else
            {
                Debug.WriteLine($"Local |    Failed to connect to MQTT broker: {connectResult.ResultCode}");
            }
        }
        
        /*public static async Task AddObject(string objTopic)
        {
            if (mqttClient != null && mqttClient.IsConnected)
            {
                await SendMessageToTopic(Request.Request.getRequest_ObjectAdd(objTopic), topicApp);
            }
        }*/
        
        /*public static async Task Main()
        {
            if (mqttClient != null && mqttClient.IsConnected)
            {
                await SendMessageToTopic(Request.Request.getRequest_HubConnection());
            }
        }*/
        
        
        
        public static async Task SendMessageToO(string message, string t)
        {
            if (mqttClient != null)
            {
                var msg = new MqttApplicationMessageBuilder()
                    .WithTopic(t)
                    .WithPayload(message)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();
                await mqttClient.PublishAsync(msg);
            }
            else
            {
                Debug.WriteLine("Can't send message");
            }
        }
        
        
        
        static async Task ConnectToTopic(string topic)
        {
            if (mqttClient is not { IsConnected: true }) return;
            await mqttClient.SubscribeAsync(topic);
        }
        public static async Task SendMessageToTopic(string message)
        {
            if (mqttClient != null)
            {
                var msg = new MqttApplicationMessageBuilder()
                    .WithTopic(topicApp)
                    .WithPayload(message)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();
                await mqttClient.PublishAsync(msg);
            }
            else
            {
                Debug.WriteLine("Can't send message");
            }
        }

        static async Task StopMqtt()
        {
            Debug.WriteLine("MQTT Stop");
            await mqttClient.UnsubscribeAsync(topicApp);
            await mqttClient.DisconnectAsync();
        }


    }


    



    public class ConnectionRequestState : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string name = "";
        private int state = 0;

        public int ConnectionState
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged("ConnectionState");
            }
        }

        protected void OnPropertyChanged(string state)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(state));
            }
        }
    }
}
