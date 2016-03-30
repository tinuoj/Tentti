using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(ipAddress, 8221);

            tcpListener.Start();

            // luodaan uusi kaikkia asiakkaita palveleva luokka
            ClientThread ct = new ClientThread();
            // luodaan säie
            Thread t = new Thread(ct.ServeClient);
            // käynnistetään säie
            t.Start();

            while (true)
            {
                // odotetaan uutta asiakasta
                TcpClient tcpClient = tcpListener.AcceptTcpClient();

                // lisätään uusi asiakas listaan
                ct.AddClient(tcpClient);                                
            }
        }
    }
}
