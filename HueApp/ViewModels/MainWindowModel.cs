
using MyToolkit.Mvvm;
using System.Net;
using HueApp.Models;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Collections.Concurrent;

namespace HueApp.ViewModels
{
    /// <summary>The main window view model. </summary>
    public class MainWindowModel: ViewModelBase
    {
        public ConcurrentDictionary<string, LightState> Lights { get; set; }
        /// <summary>Initializes a new instance of the <see cref="MainWindowModel"/> class. </summary>
        public MainWindowModel()
        {
            
        }
       
       
        public override void Initialize()
        {
            // TODO: Add your view model initialization logic here. 
            base.Initialize();
        }

        public ConcurrentDictionary<string, LightState> GetLightStates()
        {
            List<LightState> response = new List<LightState>();
            string ipAddress;
           
            GetUrl(out ipAddress);
            using (var client = new WebClient())
            {
                var responseString =  client.DownloadString("http://"+ipAddress+"/api/"+""+"/lights");
                var jss = new JavaScriptSerializer();
                var d = jss.Deserialize<dynamic>(responseString);
              
                Lights = new ConcurrentDictionary<string, LightState>();
                foreach (var light in d)
                {
                    Lights.TryAdd(light.Key, LightState.Parse(light.Value));
                }
                 
            }
            return Lights;
        }

        public void GetUrl(out string ipAddress)
        {
            ipAddress = "";
            
            using (var client = new WebClient())
            {
                List<Dictionary<string, string>> hueData = new List<Dictionary<string, string>>();
                var responseString = client.DownloadString("https://www.meethue.com/api/nupnp");
                var jss = new JavaScriptSerializer();
                hueData = jss.Deserialize<List<Dictionary<string, string>>>(responseString);
                ipAddress = hueData[0]["internalipaddress"];

            }
           
        }

        public bool SetLightState(string chgValue, string selectedLight, string lightSetting)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string ipAddress;
                    GetUrl(out ipAddress);
                    lightSetting = lightSetting.ToLower();
                    string body = "{"+"\""+lightSetting+"\""+":"+chgValue+"}";
                    try
                    {
                        var briResult = client.UploadString("http://" + ipAddress + "/api/" + "/" + "lights/" + selectedLight + "/state", "PUT", body);
                    }
                    catch(System.Exception e)
                    {

                    }

                }
                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }

        }

        public bool TurnLightOn(string lightSelected)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string ipAddress;
                    GetUrl(out ipAddress);
                    string status = "true";
                    string setting = "on";
                    string body = "{" + "\"" + setting + "\"" + ":" + status + "}";
                    try
                    {
                        var briResult = client.UploadString("http://" + ipAddress + "/api/" + "/" + "lights/" + lightSelected + "/state", "PUT", body);
                    }
                    catch (System.Exception e)
                    {

                    }

                }
                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
           
        }

        public bool TurnLightOff(string lightSelected)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string ipAddress;
                    GetUrl(out ipAddress);

                    string status = "false";
                    string setting = "on";
                    string body = "{" + "\"" + setting + "\"" + ":" + status + "}";
                    try
                    {
                        var briResult = client.UploadString("http://" + ipAddress + "/api/" + "/" + "lights/" + lightSelected + "/state", "PUT", body);
                    }
                    catch (System.Exception e)
                    {

                    }

                }
                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
            
        }

    }
}
