using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        OleDbConnection connection = new OleDbConnection();
        
        string log;
        string pass;
        int id_User;
        Form4_AnimeProfil frm4;
        //для открытия подробностей об аниме из списков пользователя, а то западло каждый раз искать подробности от той или иной в поиске 
        string ReadProfSpis = "";
       
        public Form2(string log, string pass, int usId)
        {
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_MainFormClosing);
            this.log = log;
            this.pass = pass;
            id_User = usId;
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=AnimeBase.accdb;
Persist Security Info = False; ";
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text = "Добро пожаловать, "+log;
            //заполнение профиля из бд
            UserDataOn();
            UserDataUpdate();
            //заполнение списка аниме
            AnimeInfoOn();
            //заполнение листбоксов для поиска
            AnimeSearchOn();
            //обнавляем чат
            Chat();
            //последние добавленные аниме
            EndAnime();
            //Обновление новостей
            News();
            //загрузка фото
            //Size s = new Size();
           // s.Width = tabControl1.Width;
           // s.Height = tabControl1.Height;
            //tabControl1.TabPages[0].BackgroundImage = new Bitmap(Image.FromFile(@"..\..\image\7.jpg"), s);
            //tabControl1.TabPages[1].BackgroundImage = new Bitmap(Image.FromFile(@"..\..\image\5.jpg"), s);
            //tabControl1.TabPages[2].BackgroundImage = new Bitmap(Image.FromFile(@"..\..\image\bg.jpg"), s);
        }
        //обработка закрытия формы
        private void Form2_MainFormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите завершить сеанс?", "Подтвердите выход", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {  e.Cancel = true; }           
        }
        //вызов аниме профиля
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                
                if (ReadProfSpis == "Смотрю")
                {
                    if(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString()!="")
                    command.CommandText = "SELECT id FROM Anime_info WHERE Anime_info.name_an='" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + "'";
                    else ReadProfSpis = "-1";
                }
                if (ReadProfSpis == "Смотрел")
                {
                    if (dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString() != "")
                        command.CommandText = "SELECT id FROM Anime_info WHERE Anime_info.name_an='" + dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString() + "'";
                    else ReadProfSpis = "-1";

                }
                if (ReadProfSpis == "Буду смотреть")
                {
                    if (dataGridView3[0, dataGridView3.CurrentRow.Index].Value.ToString() != "")
                        command.CommandText = "SELECT id FROM Anime_info WHERE Anime_info.name_an='" + dataGridView3[0, dataGridView3.CurrentRow.Index].Value.ToString() + "'";
                    else ReadProfSpis = "-1";
                }
                if (ReadProfSpis == "-1"){ connection.Close(); ReadProfSpis = "";  return; }
                    if (ReadProfSpis == "")
                {
                    command.CommandText = "SELECT id FROM Anime_info WHERE Anime_info.name_an='" + dataGridView7[0, dataGridView7.CurrentRow.Index].Value.ToString() + "' AND Anime_info.tip_an='" + dataGridView7[1, dataGridView7.CurrentRow.Index].Value.ToString() + "'"; //AND collSer_an=" + dataGridView7[2, dataGridView7.CurrentRow.Index].Value.ToString() + " AND ogranich_an='" + dataGridView7[3, dataGridView7.CurrentRow.Index].Value.ToString() + "'";
                }
                OleDbDataReader reader =  command.ExecuteReader();
                int i=-1;
                while (reader.Read()) { i = int.Parse(reader[0].ToString()); }
                
                connection.Close();
                frm4 = new Form4_AnimeProfil(i, id_User);
                frm4.Show();
                ReadProfSpis = "";

            }
            catch (Exception ex)
            {
                //MessageBox.Show("FAIL " + ex);
                MessageBox.Show("Не выбран тайтл" );
                connection.Close();

            }
        }        
        //Обновление информации внутри профиля
        private void button2_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Enabled == false && textBox2.Enabled == false && txt_fname.Enabled == false && txt_info.Enabled == false &&  txt_name.Enabled == false && txt_pass.Text == pass)
            {
                dateTimePicker1.Enabled = true; textBox2.Enabled = true; txt_fname.Enabled = true; txt_info.Enabled = true; txt_name.Enabled = true;  button2.Text = "Сохранить";
            }
            else
            {
                if (txt_pass.Text == pass)
                {
                    dateTimePicker1.Enabled = false; textBox2.Enabled = false; txt_fname.Enabled = false; txt_info.Enabled = false;  txt_name.Enabled = false;  button2.Text = "Редактировать";
                    UserDataUpdate();
                }
                else { MessageBox.Show("Неверный пароль"); }
            }
        }
        //получение информации о пользователе и занесение ее в профиль
        private void UserDataOn()
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM User_Data WHERE login_us='"+log+"'";
                
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    txt_log.Text = reader["login_us"].ToString();
                    txt_name.Text = reader["name_us"].ToString();
                    txt_fname.Text = reader["fName_us"].ToString();
                    dateTimePicker1.Text = reader["age_us"].ToString();
                    txt_info.Text = reader["info_us"].ToString();
                    textBox2.Text = reader["email_us"].ToString();
                }
                

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL " + ex);
                connection.Close();
            }
        }
        //тут вывод списков аниме пользователя и общего списка на экран 
        private void AnimeInfoOn()
        {
            
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                //список смотрю
                command.CommandText = "SELECT spisSee_us.Value FROM User_Data WHERE login_us='" + log + "'";
                OleDbDataAdapter da = new OleDbDataAdapter(command.CommandText, connection);
                DataTable dt1 = new DataTable();
                da.Fill(dt1);
                dataGridView1.DataSource = dt1;
                dataGridView1.Columns[0].HeaderText = "Смотрю";
                //список смотрел
                command.CommandText = "SELECT spisSmotrel_us.Value FROM User_Data WHERE login_us='" + log + "'";
                da = new OleDbDataAdapter(command.CommandText, connection);
                DataTable dt2 = new DataTable();               
                da.Fill(dt2);
                dataGridView2.DataSource = dt2;
                dataGridView2.Columns[0].HeaderText = "Смотрел";
                //список буду смотреть
                command.CommandText = "SELECT spisWait_us.Value FROM User_Data WHERE login_us='" + log + "'";
                da = new OleDbDataAdapter(command.CommandText, connection);
                DataTable dt3 = new DataTable();                
                da.Fill(dt3);
                dataGridView3.DataSource = dt3;
                dataGridView3.Columns[0].HeaderText = "Буду смотреть";
                //общий список аниме
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
        //обновление информации о пользователе и занесение ее в базу данных
        private void UserDataUpdate()
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "UPDATE User_Data set email_us='" + textBox2.Text + "',info_us='" + txt_info.Text + "', age_us='" + dateTimePicker1.Value+"', name_us='"+txt_name.Text+"', fName_us='"+txt_fname.Text+"' WHERE login_us='"+log+"' AND password_us='"+pass+"'";
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL " + ex);
                connection.Close();
            }
        }
        //добавление списков для поиска
        private void AnimeSearchOn()
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                //Обновляем список жанров
                command.CommandText = "SELECT * FROM All_Janre";
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cb_Janre.Items.Add(reader["ganr_ga"]);
                }
                //Обновляем список даты выпуска
                reader.Close();
                command.CommandText = "SELECT * FROM Data_Translat";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cb_transl.Items.Add(reader["dataTrans_da"]);
                }
                //Обновляем список количества серий
                reader.Close();
                command.CommandText = "SELECT DISTINCT collSer_an FROM Anime_Info";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cb_collSer.Items.Add(reader["collSer_an"]);
                }
                //Обновляем список ограничений
                reader.Close();
                command.CommandText = "SELECT DISTINCT ogranich_an FROM Anime_Info";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cb_ogran.Items.Add(reader["ogranich_an"]);
                }
                //Обновляем список  озвучки
                reader.Close();
                command.CommandText = "SELECT oswych_osw FROM Oswychka_Anime";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cb_osw.Items.Add(reader["oswych_osw"]);
                }
                //Обновляем список стран производства 
                reader.Close();
                command.CommandText = "SELECT strana_st FROM Strana_Proisw";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cb_strana.Items.Add(reader["strana_st"]);
                }
                //Обновляем список студий производства
                reader.Close();
                command.CommandText = "SELECT studi_st FROM Studi_Proisw";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cb_studi.Items.Add(reader["studi_st"]);
                }
                //Обновляем список типов 
                reader.Close();
                command.CommandText = "SELECT tip_ti FROM Tip_Anime";
                reader = command.ExecuteReader();
                while (reader.Read())
                {          
                    cb_tip.Items.Add(reader["tip_ti"]);
                }               
                connection.Close();
                }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL   " + ex);
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
                dataGridView4.Columns[0].HeaderText = "Имя               ";
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
        //поиск аниме по критериям
        private void button4_Click_1(object sender, EventArgs e)
        {
            
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                string com = "SELECT name_an, tip_an, collSer_an, ogranich_an, dataTrans_an FROM Anime_info WHERE id> 0 ";
                if (cb_tip.Text != "") { com += " AND  Anime_info.tip_an LIKE '%" + cb_tip.Text + "%'"; }
                if (textBox7.Text != "") { com += " AND  Anime_info.name_an LIKE '%" + textBox7.Text + "%'"; }
                if (cb_collSer.Text != "") { com += " AND Anime_info.collSer_an=" + cb_collSer.Text + ""; }
                if (cb_Janre.Text != "") { com += " AND Anime_info.ganr_an.Value='" + cb_Janre.Text + "'"; }
                if (cb_ogran.Text != "") { com += " AND Anime_info.ogranich_an=" + cb_ogran.Text + ""; }
                if (cb_osw.Text != "") { com += " AND Anime_info.osvichka_an.Value='" + cb_osw.Text + "'"; }
                if (cb_strana.Text != "") { com += " AND Anime_info.strana_an='" + cb_strana.Text + "'"; }
                if (cb_studi.Text != "") { com += " AND Anime_info.studi_an='" + cb_studi.Text + "'"; }
                if (cb_transl.Text != "") { com += " AND Anime_info.dataTrans_an=" + cb_transl.Text + ""; }
                command.CommandText = com;
                OleDbDataAdapter da = new OleDbDataAdapter(command.CommandText, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView7.DataSource = dt;
                //dataGridView7.Columns[0].HeaderText = "Имя";
                command.ExecuteNonQuery();
                connection.Close();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL " + ex);
                connection.Close();
            }
        }
      
        //обновление списка аниме
        private void button7_Click(object sender, EventArgs e)
        {
            AnimeInfoOn();
        }
     
        //5 последних добавленых аниме
        private void EndAnime()
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT TOP 5 name_an FROM Anime_info ORDER BY id DESC";
                OleDbDataAdapter da = new OleDbDataAdapter(command.CommandText, connection);
                DataTable dt5 = new DataTable();
                da.Fill(dt5);
                dataGridView5.DataSource = dt5;
                dataGridView5.Columns[0].HeaderText = "Имя";
                command.ExecuteNonQuery();
                connection.Close();
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
                    command.CommandText = "INSERT INTO Cheat(userName_ch, mail_ch) VALUES ('" + log + "', '" + textBox1.Text + "')";
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
        //сброс поиска списка аниме
        private void button5_Click_1(object sender, EventArgs e)
        {
            AnimeInfoOn();
            cb_Janre.Text = "";
            cb_collSer.Text = "";
            cb_ogran.Text = "";
            cb_osw.Text = "";
            cb_strana.Text = "";
            cb_studi.Text = "";
            cb_transl.Text = "";
            cb_tip.Text = "";
            textBox7.Text = "";
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button8_Click(sender, e); }
        }

        private void txt_pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button2_Click(sender, e); }
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button4_Click_1(sender, e); }
        }
        //кнопка подробнее в списках пользователя
        private void button3_Click(object sender, EventArgs e)
        {
            ReadProfSpis= tabControl2.SelectedTab.Text;
            button1_Click_1(sender,e);
        }
        //ввод только цифр и их стирание
        private void cb_collSer_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number)&& e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
        //поля только для текста из обычных букв
        private void txt_name_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (e.KeyChar != (char)Keys.Back && !Char.IsLetter(number))
            {
                e.Handled = true;
            }
        }
        //пароль из букв и цифр
        private void txt_pass_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (e.KeyChar != (char)Keys.Back && !Char.IsLetterOrDigit(number))
            {
                e.Handled = true;
            }
        }
//если нажат ентер то ищем
        private void cb_tip_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button4_Click_1(sender, e); }
        }
        //если ентер нажали в гриде
        private void dataGridView7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button1_Click_1(sender, e); }
        }
    }
}
