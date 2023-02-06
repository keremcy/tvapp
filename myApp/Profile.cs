using myApp.DataSet1TableAdapters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace myApp
{
    public partial class Profile : Form
    {
        
        public Profile()
        {
            InitializeComponent();
        }

        OleDbConnection Aconnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\kerem\\Documents\\tvapp.accdb");
        private PictureBox pic;
        OleDbCommand cm;
        OleDbDataReader dr;
        

        private void Profile_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'tvappDataSet.Series' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.seriesTableAdapter.Fill(this.tvappDataSet.Series);
            GetData();
            getTvTime();
            seasonSum();
        }

        private void GetData()
        {

            Aconnection.Open();
            OleDbCommand cm = new OleDbCommand("SELECT TOP 5 image FROM Series ORDER BY id DESC", Aconnection);
            cm.Connection = Aconnection;
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                long len = dr.GetBytes(0, 0, null, 0, 0);
                byte[] array = new byte[System.Convert.ToInt32(len) + 1];
                dr.GetBytes(0, 0, array, 0, System.Convert.ToInt32(len));
                pic = new PictureBox();
                pic.Width = 134;
                pic.Height = 200;
                pic.BackgroundImageLayout = ImageLayout.Stretch;

                MemoryStream ms = new MemoryStream(array);
                Bitmap bitmap = new Bitmap(ms);
                pic.BackgroundImage = bitmap;

                flowLayoutPanel1.Controls.Add(pic);
            }
            dr.Close();
            Aconnection.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.DataSource = null;
            chart1.Series["Series1"].Points.Clear();

            OleDbConnection conn = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();

            conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\kerem\\Documents\\tvapp.accdb";
            conn.Open();

            cmd.CommandText = "SELECT genre, COUNT([serieName]) AS adam FROM Series GROUP BY genre";
            cmd.Connection = conn;

            OleDbDataReader reader;
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string age = reader.GetString(0);
                int num = reader.GetInt32(1);
                chart1.Series["Series1"].Points.AddXY(age, num);
            }
            reader.Close();
            cmd.Dispose();
            conn.Close();
            
        }



        private void getTvTime()
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\kerem\\Documents\\tvapp.accdb";
            string queryString = "SELECT COUNT(*) FROM Series";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(queryString, connection);
                connection.Open();
                int seriesCount = (int)command.ExecuteScalar();
                label9.Text = seriesCount.ToString();
                
            }
        }
        private void seasonSum()
        {
            OleDbConnection conn = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataReader reader;

            conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\kerem\\Documents\\tvapp.accdb";
            conn.Open();

            cmd.CommandText = "SELECT season FROM Series";
            cmd.Connection = conn;

            reader = cmd.ExecuteReader();

            int season = 0;

            while (reader.Read())
            {
                season += reader.GetInt32(0);
            }
            int episodes = season * 13;
            int elapsed = episodes * 40;     
            int months = elapsed / 43800;
            elapsed -= months * 43800;
            int days = elapsed / 1440;
            elapsed -= days * 1440;
            int hours = elapsed / 60;
            string output = string.Format("{0:N1}k", (double)episodes / 1000);

            reader.Close();
            cmd.Dispose();
            conn.Close();
            label8.Text = output;
            label12.Text = months.ToString();
            label14.Text = days.ToString();
            label11.Text = hours.ToString();


        }


        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
