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
    public partial class Form3 : Form
    {
        OleDbConnection connection = new OleDbConnection();
        string log;
        string pass;
        string _pass;
        public Form3()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=AnimeBase.accdb;
Persist Security Info = False; ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            log = textBox1.Text;
            pass = textBox2.Text;
            _pass = textBox3.Text;
            if (log.Length < 4 || pass.Length < 4 || _pass.Length < 4) { MessageBox.Show("Слишком мало символов"); return; }
            if (pass == _pass && log!="" && pass!="")
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM User_Data WHERE login_us='" + textBox1.Text + "'";
                    OleDbDataReader reader = command.ExecuteReader();
                    int count = 0;
                    //проверяем, есть ли уже поьзователи с таким именем
                    while (reader.Read()) { count++; }
                    //если есть вывести ошибку
                    if (count == 1) { System.Windows.MessageBox.Show("Пользователь с  таким именем уже существует.", "Регистрация"); }
                    //если нет, добавляем пользователя
                    else if (count == 0)
                    {
                        OleDbCommand newUser = new OleDbCommand();
                        newUser.Connection = connection;
                        newUser.CommandText = "INSERT INTO User_Data (login_us, password_us) VALUES ('" + log+"','"+pass+"')";
                        newUser.ExecuteNonQuery();
                        System.Windows.MessageBox.Show("Регистрация прошла успешно! Войдите под своим пользователем.");
                        this.Close();
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("FAIL" + ex);
                    connection.Close();
                }
            }
            else { System.Windows.MessageBox.Show("Неверное повторили пароль или ввели не все данные."); }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button1_Click(sender, e); }
        }

       

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (e.KeyChar != (char)Keys.Back && !Char.IsLetterOrDigit(number))
            {
                e.Handled = true;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            ToolTip t1 = new ToolTip();
            t1.SetToolTip(textBox1, "Логин для входа. Длина от 4 до 20");
            ToolTip t2 = new ToolTip();
            t2.SetToolTip(textBox2, "Пароль для входа. Длина от 4 до 20");
            ToolTip t3 = new ToolTip();
            t3.SetToolTip(textBox3, "Пароль для входа. Длина от 4 до 20");
        }
    }
}
