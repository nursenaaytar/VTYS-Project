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
    public partial class EtkinlikTalebi : Form
    {
        public EtkinlikTalebi()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Etkinlik;user Id=postgres; password=postgres");

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            int id = int.Parse(textBox3.Text);
            string sqlSilme = "DELETE FROM \"etkinliktalebi\" WHERE \"etkinlikTalebiId\" =" + id + "";

            DialogResult dr = new DialogResult();
            dr = MessageBox.Show("Silmek istediğinize emin misiniz?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                NpgsqlCommand sil = new NpgsqlCommand(sqlSilme, baglanti);
                sil.ExecuteNonQuery();
                MessageBox.Show("Silme işlemi gerçekleşti.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            baglanti.Close();
            Getir();
        }

        private void EtkinlikTalebi_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select*from etkinlikturu", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "etkinlikTuruAdi";
            comboBox1.ValueMember = "etkinlikTuruId";
            comboBox1.DataSource = dt;
            baglanti.Close();
            Temizle(); 
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                MessageBox.Show("Id alanı otomatik belirlenir. Lütfen boş bırakınız.");
            }

            else
            {
                baglanti.Open();
                string sqlEkleme = "INSERT INTO \"etkinliktalebi\"(\"etkinlikTalepAdi\",\"yorum\",\"etkinlikTuruId\")" + "VALUES(@p2,@p3,@p4)";

                NpgsqlCommand ekle1 = new NpgsqlCommand(sqlEkleme, baglanti);
                ekle1.Parameters.AddWithValue("@p2", textBox1.Text);
                ekle1.Parameters.AddWithValue("@p3", textBox2.Text);
                ekle1.Parameters.AddWithValue("@p4", int.Parse(comboBox1.SelectedValue.ToString()));
                ekle1.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Ekleme işlemi gerçekleşti."); ;
                Getir();
                Temizle();
            }

        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            string sorgu = "(select \"etkinliktalebi\".\"etkinlikTalebiId\",\"etkinliktalebi\".\"etkinlikTalepAdi\",\"etkinliktalebi\".\"yorum\",\"etkinlikturu\".\"etkinlikTuruAdi\" from \"etkinliktalebi\" inner join \"etkinlikturu\" on \"etkinlikturu\".\"etkinlikTuruId\"=\"etkinliktalebi\".\"etkinlikTuruId\"  ORDER BY \"etkinliktalebi\".\"etkinlikTalebiId\" ASC)";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            DataTable dt = new DataTable();
            NpgsqlDataAdapter ara = new NpgsqlDataAdapter("SELECT * FROM \"etkinliktalebi\" WHERE \"etkinlikTalepAdi\" LIKE '%" + textBox1.Text + "%' ", baglanti);
            ara.Fill(dt);
            baglanti.Close();
            dataGridView1.DataSource = dt;
            Temizle();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string sqlGuncelleme = "UPDATE \"etkinliktalebi\" SET \"etkinlikTalepAdi\"=@p2,\"yorum\"=@p3,\"etkinlikTuruId\"=@p4 WHERE \"etkinlikTalebiId\"=@p1";

            NpgsqlCommand guncelle = new NpgsqlCommand(sqlGuncelleme, baglanti);
            guncelle.Parameters.AddWithValue("@p1", int.Parse(textBox3.Text));
            guncelle.Parameters.AddWithValue("@p2", textBox1.Text);
            guncelle.Parameters.AddWithValue("@p3", textBox2.Text);
            guncelle.Parameters.AddWithValue("@p4", int.Parse(comboBox1.SelectedValue.ToString()));
            guncelle.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Güncelleme işlemi gerçekleşti.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Getir();
            Temizle();
        }
        public void Getir()
        {
            string sorgu = "(select \"etkinliktalebi\".\"etkinlikTalebiId\",\"etkinliktalebi\".\"etkinlikTalepAdi\",\"etkinliktalebi\".\"yorum\",\"etkinlikturu\".\"etkinlikTuruAdi\" from \"etkinliktalebi\" inner join \"etkinlikturu\" on \"etkinlikturu\".\"etkinlikTuruId\"=\"etkinliktalebi\".\"etkinlikTuruId\" ORDER BY \"etkinliktalebi\".\"etkinlikTalebiId\" ASC )";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
        public void Temizle()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedItem= null;
            textBox3.Text = "";
        }
    }
}
