using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myApp
{
    public partial class ListSeries : Form
    {

        OleDbConnection Aconnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\kerem\\Documents\\tvapp.accdb");
        public ListSeries()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string sql = "SELECT * FROM Series";
            using (OleDbCommand command = new OleDbCommand(sql, Aconnection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataTable.Columns[3].ColumnName = "Still Continue";
                dataTable.AcceptChanges();
                dataTable.Columns.Add("Still Continue Text", typeof(string), "IIF([Still Continue], 'Yes', 'No')");
                dataGridView1.DataSource = dataTable;
                dataGridView1.Columns.Remove("image");
                dataGridView1.Columns.Remove("id");
                dataGridView1.Columns.Remove("Still Continue");
                dataGridView1.Columns[0].HeaderText = "Title";
                dataGridView1.Columns[1].HeaderText = "Genre";
                dataGridView1.Columns[2].HeaderText = "Release Year";
                dataGridView1.Columns[3].HeaderText = "Number of Season";
                dataGridView1.Columns[3].Width = 120;
                dataGridView1.Columns[4].HeaderText = "Still Continue";
                
            }

        }
        
        private void button3_Click(object sender, DataGridViewCellFormattingEventArgs e)
        {
           
        }


        private void ListSeries_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'dataSet1.Series' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.seriesTableAdapter.Fill(this.dataSet1.Series);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string tempTitle = textBox1.Text;
                Aconnection.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM Series WHERE serieName ='" + tempTitle + "'", Aconnection);
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    cmd = new OleDbCommand("DELETE FROM Series WHERE serieName ='" + tempTitle + "'", Aconnection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data Deleted", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No match data for entry: " + tempTitle, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Aconnection.Close();
                dataGridView1.Refresh();
            }
            else
            {
                MessageBox.Show("Can not leave empty", "Fill the form", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
