using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Net;

namespace PingPlotter
{

    public partial class Form1 : Form
    {
        //System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        public System.Windows.Forms.Timer sw = new System.Windows.Forms.Timer();
        public float elapsed = 0.0f;
        public bool download = false;
        public int counter;
        public long cumulativeRTT;
        public long averageRTT;
        public string IPAddress = "8.8.8.8";
        public long RTT;
        double maxYValue = 0;

        System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series
        {
            Name = "Ping",
            Color = System.Drawing.Color.Green,
            IsVisibleInLegend = false,
            IsXValueIndexed = true,
            ChartType = SeriesChartType.Line
        };

        System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series
        {
            Name = "Download Speed",
            Color = System.Drawing.Color.Blue,
            IsVisibleInLegend = false,
            IsXValueIndexed = true,
            ChartType = SeriesChartType.Line
        };


        public Form1()
        {
            InitializeComponent();

            //Button
            button1.Text = "Download Speed";

            sw.Tick += sw_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Series.Clear();         

            chart1.Series.Add(series1);            
           
            sw.Start();
            chart1.Invalidate();
        }

        private void sw_Tick(object sender, EventArgs e)
        {
            elapsed += sw.Interval;
            //long RTT = 0;
            //var thread = new Thread(() =>
            //{
            //    Ping pingSender = new Ping();
            //    PingReply pingReceiver = pingSender.Send(IPAddress);
            //    RTT = pingReceiver.RoundtripTime;
            //});
            //thread.IsBackground = true;
            //thread.Start();
            if (!download)
            {
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            else
            {
                chart1.Series[0].Points.AddXY((elapsed / 1000) - 0.1, CheckInternetSpeed());
            }

        }

        public double CheckInternetSpeed()
        {
            // Create Object Of WebClient
            System.Net.WebClient wc = new System.Net.WebClient();

            //DateTime Variable To Store Download Start Time.
            DateTime dt1 = DateTime.Now;

            //Number Of Bytes Downloaded Are Stored In ‘data’
            byte[] data = wc.DownloadData("http://google.com");

            //DateTime Variable To Store Download End Time.
            DateTime dt2 = DateTime.Now;

            //To Calculate Speed in Kb Divide Value Of data by 1024 And Then by End Time Subtract Start Time To Know Download Per Second.
            return Math.Round((data.Length / 1024) / (dt2 - dt1).TotalSeconds, 2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearChart(chart1);
            if (!download)
            {              
                chart1.Series.Add(series2);
                download = true;
                button1.Text = "Ping";
                button1.Update();
                chart1.Titles.Clear();
                chart1.Titles.Add("Download Speed (kb/s) / Time (s)");
                label1.Hide();
                button2.Hide();
                button3.Hide();
                button4.Hide();
                label2.Hide();
            }
            else
            {
                chart1.Series.Add(series1);
                download = false;
                button1.Text = "Download Speed";
                button1.Update();
                chart1.Titles.Clear();
                chart1.Titles.Add(" Ping (ms) / Time (s)");
                label1.Show();
                button2.Show();
                button3.Show();
                button4.Show();
                label2.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearChart(chart1);
            IPAddress = "1.1.1.1";
            chart1.Series.Add(series1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearChart(chart1);
            IPAddress = "8.8.8.8";
            chart1.Series.Add(series1);
        }
        private void ClearChart(System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {
            elapsed = 0;
            averageRTT = 0;             
            foreach (var series in chart.Series)
            {
                series.Points.Clear();
            }
            chart.Series.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearChart(chart1);           
            chart1.Series.Add(series1);
            download = false;
            button1.Text = "Download Speed";
            button1.Update();
            chart1.Titles.Clear();
            chart1.Titles.Add(" Ping (ms) / Time (s)");
            label1.Show();
            button2.Show();
            button3.Show();
            label2.Show();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Ping pingSender = new Ping();
            PingReply pingReceiver = pingSender.Send(IPAddress);
            RTT = pingReceiver.RoundtripTime;
            e.Result = RTT;
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateGraph();
        }

        private void UpdateGraph()
        {
            chart1.Series[0].Points.AddXY((elapsed / 1000) - 0.1, RTT);
            cumulativeRTT += RTT;
            counter++;
            averageRTT = cumulativeRTT / counter;
            if(label1 != null) { label1.Text = "Average ping: " + averageRTT.ToString() + "ms"; }
            label1.Text = "Average ping: " + averageRTT.ToString() + "ms";
            //updateMaxY();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            maxYValue = trackBar1.Value;
            updateMaxY();
        }

        private void updateMaxY()
        {
            DataPoint maxYPoint;
            if (maxYValue == 0)
            {
                //maxYPoint = chart1.Series[0].Points.FindMaxByValue("Y1", 0);
                //double[] arrayY = maxYPoint.YValues;
                //maxYValue = arrayY[0];
                chart1.ChartAreas[0].AxisY.Maximum = Double.NaN;
            }
            else
            {
                chart1.ChartAreas[0].AxisY.Maximum = maxYValue;
            }

        }
    }
}
