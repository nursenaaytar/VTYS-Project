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
    public partial class EtkinlikKaydi : Form
    {
        public EtkinlikKaydi()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Etkinlik;user Id=postgres; password=postgres");

        private void btnListele_Click(object sender, EventArgs e)
        {
            string sorgu = "(select \"etkinlik\".\"etkinlikId\",\"etkinlik\".\"etkinlikAdi\",\"etkinlik\".\"etkinlikTarihi\",\"etkinlik\".\"biletFiyati\" ,\"etkinlikturu\".\"etkinlikTuruAdi\" ,\"etkinlik\".\"biletStok\" from \"etkinlik\" inner join \"etkinlikturu\" on \"etkinlikturu\".\"etkinlikTuruId\"=\"etkinlik\".\"etkinlikTuruId\" ORDER BY \"etkinlik\".\"etkinlikId\" ASC )";
            //select "etkinlikId","etkinlikAdi","etkinlikTarihi","biletFiyati",kdvl("biletFiyati") from public.etkinlik

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            //Listele();
        }

        private void EtkinlikKaydi_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select*from etkinlikturu", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbTur.DisplayMember = "etkinlikTuruAdi";
            cmbTur.ValueMember = "etkinlikTuruId";
            cmbTur.DataSource = dt;
            baglanti.Close();
            Sil();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (txtId.Text != "")
            {
                MessageBox.Show("Id alanı otomatik belirlenir. Lütfen boş bırakınız.");
            }

            else
            {
                baglanti.Open();
                string sqlEkleme = "INSERT INTO \"etkinlik\"(\"etkinlikAdi\",\"etkinlikTarihi\",\"biletFiyati\",\"etkinlikTuruId\",\"biletStok\")" + "VALUES(@p2,@p3,@p4,@p5,@p6)";

                NpgsqlCommand etkinlikEkle = new NpgsqlCommand(sqlEkleme, baglanti);
                etkinlikEkle.Parameters.AddWithValue("@p2", txtAdi.Text);
                etkinlikEkle.Parameters.AddWithValue("@p3", dateTimePicker1.Value);
                etkinlikEkle.Parameters.AddWithValue("@p4", double.Parse(textBox1.Text));
                etkinlikEkle.Parameters.AddWithValue("@p5", int.Parse(cmbTur.SelectedValue.ToString()));
                etkinlikEkle.Parameters.AddWithValue("@p6", nuStok.Value);
                etkinlikEkle.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Ekleme işlemi gerçekleşti.");
                Listele();
                Sil();
                //Listele();
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string sqlGuncelleme = "UPDATE \"etkinlik\" SET \"etkinlikAdi\"=@p2,\"biletFiyati\"=@p3,\"etkinlikTuruId\"=@p4,\"etkinlikTarihi\"=@p5,\"biletStok\"=@p6 WHERE \"etkinlikId\"=@p1";

            NpgsqlCommand guncelle = new NpgsqlCommand(sqlGuncelleme, baglanti);
            guncelle.Parameters.AddWithValue("@p1", int.Parse(txtId.Text));
            guncelle.Parameters.AddWithValue("@p2", txtAdi.Text);
            guncelle.Parameters.AddWithValue("@p5", dateTimePicker1.Value);
            guncelle.Parameters.AddWithValue("@p3", double.Parse(textBox1.Text));
            guncelle.Parameters.AddWithValue("@p4", int.Parse(cmbTur.SelectedValue.ToString()));
            guncelle.Parameters.AddWithValue("@p6", nuStok.Value);
            guncelle.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Güncelleme işlemi gerçekleşti.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Listele();
            Sil();
            //Listele();
            //txtKisiId.Text = "";
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            int id = int.Parse(txtId.Text);
            string sqlSilme = "DELETE FROM \"etkinlik\" WHERE \"etkinlikId\" =" + id + "";

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
            Sil();
            //Listele();
            //txtKisiId.Text = "";
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            DataTable dt = new DataTable();
            NpgsqlDataAdapter ara = new NpgsqlDataAdapter("SELECT * FROM \"etkinlik\" WHERE \"etkinlikAdi\" LIKE '%" + txtAdi.Text + "%' ", baglanti);
            ara.Fill(dt);
            baglanti.Close();
            dataGridView1.DataSource = dt;
            Sil();
            //Listele();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       public void Sil()
        {
            textBox1.Text = "";
            txtAdi.Text = "";
            txtId.Text = "";
            cmbTur.SelectedItem = null;
        }

        public void Listele()
        {
            string sorgu = "(select \"etkinlik\".\"etkinlikId\",\"etkinlik\".\"etkinlikAdi\",\"etkinlik\".\"etkinlikTarihi\",\"etkinlik\".\"biletFiyati\" ,\"etkinlikturu\".\"etkinlikTuruAdi\" ,\"etkinlik\".\"biletStok\" from \"etkinlik\" inner join \"etkinlikturu\" on \"etkinlikturu\".\"etkinlikTuruId\"=\"etkinlik\".\"etkinlikTuruId\" ORDER BY \"etkinlik\".\"etkinlikId\" ASC)";
            //select "etkinlikId","etkinlikAdi","etkinlikTarihi","biletFiyati",kdvl("biletFiyati") from public.etkinlik

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

    }
}
