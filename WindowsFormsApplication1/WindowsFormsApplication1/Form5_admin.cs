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
    public partial class Form5_admin : Form
    {
        OleDbConnection connection = new OleDbConnection();
        Form4_newAnime frm5;
        Form4_AnimeProfil frm4;
        Form6_RedBD frm6;
        Form7_stat frm7;
        int id_User = -1;
        string read = "";
        public Form5_admin()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=AnimeBase.accdb;
Persist Security Info = False; ";
        }

        private void Form5_admin_Load(object sender, EventArgs e)
        {
            //заполнение сиска аниме
            AnimeInfoOn();
            NonWorkLink();
            Chat();
            News();
            //список пользователей
            button12_Click(sender, e);
           // Size s = new Size();
           // s.Width = tabControl1.Width;
           // s.Height = tabControl1.Height;
           // tabControl1.TabPages[2].BackgroundImage = new Bitmap(Image.FromFile(@"..\..\image\4.jpg"), s);
        }
        private void AnimeInfoOn()
        {

            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                var da = new OleDbDataAdapter(command.CommandText, connection);
                command.CommandText = "SELECT name_an, tip_an, collSer_an, ogranich_an, dataTrans_an FROM Anime_info ";
                da = new OleDbDataAdapter(command.CommandText, connection);
                DataTable dt7 = new DataTable();
                da.Fill(dt7);
                dataGridView7.DataSource = dt7;
                dataGridView7.Columns[0].HeaderText = "Название аниме";
                dataGridView7.Columns[1].HeaderText = "Тип";
                dataGridView7.Columns[2].HeaderText = "Количество серий";
                dataGridView7.Columns[3].HeaderText = "Ограничение";
                dataGridView7.Columns[4].HeaderText = "Дата трансляции";
                connection.Close();
                dataGridView7.Columns[0].Width = 538;
                dataGridView7.Columns[1].Width = 100;
                dataGridView7.Columns[2].Width = 100;
                dataGridView7.Columns[3].Width = 100;
                dataGridView7.Columns[4].Width = 100;

            }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL " + ex);
                connection.Close();
            }

        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Действительно удалить аниме?", "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {

                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM Anime_info WHERE id = (SELECT TOP 1 id FROM Anime_info WHERE  ";
                    if (dataGridView7[0, dataGridView7.CurrentRow.Index].Value.ToString() == "") { command.CommandText += "Anime_info.name_an IS NULL"; } else command.CommandText += "Anime_info.name_an='" + dataGridView7[0, dataGridView7.CurrentRow.Index].Value.ToString() + "' ";
                    if (dataGridView7[1, dataGridView7.CurrentRow.Index].Value.ToString() == "") { command.CommandText += " AND Anime_info.tip_an IS NULL)"; } else command.CommandText += " AND Anime_info.tip_an='" + dataGridView7[1, dataGridView7.CurrentRow.Index].Value.ToString() + "') ";

                    command.ExecuteNonQuery();
                    MessageBox.Show("Аниме удалено");
                    connection.Close();


                }
                catch (Exception ex)
                {
                    MessageBox.Show("FAIL " + ex);
                    connection.Close();

                }
                AnimeInfoOn();
            }
        }
        //редактирование аниме
        private void button11_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Действительно редактировать аниме?", "Подтвердите редактирование", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    //получаем ид редактируемого аниме и отправляем на редакцию
                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT id FROM Anime_info WHERE Anime_info.name_an='" + dataGridView7[0, dataGridView7.CurrentRow.Index].Value.ToString() + "' AND Anime_info.tip_an='" + dataGridView7[1, dataGridView7.CurrentRow.Index].Value.ToString() + "'"; //AND collSer_an=" + dataGridView7[2, dataGridView7.CurrentRow.Index].Value.ToString() + " AND ogranich_an='" + dataGridView7[3, dataGridView7.CurrentRow.Index].Value.ToString() + "'";
                    if (read != "") { command.CommandText = "SELECT id FROM Anime_info WHERE Anime_info.name_an='" + dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString() + "' "; }
                    int idd = 0;
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        idd = int.Parse(reader["id"].ToString());
                    }


                    connection.Close();
                    frm5 = new Form4_newAnime(idd);
                    frm5.Show();
                    read = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("FAIL " + ex);
                    connection.Close();

                }
            }
        }

        //окно добавления нового аниме
        private void button6_Click(object sender, EventArgs e)
        {
            frm5 = new Form4_newAnime();
            frm5.Show();
        }
        //обновить анмие
        private void button5_Click(object sender, EventArgs e)
        {
            AnimeInfoOn();
        }
        //подробнее об аниме
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT id FROM Anime_info WHERE Anime_info.name_an='" + dataGridView7[0, dataGridView7.CurrentRow.Index].Value.ToString() + "' AND Anime_info.tip_an='" + dataGridView7[1, dataGridView7.CurrentRow.Index].Value.ToString() + "'"; //AND collSer_an=" + dataGridView7[2, dataGridView7.CurrentRow.Index].Value.ToString() + " AND ogranich_an='" + dataGridView7[3, dataGridView7.CurrentRow.Index].Value.ToString() + "'";

                OleDbDataReader reader = command.ExecuteReader();
                int i = -1;
                while (reader.Read()) { i = int.Parse(reader[0].ToString()); }

                connection.Close();
                frm4 = new Form4_AnimeProfil(i, id_User);
                frm4.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL " + ex);
                connection.Close();

            }
        }

        //добавление смс в чат
        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Cheat(userName_ch, mail_ch) VALUES ('Admin ', '" + textBox1.Text + "')";
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Сообщение отправлено");
                    textBox1.Text = "";

                    Chat();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("FAIL " + ex);
                    connection.Close();
                }
            }
        }

        //добавление новостей бд
        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO News(text_ne, date_ne) VALUES ('" + textBox2.Text + "', Date())";
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Новость добавлена");
                    textBox2.Text = "";
                    News();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("FAIL " + ex);
                    connection.Close();
                }
            }
        }

        //обновление битых ссылок
        private void NonWorkLink()
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT name_an FROM Anime_info Where nwlink_an = 2";
                OleDbDataAdapter da = new OleDbDataAdapter(command.CommandText, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;              
                dataGridView2.Columns[0].HeaderText = "Название аниме";                
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL " + ex);
                connection.Close();
            }
        }

        //обновление чата
        private void Chat()
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT userName_ch, mail_ch FROM Cheat ORDER BY id_ch DESC";
                OleDbDataAdapter da = new OleDbDataAdapter(command.CommandText, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView4.DataSource = dt;
                //dataGridView4.Columns[0].HeaderText = "№";
                dataGridView4.Columns[0].HeaderText = "Имя";
                dataGridView4.Columns[1].HeaderText = "   Сообщение пользователя   ";
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL " + ex);
                connection.Close();
            }
        }  
        //обновение новостей бд
        private void News()
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT text_ne FROM News ORDER BY id_ne DESC";
                OleDbDataAdapter da = new OleDbDataAdapter(command.CommandText, connection);
                DataTable dt6 = new DataTable();
                da.Fill(dt6);
                dataGridView6.DataSource = dt6;
                dataGridView6.Columns[0].HeaderText = "Обновления, изменения и дополнения";
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL " + ex);
                connection.Close();

            }
        }
        //изменить смс
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (MessageBox.Show("Действительно редактировать сообщение этого пользователя?", "Подтвердите редактирование", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        OleDbCommand command = new OleDbCommand();
                        command.Connection = connection;
                        command.CommandText = "UPDATE  Cheat SET mail_ch = '" + textBox1.Text + "'  WHERE Cheat.mail_ch='" + dataGridView4[1, dataGridView4.CurrentRow.Index].Value.ToString() + "' ";

                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Сообщение изменено ");
                        Chat();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("FAIL " + ex);
                        connection.Close();

                    }
                }
            }
        }
        //удалить смс
        private void button7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Действительно удалить сообщение?", "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM Cheat WHERE mail_ch='" + dataGridView4[1, dataGridView4.CurrentRow.Index].Value.ToString() + "'";
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Сообщение удалено ");
                    Chat();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("FAIL " + ex);
                    connection.Close();

                }
            }
        }
        //изменить новость
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                if (MessageBox.Show("Действительно редактировать новость?", "Подтвердите редактирование", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        OleDbCommand command = new OleDbCommand();
                        command.Connection = connection;
                        command.CommandText = "UPDATE  News SET text_ne = '" + textBox2.Text + "'  WHERE text_ne='" + dataGridView6[0, dataGridView4.CurrentRow.Index].Value.ToString() + "' ";

                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Новость изменена ");
                        News();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("FAIL " + ex);
                        connection.Close();

                    }
                }
            }
        }
        //удалить новость
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Действительно удалить новость?", "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM News WHERE text_ne='" + dataGridView6[0, dataGridView4.CurrentRow.Index].Value.ToString() + "'";
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Новость удалена ");
                    News();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("FAIL " + ex);
                    connection.Close();

                }
            }
        }
        //обновление списка пользователей
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT login_us, password_us, email_us,  name_us, fName_us, age_us, info_us FROM User_Data WHERE admin_us = false";
                OleDbDataAdapter da = new OleDbDataAdapter(command.CommandText, connection);
                DataTable dt1 = new DataTable();
                da.Fill(dt1);
                dataGridView1.DataSource = dt1;
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL " + ex);
                connection.Close();

            }
        }
        //удаление пользователя
        private void button14_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Действительно удалить пользователя?", "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM User_Data WHERE login_us='" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + "' AND password_us = '" + dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString() + "'";
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Пользователь удален ");
                    button12_Click(sender, e);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("FAIL " + ex);
                    connection.Close();

                }
            }
        }
        //редактор логина пользователя
        private void button13_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                if (MessageBox.Show("Действительно редактировать логин пользователя?", "Подтвердите редактирование", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        OleDbCommand command = new OleDbCommand();
                        command.Connection = connection;
                        command.CommandText = "UPDATE  User_Data SET login_us = '" + textBox3.Text + "'  WHERE login_us='" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + "' ";

                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Логин пользователя изменен ");
                        button12_Click(sender, e);


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("FAIL " + ex);
                        connection.Close();

                    }
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button13_Click(sender, e); }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button8_Click(sender, e); }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button9_Click(sender, e); }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            NonWorkLink();
        }
        //кнопка изменить у грида с битыми ссылками
        private void button16_Click(object sender, EventArgs e)
        {
            if (dataGridView2.RowCount > 0)
            {
                read = "yes";
                button11_Click(sender, e);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            frm6 = new Form6_RedBD();
            frm6.Show();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                if (MessageBox.Show("Действительно редактировать пароль пользователя?", "Подтвердите редактирование", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        OleDbCommand command = new OleDbCommand();
                        command.Connection = connection;
                        command.CommandText = "UPDATE  User_Data SET password_us = '" + textBox4.Text + "'  WHERE password_us='" + dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString() + "' ";

                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Пароль пользователя изменен ");
                        button12_Click(sender, e);


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("FAIL " + ex);
                        connection.Close();

                    }
                }
            }
        }
        //статистика
        private void button19_Click(object sender, EventArgs e)
        {
            frm7 = new Form7_stat();
            frm7.Show();
        }
    }
}
