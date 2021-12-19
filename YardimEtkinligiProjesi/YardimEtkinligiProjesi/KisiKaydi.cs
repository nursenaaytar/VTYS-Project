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
    public partial class KisiKaydi : Form
    {
        public KisiKaydi()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Etkinlik;user Id=postgres; password=postgres");
        private void KisiKaydi_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select*from cinsiyet", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbCinsiyet.DisplayMember = "cinsiyetAd";
            cmbCinsiyet.ValueMember = "cinsiyetId";
            cmbCinsiyet.DataSource = dt;

            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter("select*from medenihal", baglanti);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            cmbMedeniHal.DisplayMember = "medeniHalAd";
            cmbMedeniHal.ValueMember = "medeniHalId";
            cmbMedeniHal.DataSource = dt1;

            NpgsqlDataAdapter da2 = new NpgsqlDataAdapter("select*from meslek", baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            cmbMeslek.DisplayMember = "meslekAd";
            cmbMeslek.ValueMember = "meslekId";
            cmbMeslek.DataSource = dt2;

            NpgsqlDataAdapter da3 = new NpgsqlDataAdapter("select*from kisituru", baglanti);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            comboBox1.DisplayMember = "kisiTuruAdi";
            comboBox1.ValueMember = "kisiTuruId";
            comboBox1.DataSource = dt3;

            baglanti.Close();

            Clean();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sorgu = "(select \"kisi\".\"kisiId\",\"kisi\".\"kisiAdi\",\"kisi\".\"kisiSoyadi\",\"cinsiyet\".\"cinsiyetAd\",\"meslek\".\"meslekAd\"  ,\"medenihal\".\"medeniHalAd\"  ,\"kisituru\".\"kisiTuruAdi\",\"kisi\".\"takmaAd\"  from \"kisi\" inner join \"cinsiyet\" on \"cinsiyet\".\"cinsiyetId\"=\"kisi\".\"cinsiyetId\" inner join \"meslek\" on \"meslek\".\"meslekId\"=\"kisi\".\"meslekId\"  inner join \"medenihal\" on \"medenihal\".\"medeniHalId\"=\"kisi\".\"medeniHalId\" inner join \"kisituru\" on \"kisituru\".\"kisiTuruId\"=\"kisi\".\"kisiTuruId\" ORDER BY \"kisi\".\"kisiId\" ASC)";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

            string sorgu1 = "(select \"toplamkisi\".\"sayi\"  from \"toplamkisi\")";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sorgu1, baglanti);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            dataGridView3.DataSource = ds1.Tables[0];
            Clean();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(txtKisiId.Text!="")
            {
                MessageBox.Show("Id alanı otomatik belirlenir. Lütfen boş bırakınız.");
            }

           else
            {
                baglanti.Open();
                string sqlEkleme = "INSERT INTO \"kisi\"(\"kisiAdi\",\"kisiSoyadi\",\"meslekId\",\"cinsiyetId\",\"medeniHalId\",\"kisiTuruId\",\"takmaAd\")" + "VALUES(@p2,@p3,@p4,@p5,@p6,@p7,@p8)";

                NpgsqlCommand kisiEkle = new NpgsqlCommand(sqlEkleme, baglanti);
                kisiEkle.Parameters.AddWithValue("@p2", txtAdi.Text);
                kisiEkle.Parameters.AddWithValue("@p3", txtSoyadi.Text);
                kisiEkle.Parameters.AddWithValue("@p4", int.Parse(cmbMeslek.SelectedValue.ToString()));
                kisiEkle.Parameters.AddWithValue("@p5", int.Parse(cmbCinsiyet.SelectedValue.ToString()));
                kisiEkle.Parameters.AddWithValue("@p6", int.Parse(cmbMedeniHal.SelectedValue.ToString()));
                kisiEkle.Parameters.AddWithValue("@p7", int.Parse(comboBox1.SelectedValue.ToString()));
                kisiEkle.Parameters.AddWithValue("@p8", textBox1.Text);

                kisiEkle.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Ekleme işlemi gerçekleşti."); ;
                Listele();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            int id = int.Parse(txtKisiId.Text);
            string sqlSilme = "DELETE FROM \"kisi\" WHERE \"kisiId\" =" + id + "";

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
            txtKisiId.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string sqlGuncelleme = "UPDATE \"kisi\" SET \"kisiAdi\"=@p2,\"kisiSoyadi\"=@p3,\"meslekId\"=@p4,\"cinsiyetId\"=@p5,\"medeniHalId\"=@p6,\"kisiTuruId\"=@p7,\"takmaAd\"=@p8 WHERE \"kisiId\"=@p1";

            NpgsqlCommand guncelle = new NpgsqlCommand(sqlGuncelleme, baglanti);
            guncelle.Parameters.AddWithValue("@p1", int.Parse(txtKisiId.Text));
            guncelle.Parameters.AddWithValue("@p2", txtAdi.Text);
            guncelle.Parameters.AddWithValue("@p3", txtSoyadi.Text);
            guncelle.Parameters.AddWithValue("@p4", int.Parse(cmbMeslek.SelectedValue.ToString()));
            guncelle.Parameters.AddWithValue("@p5", int.Parse(cmbCinsiyet.SelectedValue.ToString()));
            guncelle.Parameters.AddWithValue("@p6", int.Parse(cmbMedeniHal.SelectedValue.ToString()));
            guncelle.Parameters.AddWithValue("@p7", int.Parse(comboBox1.SelectedValue.ToString()));
            guncelle.Parameters.AddWithValue("@p8", textBox1.Text);


            guncelle.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Güncelleme işlemi gerçekleşti.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Listele();
            txtKisiId.Text = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            DataTable dt = new DataTable();
            NpgsqlDataAdapter ara = new NpgsqlDataAdapter("SELECT * FROM \"kisi\" WHERE \"kisiAdi\" LIKE '%" + txtAdi.Text + "%' ", baglanti);
            ara.Fill(dt);
            baglanti.Close();
            dataGridView1.DataSource = dt;
            //Listele();
        }
        public void Clean()
        {
            txtAdi.Text = "";
            txtSoyadi.Text = "";
            cmbMeslek.SelectedItem = null;
            cmbCinsiyet.SelectedItem = null;
            cmbMedeniHal.SelectedItem = null;
            comboBox1.SelectedItem = null;
            textBox1.Text = "";
        }

        void Listele()
        {
            string sorgu = "(select \"kisi\".\"kisiId\",\"kisi\".\"kisiAdi\",\"kisi\".\"kisiSoyadi\",\"cinsiyet\".\"cinsiyetAd\",\"meslek\".\"meslekAd\"  ,\"medenihal\".\"medeniHalAd\"  ,\"kisituru\".\"kisiTuruAdi\",\"kisi\".\"takmaAd\"  from \"kisi\" inner join \"cinsiyet\" on \"cinsiyet\".\"cinsiyetId\"=\"kisi\".\"cinsiyetId\" inner join \"meslek\" on \"meslek\".\"meslekId\"=\"kisi\".\"meslekId\"  inner join \"medenihal\" on \"medenihal\".\"medeniHalId\"=\"kisi\".\"medeniHalId\" inner join \"kisituru\" on \"kisituru\".\"kisiTuruId\"=\"kisi\".\"kisiTuruId\" ORDER BY \"kisi\".\"kisiId\" ASC)";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

            string sorgu1 = "(select \"toplamkisi\".\"sayi\"  from \"toplamkisi\")";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sorgu1, baglanti);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            dataGridView3.DataSource = ds1.Tables[0];
            Clean();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}