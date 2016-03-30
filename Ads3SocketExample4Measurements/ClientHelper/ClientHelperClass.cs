using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientHelper
{
    public class COMMANDS
    {
        public const string TIME = "TIME";
        public const string ACK = "ACK";
        public const string QUIT = "QUIT";
        public const string NUMBER_OF_REQUESTS = "NUMBER_OF_REQUESTS";
    }

    public class ClientHelperClass
    {
        private TcpClient tcpClient;
        private NetworkStream ns;
        private StreamWriter sw;
        private StreamReader sr;

        public ClientHelperClass(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }

        public bool IsOpen()
        {
            return ns != null && sw != null && sr != null;
        }

        public void Open()
        {
            // avataan streamit
            ns = tcpClient.GetStream();
            sw = new StreamWriter(ns);
            sr = new StreamReader(ns);

            sw.AutoFlush = true;
        }

        public bool IsDataAvailable()
        {
            return ns.DataAvailable;
        }

        public void Write(string s)
        {
            sw.WriteLine(s);
        }

        public void WriteCommand(COMMANDS cmd)
        {
            sw.WriteLine(cmd.ToString());
        }

        public string Read()
        {
            return sr.ReadLine();
        }

        public void Close()
        {
            sr.Close();
            sw.Close();
            ns.Close();
            tcpClient.Close();
        }
    }
}
