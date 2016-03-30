using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MeasurementLibrary;
using ClientHelper;
using System.Net.Sockets;

// 1.Lisää tämä projekti SocketExample4-solutioniin

namespace ListViewTesti1
{

    // 2. Tämä Measuremenst-luokka pois. Käytäkirjastossa olevaa määrittelyä


    // delegate is needed
    public delegate void OutputMessage(Measurements m);
    public class WorkerThread
    {
        private volatile bool bContinue = true;
        private OutputMessage outMsg;
        private Form form;
        // constructor
        public WorkerThread(Form form, OutputMessage outMsg)
        {
            // We need to know the form which contains the textbox to be updated
            this.form = form;
            // The method, which updates the textbox in UI thread is given through delegate OutputMessage
            this.outMsg = outMsg;
        }

        // run-metodi
        public void ThreadProc()
        {
            int i = 0;
            // 3. avaa Socket-yhteys (kirjasto ClientHelper)
            // Tee myös TcpClient-olio, jossa annetaan palvelimen IP-osoite ja palvelun portti
            // Ota mallia komentorivi-clientistä
            TcpClient client = new TcpClient("localhost", 8221);

            ClientHelperClass clientHelper = new ClientHelperClass(client);
            clientHelper.Open();

            while (bContinue)
            {
                // generoidaan mittauksia
                // 4. Tilalle Socketin kautta lukeminen
                string json = clientHelper.Read();

                MeasurementLibrary.Message message =
                    MeasurementLibrary.Message.CreateMessageFromJson(json);
                Measurements m = message.Data;

                // "call" the UI to update the textbox indirectly
                form.BeginInvoke(outMsg, new object[] { m });
                Thread.Sleep(1000);
                i++;
            }
            // 5. Socket-yhteyden sulkeminen
            clientHelper.Write(COMMANDS.QUIT);
            // jäädään odottamaan ACKia
            while (true)
            {
                string command = clientHelper.Read();
                if (command == COMMANDS.ACK)
                    break;
            }

            clientHelper.Close();
        }

        public void StopThread()
        {
            bContinue = false;
        }
    }
}
