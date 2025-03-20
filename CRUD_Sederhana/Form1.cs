using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Sederhana
{
    public partial class Form1 : Form
    {
        private string ConnectionString = "Data Source = OCTAVIANIPTR\\OTHALLIA;" + "Initial Catalog =OrganisasiMahasiswaa;" + "Integrated Security = True";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ClearFrom()
        {
            textBox1.Clear(); //untuk nim
            textBox2.Clear(); //untuk nama
            textBox3.Clear(); //untuk email
            textBox4.Clear(); //untuk telepon
            textBox5.Clear(); //untuk alamat

            label1.Focus();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open(); //connect
                    string query = "SELECT NIM AS [NIM], Nama, Email, Telepon, Alamat FROM Mahasiswa"; //data dari sql
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);  //nyimpen

                    dgvMahasiswa.AutoGenerateColumns = true; //otomatis
                    dgvMahasiswa.DataSource = dt; //nampilin

                    ClearFrom(); //hapus
                }
                catch (Exception ex) //benerin kesalahan
                {
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error); //nampilin yg eror
                }
            }
        }
        private void BtnTambah(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "") //validasi
                    {
                        MessageBox.Show("Harap isi semua data!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning); //ngasih pringatan
                        return;
                    }

                    conn.Open();
                    string query = "INSERT INTO Mahasiswa (NIM, Nama, Email, Telepon, Alamat) VALUES (@NIM, @Nama, @Email, @Telepon, @Alamat)";
                    using (SqlCommand cmd = new SqlCommand(query, conn)) //bikin objek
                    {
                        cmd.Parameters.AddWithValue("@NIM", textBox1.Text.Trim());               //{nambahin nilai}
                        cmd.Parameters.AddWithValue("@Nama", textBox2.Text.Trim());             //{nambahin nilai}
                        cmd.Parameters.AddWithValue("@Email", textBox3.Text.Trim());           //{nambahin nilai}
                        cmd.Parameters.AddWithValue("@Telepon", textBox4.Text.Trim());        //{nambahin nilai}
                        cmd.Parameters.AddWithValue("@Alamat", textBox5.Text.Trim());        //{nambahin nilai}

                        int rowsAffected = cmd.ExecuteNonQuery(); //run
                        if (rowsAffected > 0) //ngecek
                        {
                            MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            ClearFrom();
                        }
                        else
                        {
                            MessageBox.Show("Data tidak berhasil di tambahkan!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error); //ngasi tau kalo gagal
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void BtnHapus(object sender, EventArgs e)
        {
            if (dgvMahasiswa.SelectedRows.Count > 0) //meriksa data
            {
                DialogResult confirm = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question); //nampilin kofirmasi
                if (confirm == DialogResult.Yes) //ngecek jawaban
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        try
                        {
                            string nim = dgvMahasiswa.SelectedRows[0].Cells["NIM"].Value.ToString(); //ngambil nim
                            conn.Open();
                            string query = "DELETE FROM Mahasiswa WHERE NIM = @NIM"; //nulis delete

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@NIM", nim); //agar aman
                                int rowsAffected = cmd.ExecuteNonQuery(); //eksekusi

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Data berhasil di hapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadData();
                                    ClearFrom();
                                }
                                else
                                {
                                    MessageBox.Show("Data tidak ditemukan atau gagal dihapus!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Pilih data yang akan dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRefresh(object sender, EventArgs e)
        {
            LoadData();
            MessageBox.Show($"Jumlah Kolom: {dgvMahasiswa.ColumnCount}\nJumlah Baris: {dgvMahasiswa.RowCount}",
                "Debugging DataGridView", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvMahasiswa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMahasiswa.Rows[e.RowIndex];
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                textBox4.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[4].Value.ToString();
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }


}