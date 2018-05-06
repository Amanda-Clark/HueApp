using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApp.Models
{
    public class State
    {
        public bool On { get; set; }

        public int Bri { get; set; }

        public long Hue { get; set; }

        public int Sat { get; set; }

        public string Effect { get; set; }

        public decimal [] Xy { get; set; }

        public string Ct { get; set; }

        public string Alert { get; set; }

        public string Colormode { get; set; }

        public bool Reachable { get; set; }

        public static State Parse(dynamic d)
        {
            var instance = new State();

            instance.On = d["on"];
            instance.Bri = d["bri"];
            instance.Hue = d["hue"];
            instance.Sat = d["sat"];
            instance.Xy = new decimal[] { d["xy"][0], d["xy"][1] };
            instance.Alert = d["alert"];
            instance.Effect = d["effect"];
            instance.Colormode = d["colormode"];
            instance.Reachable = d["reachable"];
            return instance;
        }
    }
}
