using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementLibrary
{
    public class Measurements
    {
        public DateTime Time { get; set; }
        public UInt32 timeHi { get; set; }
        public UInt32 timeLo { get; set; }
        public double measurement1 { get; set; }
        public double measurement2 { get; set; }
        public double measurement3 { get; set; }
        public Int16 counter { get; set; }
        public Int16 arrayVal { get; set; }

        public Measurements() { }

        public Measurements(string s)
        {
            string[] p = s.Split();
        }


        public override string ToString()
        {
            return timeHi+" " +timeLo * 1.0e-7 + " " + measurement1 + " "
                + measurement2 + " " + measurement3 + " " +counter;
        } 

    }
}
