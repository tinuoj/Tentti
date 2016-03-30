using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinCAT.Ads; // Otetaan ads kirjasto mukaan
using System.Threading;
using System.IO;
using MeasurementLibrary;

namespace Server
{
    public class AdsClient : IDevice
    {
        // viittaus adsClient-olioon
        private TcAdsClient adsClient;

        // kahvat TwinCAT-muuttujiin
        private int startHandle;
        private int stopHandle;
        private int measurementDataHandle;

        public void Connect()
        {
            // luodaan AdsClient-olio yhteyttä varten
            adsClient = new TcAdsClient();
            // otetaan yhteys TwinCAT PLC:hen portin 851 kautta
            adsClient.Connect(851);

            // kytketään "kahvat" TwinCAT-muuttujiin. 
            startHandle = adsClient.CreateVariableHandle("MAIN.start");
            stopHandle = adsClient.CreateVariableHandle("MAIN.reset");
            measurementDataHandle = adsClient.CreateVariableHandle("MAIN.measurementData");
        }

        public Measurements GetMeasurement()
        {
            // avataan Ads-stream (luettava byte-määrä pitänee antaa parametrina
            AdsStream adsStream = new AdsStream(24);
            // luetaan bytet Ads:n kautta
            adsClient.Read(measurementDataHandle, adsStream);

            // avataan binary stream lukemista varten
            BinaryReader binReader = new BinaryReader(adsStream);

            adsStream.Position = 0;

            Measurements meas = new Measurements();

            meas.timeHi = binReader.ReadUInt32();
            meas.timeLo = binReader.ReadUInt32();
            meas.measurement1 = binReader.ReadSingle();
            meas.measurement2 = binReader.ReadSingle();
            meas.measurement3 = binReader.ReadSingle();
            meas.counter = binReader.ReadInt16();
            meas.arrayVal = binReader.ReadInt16();

            return meas;
        }

        public void SendStart()
        {
                        // Laitetaan Start päälle
            adsClient.WriteAny(startHandle, true);
            // Odotetaan 0.5 s
            System.Threading.Thread.Sleep(500);
            // laitetaan Start pois päältä
            adsClient.WriteAny(startHandle, false);
            System.Threading.Thread.Sleep(500);
        }

        public void SendStop()
        {
            adsClient.WriteAny(stopHandle, true);
            System.Threading.Thread.Sleep(500);
            adsClient.WriteAny(stopHandle, false);
            System.Threading.Thread.Sleep(500);

            adsClient.Dispose();
        }
    }
}
