using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClientHelper;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // avataan yhteys palvelimelle (tehdään yhteyspyyntö)
            TcpClient client = new TcpClient("localhost", 8221);

            ClientHelperClass clientHelper = new ClientHelperClass(client);
            clientHelper.Open();

            int i = 0;
            while (i < 1000)
            {
                Console.WriteLine(clientHelper.Read());
                i++;
            }

            // kysytään lopetetaan
            clientHelper.Write(COMMANDS.QUIT);
            // luetaan vastaus
            Console.WriteLine(clientHelper.Read());

            // suljetaan streamit
            clientHelper.Close();

            Console.Read();
        }
    }
}
