using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLKHO.BLL.Services;
using QLKHO.DAL.Models;

namespace QLKHO.GUI
{
    public partial class FrmDonViTinh : Form
    {
        private DonViTinhService _donViTinhService;
        private List<DonViTinh> _danhSachDonViTinh;
        private DonViTinh _donViTinhHienTai;
        private bool _isEditMode = false;

        public FrmDonViTinh()
        {
            InitializeComponent();
            _donViTinhService = new DonViTinhService();
        }

        private void FrmDonViTinh_Load(object sender, EventArgs e)
        {
            SetupForm();
            LoadDanhSachDonViTinh();
            SetupFilterControls();
            SetTrangThaiForm(false);
        }

        private void SetupForm()
        {
            // Setup DataGridView
            dgvDonViTinh.AutoGenerateColumns = false;
            dgvDonViTinh.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDonViTinh.MultiSelect = false;
            dgvDonViTinh.ReadOnly = true;
            dgvDonViTinh.AllowUserToAddRows = false;
            dgvDonViTinh.AllowUserToDeleteRows = false;
            dgvDonViTinh.SelectionChanged += DgvDonViTinh_SelectionChanged;

            // Thêm columns
            dgvDonViTinh.Columns.Clear();
            dgvDonViTinh.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaDVT",
                HeaderText = "Mã DVT",
                DataPropertyName = "MaDVT",
                Width = 80,
                Visible = false
            });
            dgvDonViTinh.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenDVT",
                HeaderText = "Tên đơn vị tính",
                DataPropertyName = "TenDVT",
                Width = 200
            });
            dgvDonViTinh.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "KyHieu",
                HeaderText = "Ký hiệu",
                DataPropertyName = "KyHieu",
                Width = 100
            });
            dgvDonViTinh.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MoTa",
                HeaderText = "Mô tả",
                DataPropertyName = "MoTa",
                Width = 300
            });
            dgvDonViTinh.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Trạng thái",
                DataPropertyName = "TrangThai",
                Width = 100
            });

            // Setup validation
            txtTenDVT.MaxLength = 50;
            txtKyHieu.MaxLength = 10;
            txtMoTa.MaxLength = 200;
        }

        private void LoadDanhSachDonViTinh()
        {
            try
            {
                _danhSachDonViTinh = _donViTinhService.GetAllActive().ToList();
                dgvDonViTinh.DataSource = _danhSachDonViTinh;
                UpdateTotalLabel(_danhSachDonViTinh.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách đơn vị tính: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DgvDonViTinh_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDonViTinh.SelectedRows.Count > 0)
            {
                var selectedRow = dgvDonViTinh.SelectedRows[0];
                LoadDonViTinhToForm(selectedRow);
                SetTrangThaiForm(true);
                UpdateButtonStates();
            }
        }

        private void LoadDonViTinhToForm(DataGridViewRow row)
        {
            if (row.DataBoundItem is DonViTinh dvt)
            {
                _donViTinhHienTai = dvt;
                txtMaDVT.Text = dvt.MaDVT.ToString();
                txtTenDVT.Text = dvt.TenDVT;
                txtKyHieu.Text = dvt.KyHieu;
                txtMoTa.Text = dvt.MoTa ?? "";
                chkTrangThai.Checked = dvt.TrangThai;
            }
        }

        private void SetTrangThaiForm(bool enable)
        {
            txtTenDVT.Enabled = enable;
            txtKyHieu.Enabled = enable;
            txtMoTa.Enabled = enable;
            chkTrangThai.Enabled = enable;
        }

        private void UpdateButtonStates()
        {
            if (_donViTinhHienTai != null)
            {
                btnLuu.Text = "Cập nhật";
                btnLuu.BackColor = Color.Orange;
                btnHuy.Text = "Hủy";
                btnHuy.BackColor = Color.LightCoral;
                btnXoa.Enabled = true;
            }
            else
            {
                btnLuu.Text = "Thêm mới";
                btnLuu.BackColor = Color.LightGreen;
                btnHuy.Text = "Làm mới";
                btnHuy.BackColor = Color.LightBlue;
                btnXoa.Enabled = false;
            }
        }

        private void ClearForm()
        {
            _donViTinhHienTai = null;
            txtMaDVT.Clear();
            txtTenDVT.Clear();
            txtKyHieu.Clear();
            txtMoTa.Clear();
            chkTrangThai.Checked = true;
            SetTrangThaiForm(false);
            UpdateButtonStates();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenDVT.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đơn vị tính!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDVT.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtKyHieu.Text))
            {
                MessageBox.Show("Vui lòng nhập ký hiệu!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKyHieu.Focus();
                return false;
            }

            return true;
        }

        private DonViTinh GetDonViTinhFromForm()
        {
            var dvt = new DonViTinh
            {
                TenDVT = txtTenDVT.Text.Trim(),
                KyHieu = txtKyHieu.Text.Trim(),
                MoTa = string.IsNullOrWhiteSpace(txtMoTa.Text) ? null : txtMoTa.Text.Trim(),
                TrangThai = chkTrangThai.Checked,
                NgayCapNhat = DateTime.Now
            };

            if (_donViTinhHienTai != null)
            {
                dvt.MaDVT = _donViTinhHienTai.MaDVT;
                dvt.NgayTao = _donViTinhHienTai.NgayTao;
            }
            else
            {
                dvt.NgayTao = DateTime.Now;
            }

            return dvt;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var dvt = GetDonViTinhFromForm();
                var result = _donViTinhHienTai == null ? 
                    _donViTinhService.Add(dvt) : 
                    _donViTinhService.Update(dvt);

                if (result)
                {
                    MessageBox.Show(_donViTinhHienTai == null ? 
                        "Thêm đơn vị tính thành công!" : 
                        "Cập nhật đơn vị tính thành công!", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LoadDanhSachDonViTinh();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi lưu dữ liệu!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_donViTinhHienTai == null) return;

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa đơn vị tính '{_donViTinhHienTai.TenDVT}'?", 
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (_donViTinhService.Delete(_donViTinhHienTai.MaDVT))
                    {
                        MessageBox.Show("Xóa đơn vị tính thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachDonViTinh();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra khi xóa dữ liệu!", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDanhSachDonViTinh();
            ClearForm();
        }

        private void SetupFilterControls()
        {
            // Setup filter controls
            if (cboTrangThaiFilter.Items.Count > 0)
                cboTrangThaiFilter.SelectedIndex = 0; // "Tất cả"
            txtTimKiem.Text = "";
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void cboTrangThaiFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void FilterData()
        {
            try
            {
                var filteredData = _danhSachDonViTinh.AsEnumerable();

                // Filter by search text
                if (!string.IsNullOrWhiteSpace(txtTimKiem.Text))
                {
                    var searchText = txtTimKiem.Text.ToLower();
                    filteredData = filteredData.Where(dvt => 
                        dvt.TenDVT.ToLower().Contains(searchText) ||
                        dvt.KyHieu.ToLower().Contains(searchText) ||
                        (dvt.MoTa != null && dvt.MoTa.ToLower().Contains(searchText)));
                }

                // Filter by status
                if (cboTrangThaiFilter.SelectedIndex == 1) // "Hoạt động"
                {
                    filteredData = filteredData.Where(dvt => dvt.TrangThai);
                }
                else if (cboTrangThaiFilter.SelectedIndex == 2) // "Không hoạt động"
                {
                    filteredData = filteredData.Where(dvt => !dvt.TrangThai);
                }

                // Update DataGridView
                dgvDonViTinh.DataSource = filteredData.ToList();
                UpdateTotalLabel(filteredData.Count());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lọc dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTotalLabel(int count)
        {
            lblTotal.Text = $"Tổng: {count} đơn vị tính";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _donViTinhService?.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
