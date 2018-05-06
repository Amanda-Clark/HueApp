using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApp.Models
{
    public class LightState
    {
        public State HueState { get; set; }

        public string LightType { get; set; }

        public string LampName { get; set; }

        public string ModelId { get; set; }

        public string ManufacturerName { get; set; }

        public string UniqueId { get; set; }

        public string SoftwareVersion { get; set; }

        public static LightState Parse(dynamic d)
        {
            var instance = new LightState();
            instance.HueState =State.Parse(d["state"]);
            instance.LightType = d["type"];
            instance.LampName = d["name"];
            instance.ModelId = d["modelid"];
            instance.SoftwareVersion = (d["swversion"]);
            return instance;
        }


    }
}
