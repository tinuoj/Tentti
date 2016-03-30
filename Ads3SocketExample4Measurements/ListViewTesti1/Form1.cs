using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MeasurementLibrary;

namespace ListViewTesti1
{
    public partial class Form1 : Form
    {
        // member variables
        // background thread
        private Thread thread;
        // background thread information
        private WorkerThread worker;

        public Form1()
        {
            InitializeComponent();
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
        }

        private void buttonLisaa_Click(object sender, EventArgs e)
        {
            int rivi = listView1.Items.Count;

            // new row and first colomn
            listView1.Items.Add("terve" + rivi);            

            // second and third columns
            listView1.Items[rivi].SubItems.Add("moi" + rivi);
            listView1.Items[rivi].SubItems.Add("hei" + rivi);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            // create an object of the delegate. Give the UI update method as a parameter
            OutputMessage om = new OutputMessage(updateMeasurements);
            worker = new WorkerThread(this, om);
            thread = new Thread(new ThreadStart(worker.ThreadProc));
            thread.Name = "Timer";
            thread.Start();
            buttonStop.Enabled = true;
        }

        private void updateMeasurements(Measurements m)
        {   // the seconds are updated here
            // the worker thread must not call this directly
            //Debug.Assert(this.InvokeRequired == false);
            
            // mittaukset ListView:n
            int rivi = listView1.Items.Count;

            // new row and first colomn
            listView1.Items.Insert(0, m.Time.ToString());

            // second and third columns
            listView1.Items[0].SubItems.Add(m.measurement1.ToString());
            listView1.Items[0].SubItems.Add(m.measurement2.ToString());            
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            buttonStop.Enabled = false;
            worker.StopThread();
            // wait the worker thread to stop
            thread.Join();
            buttonStart.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (worker != null)
            {
                if (thread.IsAlive)
                {
                    worker.StopThread();
                    thread.Join();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}