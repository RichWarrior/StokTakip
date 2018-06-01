using MySql.Data.MySqlClient;
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
    public partial class HomeScreen : Form
    {
        //MessageBox.Show(dataGridView1.CurrentRow.Cells["ürün_adı"].Value.ToString());
        //MessageBox.Show(dataGridView1.SelectedCells.Count.ToString());
        MyManager manager = new MyManager();
        bool activasyon = false;
        string panel_name, process;
        #region Form Initialize
        public HomeScreen()
        {
            InitializeComponent();
        }
        #endregion
        #region Database Veri Çek
        void database_stocks()
        {
            DataTable table = new DataTable();
            manager.stocks_database(manager.business_name_two(LoginScreen.login_user)).Fill(table);
            dataGridView1.DataSource = table;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(table);
        }
        #endregion
        #region Form_Load
        private void HomeScreen_Load(object sender, EventArgs e)
        {
            if (!manager.network_control())
            {
                MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!","BAĞLANTI HATASI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Application.Exit();
            }else
            {
                    lbl_username.Text = "Merhaba "+LoginScreen.login_user;
                    timer1.Interval = 1000;
                    timer1.Start();
                    if(manager.activation_state(LoginScreen.login_user))
                    {
                        activasyon = true;
                    }
                    database_stocks();
                    panel3.Width = 0;
              
            }
        }
        #endregion
        #region Tarih Sayacı
        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_tarih.Text = DateTime.Now.ToString();
        }
        #endregion
        #region Oturum Kapat Button
        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(LoginScreen.login_user+" Oturumu Kapatmak İstiyor Musun?","DİKKAT",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                if(manager.network_control()&&manager.last_login(LoginScreen.login_user,DateTime.Now.ToString()))
                {
                    Form x = new LoginScreen();
                    this.Close();
                    x.Show();
                }
                else
                {
                    MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!","BAĞLANTI HATASI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }
        #endregion
        #region Ara Butonu
        private void button1_Click(object sender, EventArgs e)
        {
            //Ara
            if(manager.network_control())
            {
                DataTable x = new DataTable();
                manager.stock_search(textBox5.Text,manager.business_name_two(LoginScreen.login_user)).Fill(x);
                dataGridView1.DataSource = x;
            }else
            {
                MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamdı!","BAĞLANTI HATASI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        #endregion
        #region Satış Yap
        private void button7_Click(object sender, EventArgs e)
        {
            if(activasyon == false)
            {
                MessageBox.Show("Şirketiniz Ücretsiz Lisans Kullanmakta Bu Sebepten Dolayı Satış Yapamıyorsunuz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }else
            {
                //Devam Et
            }
        }
        #endregion
        #region Ürün Ekle
        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if(manager.network_control())
            {
                if(textBox1.Text!=""&&textBox2.Text!=""&&textBox3.Text!=""&&textBox4.Text!=""&&textBox6.Text!="")
                {
                    if(manager.add_stock(textBox1.Text,textBox2.Text,textBox3.Text,Convert.ToInt32(textBox4.Text),LoginScreen.login_user,Convert.ToInt32(textBox6.Text)))
                    {
                        MessageBox.Show(textBox1.Text+" Adlı Ürün Başarıyla Eklendi!","BAŞARILI",MessageBoxButtons.OK);
                        textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox6.Text = "";
                    }else
                    {
                        MessageBox.Show("Ürün Eklenemedi Bir Hata Oluştu!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen Eksik Kısımları Doldurunuz!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!", "BAĞLANTI HATASI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        #endregion
        #region İstek Öneri
        private void button4_Click(object sender, EventArgs e)
        {
            if (activasyon == false)
            {
                MessageBox.Show("Şirketiniz Ücretsiz Lisans Kullanmakta Bu Sebepten Dolayı Satış Yapamıyorsunuz!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //Devam Et
            }
        }
        #endregion
        #region Yenile Butonu
        private void button8_Click(object sender, EventArgs e)
        {
            database_stocks();
        }
        #endregion
        #region Sil Butonu
        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count>0)
            {
                try
                {
                    DialogResult result = MessageBox.Show(dataGridView1.CurrentRow.Cells["ürün_adı"].Value.ToString() + " Adlı Ürünü Silmek İstiyor Musunuz?", "DİKKAT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        //Silme İşlemiYap
                        if (manager.stocks_delete_database(manager.business_name_two(LoginScreen.login_user), dataGridView1.CurrentRow.Cells["ürün_adı"].Value.ToString()))
                        {
                            MessageBox.Show(dataGridView1.CurrentRow.Cells["ürün_adı"].Value.ToString() + " Adlı Ürün Başarıyla Silindi!", "BAŞARILI", MessageBoxButtons.OK);
                        }
                    }
                }catch(Exception)
                {
                    MessageBox.Show("Lütfen Dolu Bir Satır Seçiniz!","HATA",MessageBoxButtons.OK);
                }

            }
        }
        #endregion
        #region Temizle Butonu
        private void button6_Click(object sender, EventArgs e)
        {
            if(manager.network_control())
            {
                textBox5.Text = "";
                database_stocks();
            }else
            {
                MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!","BAĞLANTI HATASI",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Panel Açma Timer
        private void timer2_Tick(object sender, EventArgs e)
        {
            if(process=="Aç")
            {
                if(panel_name == "panel3")
                {
                    panel3.Width += 5;
                    if(panel3.Width == 300)
                    {
                        timer2.Stop();
                    }
                }
            }else
            {
                if(panel_name=="panel3")
                {
                    panel3.Width -= 5;
                    if(panel3.Width == 0)
                    {
                        timer2.Stop();
                    }
                }
            }
        }
        #endregion
        #region Güncelle Panel Kapat
        private void button10_Click(object sender, EventArgs e)
        {
            panel_name = "panel3";
            process = "Kapa";
            timer2.Interval = 1;
            timer2.Start();
        }
        #endregion
        #region Güncelleme İşlemi
        private void button9_Click(object sender, EventArgs e)
        {
            if(textBox8.Text!="")
            {
                if(manager.network_control())
                {
                    if(radioButton1.Checked == true || radioButton2.Checked==true||radioButton3.Checked==true||radioButton4.Checked==true||radioButton5.Checked==true)
                    {
                        if (manager.update_stocks(textBox7.Text, textBox8.Text, manager.business_name_two(LoginScreen.login_user), process))
                        {
                            MessageBox.Show(textBox7.Text + " Adlı Ürün " + process + " İşlemi Göre Yeni Veri " + textBox8.Text + " Olarak Güncellendi!", "BAŞARILI", MessageBoxButtons.OK);
                            process = "Kapa";
                            panel_name ="panel3";
                            timer2.Interval = 1;
                            timer2.Start();
                            textBox8.ResetText();
                        }
                        else
                        {
                            MessageBox.Show("Güncelleme İşlemi Yapılamadı!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lütfen Bir İşlem Seçiniz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }else
                {
                    MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!","BAĞLANTI HATASI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
        }
        #endregion
        #region Update RadioButton CheckedChanged Metod
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton4.Checked == true || radioButton5.Checked == true)
            {
                MessageBox.Show("Lütfen Aşağıda Bulunan Kutuya Tam Sayı Bir Değer Yazınız!","UYARI",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                textBox8.MaxLength = 6;
            }else
            {
                textBox8.MaxLength = 50;
            }
            process = (sender as RadioButton).Text;
        }
        #endregion
        #region Form Kapanıyor ve Last Login Update
        private void HomeScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(manager.network_control())
            {
                if(manager.last_login(LoginScreen.login_user,DateTime.Now.ToString()))
                {
                    Form x = new LoginScreen();
                    x.Show();
                }
            }else
            {
                Application.Exit();
            }
        }
        #endregion
        #region Güncelle Butonu
        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count>0&&panel3.Width == 0)
            {
                panel_name = "panel3";
                process = "Aç";
                timer2.Start();
                timer2.Interval = 1;
                try
                {
                    textBox7.Text = dataGridView1.CurrentRow.Cells["ürün_adı"].Value.ToString();
                }catch(Exception)
                {
                    textBox7.Text = "Lütfen Dolu Satır Seçiniz!";
                }
            }
        }
        #endregion
    }
}
