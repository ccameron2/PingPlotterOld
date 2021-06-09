using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace PingPlotter
{

    public partial class Form1 : Form
    {
        System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        public Timer sw = new Timer();
        public float elapsed = 0.0f;
        public Form1()
        {
            InitializeComponent();
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            SuspendLayout();
            //
            // chart1
            //
            chartArea1.Name = "Ping";
            chart1.ChartAreas.Add(chartArea1);
            chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Ping (ms)";
            chart1.Legends.Add(legend1);
            chart1.Location = new System.Drawing.Point(0, 50);
            chart1.Name = "PingChart";
            //this.chart1.Size = new System.Drawing.Size(284, 212);
            chart1.TabIndex = 0;
            chart1.Text = "Ping";
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            chart1.Titles.Add("Ping (ms) / Time (ms)");
            //
            // Form1
            //
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 600);
            Controls.Add(this.chart1);
            Name = "LineGraph";
            Text = "Ping Monitor";
            Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ResumeLayout(false);

            sw.Tick += sw_Tick;
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            if (sw.Enabled)
            {
                sw.Stop();
            }
            else
            {
                sw.Start();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Series1",
                Color = System.Drawing.Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };

            this.chart1.Series.Add(series1);          
            sw.Start();

            chart1.Invalidate();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
        private void sw_Tick(object Sender, EventArgs e)
        {
            elapsed += sw.Interval;
            Ping pingSender = new Ping();
            PingReply pingReceiver = pingSender.Send("8.8.8.8");
            chart1.Series[0].Points.AddXY(elapsed, pingReceiver.RoundtripTime);
        }

    }
}
