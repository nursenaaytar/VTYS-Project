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
    public partial class Bagis : Form
    {
        public Bagis()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Etkinlik;user Id=postgres; password=postgres");

        private void Bagis_Load(object sender, EventArgs e)
        {
            Temizle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sorgu = "(select \"bagis\".\"bagisId\",\"bagis\".\"miktar\"  from \"bagis\" ORDER BY \"bagis\".\"bagisId\" ASC)";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

            string sorgu1 = "(select \"toplambagis\".\"toplam\"  from \"toplambagis\")";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sorgu1, baglanti);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            dataGridView2.DataSource = ds1.Tables[0];


            Temizle();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                MessageBox.Show("Id alanı otomatik belirlenir. Lütfen boş bırakınız.");
            }

            else
            {
                baglanti.Open();
                string sqlEkleme = "INSERT INTO \"bagis\"(\"miktar\")" + "VALUES(@p2)";

                NpgsqlCommand kisiEkle = new NpgsqlCommand(sqlEkleme, baglanti);
                kisiEkle.Parameters.AddWithValue("@p2", double.Parse(textBox1.Text));

                kisiEkle.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Ekleme işlemi gerçekleşti.");
                Temizle();
                Listele();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string sqlGuncelleme = "UPDATE \"bagis\" SET \"miktar\"=@p2 WHERE \"bagisId\"=@p1";

            NpgsqlCommand guncelle = new NpgsqlCommand(sqlGuncelleme, baglanti);
            guncelle.Parameters.AddWithValue("@p1", int.Parse(textBox2.Text));
            guncelle.Parameters.AddWithValue("@p2", double.Parse(textBox1.Text));

            guncelle.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Güncelleme işlemi gerçekleşti.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textBox2.Text = "";
            Temizle();
            Listele();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            int id = int.Parse(textBox2.Text);
            string sqlSilme = "DELETE FROM \"bagis\" WHERE \"bagisId\" =" + id + "";

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
            
            NpgsqlDataAdapter ara = new NpgsqlDataAdapter("SELECT * FROM \"bagis\" WHERE \"miktar\" LIKE '%" + textBox1.Text.ToString() + "%' ", baglanti);
            ara.Fill(dt);
            baglanti.Close();
            dataGridView1.DataSource = dt;
            Temizle();
            Listele();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Temizle()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        public void Listele()
        {
            string sorgu = "(select \"bagis\".\"bagisId\",\"bagis\".\"miktar\"  from \"bagis\" ORDER BY \"bagis\".\"bagisId\" ASC)";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

            string sorgu1 = "(select \"toplambagis\".\"toplam\"  from \"toplambagis\")";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sorgu1, baglanti);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            dataGridView2.DataSource = ds1.Tables[0];
        }
    }
    }

