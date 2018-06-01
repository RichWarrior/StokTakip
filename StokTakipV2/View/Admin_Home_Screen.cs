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
    public partial class Admin_Home_Screen : Form
    {
        MyManager manager = new MyManager();
        bool activation = false;
        string islem, tool;
        #region Form Initialize
        public Admin_Home_Screen()
        {
            InitializeComponent();
        }
        #endregion
        #region Form_Load
        private void Admin_Home_Screen_Load(object sender, EventArgs e)
        {
            if(manager.network_control())
            {
                if(manager.activation_state(LoginScreen.login_user))
                {
                    activation = true;
                }
                panel_calisan.Width = 0;
                panel3.Width = 0;
                panel4.Width = 0;
                DataTable x = new DataTable();
                manager.stocks_database(manager.business_name(LoginScreen.login_user)).Fill(x);
                dataGridView2.DataSource = x;
                DataTable y = new DataTable();
                manager.stocks_calisan(manager.business_name(LoginScreen.login_user)).Fill(y);
                dataGridView1.DataSource = y;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.SuppressFinalize(x);
                GC.SuppressFinalize(y);
                timer2.Interval = 1000;
                timer2.Start();
                label1.Text = "Merhaba " + LoginScreen.login_user;
                label2.Text = "Şirket Adı:" + manager.business_name(LoginScreen.login_user);
                if(activation == false)
                {
                    chart1.Visible = false;
                    groupBox1.Text = "Satış İstatislikleri Premium Şirketler İçindir!";
                }
            }else
            {
                MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!","BAĞLANTI HATASI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        #endregion
        #region Oturum Kapat Button
        private void button9_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(LoginScreen.login_user+" Oturumu Kapatmak İstediğinize Emin Misiniz?","DİKKAT",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                Form x = new LoginScreen();
                this.Close();
                x.Show();
            }
        }
        #endregion
        #region Yedekle Button
        private void button11_Click(object sender, EventArgs e)
        {
            //Yedekle Buton
            if(activation == false)
            {
                MessageBox.Show("Şuanda Ücretsiz Lisans Kullanıyorsunuz!\nÜcretsiz Lisans İle Bu Özelliği Kullanamazsınız!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }else
            {
                //İşleme Devam
            }
        }
        #endregion
        #region Bildirim Butonu
        private void button10_Click(object sender, EventArgs e)
        {
            //Bildirim Gönder
            if (activation == false)
            {
                MessageBox.Show("Şuanda Ücretsiz Lisans Kullanıyorsunuz!\nÜcretsiz Lisans İle Bu Özelliği Kullanamazsınız!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //İşleme Devam
            }
        }
        #endregion
        #region Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(islem == "Aç")
            {
                if(tool=="panel_calisan")
                {
                    panel_calisan.Width += 5;
                   if(panel_calisan.Width == 400)
                    {
                        timer1.Stop();
                        textBox4.Text = manager.business_name(LoginScreen.login_user);
                    }
                }else if(tool=="panel3")
                {
                    panel3.Width += 5;
                    if(panel3.Width == 400)
                    {
                        timer1.Stop();
                    }
                }else if(tool=="panel4")
                {
                    panel4.Width += 5;
                    if(panel4.Width == 300)
                    {
                        timer1.Stop();
                    }
                }
            }else
            {
                if(tool == "panel_calisan")
                {
                    panel_calisan.Width -= 5;
                    if(panel_calisan.Width == 0)
                    {
                        timer1.Stop();
                    }
                }
                else if (tool == "panel3")
                {
                    panel3.Width -= 5;
                    if (panel3.Width == 0)
                    {
                        timer1.Stop();
                    }
                }else if(tool == "panel4")
                {
                    panel4.Width -= 5;
                    if(panel4.Width == 0)
                    {
                        timer1.Stop();
                    }
                }
            }
        }
        #endregion
        #region Şifremi Göster
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(textBox3.UseSystemPasswordChar == true)
            {
                textBox3.UseSystemPasswordChar = false;
            }else
            {
                textBox3.UseSystemPasswordChar = true;
            }
        }
        #endregion
        #region Çalışan Ekle Panel Kapatma
        private void button13_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";
            comboBox1.SelectedIndex = -1;
            tool = "panel_calisan";
            islem = "Kapa";
            timer1.Interval = 1;
            timer1.Start();
        }
        #endregion
        #region Çalışan Ekle Button
        private void button14_Click(object sender, EventArgs e)
        {
            if(textBox2.Text!=""&&textBox3.Text!=""&&textBox4.Text!=""&&textBox5.Text!=""&&comboBox1.SelectedIndex!=-1)
            {
                if(manager.network_control())
                {
                    if(manager.users_add(textBox2.Text,manager.md5_crypto(textBox3.Text),textBox4.Text,comboBox1.Text,textBox5.Text))
                    {
                        MessageBox.Show(textBox2.Text + " Adlı Çalışanınız Eklendi!", "BAŞARILI", MessageBoxButtons.OK);
                        textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";
                        comboBox1.SelectedIndex = -1;
                        tool = "panel_calisan";
                        islem = "Kapa";
                        timer1.Interval = 1;
                        timer1.Start();
                    }
                    else
                    {
                        MessageBox.Show("Çalışan Eklenemedi!\nBöyle Bir Çalışan Olabilir","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }else
                {
                    MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!", "BAĞLANTI HATASI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("Lütfen Eksik Kısımları Doldurunuz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Destek Bölümü
        private void button12_Click(object sender, EventArgs e)
        {
            if(activation == false)
            {
                MessageBox.Show("Şirketiniz Ücretsiz Lisans Kullanıyor!\nBu Özelliği Kullanamazsınız!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }else
            {
                //Devam Et
            }
        }
        #endregion
        #region Çalışan Sil
        private void button7_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedCells.Count>0)
            {
               if(manager.network_control())
                {
                    DialogResult result = MessageBox.Show(dataGridView1.CurrentRow.Cells["u_name"].Value.ToString()+" Adlı Çalışanı Silmek İstiyor Musun?","DİKKAT",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                    if(result == DialogResult.Yes)
                    {
                        if (manager.delete_database_calisan(dataGridView1.CurrentRow.Cells["u_name"].Value.ToString(), manager.business_name(LoginScreen.login_user)))
                        {
                            MessageBox.Show(dataGridView1.CurrentRow.Cells["u_name"].Value.ToString() + " Adlı Çalışan Başarıyla Silindi!", "BAŞARILI", MessageBoxButtons.OK);
                        }
                    }
                }else
                {
                    MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!","BAĞLANTI HATASI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    Application.Exit();
                }
            }else
            {
                MessageBox.Show("Lütfen Silinecek Çalışanı Seçiniz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Yenile Bölümü
        private void button15_Click(object sender, EventArgs e)
        {
            if(manager.network_control())
            {
                DataTable x = new DataTable();
                manager.stocks_database(manager.business_name(LoginScreen.login_user)).Fill(x);
                dataGridView2.DataSource = x;
                DataTable y = new DataTable();
                manager.stocks_calisan(manager.business_name(LoginScreen.login_user)).Fill(y);
                dataGridView1.DataSource = y;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.SuppressFinalize(x);
                GC.SuppressFinalize(y);
            }
            else
            {
                MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!", "BAĞLANTI HATASI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        #endregion
        #region Arama Bölümü
        private void button1_Click(object sender, EventArgs e)
        {
           if(textBox1.Text!="")
            {
                if(manager.network_control())
                {
                    DataTable x = new DataTable();
                    manager.stock_search(textBox1.Text,manager.business_name(LoginScreen.login_user)).Fill(x);
                    dataGridView2.DataSource = x;
                }else
                {
                    MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!","BAĞLANTI HATASI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    Application.Exit();
                }
            }else
            {
                MessageBox.Show("Lütfen Aranacak Ürünün Adını Giriniz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Temizle
        private void button2_Click(object sender, EventArgs e)
        {
            if(manager.network_control())
            {
                textBox1.ResetText();
                DataTable x = new DataTable();
                manager.stocks_database(manager.business_name(LoginScreen.login_user)).Fill(x);
                dataGridView2.DataSource = x;
            }else
            {
                MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!", "BAĞLANTI HATASI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        #endregion
        #region Ürün Sil
        private void button5_Click(object sender, EventArgs e)
        {
            if(dataGridView2.SelectedCells.Count>0)
            {
                if(manager.network_control())
                {
                    DialogResult result = MessageBox.Show(dataGridView2.CurrentRow.Cells["ürün_adı"].Value.ToString()+" Adlı Ürünü Silmek İstiyor Musun?","DİKKAT",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                    if(result == DialogResult.Yes)
                    {
                        manager.stocks_delete_database(manager.business_name(LoginScreen.login_user),dataGridView2.CurrentRow.Cells["ürün_adı"].Value.ToString());
                        MessageBox.Show(dataGridView2.CurrentRow.Cells["ürün_adı"].Value.ToString()+" Adlı Ürün Başarıyla Silindi!","BAŞARILI",MessageBoxButtons.OK);
                    }
                }else
                {
                    MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!","BAĞLANTI HATASI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    Application.Exit();
                }
            }else
            {
                MessageBox.Show("Lütfen Silinecek Ürünü Seçiniz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Timer2 Tarih Sayacı
        private void timer2_Tick(object sender, EventArgs e)
        {
            lbl_tarih.Text = "Tarih:"+DateTime.Now.ToString();
        }
        #endregion
        #region Ürün Ekleme Panel Aç Kapa
        private void button4_Click(object sender, EventArgs e)
        {
            if(panel3.Width == 0&&panel_calisan.Width == 0&&panel4.Width==0)
            {
                islem = "Aç";
                tool = "panel3";
                timer1.Interval = 1;
                timer1.Start();
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            islem = "Kapa";
            tool = "panel3";
            timer1.Interval = 1;
            timer1.Start();
        }
        #endregion
        #region Ürün Ekle
        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if(textBox6.Text!=""&&textBox7.Text!=""&&textBox8.Text!=""&&textBox9.Text!=""&&textBox10.Text!="")
            {
                if(manager.network_control())
                {
                    if(manager.add_stock_admin(textBox10.Text,textBox9.Text,textBox8.Text,Convert.ToInt32(textBox7.Text),LoginScreen.login_user,Convert.ToInt32(textBox6.Text)))
                    {
                        MessageBox.Show(textBox10.Text+" Adlı Ürün Başarıyla Eklendi!","BAŞARILI",MessageBoxButtons.OK);
                        textBox10.Text = textBox9.Text = textBox8.Text = textBox7.Text = textBox6.Text = "";
                        tool = "panel3";
                        islem = "Kapa";
                        timer1.Interval = 1;
                        timer1.Start();
                    }
                }else
                {
                    MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!","BAĞLANTI HATASI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
        }
        #endregion
        #region Ürün Güncelle Panel Açma Kapama
        private void button6_Click(object sender, EventArgs e)
        {
            if (panel3.Width == 0 && panel4.Width == 0&& panel_calisan.Width == 0)
            {
                
                if(dataGridView2.SelectedRows.Count>0)
                {
                    tool = "panel4";
                    islem = "Aç";
                    timer1.Interval = 1;
                    timer1.Start();
                    textBox12.Text = dataGridView2.CurrentRow.Cells["ürün_adı"].Value.ToString();
                }else
                {
                    MessageBox.Show("Lütfen Güncellenecek Ürünü Seçiniz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            textBox11.ResetText();
            tool = "panel4";
            islem = "Kapa";
            timer1.Interval = 1;
            timer1.Start();
        }
        #endregion
        #region RadioButton Checked Changed
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton4.Checked==true||radioButton5.Checked== true)
            {
                MessageBox.Show("Lütfen Aşağıya Tam Sayı Bir Veri Giriniz!","UYARI",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                textBox11.MaxLength = 6;
            }else
            {
                textBox11.MaxLength = 50;
            }
            islem = (sender as RadioButton).Text;
        }
        #endregion
        #region Güncelleme İşlemi
        private void button18_Click(object sender, EventArgs e)
        {
            if(textBox11.Text!="")
            {
                if(radioButton1.Checked == true||radioButton2.Checked == true||radioButton3.Checked == true||radioButton4.Checked==true||radioButton5.Checked==true)
                {
                    if(manager.network_control())
                    {
                        if(manager.update_stocks(textBox12.Text,textBox11.Text,manager.business_name(LoginScreen.login_user),islem))
                        {
                            MessageBox.Show(textBox12.Text + "Adlı Veri Seçiminize Göre "+textBox11.Text+" Olarak Güncellendi!","BAŞARILI",MessageBoxButtons.OK);
                            textBox11.Text = "";
                            tool = "panel4";
                            islem = "Kapa";
                            timer1.Interval = 1;
                            timer1.Start();
                        }
                    }else
                    {
                        MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!","BAĞLANTI HATASI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }else
                {
                    MessageBox.Show("Lütfen Güncelleme İşlemini Seçiniz","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }else
            {
                MessageBox.Show("Lütfen Güncellenecek Veriyi Yazınız!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Form_Closing
        private void Admin_Home_Screen_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form x = new LoginScreen();
            x.Show();
        }
        #endregion
        #region Çalışan Ekle Panel Açılma
        private void button3_Click(object sender, EventArgs e)
        {
            if(panel_calisan.Width == 0&&panel3.Width == 0&&panel4.Width==0)
            {
                timer1.Interval = 1;
                timer1.Start();
                islem = "Aç";
                tool = "panel_calisan";
            }
            else
            {
                MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!", "BAĞLANTI HATASI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        #endregion
    }
}
