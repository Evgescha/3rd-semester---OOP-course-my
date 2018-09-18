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
using System.IO;
using System.Drawing.Imaging;

namespace WindowsFormsApplication1
{
    public partial class Form4_newAnime : Form
    {
        OleDbConnection connection = new OleDbConnection();
        int id_Update;
        //конструктор по умолчанию для нового аниме
        public Form4_newAnime()
        {
            InitializeComponent();
            
        }
        //конструктор с параметром для редактирования аниме
        public Form4_newAnime(int id)
        {
            id_Update = id;
            InitializeComponent();
        }

        private void Form4_newAnime_Load(object sender, EventArgs e)
        {
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=AnimeBase.accdb;
Persist Security Info = False; ";
            AnimeSearchn();
            if (id_Update != 0) { this.Text = "Редактирование"; button2.Text = "Редактировать"; Redakt(); }
            else {
                this.Text = "Добавление нового аниме";
            }
            //load bg foto
           // Size s = new Size();
           // s.Width = this.Width;
           // s.Height = this.Height;
           // this.BackgroundImage = new Bitmap(Image.FromFile(@"..\..\image\1.jpg"), s);

        }
        //запись в циклы варианты добавлений стран, даты выпуска и тд
        private void AnimeSearchn()
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                //Обновляем список жанров
                command.CommandText = "SELECT ganr_ga FROM All_Janre";
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    checkedListBox1.Items.Add(reader["ganr_ga"]);
                }
                //Обновляем список даты выпуска
                reader.Close();
                command.CommandText = "SELECT dataTrans_da FROM Data_Translat";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cb_transl.Items.Add(reader["dataTrans_da"]);
                }

                //Обновляем список  озвучки
                reader.Close();
                command.CommandText = "SELECT oswych_osw FROM Oswychka_Anime";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    checkedListBox2.Items.Add(reader["oswych_osw"]);
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
            }
        }
        //открыть файл
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        //если пришли редактировать, открываем что редактировать
        private void Redakt()
        {
            if (id_Update != 0)
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    //Обновляем список жанров
                    command.CommandText = "SELECT * FROM Anime_info WHERE id=" + id_Update + "";
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        cb_strana.Text = reader["strana_an"].ToString();
                        cb_studi.Text = reader["studi_an"].ToString();
                        cb_tip.Text = reader["tip_an"].ToString();
                        cb_transl.Text = reader["dataTrans_an"].ToString();
                        textBox1.Text = reader["info_an"].ToString();
                        textBox2.Text = reader["collSer_an"].ToString();
                        textBox3.Text = reader["ogranich_an"].ToString();
                        textBox7.Text = reader["name_an"].ToString();
                        cb_url.Text = ""+reader["url_an"].ToString()+"";

                        var st = connection.CreateCommand();
                        st.CommandText = "SELECT image_an from Anime_info WHERE id=" + id_Update + "";
                        var res = (byte[])st.ExecuteScalar();
                        MemoryStream ms = new MemoryStream(res);
                        Image img = Bitmap.FromStream(ms);
                        pictureBox1.Image = img;
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    reader.Close();
                    //отмеченые жанры
                    command.CommandText = "SELECT osvichka_an.Value FROM Anime_info WHERE id = " + id_Update + "";
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int j = 0; j < checkedListBox2.Items.Count; j++)
                        {
                            if (checkedListBox2.Items[j].ToString() == reader[0].ToString()) { checkedListBox2.SetItemCheckState(j, CheckState.Checked); }
                        }
                    }
                    reader.Close();
                    //отмеченые жанры
                    command.CommandText = "SELECT ganr_an.Value FROM Anime_info WHERE id = " + id_Update + "";
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int j = 0; j < checkedListBox1.Items.Count; j++)
                        {
                            if (checkedListBox1.Items[j].ToString() == reader[0].ToString()) { checkedListBox1.SetItemCheckState(j, CheckState.Checked); }
                        }
                    }



                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("FAIL " + ex);

                }
            }
        }
        
        //Запись нового тайтла или перезапись предыдущего
        private void New_Anime()
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox7.Text != "" && cb_url.Text !="")
            {
                try
                {
                    var c = connection;
                    c.Open();
                    // создать рисунок 
                    var bmp = new Bitmap(100, 100);
                    string fileName = openFileDialog1.FileName;
                    if (pictureBox1.Image != null) { bmp = (Bitmap)pictureBox1.Image; }
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                    // для преобразования рисунка в byte[]
                    var mss = new MemoryStream();
                    bmp.Save(mss, ImageFormat.Bmp);
                    mss.Position = 0;

                    // записать 'рисунок' и другие данные в базу
                    var it = c.CreateCommand();
                    it.CommandText = "INSERT INTO Anime_info(url_an, image_an, name_an, tip_an, collSer_an, dataTrans_an, studi_an, ogranich_an,  strana_an, info_an, dataInBase_an) VALUES ('"+ cb_url.Text + "', @image_an,'" + textBox7.Text + "' , '" + cb_tip.Text + "' , '" + textBox2.Text + "' , '" + cb_transl.Text + "' , '" + cb_studi.Text + "' , '" + textBox3.Text + "', '" + cb_strana.Text + "', '" + textBox1.Text + "', Date() ) ";
                    it.Parameters.AddWithValue("@image_an", mss.ToArray());
                    it.ExecuteNonQuery();
                   


                    //получаем ид
                    int id = 0;
                    it.CommandText = "SELECT MAX(id) FROM Anime_info";
                    OleDbDataReader reader = it.ExecuteReader();
                    while (reader.Read())
                    {
                        id = (int)reader[0];
                        //запись url
                        
                    }
                    reader.Close();
                    
                    //добавляем озвучку и жанры построково
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                        if (checkedListBox1.GetItemChecked(i))
                        {
                            it.CommandText = "INSERT INTO Anime_info(ganr_an.VALUE) VALUES('" + checkedListBox1.Items[i].ToString() + "') WHERE Anime_info.id = " + id + "";
                            it.ExecuteNonQuery();
                        }
                    for (int j = 0; j < checkedListBox2.Items.Count; j++)
                        if (checkedListBox2.GetItemChecked(j))
                        {
                            it.CommandText = "INSERT INTO Anime_info(osvichka_an.VALUE) VALUES('" + checkedListBox2.Items[j].ToString() + "') WHERE Anime_info.id = " + id + "";
                            it.ExecuteNonQuery();
                        }
                    if (id_Update > 0) { MessageBox.Show("Аниме отредактировано "); } else MessageBox.Show("Аниме добавлено ");

                    it.CommandText = "DELETE FROM Anime_info WHERE id=" + id_Update + "";
                    it.ExecuteNonQuery();
                    c.Close();
                    this.Close();
                    cb_tip.Text = "";
                    cb_strana.Text = "";
                    cb_studi.Text = "";
                    cb_transl.Text = "";
                    cb_url.Text = "";
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox7.Text = "";
                    for (int i = 0; i < checkedListBox1.Items.Count; i++) { checkedListBox1.SetItemCheckState(i, CheckState.Unchecked); }
                    for (int j = 0; j < checkedListBox2.Items.Count; j++) { checkedListBox2.SetItemCheckState(j, CheckState.Unchecked); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Проверьте, ввели ли вы все данные и выбрали ли картинку." + ex);
                    connection.Close();

                }
            } else { MessageBox.Show("Проверьте, ввели ли вы все данные и выбрали ли картинку."); }
        }
        //сораняем изменения обратно в бд или создаем новый
        private void button2_Click(object sender, EventArgs e)
        {
            New_Anime();           
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button2_Click(sender, e); }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
