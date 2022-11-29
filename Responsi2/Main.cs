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

namespace Responsi2
{
    public partial class Main : Form
    {
        private NpgsqlConnection conn;
        string connstring = "Host=localhost;Port=2022;Username=postgres;Password=informatika;Database=ResponsiLuthfi";
        public static NpgsqlCommand cmd;
        private string sql = null;
        public DataTable dt;
        private DataGridViewRow r;

        public Main()
        {
            InitializeComponent();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try 
            {
                conn = new NpgsqlConnection(connstring);
                conn.Open();
                sql = @"select * from ky_insert(:_nama,:_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_nama", tbNama.Text);
                cmd.Parameters.AddWithValue("_id_dep", cbDept.Items.ToString());
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Berhasil menambahkan karyawan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvResult.DataSource = null;
                    sql = "select * from ky_load()";
                    cmd = new NpgsqlCommand(sql, conn);
                    dt = new DataTable();
                    NpgsqlDataReader rd = cmd.ExecuteReader();
                    dt.Load(rd);
                    dgvResult.DataSource = dt;
                    conn.Close();
                    tbNama.Text = cbDept.Text = null;
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Pilih karyawan terlebih dahulu yang ingin diubah", "Row Empty!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            try
            {
                sql = @"select * from ky_update(:_id_karyawan,:_nama,:_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                cmd.Parameters.AddWithValue("_nama", tbNama.Text);
                cmd.Parameters.AddWithValue("_id_dep", cbDept.Items.ToString());
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Berhasil mengedit karyawan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvResult.DataSource = null;
                    sql = "select * from ky_load()";
                    cmd = new NpgsqlCommand(sql, conn);
                    dt = new DataTable();
                    NpgsqlDataReader rd = cmd.ExecuteReader();
                    dt.Load(rd);
                    dgvResult.DataSource = dt;
                    conn.Close();
                    tbNama.Text = cbDept.Text = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fail!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                /*if (MessageBox.Show("Apakah anda yakin akan menghapus karyawan ini?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    sql = @"select * from ky_delete(:_id_karyawan)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                    if ((int)cmd.ExecuteScalar() == 1)
                    {
                        MessageBox.Show("Berhasil menghapus karyawan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvResult.DataSource = null;
                        sql = "select * from ky_load()";
                        cmd = new NpgsqlCommand(sql, conn);
                        dt = new DataTable();
                        NpgsqlDataReader rd = cmd.ExecuteReader();
                        dt.Load(rd);
                        dgvResult.DataSource = dt;
                        conn.Close();
                        tbNama.Text = cbDept.Text = null;
                    }
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fail!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            
        }

        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                r = dgvResult.Rows[e.RowIndex];
                tbNama.Text = r.Cells["_nama"].Value.ToString();
                cbDept.Text = r.Cells["_id_dep"].Value.ToString();
            }
        }
    }
}
