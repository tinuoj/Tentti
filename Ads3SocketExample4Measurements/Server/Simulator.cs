using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MeasurementLibrary;

namespace Server
{


    public class Simulator : IDevice
    {
        private Random gen = new Random();
        double time = 1;

        public void Connect()
        {
        }

        public Measurements GetMeasurement()
        {

            time += 1.0;
            Measurements measurement = new Measurements();
            measurement.measurement1 = 10 + 5 * Math.Cos(time / 5.0) +
                gen.NextDouble();
            measurement.measurement2 = 10 * Math.Sin(time) +
                gen.NextDouble() * 2;
            measurement.measurement2 = 8 * Math.Sin(time/3.0) +
                gen.NextDouble() * 2;


            return measurement;
        }

        public void SendStart()
        {
        }

        public void SendStop()
        {
        }
    }


}
