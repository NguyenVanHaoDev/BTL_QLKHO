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
    public partial class FramDanhMuc : Form
    {
        private DanhMucService _danhMucService;
        private List<DanhMucSanPham> _danhSachDanhMuc;
        private DanhMucSanPham _danhMucHienTai;
        private bool _isEditMode = false;

        public FramDanhMuc()
        {
            InitializeComponent();
            _danhMucService = new DanhMucService();
        }

        private void FramDanhMuc_Load(object sender, EventArgs e)
        {
            SetupForm();
            LoadDanhSachDanhMuc();
            LoadDanhMucCha();
            SetupFilterControls();
            SetTrangThaiForm(false);
        }

        private void SetupForm()
        {
            // Setup DataGridView
            dgvDanhMuc.AutoGenerateColumns = false;
            dgvDanhMuc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhMuc.MultiSelect = false;
            dgvDanhMuc.ReadOnly = true;
            dgvDanhMuc.AllowUserToAddRows = false;
            dgvDanhMuc.AllowUserToDeleteRows = false;
            dgvDanhMuc.SelectionChanged += DgvDanhMuc_SelectionChanged;

            // Thêm columns
            dgvDanhMuc.Columns.Clear();
            dgvDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaDM",
                HeaderText = "Mã DM",
                DataPropertyName = "MaDM",
                Width = 80,
                Visible = false
            });
            dgvDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenDM",
                HeaderText = "Tên danh mục",
                DataPropertyName = "TenDM",
                Width = 250
            });
            dgvDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DanhMucCha",
                HeaderText = "Danh mục cha",
                DataPropertyName = "DanhMucCha.TenDM",
                Width = 200
            });
            dgvDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThuTuHienThi",
                HeaderText = "Thứ tự",
                DataPropertyName = "ThuTuHienThi",
                Width = 80
            });
            dgvDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MoTa",
                HeaderText = "Mô tả",
                DataPropertyName = "MoTa",
                Width = 300
            });
            dgvDanhMuc.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Trạng thái",
                DataPropertyName = "TrangThai",
                Width = 100
            });

            // Setup validation
            txtTenDM.MaxLength = 100;
            txtMoTa.MaxLength = 500;
            txtThuTu.MaxLength = 5;
        }

        private void LoadDanhSachDanhMuc()
        {
            try
            {
                _danhSachDanhMuc = _danhMucService.GetAllActive().ToList();
                dgvDanhMuc.DataSource = _danhSachDanhMuc;
                UpdateTotalLabel(_danhSachDanhMuc.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách danh mục: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhMucCha()
        {
            try
            {
                var danhMucCha = _danhMucService.GetRootCategories().ToList();
                cboDanhMucCha.Items.Clear();
                cboDanhMucCha.Items.Add("-- Không có danh mục cha --");
                
                foreach (var dm in danhMucCha)
                {
                    cboDanhMucCha.Items.Add(dm);
                }
                
                cboDanhMucCha.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh mục cha: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DgvDanhMuc_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDanhMuc.SelectedRows.Count > 0)
            {
                var selectedRow = dgvDanhMuc.SelectedRows[0];
                LoadDanhMucToForm(selectedRow);
                SetTrangThaiForm(true);
                UpdateButtonStates();
            }
        }

        private void LoadDanhMucToForm(DataGridViewRow row)
        {
            if (row.DataBoundItem is DanhMucSanPham dm)
            {
                _danhMucHienTai = dm;
                txtMaDM.Text = dm.MaDM.ToString();
                txtTenDM.Text = dm.TenDM;
                txtMoTa.Text = dm.MoTa ?? "";
                txtThuTu.Text = dm.ThuTuHienThi.ToString();
                chkTrangThai.Checked = dm.TrangThai;

                // Set danh mục cha
                if (dm.MaDMCapTren.HasValue)
                {
                    for (int i = 0; i < cboDanhMucCha.Items.Count; i++)
                    {
                        var item = cboDanhMucCha.Items[i];
                        if (item is DanhMucSanPham dmCha && dmCha.MaDM == dm.MaDMCapTren.Value)
                        {
                            cboDanhMucCha.SelectedIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    cboDanhMucCha.SelectedIndex = 0;
                }
            }
        }

        private void SetTrangThaiForm(bool enable)
        {
            txtTenDM.Enabled = enable;
            txtMoTa.Enabled = enable;
            txtThuTu.Enabled = enable;
            cboDanhMucCha.Enabled = enable;
            chkTrangThai.Enabled = enable;
        }

        private void UpdateButtonStates()
        {
            if (_danhMucHienTai != null)
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
            _danhMucHienTai = null;
            txtMaDM.Clear();
            txtTenDM.Clear();
            txtMoTa.Clear();
            txtThuTu.Clear();
            cboDanhMucCha.SelectedIndex = 0;
            chkTrangThai.Checked = true;
            SetTrangThaiForm(false);
            UpdateButtonStates();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenDM.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDM.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtThuTu.Text))
            {
                if (!int.TryParse(txtThuTu.Text, out int thuTu) || thuTu < 0)
                {
                    MessageBox.Show("Thứ tự hiển thị phải là số nguyên dương!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtThuTu.Focus();
                    return false;
                }
            }

            return true;
        }

        private DanhMucSanPham GetDanhMucFromForm()
        {
            var dm = new DanhMucSanPham
            {
                TenDM = txtTenDM.Text.Trim(),
                MoTa = string.IsNullOrWhiteSpace(txtMoTa.Text) ? null : txtMoTa.Text.Trim(),
                ThuTuHienThi = string.IsNullOrWhiteSpace(txtThuTu.Text) ? 0 : int.Parse(txtThuTu.Text),
                TrangThai = chkTrangThai.Checked,
                NgayCapNhat = DateTime.Now
            };

            // Set danh mục cha
            if (cboDanhMucCha.SelectedItem is DanhMucSanPham selectedDanhMucCha)
            {
                dm.MaDMCapTren = selectedDanhMucCha.MaDM;
            }

            if (_danhMucHienTai != null)
            {
                dm.MaDM = _danhMucHienTai.MaDM;
                dm.NgayTao = _danhMucHienTai.NgayTao;
                dm.NguoiTao = _danhMucHienTai.NguoiTao;
            }
            else
            {
                dm.NgayTao = DateTime.Now;
                dm.NguoiTao = 1; // Tạm thời hardcode, sau này lấy từ login
            }

            return dm;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var dm = GetDanhMucFromForm();
                var result = _danhMucHienTai == null ? 
                    _danhMucService.Add(dm) : 
                    _danhMucService.Update(dm);

                if (result)
                {
                    MessageBox.Show(_danhMucHienTai == null ? 
                        "Thêm danh mục thành công!" : 
                        "Cập nhật danh mục thành công!", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LoadDanhSachDanhMuc();
                    LoadDanhMucCha();
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
            if (_danhMucHienTai == null) return;

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa danh mục '{_danhMucHienTai.TenDM}'?", 
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (_danhMucService.Delete(_danhMucHienTai.MaDM))
                    {
                        MessageBox.Show("Xóa danh mục thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachDanhMuc();
                        LoadDanhMucCha();
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
            LoadDanhSachDanhMuc();
            LoadDanhMucCha();
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
                var filteredData = _danhSachDanhMuc.AsEnumerable();

                // Filter by search text
                if (!string.IsNullOrWhiteSpace(txtTimKiem.Text))
                {
                    var searchText = txtTimKiem.Text.ToLower();
                    filteredData = filteredData.Where(dm => 
                        dm.TenDM.ToLower().Contains(searchText) ||
                        (dm.MoTa != null && dm.MoTa.ToLower().Contains(searchText)));
                }

                // Filter by status
                if (cboTrangThaiFilter.SelectedIndex == 1) // "Hoạt động"
                {
                    filteredData = filteredData.Where(dm => dm.TrangThai);
                }
                else if (cboTrangThaiFilter.SelectedIndex == 2) // "Không hoạt động"
                {
                    filteredData = filteredData.Where(dm => !dm.TrangThai);
                }

                // Update DataGridView
                dgvDanhMuc.DataSource = filteredData.ToList();
                UpdateTotalLabel(filteredData.Count());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lọc dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTotalLabel(int count)
        {
            lblTotal.Text = $"Tổng: {count} danh mục";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _danhMucService?.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
