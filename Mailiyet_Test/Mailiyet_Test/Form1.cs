using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Mailiyet_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-493DFJA\SQLEXPRESS;Initial Catalog=TestMaliyet;Integrated Security=True");
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        void malzemelerlist()
        {
            SqlDataAdapter dt = new SqlDataAdapter("select * from TBLMALZEMELER", con);
            DataTable da = new DataTable();
            dt.Fill(da);
            dataGridView1.DataSource = da;
        }
        void ürünlerlist()
        {
            SqlDataAdapter dt = new SqlDataAdapter("select * from TBLÜRÜNLER", con);
            DataTable da = new DataTable();
            dt.Fill(da);
            dataGridView1.DataSource = da;
        }
        void KASA()
        {
            SqlDataAdapter dt = new SqlDataAdapter("select * from TBLKASA", con);
            DataTable da = new DataTable();
            dt.Fill(da);
            dataGridView1.DataSource = da;
        }
        void ürünler()
        {
            
            SqlDataAdapter dt = new SqlDataAdapter("select * from TBLÜRÜNLER", con);
            DataTable da = new DataTable();
            dt.Fill(da);
            comboBoxürün.ValueMember = "URUNID";
            comboBoxürün.DisplayMember = "AD";
            comboBoxürün.DataSource = da;
            con.Close();
        }
        void malzemeler()
        {
            con.Open();
            SqlDataAdapter dt = new SqlDataAdapter("select * from TBLMALZEMELER", con);
            DataTable da = new DataTable();
            dt.Fill(da);
            comboBoxmalzeme.ValueMember = "MALZEMEID";
            comboBoxmalzeme .DisplayMember = "AD";
            comboBoxmalzeme.DataSource = da;
            con.Close();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ürünlerlist();
            //groupBox5.Visible = false;
           // groupBox7.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            malzemelerlist();
            ürünler();
            malzemeler();
            ürünlerlist();
            
            
        }

        private void buttonmalzemeekle_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand komut = new SqlCommand("insert into TBLMALZEMELER (AD,STOK,FIYAT,NOTLAR) values (@p1,@p2,@p3,@p4)", con);
            komut.Parameters.AddWithValue("@p1", textBoxmalzemead.Text);
            komut.Parameters.AddWithValue("@p2", decimal.Parse(textBoxmalzemestok.Text));
            komut.Parameters.AddWithValue("@p3", decimal.Parse(textBoxmalzemefiyat.Text));
            komut.Parameters.AddWithValue("@p4", textBoxnotlar.Text);
            komut.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("malzeme eklendi");
            malzemelerlist();
        }

        private void buttonürünekle_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand komut = new SqlCommand("insert into TBLÜRÜNLER (AD) values (@p1)", con);
            komut.Parameters.AddWithValue("@p1", textBoxürünad.Text);
            //komut.Parameters.AddWithValue("@p2", decimal.Parse(textBoxürünmaliyetfiyat.Text));
           // komut.Parameters.AddWithValue("@p3", decimal.Parse(textBoxürünsatısfiyat.Text));
           // komut.Parameters.AddWithValue("@p4", textBoxürünstok.Text);
            komut.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("ÜRÜN  eklendi");
            ürünler();
            
        }

        private void buttonÜRÜNOLUŞTUR_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand komut = new SqlCommand("insert into TBLFIRIN (URUNID,MALZEMEID,MIKTAR,MAILIYET) values (@p1,@p2,@p3,@p4)", con);
            komut.Parameters.AddWithValue("@p1", comboBoxürün.SelectedValue);
            komut.Parameters.AddWithValue("@p2", comboBoxmalzeme.SelectedValue);
             komut.Parameters.AddWithValue("@p3", decimal.Parse(textBoxmiktar.Text));
             komut.Parameters.AddWithValue("@p4", decimal.Parse(textBoxmaliyet.Text));
            komut.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("malzeme  eklendi");
            ürünler();
            listBox1.Items.Add(comboBoxmalzeme.Text + " - " + textBoxmaliyet.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            malzemelerlist();
            groupBox5.Visible = true;
            //groupBox7.Visible = false;
        }
        

        private void textBoxmiktar_TextChanged(object sender, EventArgs e)
        {
            double maliyet;
            
            if (textBoxmiktar.Text=="")
            {
                textBoxmiktar.Text ="0";
            }
            con.Open();
            SqlCommand kom = new SqlCommand("select * from TBLMALZEMELER WHERE MALZEMEID=@P1 ", con);
            kom.Parameters.AddWithValue("@P1", comboBoxmalzeme.SelectedValue);
            SqlDataReader dr = kom.ExecuteReader();
            while (dr.Read())
            {
                textBoxmaliyet.Text = dr[3].ToString();
            }
            con.Close();
            maliyet = Convert.ToDouble(textBoxmaliyet.Text) / 1000 * Convert.ToDouble(textBoxmiktar.Text);
            textBoxmaliyet.Text = maliyet.ToString();
            
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            textBoxürünıd.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            textBoxürünad.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            textBoxürünsatısfiyat.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            double maliyettek;
            con.Open();
            SqlCommand kom = new SqlCommand("select sum(MAILIYET) from TBLFIRIN where URUNID=@P1",con);
            kom.Parameters.AddWithValue("@P1", textBoxürünıd.Text);
            SqlDataReader DR = kom.ExecuteReader();
            while (DR.Read())
            {
                textBoxürünmaliyetfiyat.Text = DR[0].ToString();
                if(textBoxürünstok.Text=="")
                {
                    label16.Text = " ";
                }
                else
                {
                    maliyettek = Convert.ToDouble(textBoxürünmaliyetfiyat.Text) / Convert.ToDouble(textBoxürünstok.Text);
                    label16.Text = maliyettek.ToString();
                }
           
            }
            con.Close();
        }

        private void comboBoxmalzeme_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxürün_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

      

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           // int secilen = dataGridView2.SelectedCells[0].RowIndex;
           // textBoxürünıd.Text = dataGridView2.Rows[secilen].Cells[0].Value.ToString();
           // textBoxürünad.Text = dataGridView2.Rows[secilen].Cells[1].Value.ToString();
        }

        private void btnürüngüncelle_Click_1(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand komut = new SqlCommand("update TBLÜRÜNLER set AD=@p1,MFIYAT=@p2,SFIYAT=@p3,STOK=@p4 where URUNID=@p5", con);
            komut.Parameters.AddWithValue("@p1", textBoxürünad.Text);
            komut.Parameters.AddWithValue("@p2", decimal.Parse(textBoxürünmaliyetfiyat.Text));
            komut.Parameters.AddWithValue("@p3", decimal.Parse(textBoxürünsatısfiyat.Text));
            komut.Parameters.AddWithValue("@p4", textBoxürünstok.Text);
            komut.Parameters.AddWithValue("@p5", textBoxürünıd.Text);
            komut.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("ÜRÜN  güncellendi");
            ürünlerlist();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            KASA();
        }

        private void textBoxürünstok_TextChanged(object sender, EventArgs e)
        {
            double maliyettek;
            // maliyettek = Convert.ToDouble(label16.Text) / Convert.ToDouble(textBoxürünstok.Text);
            // textBoxürünmaliyetfiyat.Text = maliyettek.ToString();
            try
            {
                maliyettek = Convert.ToDouble(textBoxürünmaliyetfiyat.Text) / Convert.ToDouble(textBoxürünstok.Text);
                label16.Text = maliyettek.ToString();
            }
            catch (Exception)
            {

                label16.Text = "0";
            }
         
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            double maliyettek;
            if (textBoxürünstok.Text == ""||textBox1.Text== "")
            {
                textBoxürünsatısfiyat.Text = " ";
            }
            try
            {
                maliyettek = Convert.ToDouble(textBox1.Text) * Convert.ToDouble(textBoxürünstok.Text);
                textBoxürünsatısfiyat.Text = maliyettek.ToString();
            }
            catch (Exception)
            {

            }
        }
    }
}
