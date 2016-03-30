using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ClientHelper;
using MeasurementLibrary;

namespace Server
{
    class ClientThread
    {
        //private ClientHelperClass clientHelper;
        private static int numberOfRequests = 0;

        // lista asiakkaita varten
        List<ClientHelperClass> clients = new List<ClientHelperClass>();

        // lukitusolio
        private static object objectToLock = new object();

        private volatile bool stopped = false;

        public ClientThread()
        {
            
        }

        public void Stop()
        {
            stopped = true;
        }

        // uuden asiakkaan lisääminen listaan
        public void AddClient(TcpClient client)
        {
            lock (objectToLock)
            {
                ClientHelperClass clientHelper = new ClientHelperClass(client);
                // lisätään asiakas listaan
                clients.Add(clientHelper);
            }
        }

        // ajetaan omassa säikeessä
        public void ServeClient()
        {
            Interlocked.Increment(ref numberOfRequests);

            // luodaan simulaattori tai ADS-yhteys
            IDevice device;
            if (false)
            {
                device = new Simulator();
            }
            else
            {
                device = new AdsClient();
            }
            device.Connect();
            device.SendStart();


            stopped = false;

            while (!stopped)
            {
                // luetaan uudet mittaukset
                Measurements measurements = device.GetMeasurement();
                Console.WriteLine(measurements.ToString());

                lock (objectToLock)
                {
                    // lista tuhottavista
                    List<ClientHelperClass> objectsToBeRemoved = new List<ClientHelperClass>();

                    // lähetetään data kaikille asiakkaille
                    foreach (ClientHelperClass client in clients)
                    {
                        // avataan streamit asiakkaalle
                        if (!client.IsOpen())
                            client.Open();
                        
                        Message m = new Message("MEAS", measurements);
                        string json = m.ToJSON();
                        Console.WriteLine(json);
                        client.Write(json);

                        // onko asiakkaalta tullut käsky
                        if (client.IsDataAvailable())
                        {
                            string command = client.Read();
                            if (command == COMMANDS.QUIT)
                            {
                                objectsToBeRemoved.Add(client);
                                client.Write(COMMANDS.ACK);
                                client.Close();
                            }
                        }
                    } // foreach

                    foreach (ClientHelperClass client in objectsToBeRemoved)
                    { 
                        // haetaan poistettava olio alkuperäisestä listasta
                        // ja tuhotaan se
                        clients.Remove(client);
                    }
                } // lock (objectToLock)

                Thread.Sleep(1000);
            } // end while
            device.SendStop();

            // asiakkaita palveleva säie loppumassa
            // suljetaan streamit
            lock (objectToLock)
            {
                foreach (ClientHelperClass client in clients)
                {
                    client.Close();
                }
            }
        }
    }
}
