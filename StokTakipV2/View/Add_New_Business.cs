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
    public partial class Add_New_Business : Form
    {
        MyManager manager = new MyManager();
        #region Form Initialize
        public Add_New_Business()
        {
            InitializeComponent();
        }
        #endregion
        #region Form_Çıkış
        private void Add_New_Business_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if(e.Cancel == true)
            {
                Form x = new LoginScreen();
                this.Hide();
                x.Show();
            }
        }
        #endregion
        #region Form_Load
        private void Add_New_Business_Load(object sender, EventArgs e)
        {
            if(!manager.network_control())
            {
                MessageBox.Show("Aktif Bir İnternet Bağlantısı Bulunamadı!", "BAĞLANTI HATASI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }else
            {
                textBox1.Focus();
            }
            
        }
        #endregion
        #region Firma Ekleme
        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != ""&&textBox4.Text!="")
            {
                if (manager.busines_add(textBox1.Text, textBox2.Text, manager.md5_crypto(textBox3.Text),textBox4.Text))
                {
                    MessageBox.Show(textBox1.Text + " Adlı Firma Sisteme Kayıt Edildi!\nGüvenlik Kodunuzu Unutmayınız Aksi Takdirde Şifre Sıfırlama İşlemlerinizi Yapamazsınız!", "BAŞARILI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form x = new LoginScreen();
                    this.Close();
                    x.Show();
                }
                else
                {
                    MessageBox.Show("Firma Eklenemedi!\nBöyle Bir Firma Sisteme Zaten Kayıtlı Olabilir!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Eksik Verileri Lütfen Doldurunuz!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Şifremi Göster Bölümü
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
    }
}
