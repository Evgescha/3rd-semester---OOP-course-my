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
    public partial class Form6_RedBD : Form
    {
        OleDbConnection connection = new OleDbConnection();
        string comandText = "";
        public Form6_RedBD()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=AnimeBase.accdb;
Persist Security Info = False; ";
        }

        private void Form6_RedBD_Load(object sender, EventArgs e)
        {
            LoadTable();
        }
        private void LoadTable()
        {
            try
            {
                //списки жанров

                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT ganr_ga FROM All_Janre";
                OleDbDataAdapter da = new OleDbDataAdapter(command.CommandText, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                //списки типов

                command.CommandText = "SELECT tip_ti FROM Tip_Anime";
                da = new OleDbDataAdapter(command.CommandText, connection);
                dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;

                //списки озвучка

                command.CommandText = "SELECT oswych_osw FROM Oswychka_Anime";
                da = new OleDbDataAdapter(command.CommandText, connection);
                dt = new DataTable();
                da.Fill(dt);
                dataGridView3.DataSource = dt;

                //списки Студия

                command.CommandText = "SELECT studi_st FROM Studi_Proisw";
                da = new OleDbDataAdapter(command.CommandText, connection);
                dt = new DataTable();
                da.Fill(dt);
                dataGridView5.DataSource = dt;

                //списки Страна

                command.CommandText = "SELECT strana_st FROM Strana_Proisw";
                da = new OleDbDataAdapter(command.CommandText, connection);
                dt = new DataTable();
                da.Fill(dt);
                dataGridView6.DataSource = dt;

                //списки год выпуска

                command.CommandText = "SELECT dataTrans_da FROM Data_Translat";
                da = new OleDbDataAdapter(command.CommandText, connection);
                dt = new DataTable();
                da.Fill(dt);
                dataGridView8.DataSource = dt;


                connection.Close();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL " + ex);
                connection.Close();
            }
        }
        //кнопка для удаления при подтверждении
        
        private void DeleteOrAddBD()
        {
            if (comandText != "")
            {
                if (MessageBox.Show("Вы уверены?", "Подтвердите операцию", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        OleDbCommand command = new OleDbCommand();
                        command.Connection = connection;
                        command.CommandText = comandText;
                        this.Name = comandText;
                        command.ExecuteNonQuery();
                        MessageBox.Show("Успешно");
                        connection.Close();
                        comandText = "";
                        LoadTable();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("FAIL " + ex);
                        connection.Close();
                    }

                }
            }
        }
        private bool Sowpad()
        {
            if (comandText != "")
            {
                    try
                    {
                        connection.Open();
                        OleDbCommand command = new OleDbCommand();
                        command.Connection = connection;
                        command.CommandText = comandText;
                        OleDbDataReader reader = command.ExecuteReader();
                        int count = 0;
                    while (reader.Read())
                    {
                        count++;
                    }
                    if(count == 0) {
                        connection.Close();
                        comandText = "";
                        return true;
                    } else {
                        connection.Close();
                        comandText = "";
                       // MessageBox.Show("Такоеуже есть");
                        return false;
                    }
                    
                }
                    catch (Exception ex)
                    {
                        MessageBox.Show("FAIL " + ex);
                        connection.Close();
                    comandText = "";
                    return false;
                }                
            }
            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            comandText = "DELETE FROM All_Janre WHERE ganr_ga = '" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + "'";
            DeleteOrAddBD();
            comandText = "";
        }

        

        private void button4_Click(object sender, EventArgs e)
        {
            comandText = "DELETE FROM Tip_Anime WHERE tip_ti = '" + dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString() + "'";
            DeleteOrAddBD();
            comandText = "";
        }
       
        private void button6_Click(object sender, EventArgs e)
        {
            comandText = "DELETE FROM Oswychka_Anime WHERE oswych_osw = '" + dataGridView3[0, dataGridView3.CurrentRow.Index].Value.ToString() + "'";
            DeleteOrAddBD();
            comandText = "";
        }
        
        private void button10_Click(object sender, EventArgs e)
        {
            comandText = "DELETE FROM Studi_Proisw WHERE studi_st = '" + dataGridView5[0, dataGridView5.CurrentRow.Index].Value.ToString() + "'";
            DeleteOrAddBD();
            comandText = "";
        }
        
        private void button12_Click(object sender, EventArgs e)
        {
            comandText = "DELETE FROM Strana_Proisw WHERE strana_st = '" + dataGridView6[0, dataGridView6.CurrentRow.Index].Value.ToString() + "'";
            DeleteOrAddBD();
            comandText = "";
        }
        
        private void button16_Click(object sender, EventArgs e)
        {
            comandText = "DELETE FROM Data_Translat WHERE dataTrans_da = " + dataGridView8[0, dataGridView8.CurrentRow.Index].Value.ToString() + "";
            DeleteOrAddBD();
            comandText = "";
        }
        //добавление 
        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                comandText = "SELECT ganr_ga FROM All_Janre WHERE ganr_ga = '" + textBox1.Text + "' ";
                if (Sowpad())
                {

                    comandText = "INSERT INTO All_Janre(ganr_ga) VALUES('" + textBox1.Text + "')";
                    DeleteOrAddBD();
                    comandText = "";
                }
                else { MessageBox.Show("Такое уже есть"); }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            
            if (textBox2.Text != "")
            {
                comandText = "SELECT tip_ti FROM Tip_Anime WHERE tip_ti = '" + textBox2.Text + "' ";
                if (Sowpad())
                {

                    comandText = "INSERT INTO Tip_Anime(tip_ti) VALUES('" + textBox2.Text + "')";
                    DeleteOrAddBD();
                    comandText = "";
                }
                else { MessageBox.Show("Такое уже есть"); }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
          
            if (textBox3.Text != "")
            {
                comandText = "SELECT oswych_osw FROM Oswychka_Anime WHERE oswych_osw = '" + textBox3.Text + "' ";
                if (Sowpad())
                {

                    comandText = "INSERT INTO Oswychka_Anime(oswych_osw) VALUES('" + textBox3.Text + "')";
                    DeleteOrAddBD();
                    comandText = "";
                }
                else { MessageBox.Show("Такое уже есть"); }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
           
            if (textBox5.Text != "")
            {
                comandText = "SELECT studi_st FROM Studi_Proisw WHERE studi_st = '" + textBox5.Text + "' ";
                if (Sowpad())
                {

                    comandText = "INSERT INTO Studi_Proisw(studi_st) VALUES('" + textBox5.Text + "')";
                    DeleteOrAddBD();
                    comandText = "";
                }
                else { MessageBox.Show("Такое уже есть"); }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            
            if (textBox6.Text != "")
            {
                comandText = "SELECT strana_st FROM Strana_Proisw WHERE strana_st = '" + textBox6.Text + "' ";
                if (Sowpad())
                {

                    comandText = "INSERT INTO Strana_Proisw(strana_st) VALUES('" + textBox6.Text + "')";
                    DeleteOrAddBD();
                    comandText = "";
                }
                else { MessageBox.Show("Такое уже есть"); }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
           
            if (textBox8.Text != "")
            {
                comandText = "SELECT dataTrans_da FROM Data_Translat WHERE dataTrans_da = " + textBox8.Text + " ";
                if (Sowpad())
                {

                    comandText = "INSERT INTO Data_Translat(dataTrans_da) VALUES(" + textBox8.Text + ")";
                    DeleteOrAddBD();
                    comandText = "";
                }
                else { MessageBox.Show("Такое уже есть"); }
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
