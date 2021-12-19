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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EtkinlikKaydi show = new EtkinlikKaydi();
            show.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            KisiKaydi show = new KisiKaydi();
            show.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Yorum show=new Yorum();
            show.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MekanKaydi show = new MekanKaydi();
            show.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EtkinlikTalebi show=new EtkinlikTalebi();
            show.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bagis show=new Bagis();
            show.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
