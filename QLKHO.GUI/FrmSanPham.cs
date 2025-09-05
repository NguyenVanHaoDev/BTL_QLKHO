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
    public partial class FrmSanPham : Form
    {
        private SanPhamService _sanPhamService;
        private DanhMucService _danhMucService;
        private DonViTinhService _donViTinhService;
        private List<SanPham> _danhSachSanPham;
        private SanPham _sanPhamHienTai;
        private bool _isEditMode = false;

        public FrmSanPham()
        {
            InitializeComponent();
            _sanPhamService = new SanPhamService();
            _danhMucService = new DanhMucService();
            _donViTinhService = new DonViTinhService();
        }

        private void FrmSanPham_Load(object sender, EventArgs e)
        {
            SetupForm();
            LoadDanhSachSanPham();
            LoadDanhMuc();
            LoadDonViTinh();
            SetupFilterControls();
            SetTrangThaiForm(false);
            
            // Debug: Kiểm tra data
            DebugComboBoxData();
        }

        private void DebugComboBoxData()
        {
            System.Diagnostics.Debug.WriteLine($"Danh mục count: {cboDanhMuc.Items.Count}");
            System.Diagnostics.Debug.WriteLine($"Đơn vị tính count: {cboDonViTinh.Items.Count}");
            
            if (cboDanhMuc.Items.Count > 0)
            {
                for (int i = 0; i < cboDanhMuc.Items.Count; i++)
                {
                    var item = cboDanhMuc.Items[i];
                    System.Diagnostics.Debug.WriteLine($"Danh mục {i}: {item} (Type: {item?.GetType().Name})");
                }
            }
            
            if (cboDonViTinh.Items.Count > 0)
            {
                for (int i = 0; i < cboDonViTinh.Items.Count; i++)
                {
                    var item = cboDonViTinh.Items[i];
                    System.Diagnostics.Debug.WriteLine($"Đơn vị tính {i}: {item} (Type: {item?.GetType().Name})");
                }
            }
        }

        private void SetupForm()
        {
            // Setup DataGridView
            dgvSanPham.AutoGenerateColumns = false;
            dgvSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSanPham.MultiSelect = false;
            dgvSanPham.ReadOnly = true;
            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.AllowUserToDeleteRows = false;
            dgvSanPham.SelectionChanged += DgvSanPham_SelectionChanged;

            // Thêm columns
            dgvSanPham.Columns.Clear();
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaSP",
                HeaderText = "Mã SP",
                DataPropertyName = "MaSP",
                Width = 80,
                Visible = false
            });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenSP",
                HeaderText = "Tên sản phẩm",
                DataPropertyName = "TenSP",
                Width = 200
            });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DanhMuc",
                HeaderText = "Danh mục",
                DataPropertyName = "DanhMucSanPham.TenDM",
                Width = 150
            });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DonViTinh",
                HeaderText = "Đơn vị tính",
                DataPropertyName = "DonViTinh.TenDVT",
                Width = 100
            });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaNhap",
                HeaderText = "Giá nhập",
                DataPropertyName = "GiaNhap",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaBan",
                HeaderText = "Giá bán",
                DataPropertyName = "GiaBan",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoLuongTon",
                HeaderText = "Số lượng tồn",
                DataPropertyName = "SoLuongTon",
                Width = 100
            });
            dgvSanPham.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Trạng thái",
                DataPropertyName = "TrangThai",
                Width = 100
            });

            // Setup validation
            txtTenSP.MaxLength = 100;
            txtMoTa.MaxLength = 500;
        }

        private void LoadDanhSachSanPham()
        {
            try
            {
                _danhSachSanPham = _sanPhamService.GetAllActive().ToList();
                dgvSanPham.DataSource = _danhSachSanPham;
                UpdateTotalLabel(_danhSachSanPham.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách sản phẩm: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhMuc()
        {
            try
            {
                var danhMuc = _danhMucService.GetAllActive().ToList();
                cboDanhMuc.Items.Clear();
                cboDanhMuc.Items.Add("-- Chọn danh mục --");
                
                foreach (var dm in danhMuc)
                {
                    cboDanhMuc.Items.Add(dm);
                }
                
                cboDanhMuc.SelectedIndex = 0;

                // Load danh mục vào filter
                LoadDanhMucFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh mục: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhMucFilter()
        {
            try
            {
                var danhMuc = _danhMucService.GetAllActive().ToList();
                cboDanhMucFilter.Items.Clear();
                cboDanhMucFilter.Items.Add("Tất cả");
                
                foreach (var dm in danhMuc)
                {
                    cboDanhMucFilter.Items.Add(dm);
                }
                
                cboDanhMucFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh mục filter: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDonViTinh()
        {
            try
            {
                var donViTinh = _donViTinhService.GetAllActive().ToList();
                cboDonViTinh.Items.Clear();
                cboDonViTinh.Items.Add("-- Chọn đơn vị tính --");
                
                foreach (var dvt in donViTinh)
                {
                    cboDonViTinh.Items.Add(dvt);
                }
                
                cboDonViTinh.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải đơn vị tính: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DgvSanPham_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSanPham.SelectedRows.Count > 0)
            {
                var selectedRow = dgvSanPham.SelectedRows[0];
                LoadSanPhamToForm(selectedRow);
                SetTrangThaiForm(true);
                UpdateButtonStates();
            }
        }

        private void LoadSanPhamToForm(DataGridViewRow row)
        {
            if (row.DataBoundItem is SanPham sp)
            {
                _sanPhamHienTai = sp;
                txtMaSP.Text = sp.MaSP.ToString();
                txtTenSP.Text = sp.TenSP;
                txtMoTa.Text = sp.MoTa ?? "";
                txtGiaNhap.Text = sp.GiaNhap.ToString();
                txtGiaBan.Text = sp.GiaBan.ToString();
                txtSoLuongTon.Text = sp.SoLuongTon.ToString();
                txtSoLuongToiThieu.Text = sp.SoLuongToiThieu.ToString();
                chkTrangThai.Checked = sp.TrangThai;

                // Set danh mục
                for (int i = 0; i < cboDanhMuc.Items.Count; i++)
                {
                    var item = cboDanhMuc.Items[i];
                    if (item is DanhMucSanPham dm && dm.MaDM == sp.MaDM)
                    {
                        cboDanhMuc.SelectedIndex = i;
                        break;
                    }
                }

                // Set đơn vị tính
                for (int i = 0; i < cboDonViTinh.Items.Count; i++)
                {
                    var item = cboDonViTinh.Items[i];
                    if (item is DonViTinh dvt && dvt.MaDVT == sp.MaDVT)
                    {
                        cboDonViTinh.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void SetTrangThaiForm(bool enable)
        {
            txtTenSP.Enabled = enable;
            txtMoTa.Enabled = enable;
            txtGiaNhap.Enabled = enable;
            txtGiaBan.Enabled = enable;
            txtSoLuongTon.Enabled = enable;
            txtSoLuongToiThieu.Enabled = enable;
            cboDanhMuc.Enabled = enable;
            cboDonViTinh.Enabled = enable;
            chkTrangThai.Enabled = enable;
        }

        private void UpdateButtonStates()
        {
            if (_sanPhamHienTai != null)
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
            _sanPhamHienTai = null;
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtMoTa.Clear();
            txtGiaNhap.Clear();
            txtGiaBan.Clear();
            txtSoLuongTon.Clear();
            txtSoLuongToiThieu.Clear();
            cboDanhMuc.SelectedIndex = 0;
            cboDonViTinh.SelectedIndex = 0;
            chkTrangThai.Checked = true;
            SetTrangThaiForm(false);
            UpdateButtonStates();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSP.Focus();
                return false;
            }

            if (cboDanhMuc.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDanhMuc.Focus();
                return false;
            }

            if (cboDonViTinh.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDonViTinh.Focus();
                return false;
            }

            if (!decimal.TryParse(txtGiaNhap.Text, out decimal giaNhap) || giaNhap <= 0)
            {
                MessageBox.Show("Giá nhập phải là số dương!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap.Focus();
                return false;
            }

            if (!decimal.TryParse(txtGiaBan.Text, out decimal giaBan) || giaBan <= 0)
            {
                MessageBox.Show("Giá bán phải là số dương!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaBan.Focus();
                return false;
            }

            if (!int.TryParse(txtSoLuongTon.Text, out int soLuongTon) || soLuongTon < 0)
            {
                MessageBox.Show("Số lượng tồn phải là số nguyên không âm!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongTon.Focus();
                return false;
            }

            if (!int.TryParse(txtSoLuongToiThieu.Text, out int soLuongToiThieu) || soLuongToiThieu < 0)
            {
                MessageBox.Show("Số lượng tối thiểu phải là số nguyên không âm!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongToiThieu.Focus();
                return false;
            }

            return true;
        }

        private SanPham GetSanPhamFromForm()
        {
            var sp = new SanPham
            {
                TenSP = txtTenSP.Text.Trim(),
                MoTa = string.IsNullOrWhiteSpace(txtMoTa.Text) ? null : txtMoTa.Text.Trim(),
                GiaNhap = decimal.Parse(txtGiaNhap.Text),
                GiaBan = decimal.Parse(txtGiaBan.Text),
                SoLuongTon = int.Parse(txtSoLuongTon.Text),
                SoLuongToiThieu = int.Parse(txtSoLuongToiThieu.Text),
                TrangThai = chkTrangThai.Checked,
                NgayCapNhat = DateTime.Now
            };

            // Set danh mục
            if (cboDanhMuc.SelectedItem is DanhMucSanPham selectedDanhMuc)
            {
                sp.MaDM = selectedDanhMuc.MaDM;
            }
            else if (cboDanhMuc.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục sản phẩm!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDanhMuc.Focus();
                return null;
            }
            else
            {
                MessageBox.Show("Danh mục được chọn không hợp lệ!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDanhMuc.Focus();
                return null;
            }

            // Set đơn vị tính
            if (cboDonViTinh.SelectedItem is DonViTinh selectedDonViTinh)
            {
                sp.MaDVT = selectedDonViTinh.MaDVT;
            }
            else if (cboDonViTinh.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDonViTinh.Focus();
                return null;
            }
            else
            {
                MessageBox.Show("Đơn vị tính được chọn không hợp lệ!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDonViTinh.Focus();
                return null;
            }

            if (_sanPhamHienTai != null)
            {
                sp.MaSP = _sanPhamHienTai.MaSP;
                sp.NgayTao = _sanPhamHienTai.NgayTao;
                sp.NguoiTao = _sanPhamHienTai.NguoiTao;
            }
            else
            {
                sp.NgayTao = DateTime.Now;
                sp.NguoiTao = 1; // Tạm thời hardcode, sau này lấy từ login
            }

            return sp;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var sp = GetSanPhamFromForm();
                if (sp == null) return; // Validation failed in GetSanPhamFromForm
                
                var result = _sanPhamHienTai == null ? 
                    _sanPhamService.Add(sp) : 
                    _sanPhamService.Update(sp);

                if (result)
                {
                    MessageBox.Show(_sanPhamHienTai == null ? 
                        "Thêm sản phẩm thành công!" : 
                        "Cập nhật sản phẩm thành công!", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LoadDanhSachSanPham();
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
            if (_sanPhamHienTai == null) return;

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm '{_sanPhamHienTai.TenSP}'?", 
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (_sanPhamService.Delete(_sanPhamHienTai.MaSP))
                    {
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachSanPham();
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
            LoadDanhSachSanPham();
            LoadDanhMuc();
            LoadDonViTinh();
            ClearForm();
        }

        private void SetupFilterControls()
        {
            // Setup filter controls
            if (cboTrangThaiFilter.Items.Count > 0)
                cboTrangThaiFilter.SelectedIndex = 0; // "Tất cả"
            if (cboDanhMucFilter.Items.Count > 0)
                cboDanhMucFilter.SelectedIndex = 0; // "Tất cả"
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

        private void cboDanhMucFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void FilterData()
        {
            try
            {
                var filteredData = _danhSachSanPham.AsEnumerable();

                // Filter by search text
                if (!string.IsNullOrWhiteSpace(txtTimKiem.Text))
                {
                    var searchText = txtTimKiem.Text.ToLower();
                    filteredData = filteredData.Where(sp => 
                        sp.TenSP.ToLower().Contains(searchText) ||
                        (sp.MoTa != null && sp.MoTa.ToLower().Contains(searchText)));
                }

                // Filter by status
                if (cboTrangThaiFilter.SelectedIndex == 1) // "Hoạt động"
                {
                    filteredData = filteredData.Where(sp => sp.TrangThai);
                }
                else if (cboTrangThaiFilter.SelectedIndex == 2) // "Không hoạt động"
                {
                    filteredData = filteredData.Where(sp => !sp.TrangThai);
                }

                // Filter by category
                if (cboDanhMucFilter.SelectedItem is DanhMucSanPham selectedCategory)
                {
                    filteredData = filteredData.Where(sp => sp.MaDM == selectedCategory.MaDM);
                }

                // Update DataGridView
                dgvSanPham.DataSource = filteredData.ToList();
                UpdateTotalLabel(filteredData.Count());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lọc dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTotalLabel(int count)
        {
            lblTotal.Text = $"Tổng: {count} sản phẩm";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _sanPhamService?.Dispose();
                _danhMucService?.Dispose();
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
