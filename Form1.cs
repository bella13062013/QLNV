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

namespace VD_QLNS
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        SqlDataAdapter da;
        DataSet ds;
        SqlCommandBuilder builder;
        string st = @"Data Source=LAB2-MAY11\SQLEXPRESS;Initial Catalog=QLNS1;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }
        //lay du lieu phong ban
        public void loadPhongBan()
        {
            string sql = "select * from DMPHONG";
            da = new SqlDataAdapter(sql, conn);
            da.Fill(ds, "PHONGBAN");


            cboPhongBan.DataSource = ds.Tables["PHONGBAN"];
            cboPhongBan.DisplayMember = "TenPhong";
            cboPhongBan.ValueMember = "MaPhong";

        }
        // lay du lieu DSNV
        public void loadData()
        {
            string sql = "select * from DSNV";
            da = new SqlDataAdapter(sql, conn);
            da.Fill(ds, "NHANVIEN");

            data.DataSource = ds.Tables["NHANVIEN"];
        }
        // kiem tra ma nhan vien co bi trung khong
        public int kTraMNV(string ma)
        {
            conn.Open();
            string sql = string.Format("select count(*) from DSNV where MaNV ='{0}'", ma.Trim());
            SqlCommand cmd = new SqlCommand(sql, conn);
            int sl = (int)cmd.ExecuteScalar();
            conn.Close();
            return sl;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(st);
            ds = new DataSet();
            loadData();
            loadPhongBan();
            builder = new SqlCommandBuilder(da);
        }


        private void txtTen_TextChanged(object sender, EventArgs e)
        {
            txtTen.Clear();
        }

        private void btnLmoi_Click(object sender, EventArgs e)
        {
            txtTen.Clear();
            txtMaNV.Clear();
            txtHSL.Clear();

            txtTen.Focus();

            data.DataSource = ds.Tables["NHANVIEN"];
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            string sql = string.Format("select * from DSNV where HoTen Like N'%{0}'", txtTen.Text);
            da = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            da.Fill(dt);

            data.DataSource = dt;
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            string sql = "select B.TenPhong , count(A.MaPhong) as SoLuongNV form DSNV as A. DMPHONG as B " +
                "where A.MAPHONG = B.MAPHONG group By B.TenPhong";
            da = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            da.Fill(dt);

            data.DataSource = dt;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (kTraMNV(txtMaNV.Text) == 0)
            {
                DataTable dt = ds.Tables["NHANVIEN"];
                DataRow r = dt.NewRow();
                r[0] = txtMaNV.Text;
                r[1] = txtTen.Text;
                r[2] = cboPhongBan.SelectedItem.ToString();
                r[3] = float.Parse(txtHSL.Text);
                r[4] = "NV";
                dt.Rows.Add(r);

                da.Update(ds, "NHANVIEN");
            }
            else
                MessageBox.Show("Bi trung ma nhan vien");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Ban co chac chan muon xoa?", "thong bao", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(dr == DialogResult.Yes)
            {
                int i = data.CurrentCell.RowIndex;//chon 1 dong tren datagridview
                DataTable dt = ds.Tables["NHANVIEN"];
                dt.Rows[i].Delete();

                da.Update(ds, "NHANIEN");
            }
        }

        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = data.CurrentCell.RowIndex;
            txtMaNV.Text = data.Rows[i].Cells[0].Value.ToString();
            txtTen.Text = data.Rows[i].Cells[1].Value.ToString();
            cboPhongBan.SelectedValue = data.Rows[i].Cells[2].Value.ToString();
            txtHSL.Text = data.Rows[i].Cells[3].Value.ToString();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            int i = data.CurrentCell.RowIndex;
            DataTable dt = ds.Tables["NHANVIEN"];
            DataRow r = dt.Rows[i];
            r[0] = txtMaNV.Text;
            r[1] = txtTen.Text;
            r[2] = cboPhongBan.SelectedValue.ToString();
            r[3] = float.Parse(txtHSL.Text);
            r[4] = "NV";

            da.Update(ds, "NHANVIEN");
        }
    }

}