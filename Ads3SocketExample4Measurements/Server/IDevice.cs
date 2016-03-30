using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeasurementLibrary;

namespace Server
{
    public interface IDevice
    {
        void Connect();
        Measurements GetMeasurement();
        void SendStart();
        void SendStop();
    }
}
