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
using System.Data.Common;
using System.IO;
using System.Drawing.Imaging;


namespace WindowsFormsApplication1
{

    public partial class Form4_AnimeProfil : Form
    {
        private string url;
        OleDbConnection connection = new OleDbConnection();
        private int id = 0;
        private int user;
        public Form4_AnimeProfil(int id_Anime, int id_User)
        {
            id = id_Anime;
            user = id_User;
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=AnimeBase.accdb;
Persist Security Info = False; ";
        }

       // Bitmap bmp = null;

        private void Form4_AnimeProfil_Load(object sender, EventArgs e)
        {
            Load_Data();
            // load bg 
            //Size s = new Size();
            //s.Width = this.Width;
            //s.Height = this.Height;
            //this.BackgroundImage = new Bitmap(Image.FromFile(@"..\..\image\2.jpg"), s);
            if (user == -1) { button1.Visible = false; button2.Visible = false; button3.Visible = false; button4.Visible = false; }

        }

        private void Load_Data()
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Anime_Info WHERE id=" + id + "";
                //заполняем данные по аниме в инфо
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    label12.Text = reader["name_an"].ToString();
                    label13.Text = reader["tip_an"].ToString();
                    label14.Text = reader["dataTrans_an"].ToString();
                    label15.Text = reader["collSer_an"].ToString();
                    label18.Text = reader["ogranich_an"].ToString();
                    label19.Text = reader["studi_an"].ToString() + ", " + reader["strana_an"].ToString();
                    label20.Text = reader["dataInBase_an"].ToString();
                    label10.Text = reader["info_an"].ToString();
                    url = reader["url_an"].ToString();
                    this.Text = reader["name_an"].ToString();
                }
                reader.Close();
                //заполняем данные кто озвучил
                command.CommandText = "SELECT osvichka_an.Value FROM Anime_info WHERE id = " + id + "";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    label16.Text += (reader[0].ToString()) + " ";
                }
                reader.Close();

                // заполняем жанры тайтла
                command.CommandText = "SELECT ganr_an.Value FROM Anime_info WHERE id = " + id + "";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    label17.Text += (reader[0].ToString()) + " ";
                }
                reader.Close();

                //достаем рисунок из базы
                // прочесть 'рисунок' из базы

                var st = connection.CreateCommand();
                st.CommandText = "SELECT image_an from Anime_info WHERE id=" + id + "";
                var res = (byte[])st.ExecuteScalar();

                // восстановить рисунок из byte[]
                MemoryStream ms = new MemoryStream(res);
                Image img = Bitmap.FromStream(ms);

                // вывести рисунок на экран  
                pictureBox1.Image = img;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                connection.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL " + ex);
                connection.Close();
            }
        }


        //добавление и удаление аниме в список смотрящихся
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO User_Data(spisSee_us.Value) VALUES ((SELECT name_an FROM Anime_info WHERE Anime_info.id = " + id + "))WHERE id=" + user + "";
                command.ExecuteNonQuery();
                MessageBox.Show("Аниме добавлено в список просматриваемых");
                connection.Close();
            }
            catch (Exception ex)
            {

                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "DELETE spisSee_us.Value FROM  User_Data WHERE spisSee_us.Value = (SELECT name_an FROM Anime_info WHERE Anime_info.id = " + id + ")";
                command.ExecuteNonQuery();
                MessageBox.Show("Аниме удалено из вашего списка просматриваемых");
                connection.Close();
            }
        }
        //Добавление и удаление аниме в список просмотренных
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO User_Data(spisSmotrel_us.Value) VALUES ((SELECT name_an FROM Anime_info WHERE Anime_info.id = " + id + "))WHERE id=" + user + "";
                command.ExecuteNonQuery();
                MessageBox.Show("Аниме добавлено в список просмотренных");
                connection.Close();
            }
            catch (Exception ex)
            {
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "DELETE spisSmotrel_us.Value FROM  User_Data WHERE spisSee_us.Value = (SELECT name_an FROM Anime_info WHERE Anime_info.id = " + id + ")";
                command.ExecuteNonQuery();
                MessageBox.Show("Аниме удалено из вашего списка просмотренных");
                connection.Close();
            }
        }
        //добавление и удаление аниме в список просмотра позже
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO User_Data(spisWait_us.Value) VALUES ((SELECT name_an FROM Anime_info WHERE Anime_info.id = " + id + "))WHERE id=" + user + "";
                command.ExecuteNonQuery();
                MessageBox.Show("Аниме добавлено в список отложенных просмотров");
                connection.Close();
            }
            catch (Exception ex)
            {
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "DELETE spisWait_us.Value FROM  User_Data WHERE spisSee_us.Value = (SELECT name_an FROM Anime_info WHERE Anime_info.id = " + id + ")";
                command.ExecuteNonQuery();
                MessageBox.Show("Аниме удалено из вашего списка просмотреть позже");
                connection.Close();
            }
        }



        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(url);

        }
        //кнопка о битой ссылке
        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены что ссылка битая? Убедитесь, что подождали достаточно времени, чтоб ваш браузер успел прогрузиться и подключиться к интернету. Данное действие нельзя будет отменить", "Подтвердите наличие битой ссылки", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE Anime_info SET nwlink_an = 2 WHERE id=" + id + "";
                    command.ExecuteNonQuery();
                    MessageBox.Show("Спасибо за содействие. Ваше заявление отправлено администратору");
                    connection.Close();
                }
                catch (Exception ex)
                {                   
                    MessageBox.Show("ФАТАЛ ЭРОРР " + ex);
                    connection.Close();
                }


            }
        }
    }
}
    

