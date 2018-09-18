using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApplication1
{
    public partial class Form7_stat : Form
    {
        OleDbConnection connection = new OleDbConnection();
        public Form7_stat()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=AnimeBase.accdb;
Persist Security Info = False; ";
        }

        private void Form7_stat_Load(object sender, EventArgs e)
        {
            Stat();
        }
        private void Stat()
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                var da = new OleDbDataAdapter(command.CommandText, connection);
                command.CommandText = "SELECT name_an, tip_an FROM Anime_info ";
                da = new OleDbDataAdapter(command.CommandText, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dt.Columns.Add("Смотрят");
                dt.Columns.Add("Смотрели");
                dt.Columns.Add("Будут");
                dataGridView1.DataSource = dt;
                
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    int see = 0;
                    //смотрят сейчас
                    command.CommandText = "SELECT login_us FROM User_Data WHERE User_Data.spisSee_us.Value Like '" + dataGridView1[0, i].Value.ToString() + "' ";
                    
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        see++;
                    }
                    reader.Close();
                    dataGridView1[2, i].Value = see.ToString();
                    
                    //смотрели
                    command.CommandText = "SELECT login_us FROM User_Data WHERE User_Data.spisSmotrel_us.Value Like '" + dataGridView1[0, i].Value.ToString() + "' ";
                    see = 0;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        see++;
                    }
                    reader.Close();
                    dataGridView1[3, i].Value = see.ToString();
                    
                    //будут смотреть
                    command.CommandText = "SELECT login_us FROM User_Data WHERE User_Data.spisWait_us.Value Like '" + dataGridView1[0, i].Value.ToString() + "' ";
                    see = 0;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        see++;
                    }
                    reader.Close();
                    dataGridView1[4, i].Value = see.ToString();

                }


                connection.Close();

                
                dataGridView1.Columns[0].HeaderText = "Название аниме";
                dataGridView1.Columns[1].HeaderText = "Тип";
                
                dataGridView1.Columns[0].Width = 555;
                dataGridView1.Columns[1].Width = 100;

                dataGridView1.Columns[2].Width = 60;
                dataGridView1.Columns[3].Width = 60;
                dataGridView1.Columns[4].Width = 60;


            }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL " + ex);
                connection.Close();
            }
        }
    }
}
