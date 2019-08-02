using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
   public class BarChartValue
    {

        public string Label { get; set; }
        public float value { get; set; }
        public String Color { get; set; }

        public BarChartValue(string label, float _value, String _c)
        {
            Label = label;//produsul
            value = _value;//nr de biddersi pe produs
            Color = _c;
        }
    }
}
