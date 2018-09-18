using System;

using System.Data.OleDb;

using System.Windows.Forms;


namespace WindowsFormsApplication1
{
    
    public partial class Form1 : Form
    {
        public OleDbConnection connection = new OleDbConnection();
        Form1 frm1;
        Form2 frm2;
        Form3 frm3;
        Form5_admin frm5;
        string log;
        string pass;
        string id;
        bool admin;
        public Form1()
        {
            InitializeComponent();
            frm1 = this;
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=AnimeBase.accdb;
Persist Security Info = False; ";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            log = textBox1.Text;
            pass = textBox2.Text;
            if(log.Length < 4 || pass.Length < 4) { MessageBox.Show("Слишком мало символов"); return; }
            //при закрытии второй формы возвращаемся в первую
            //скрываем главную форму и показываем профили
            if (LogIn(log,pass))
            {
                frm1.Hide();               
                if (!admin)
                {
                    frm2 = new Form2(log, pass, int.Parse(id));
                    frm2.FormClosed += (frm2, ee) => this.Show();
                    frm2.Show();
                }
                else
                {
                    frm5 = new Form5_admin();
                    frm5.FormClosed += (frm5, ee) => this.Show();
                    frm5.Show();
                }
            }
        }      
        //подсказки при наводке мышью
        private void Form1_Load(object sender, EventArgs e)
        {
            ToolTip t1 = new ToolTip();
            t1.SetToolTip(textBox1, "Логин для входа. Длина от 4 до 20");
            ToolTip t2 = new ToolTip();
            t2.SetToolTip(textBox2, "Пароль для входа. Длина от 4 до 20");

        }
        //регистрация
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frm1.Hide();
            frm3 = new Form3();
            frm3.FormClosed += (o, eq) => this.Show();
            frm3.Show();            
        }
        //Проверка логина и пароля
        private bool LogIn(string log, string pass) 
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM User_Data WHERE login_us='"+log+"' AND password_us='"+pass+"'";
                OleDbDataReader reader = command.ExecuteReader();
                int count = 0;
                while (reader.Read())
                {
                    id = reader["id"].ToString();
                    admin = (bool)reader["admin_us"];
                    count++;
                }
                if (count == 1) { MessageBox.Show("Вход выполнен успешно"); connection.Close(); return true; }
                else { MessageBox.Show("Неправильный логин или пароль. Повторите еще раз"); connection.Close(); return false; }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAIL" + ex);
                connection.Close();
                return false;
            }
            
        }
        //если нажат энтер
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button1_Click(sender, e); }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button1_Click(sender, e); }
        }
        //логин и пароль только с цифр и букв
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (e.KeyChar != (char)Keys.Back && !Char.IsLetterOrDigit(number))
            {
                e.Handled = true;
            }
        }
    }
}
