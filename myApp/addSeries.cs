using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace myApp
{
    public partial class addSeries : Form
    {
        public addSeries()
        {
            InitializeComponent();
        }

        OleDbConnection Aconnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\kerem\\Documents\\tvapp.accdb");

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                string myPictures = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                ofd.Filter = "";
                ofd.FileName = myPictures;
                ofd.Title = "Choose an image";
                ofd.AddExtension = true;
                ofd.FilterIndex = 0;
                ofd.Multiselect = false;
                ofd.ValidateNames = true;
                ofd.InitialDirectory = myPictures;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.pictureBox1.Image = Image.FromFile(ofd.FileName);
                }
                else
                {
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Error : Something went wrong", "Choose image", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            double parsedValue;

            if (!double.TryParse(textBox2.Text, out parsedValue))
            {
                textBox2.Text = "";
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            double parsedValue;

            if (!double.TryParse(textBox3.Text, out parsedValue))
            {
                textBox3.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.pictureBox1.Image != null)
            {
                try
                {
                    this.pictureBox1.Image = null;
                }
                catch
                {

                    //
                }
            }
            else
            {
                MessageBox.Show("Error : There is no image to clear", "Choose an image", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Aconnection.Open();
            OleDbCommand cmd = new OleDbCommand();
            MemoryStream ms;
            cmd.Connection = Aconnection;
            cmd.CommandText = "INSERT INTO [Series] ([serieName], [genre], [releaseYear], [isContinue], [season], [image]) VALUES (@name,@genre,@year,@continue,@season,@image)";
            cmd.Parameters.AddWithValue("@serieName", textBox1.Text);
            cmd.Parameters.AddWithValue("@genre", comboBox1.Text);
            cmd.Parameters.AddWithValue("@releaseYear", textBox2.Text);
            cmd.Parameters.AddWithValue("@isContinue", checkBox1.Checked);
            cmd.Parameters.AddWithValue("@season", textBox3.Text);
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Can not leave empty", "Fill the form", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Aconnection.Close();
                return;
            }
            if (pictureBox1.Image != null)
            {
                ms = new MemoryStream();
                pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                byte[] photo_array = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(photo_array, 0, photo_array.Length);
                cmd.Parameters.AddWithValue("@image", photo_array);

            }
            else
            {
                MessageBox.Show("You forgot to choose an image", "Choose image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Aconnection.Close();
                return;
            }


            cmd.ExecuteNonQuery();
            Aconnection.Close();

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            checkBox1.Checked = false;
            pictureBox1.Image = null;
        }
    }
}
