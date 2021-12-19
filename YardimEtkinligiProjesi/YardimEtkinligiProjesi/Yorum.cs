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
    public partial class Yorum : Form
    {
        public Yorum()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Etkinlik;user Id=postgres; password=postgres");

        private void Yorum_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select*from etkinlik", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox2.DisplayMember = "etkinlikAdi";
            comboBox2.ValueMember = "etkinlikId";
            comboBox2.DataSource = dt;

            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter("select*from puanlama", baglanti);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            comboBox1.DisplayMember = "puanlamaAdi";
            comboBox1.ValueMember = "puanlamaId";
            comboBox1.DataSource = dt1;
            baglanti.Close();

            Clean();
        }

        public void Clean()
        {
            textBox2.Text = "";
            numericUpDown1.Value = 0;
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sorgu = "(select \"yorum\".\"yorumId\",\"etkinlik\".\"etkinlikAdi\",\"yorum\".\"yorumAciklamasi\",\"puanlama\".\"puanlamaAdi\",\"yorum\".\"yas\" from \"yorum\" inner join \"puanlama\" on \"puanlama\".\"puanlamaId\"=\"yorum\".\"puanlamaId\"  inner join \"etkinlik\" on \"etkinlik\".\"etkinlikId\"=\"yorum\".\"etkinlikId\" ORDER BY  \"yorum\".\"yorumId\" ASC)";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                MessageBox.Show("Id alanı otomatik belirlenir. Lütfen boş bırakınız.");
            }

            else
            {
                baglanti.Open();
                string sqlEkleme = "INSERT INTO \"yorum\"(\"yorumAciklamasi\",\"puanlamaId\",\"yas\",\"etkinlikId\")" + "VALUES(@p3,@p4,@p5,@p2)";

                NpgsqlCommand kisiEkle = new NpgsqlCommand(sqlEkleme, baglanti);
                kisiEkle.Parameters.AddWithValue("@p2", int.Parse(comboBox2.SelectedValue.ToString()));
                kisiEkle.Parameters.AddWithValue("@p3", textBox2.Text);
                kisiEkle.Parameters.AddWithValue("@p4", int.Parse(comboBox1.SelectedValue.ToString()));
                kisiEkle.Parameters.AddWithValue("@p5", numericUpDown1.Value);
                kisiEkle.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Ekleme işlemi gerçekleşti.");
                Listele();
                Temizle();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string sqlGuncelleme = "UPDATE \"yorum\" SET \"etkinlikId\"=@p2,\"yorumAciklamasi\"=@p3,\"puanlamaId\"=@p4,\"yas\"=@p5 WHERE \"yorumId\"=@p1";

            NpgsqlCommand guncelle = new NpgsqlCommand(sqlGuncelleme, baglanti);
            guncelle.Parameters.AddWithValue("@p1", int.Parse(textBox1.Text));
            guncelle.Parameters.AddWithValue("@p2", int.Parse(comboBox2.SelectedValue.ToString()));
            guncelle.Parameters.AddWithValue("@p3", textBox2.Text);
            guncelle.Parameters.AddWithValue("@p4", int.Parse(comboBox1.SelectedValue.ToString()));
            guncelle.Parameters.AddWithValue("@p5", numericUpDown1.Value);
            guncelle.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Güncelleme işlemi gerçekleşti.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //Listele();
            textBox1.Text = "";
            Listele();
            Temizle();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            int id = int.Parse(textBox1.Text);
            string sqlSilme = "DELETE FROM \"yorum\" WHERE \"yorumId\" =" + id + "";

            DialogResult dr = new DialogResult();
            dr = MessageBox.Show("Silmek istediğinize emin misiniz?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                NpgsqlCommand sil = new NpgsqlCommand(sqlSilme, baglanti);
                sil.ExecuteNonQuery();
                MessageBox.Show("Silme işlemi gerçekleşti.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            baglanti.Close();
            Listele();
            textBox1.Text = "";
            Temizle();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            DataTable dt = new DataTable();
            NpgsqlDataAdapter ara = new NpgsqlDataAdapter("SELECT * FROM \"yorum\" WHERE \"yorumAciklamasi\" LIKE '%" + textBox2.Text + "%' ", baglanti);
            ara.Fill(dt);
            baglanti.Close();
            dataGridView1.DataSource = dt;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Temizle()
        {
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
            textBox1.Text = "";
            textBox2.Text = "";
        }

        public void Listele()
        {
            string sorgu = "(select \"yorum\".\"yorumId\",\"etkinlik\".\"etkinlikAdi\",\"yorum\".\"yorumAciklamasi\",\"puanlama\".\"puanlamaAdi\",\"yorum\".\"yas\" from \"yorum\" inner join \"puanlama\" on \"puanlama\".\"puanlamaId\"=\"yorum\".\"puanlamaId\"  inner join \"etkinlik\" on \"etkinlik\".\"etkinlikId\"=\"yorum\".\"etkinlikId\" ORDER BY  \"yorum\".\"yorumId\" ASC)";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
