using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace StokTakipV2
{
    public class MyManager
    {
        MySqlConnection con = new MySqlConnection("Server=sql121.main-hosting.eu;Database=u546323105_stok;Uid=u546323105_root;Pwd=03102593");
        MySqlCommand cmd;
        MySqlDataReader reader = null;
        #region Network Kontrol
        public bool network_control()
        {
            try
            {
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("www.google.com",80);
                client.Close();
                return true;
            }catch(Exception)
            {
                return false;
            }
        }
        #endregion
        #region Md5_Şifreleme
        public string md5_crypto(string pass)
        {
            /*1-MD5 Şifreleme Servisi Nesnesi Oluştur
             * Byte Nesnesi Oluştur verileri byte dönüştür
             * Byte Nesnesinin Hashlerini Hesapla 
             * String Builder Oluştur
             * Diziyi Foreach ile dön x2 ile formatla
             * Geri Döndür
             * */
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] crypto = Encoding.UTF8.GetBytes(pass);
            crypto = md5.ComputeHash(crypto);
            StringBuilder sb = new StringBuilder();
            foreach(byte byt in crypto)
            {
                sb.Append(byt.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
        #endregion
        #region Giriş
        public bool login(string u_name,string u_pass)
        {
            try
            {
                con.OpenAsync();
                cmd = new MySqlCommand("SELECT * FROM users WHERE u_name='"+u_name+"' and u_pass='"+u_pass+"'",con);
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    reader.Close();
                    return true;
                }else
                {
                    reader.Close();
                    return false;
                }
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
        #region Firma Ekleme
        public bool busines_add(string business_name,string admin_name,string admin_pass,string sec_code)
        {
            try
            {
                con.OpenAsync();
                cmd = new MySqlCommand("SELECT * FROM business WHERE business_name='"+business_name+"' and admin_name='"+admin_name+"'",con);
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    reader.Close();
                    return false;
                }else
                {
                    reader.Close();
                    cmd = new MySqlCommand("INSERT INTO business VALUES('"+business_name+"','"+admin_name+"','"+admin_pass+"','"+sec_code+"','False')",con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    return true;
                }
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion
        #region Kullanıcı Tipi Yönetici Modülü
        public bool user_type(string user_name)
        {
            try
            {
                con.OpenAsync();
                cmd = new MySqlCommand("SELECT * FROM business WHERE admin_name='"+user_name+"'",con);
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    reader.Close();
                    return true;
                }else
                {
                    reader.Close();
                    return false;
                }
            }catch(Exception)
            {
                return false;
            }
            finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
        #region Aktivasyon Onay
        public bool activation_state(string u_name)
        {
            try
            {
                bool kontrol = false;
                con.OpenAsync();
                cmd = new MySqlCommand("SELECT activation FROM business WHERE admin_name='"+u_name+"' OR business_name=(SELECT business_name FROM users WHERE u_name='"+u_name+"')",con);
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    while(reader.Read())
                    {
                        if(reader[0].ToString()=="True")
                        {
                            kontrol = true;
                        }
                    }
                    reader.Close();
                    if(kontrol == true)
                    {
                        return true;
                    }else
                    {
                        return false;
                    }
                    
                }else
                {
                    reader.Close();
                    return false;
                }
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
        #region Şirket_Adı
        public string business_name(string u_name)
        {
            try
            {
                string bs_name = "";
                con.OpenAsync();
                cmd = new MySqlCommand("SELECT business_name FROM business WHERE admin_name='"+u_name+"'",con);
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    bs_name = reader[0].ToString();
                }
                reader.Close();
                return bs_name;
            }catch(Exception ex)
            {
                return ex.Message;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
        #region Şirket Adı 2
        public string business_name_two(string u_name)
        {
            //Giirş Yapan Çalışan Bilgisini Çekip O Firmaya Göre Ürünleri Çekecek
            try
            {
                string x="";
                con.Open();
                cmd = new MySqlCommand("SELECT business_name FROM users WHERE u_name='"+u_name+"'",con);
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    x = reader[0].ToString();
                }
                reader.Close();
                return x;
            }catch(Exception ex)
            {
                return ex.Message;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
        #region Çalışan Ekle
        public bool users_add(string u_name,string u_pass,string business_name,string sec_question,string sec_answer)
        {
            try
            {
                con.OpenAsync();
                cmd = new MySqlCommand("SELECT * FROM users WHERE u_name='"+u_name+"'",con);
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    reader.Close();
                    return false;
                }else
                {
                    reader.Close();
                    cmd = new MySqlCommand("INSERT INTO users VALUES('" + u_name + "','" + u_pass + "','" + business_name + "','" + sec_question + "','" + sec_answer + "','" + DateTime.Now.ToString() + "')", con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    return true;
                }
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
        #region Ürün Ekle
        public bool add_stock(string ad,string model,string seri_no,int adet,string ekleyen,int fiyat)
        {
            try
            {
                con.OpenAsync();
                string firma = "";
                cmd = new MySqlCommand("SELECT business_name FROM users WHERE u_name='"+ekleyen+"'",con);
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    firma = reader[0].ToString();
                }
                reader.Close();
                cmd = new MySqlCommand("INSERT INTO stocks VALUES('"+ad+"','"+model+"','"+seri_no+"','"+adet+"','"+DateTime.Now.ToShortDateString().ToString()+"','"+ekleyen+"','"+fiyat+"','"+firma+"')",con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
        #region Ürünleri_Çek Çalışan
        public MySqlDataAdapter stocks_database(string bs_name)
        {
            con.Open();
            cmd = new MySqlCommand("SELECT ürün_adı,ürün_modeli,ürün_seri_no,ürün_adet,ürün_tarih,ürün_ekleyen,ürün_fiyat FROM stocks WHERE firma='"+bs_name+"'",con);
            MySqlDataAdapter x = new MySqlDataAdapter(cmd);
            con.Close();
            return x;           
        }
        #endregion
        #region Ürün Sil Çalışan
        public bool stocks_delete_database(string bs_name,string s_name)
        {
            try
            {
                con.OpenAsync();
                cmd = new MySqlCommand("DELETE FROM stocks WHERE ürün_adı='"+s_name+"' and firma='"+bs_name+"'",con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State== System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
        #region Stok Arama
        public MySqlDataAdapter stock_search(string stock_name,string bs_name)
        {
            con.OpenAsync();
            cmd = new MySqlCommand("SELECT * FROM stocks WHERE ürün_adı='"+stock_name+"' and firma='"+bs_name+"'",con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.CloseAsync();
            }
            return adapter;            
        }
        #endregion
        #region Çalışan Çek
        public MySqlDataAdapter stocks_calisan(string bs_name)
        {
            con.OpenAsync();
            cmd = new MySqlCommand("SELECT * FROM users WHERE business_name='"+bs_name+"'",con);
            MySqlDataAdapter x = new MySqlDataAdapter(cmd);
            if(con.State == System.Data.ConnectionState.Open)
            {
                con.CloseAsync();
            }
            return x;
        }
        #endregion
        #region Çalışan Sil
        public bool delete_database_calisan(string u_name,string bs_name)
        {
            try
            {
                con.OpenAsync();
                cmd = new MySqlCommand("DELETE FROM users WHERE u_name='"+u_name+"' and business_name='"+bs_name+"'",con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
        #region Ürün Veri Güncelle
        public bool update_stocks(string old_name,string new_data,string bs_name,string process)
        {
            try
            {
                con.OpenAsync();
                if(process == "Ürün Adı")
                {
                    cmd = new MySqlCommand("UPDATE stocks SET ürün_adı='"+new_data+"' WHERE ürün_adı='"+old_name+"' and firma='"+bs_name+"'",con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }else if(process == "Ürün Modeli")
                {
                    cmd = new MySqlCommand("UPDATE stocks SET ürün_modeli='"+new_data+ "' WHERE ürün_adı='" + old_name+"'and firma='"+bs_name+"'",con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }else if(process == "Ürün Seri No")
                {
                    cmd = new MySqlCommand("UPDATE stocks SET ürün_seri_no='"+new_data+ "' WHERE ürün_adı='" + old_name+"' and firma='"+bs_name+"' ",con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }else if(process == "Ürün Adet")
                {
                    cmd = new MySqlCommand("UPDATE stocks SET ürün_adet='"+Convert.ToInt32(new_data)+ "' WHERE ürün_adı='"+old_name+"' and firma='"+bs_name+"'", con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    //Ürün Fiyat
                    cmd = new MySqlCommand("UPDATE stocks SET ürün_fiyat='"+Convert.ToInt32(new_data)+"' WHERE ürün_adı='"+old_name+"' and firma ='"+bs_name+"'",con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return true;
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
        #region Son Giriş 
        public bool last_login(string u_name,string last_login)
        {
            try
            {
                con.OpenAsync();
                cmd = new MySqlCommand("UPDATE users SET last_login='" + last_login + "' WHERE u_name='" + u_name + "'", con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }
            catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
            
        }
        #endregion
        #region Ürün_Ekle_Admin
        public bool add_stock_admin(string ad, string model, string seri_no, int adet, string ekleyen, int fiyat)
        {
            try
            {
                con.OpenAsync();
                string firma = "";
                cmd = new MySqlCommand("SELECT business_name FROM business WHERE admin_name='" + ekleyen + "'", con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    firma = reader[0].ToString();
                }
                reader.Close();
                cmd = new MySqlCommand("INSERT INTO stocks VALUES('" + ad + "','" + model + "','" + seri_no + "','" + adet + "','" + DateTime.Now.ToShortDateString().ToString() + "','" + ekleyen + "','" + fiyat + "','" + firma + "')", con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
        #region Admin Giriş
        public bool login_admin(string u_name,string u_pass)
        {
            try
            {
                con.OpenAsync();
                cmd = new MySqlCommand("SELECT * FROM business WHERE admin_name='"+u_name+"'and admin_pass='"+u_pass+"'",con);
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    reader.Close();
                    return true;
                }else
                {
                    reader.Close();
                    return false;
                }
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
        #region Yönetici Şifre Değiştir
        public bool update_admin_pass(string u_name,string sec_code,string new_pass)
        {
            try
            {
                con.OpenAsync();
                cmd = new MySqlCommand("UPDATE business SET admin_pass='"+new_pass+"' WHERE admin_name='"+u_name+"' and sec_code='"+sec_code+"'",con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
        #region Çalışan Şifre Değiştir
        public bool update_calisan_pass(string u_name,string sec_answer,string new_pass)
        {
            try
            {
                con.OpenAsync();
                cmd = new MySqlCommand("UPDATE users SET u_pass='"+new_pass+"' WHERE u_name='"+u_name+"' and sec_answer='"+sec_answer+"'",con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.CloseAsync();
                }
            }
        }
        #endregion
    }
}
