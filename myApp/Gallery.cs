using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myApp
{
    public partial class Gallery : Form
    {
        private PictureBox pic;
        OleDbCommand cm;
        OleDbDataReader dr;

        OleDbConnection Aconnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\kerem\\Documents\\tvapp.accdb");

        public Gallery()
        {
            InitializeComponent();
        }

        private void Gallery_Load(object sender, EventArgs e)
        {
            GetData();
        }

        
        private void GetData()
        {
            //SELECT TOP 5 image FROM Series ORDER BY id DESC
            Aconnection.Open();
            OleDbCommand cm = new OleDbCommand("SELECT image FROM Series ORDER BY id DESC", Aconnection);
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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
