using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YardimEtkinligiProjesi
{
    public partial class MekanKaydi : Form
    {
        public MekanKaydi()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Etkinlik;user Id=postgres; password=postgres");

        private void MekanKaydi_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select*from ulke", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "ulkeAd";
            comboBox1.ValueMember = "ulkeId";
            comboBox1.DataSource = dt;

            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter("select*from sehir", baglanti);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            comboBox2.DisplayMember = "sehirAd";
            comboBox2.ValueMember = "sehirId";
            comboBox2.DataSource = dt1;

            NpgsqlDataAdapter da2 = new NpgsqlDataAdapter("select*from ilce", baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            comboBox3.DisplayMember = "ilceAd";
            comboBox3.ValueMember = "ilceId";
            comboBox3.DataSource = dt2;
            baglanti.Close();
            Temizle();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            int id = int.Parse(textBox2.Text);
            string sqlSilme = "DELETE FROM \"adres\" WHERE \"adresId\" =" + id + "";

            DialogResult dr = new DialogResult();
            dr = MessageBox.Show("Silmek istediğinize emin misiniz?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                NpgsqlCommand sil = new NpgsqlCommand(sqlSilme, baglanti);
                sil.ExecuteNonQuery();
                MessageBox.Show("Silme işlemi gerçekleşti.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            baglanti.Close();
            Temizle();
            Listele();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            DataTable dt = new DataTable();
            NpgsqlDataAdapter ara = new NpgsqlDataAdapter("SELECT * FROM \"adres\" WHERE \"mekanAdi\" LIKE '%" + textBox1.Text + "%' ", baglanti);
            ara.Fill(dt);
            baglanti.Close();
            dataGridView1.DataSource = dt;
            Temizle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string sorgu = "(select \"adres\".\"adresId\",\"adres\".\"mekanAdi\",\"ulke\".\"ulkeAd\",\"sehir\".\"sehirAd\",\"ilce\".\"ilceAd\" from \"adres\" inner join \"ulke\" on \"ulke\".\"ulkeId\"=\"adres\".\"ulkeId\" inner join \"sehir\" on \"sehir\".\"sehirId\"=\"adres\".\"ilId\" inner join \"ilce\" on \"ilce\".\"ilceId\"=\"ilce\".\"ilceId\")";
            string sorgu = "(select \"adres\".\"adresId\",\"adres\".\"mekanAdi\",\"ulke\".\"ulkeAd\",\"sehir\".\"sehirAd\" ,\"ilce\".\"ilceAd\" from \"adres\" inner join \"ulke\" on \"ulke\".\"ulkeId\"=\"adres\".\"ulkeId\" inner join \"sehir\" on \"sehir\".\"sehirId\"=\"adres\".\"ilId\" inner join \"ilce\" on \"ilce\".\"ilceId\"=\"adres\".\"ilceId\" ORDER BY \"adres\".\"adresId\" ASC)";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ekleme işlemi gerçekleşti.");

            {
                if (textBox2.Text != "")
                {
                    MessageBox.Show("Id alanı otomatik belirlenir. Lütfen boş bırakınız.");
                }

                else
                {
                    baglanti.Open();
                    string sqlEkleme = "INSERT INTO \"adres\"(\"mekanAdi\",\"ulkeId\",\"ilId\",\"ilceId\")" + "VALUES(@p2,@p3,@p4,@p5)";

                    NpgsqlCommand kisiEkle = new NpgsqlCommand(sqlEkleme, baglanti);
                    kisiEkle.Parameters.AddWithValue("@p2", textBox1.Text);
                    kisiEkle.Parameters.AddWithValue("@p3", int.Parse(comboBox1.SelectedValue.ToString()));
                    kisiEkle.Parameters.AddWithValue("@p4", int.Parse(comboBox2.SelectedValue.ToString()));
                    kisiEkle.Parameters.AddWithValue("@p5", int.Parse(comboBox3.SelectedValue.ToString()));

                    kisiEkle.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Ekleme işlemi gerçekleşti.");
                    Temizle(); 
                    Listele();
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string sqlGuncelleme = "UPDATE \"adres\" SET \"mekanAdi\"=@p2,\"ulkeId\"=@p3,\"ilId\"=@p4,\"ilceId\"=@p5 WHERE \"adresId\"=@p1";

            NpgsqlCommand guncelle = new NpgsqlCommand(sqlGuncelleme, baglanti);
            guncelle.Parameters.AddWithValue("@p1", int.Parse(textBox2.Text));
            guncelle.Parameters.AddWithValue("@p2", textBox1.Text);
            guncelle.Parameters.AddWithValue("@p3", int.Parse(comboBox1.SelectedValue.ToString()));
            guncelle.Parameters.AddWithValue("@p4", int.Parse(comboBox2.SelectedValue.ToString()));
            guncelle.Parameters.AddWithValue("@p5", int.Parse(comboBox3.SelectedValue.ToString()));

            guncelle.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Güncelleme işlemi gerçekleşti.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Temizle();
            Listele();
        }
        public void Temizle()
        {
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
            comboBox3.SelectedItem = null;
            textBox1.Text = "";
            textBox2.Text = "";
        }
        public void Listele()
        {
            //string sorgu = "(select \"adres\".\"adresId\",\"adres\".\"mekanAdi\",\"ulke\".\"ulkeAd\",\"sehir\".\"sehirAd\",\"ilce\".\"ilceAd\" from \"adres\" inner join \"ulke\" on \"ulke\".\"ulkeId\"=\"adres\".\"ulkeId\" inner join \"sehir\" on \"sehir\".\"sehirId\"=\"adres\".\"ilId\" inner join \"ilce\" on \"ilce\".\"ilceId\"=\"ilce\".\"ilceId\")";
            string sorgu = "(select \"adres\".\"adresId\",\"adres\".\"mekanAdi\",\"ulke\".\"ulkeAd\",\"sehir\".\"sehirAd\" ,\"ilce\".\"ilceAd\" from \"adres\" inner join \"ulke\" on \"ulke\".\"ulkeId\"=\"adres\".\"ulkeId\" inner join \"sehir\" on \"sehir\".\"sehirId\"=\"adres\".\"ilId\" inner join \"ilce\" on \"ilce\".\"ilceId\"=\"adres\".\"ilceId\" ORDER BY \"adres\".\"adresId\" ASC)";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
