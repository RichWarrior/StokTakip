using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StokTakipV2.View
{
    public partial class LoginScreen : Form
    {
        string islem, tool;
        #region Form Initialize
        MyManager manager = new MyManager();
        public static string login_user;
        public LoginScreen()
        {
            InitializeComponent();
        }
        #endregion
        #region Form_Load
        private void LoginScreen_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            if(!manager.network_control())
            {
                MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        #endregion
        #region Şifremi Göster Paneli
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(textBox2.UseSystemPasswordChar == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
        #endregion
        #region Çıkış Butonu Menü
        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Çıkmak İstediğinize Emin Misiniz?","DİKKAT",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        #endregion
        #region Giriş Yap Butonu
        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            bunifuImageButton1.Enabled = false;
            if (manager.network_control())
            {
                if(manager.login(textBox1.Text,manager.md5_crypto(textBox2.Text)))
                {
                    login_user = textBox1.Text;
                    Form yeni = new HomeScreen();
                    this.Hide();
                    yeni.Show();
                }else if (manager.login_admin(textBox1.Text,manager.md5_crypto(textBox2.Text)))
                {
                    login_user = textBox1.Text;
                    Form x = new Admin_Home_Screen();
                    this.Hide();
                    x.Show();
                }
                else
                {
                    MessageBox.Show("Kullanıcı Adı veya Şifre Hatalı!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }else
            {
                MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            bunifuImageButton1.Enabled = true;
        }
        #endregion
        #region Yeni Firma Oluştur Kısmı
        private void yeniFirmaOluşturToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form x = new Add_New_Business();
            this.Hide();
            x.Show();
        }
        #endregion
        #region Panel Açma Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(islem == "Aç")
            {
                if(tool=="panel1")
                {
                    panel1.Width += 5;
                    if(panel1.Width == 300)
                    {
                        timer1.Stop();
                    }
                }else if(tool == "panel2")
                {
                    panel2.Width += 5;
                    if(panel2.Width == 300)
                    {
                        timer1.Stop();
                    }
                }
            }else
            {
                if(tool=="panel1")
                {
                    panel1.Width -= 5;
                    if(panel1.Width == 0)
                    {
                        timer1.Stop();
                    }
                }
                else if (tool == "panel2")
                {
                    panel2.Width -= 5;
                    if (panel2.Width == 0)
                    {
                        timer1.Stop();
                    }
                }
            }
        }
        #endregion
        #region Panel Kapa Button
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            islem = "Kapa";
            tool = "panel1";
            timer1.Start();
            textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = "";
        }
        #endregion       
        #region Şifre Göster
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(textBox5.UseSystemPasswordChar == true && textBox6.UseSystemPasswordChar==true)
            {
                textBox5.UseSystemPasswordChar = textBox6.UseSystemPasswordChar = false;
            }else
            {
                textBox5.UseSystemPasswordChar = textBox6.UseSystemPasswordChar = true;
            }
        }
        #endregion
        #region Yönetici Şifre Değiştir Panel Aç
        private void yöneticiŞifresiSıfırlaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(panel1.Width == 0&&panel2.Width == 0)
            {
                timer1.Interval = 1;
                islem = "Aç";
                tool = "panel1";
                timer1.Start();
            }
        }
        #endregion
        #region Yönetici Şifre Değiştir Buton
        private void button1_Click(object sender, EventArgs e)
        {
            if((textBox3.Text!=""&&textBox4.Text!=""&&textBox5.Text!=""&&textBox5.Text!="")&&textBox6.Text==textBox6.Text)
            {
                if(manager.network_control()&&manager.update_admin_pass(textBox3.Text,textBox4.Text,manager.md5_crypto(textBox5.Text)))
                {
                    MessageBox.Show("Şifre Değiştirme İşlemi Başarılı","BAŞARILI",MessageBoxButtons.OK);
                    tool = "panel1";
                    islem = "Kapa";
                    timer1.Interval = 1;
                    timer1.Start();
                    textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = "";
                }else
                {
                    MessageBox.Show("Şifre Değiştirme İşlemi Yapılamadı!\nKullanıcı Adı, Güvenlik Kodu ve İnternet Bağlantınızı Kontrol Ediniz!","BAŞARISIZ",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }else
            {
                MessageBox.Show("Lütfen Eksik Veri Girişlerini Düzeltiniz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Çalışan Şifre Değiştir Panel Aç
        private void çalışanŞifresiSıfırlaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(panel1.Width == 0 &&panel2.Width == 0)
            {
                tool = "panel2";
                islem = "Aç";
                timer1.Interval = 1;
                timer1.Start();
            }
        }
        #endregion
        #region Çalışan Şifre Değiştir Panel Kapa
        private void button4_Click(object sender, EventArgs e)
        {
            islem = "Kapa";
            tool = "panel2";
            timer1.Interval = 1;
            timer1.Start();
            textBox7.Text = textBox8.Text = textBox9.Text = textBox10.Text = "";
        }
        #endregion
        #region Şifre Göster Çalışan
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if(textBox9.UseSystemPasswordChar == true&&textBox10.UseSystemPasswordChar==true)
            {
                textBox9.UseSystemPasswordChar = textBox10.UseSystemPasswordChar = false;
            }else
            {
                textBox9.UseSystemPasswordChar = textBox10.UseSystemPasswordChar = true;
            }
        }
        #endregion
        #region Şifre Update
        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox7.Text!=""&&textBox8.Text!=""&&textBox9.Text!=""&&textBox10.Text!="")
            {
                if(textBox9.Text == textBox10.Text)
                {
                    if (manager.network_control())
                    {
                        if (manager.update_calisan_pass(textBox7.Text, textBox8.Text, manager.md5_crypto(textBox9.Text)))
                        {
                            MessageBox.Show("Şifreniz Başarıyla Değiştirildi!", "BAŞARILI", MessageBoxButtons.OK);
                            textBox7.Text = textBox8.Text = textBox9.Text = textBox10.Text = "";
                            tool = "panel2";
                            islem = "Kapa";
                            timer1.Interval = 1;
                            timer1.Start();
                        }else
                        {
                            MessageBox.Show("Şifre Değiştirilemedi!","HATA",MessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }else
                {
                    MessageBox.Show("Şifreler Birbirleriyle Uyumsuz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }else
            {
                MessageBox.Show("Lütfen Eksik Bilgileri Tamamlayınız!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Form Closing
        private void LoginScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        #endregion

    }
}
