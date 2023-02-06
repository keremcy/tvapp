using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace myApp
{
    public partial class Homie : Form
    {
        public Homie()
        {
            InitializeComponent();

        }

        public void rows()
        {
            Chart chart = new Chart();

            chart.ChartAreas.Add(new ChartArea());
            chart.Series.Add(new Series("PieSeries"));
            chart.Series["PieSeries"].ChartType = SeriesChartType.Pie;

            OleDbConnection conn = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();

            conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\tvapp.accdb";
            conn.Open();

            cmd.CommandText = "SELECT genre, COUNT([serieName]) AS adam FROM Series GROUP BY genre";
            cmd.Connection = conn;

            OleDbDataReader reader;
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string age = reader.GetString(0);
                int num = reader.GetInt32(1);
                chart.Series["PieSeries"].Points.AddXY(age, num);
            }
            // Customize the appearance of the pie slices
            chart.Series["PieSeries"]["PieLabelStyle"] = "Disabled";
            chart.Legends.Add(new Legend("PieLegend"));
            chart.Legends["PieLegend"].Enabled = true;

            chart.Parent = this;
            //
            reader.Close();
            cmd.Dispose();
            conn.Close();
        }
        private void Homie_Load(object sender, EventArgs e)
        {
           
        }
    }
}
