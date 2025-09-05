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
using Microsoft.VisualBasic;

namespace QLKHO.GUI
{
    public partial class FrmNguoiDung : Form
    {
        private NguoiDungService _nguoiDungService;
        private List<NguoiDung> _danhSachNguoiDung;
        private NguoiDung _nguoiDungHienTai;
        private bool _dangChinhSua = false;
        private bool _dangThemMoi = false;

        public FrmNguoiDung()
        {
            InitializeComponent();
            _nguoiDungService = new NguoiDungService();
            SetupForm();
        }

        private void SetupForm()
        {
            // Setup DataGridView
            SetupDataGridView();
            
            // Load dữ liệu
            LoadDanhSachNguoiDung();
            
            // Setup trạng thái ban đầu
            SetTrangThaiForm(false);
            
            // Setup events
            SetupEvents();
        }

        private void SetupDataGridView()
        {
            dgvNguoiDung.AutoGenerateColumns = false;
            dgvNguoiDung.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNguoiDung.MultiSelect = false;
            dgvNguoiDung.ReadOnly = true;
            dgvNguoiDung.AllowUserToAddRows = false;
            dgvNguoiDung.AllowUserToDeleteRows = false;

            // Thêm columns
            dgvNguoiDung.Columns.Clear();
            dgvNguoiDung.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaND",
                HeaderText = "Mã ND",
                DataPropertyName = "MaND",
                Width = 80
            });
            dgvNguoiDung.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenDangNhap",
                HeaderText = "Tên đăng nhập",
                DataPropertyName = "TenDangNhap",
                Width = 120
            });
            dgvNguoiDung.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HoTen",
                HeaderText = "Họ tên",
                DataPropertyName = "HoTen",
                Width = 150
            });
            dgvNguoiDung.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                HeaderText = "Email",
                DataPropertyName = "Email",
                Width = 150
            });
            dgvNguoiDung.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "VaiTro",
                HeaderText = "Vai trò",
                DataPropertyName = "VaiTro",
                Width = 100
            });
            dgvNguoiDung.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Trạng thái",
                DataPropertyName = "TrangThai",
                Width = 80
            });
            dgvNguoiDung.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "KhoaTaiKhoan",
                HeaderText = "Khóa TK",
                DataPropertyName = "KhoaTaiKhoan",
                Width = 80
            });
        }

        private void SetupEvents()
        {
            // Text change events để enable/disable buttons
            txtTenDangNhap.TextChanged += (s, e) => ValidateInput();
            txtMatKhau.TextChanged += (s, e) => ValidateInput();
            txtHoTen.TextChanged += (s, e) => ValidateInput();
            cboVaiTro.SelectedIndexChanged += (s, e) => ValidateInput();
        }

        private void LoadDanhSachNguoiDung()
        {
            try
            {
                _danhSachNguoiDung = _nguoiDungService.LayDanhSachNguoiDung();
                dgvNguoiDung.DataSource = _danhSachNguoiDung;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách người dùng: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachNguoiDung(string searchText)
        {
            try
            {
                var filteredList = _danhSachNguoiDung.Where(nd => 
                    nd.HoTen.ToLower().Contains(searchText.ToLower()) ||
                    nd.TenDangNhap.ToLower().Contains(searchText.ToLower()) ||
                    nd.Email.ToLower().Contains(searchText.ToLower()) ||
                    nd.VaiTro.ToLower().Contains(searchText.ToLower())
                ).ToList();
                
                dgvNguoiDung.DataSource = filteredList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtMaND.Clear();
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtHoTen.Clear();
            txtEmail.Clear();
            txtDienThoai.Clear();
            cboVaiTro.SelectedIndex = -1;
            chkTrangThai.Checked = true;
            _nguoiDungHienTai = null;
        }

        private void SetTrangThaiForm(bool editing)
        {
            _dangChinhSua = editing;
            _dangThemMoi = editing && _nguoiDungHienTai == null;

            // Enable/disable controls
            txtTenDangNhap.ReadOnly = !editing;
            txtMatKhau.ReadOnly = !editing;
            txtHoTen.ReadOnly = !editing;
            txtEmail.ReadOnly = !editing;
            txtDienThoai.ReadOnly = !editing;
            cboVaiTro.Enabled = editing;
            chkTrangThai.Enabled = editing;

            // Enable/disable buttons
            btnThem.Enabled = !editing;
            btnSua.Enabled = !editing && _nguoiDungHienTai != null;
            btnXoa.Enabled = !editing && _nguoiDungHienTai != null;
            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;
            btnDatLaiMatKhau.Enabled = !editing && _nguoiDungHienTai != null;
            btnKhoaMoKhoa.Enabled = !editing && _nguoiDungHienTai != null;

            // Update button text
            if (_nguoiDungHienTai != null && _nguoiDungHienTai.KhoaTaiKhoan)
            {
                btnKhoaMoKhoa.Text = "Mở khóa";
            }
            else
            {
                btnKhoaMoKhoa.Text = "Khóa tài khoản";
            }
        }

        private void ValidateInput()
        {
            if (_dangChinhSua)
            {
                bool isValid = !string.IsNullOrWhiteSpace(txtTenDangNhap.Text) &&
                              !string.IsNullOrWhiteSpace(txtMatKhau.Text) &&
                              !string.IsNullOrWhiteSpace(txtHoTen.Text) &&
                              cboVaiTro.SelectedIndex >= 0;

                btnLuu.Enabled = isValid;
            }
        }

        private void LoadNguoiDungToForm(NguoiDung nguoiDung)
        {
            if (nguoiDung == null) return;

            txtMaND.Text = nguoiDung.MaND.ToString();
            txtTenDangNhap.Text = nguoiDung.TenDangNhap;
            txtMatKhau.Text = nguoiDung.MatKhau;
            txtHoTen.Text = nguoiDung.HoTen;
            txtEmail.Text = nguoiDung.Email ?? "";
            txtDienThoai.Text = nguoiDung.DienThoai ?? "";
            cboVaiTro.Text = nguoiDung.VaiTro;
            chkTrangThai.Checked = nguoiDung.TrangThai;
        }

        private NguoiDung GetNguoiDungFromForm()
        {
            var nguoiDung = new NguoiDung
            {
                TenDangNhap = txtTenDangNhap.Text.Trim(),
                MatKhau = txtMatKhau.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                DienThoai = string.IsNullOrWhiteSpace(txtDienThoai.Text) ? null : txtDienThoai.Text.Trim(),
                VaiTro = cboVaiTro.Text,
                TrangThai = chkTrangThai.Checked
            };

            if (_dangThemMoi)
            {
                nguoiDung.MaND = 0; // Sẽ được auto-increment
            }
            else
            {
                nguoiDung.MaND = _nguoiDungHienTai.MaND;
            }

            return nguoiDung;
        }

        #region Event Handlers

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadDanhSachNguoiDung();
            }
            else
            {
                LoadDanhSachNguoiDung(txtSearch.Text);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadDanhSachNguoiDung();
            }
            else
            {
                LoadDanhSachNguoiDung(txtSearch.Text);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadDanhSachNguoiDung();
        }

        private void dgvNguoiDung_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvNguoiDung.SelectedRows.Count > 0)
            {
                var selectedRow = dgvNguoiDung.SelectedRows[0];
                var nguoiDung = selectedRow.DataBoundItem as NguoiDung;
                
                if (nguoiDung != null)
                {
                    _nguoiDungHienTai = nguoiDung;
                    LoadNguoiDungToForm(nguoiDung);
                    SetTrangThaiForm(false);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            _nguoiDungHienTai = null;
            SetTrangThaiForm(true);
            txtTenDangNhap.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_nguoiDungHienTai == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần sửa", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetTrangThaiForm(true);
            txtTenDangNhap.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_nguoiDungHienTai == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần xóa", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa người dùng '{_nguoiDungHienTai.HoTen}'?", 
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var serviceResult = _nguoiDungService.XoaNguoiDung(_nguoiDungHienTai.MaND);
                    
                    if (serviceResult.ThanhCong)
                    {
                        MessageBox.Show(serviceResult.ThongBao, "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachNguoiDung();
                        ClearForm();
                        _nguoiDungHienTai = null;
                        SetTrangThaiForm(false);
                    }
                    else
                    {
                        MessageBox.Show(serviceResult.ThongBao, "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa người dùng: {ex.Message}", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                var nguoiDung = GetNguoiDungFromForm();
                ServiceResult result;

                if (_dangThemMoi)
                {
                    result = _nguoiDungService.ThemNguoiDung(nguoiDung);
                }
                else
                {
                    result = _nguoiDungService.CapNhatNguoiDung(nguoiDung);
                }

                if (result.ThanhCong)
                {
                    MessageBox.Show(result.ThongBao, "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachNguoiDung();
                    SetTrangThaiForm(false);
                }
                else
                {
                    MessageBox.Show(result.ThongBao, "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu người dùng: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (_nguoiDungHienTai != null)
            {
                LoadNguoiDungToForm(_nguoiDungHienTai);
            }
            else
            {
                ClearForm();
            }
            SetTrangThaiForm(false);
        }

        private void btnDatLaiMatKhau_Click(object sender, EventArgs e)
        {
            if (_nguoiDungHienTai == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newPassword = Microsoft.VisualBasic.Interaction.InputBox(
                "Nhập mật khẩu mới:", "Đặt lại mật khẩu", "", -1, -1);

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                try
                {
                    var result = _nguoiDungService.DatLaiMatKhau(_nguoiDungHienTai.MaND, newPassword);
                    
                    if (result.ThanhCong)
                    {
                        MessageBox.Show(result.ThongBao, "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachNguoiDung();
                    }
                    else
                    {
                        MessageBox.Show(result.ThongBao, "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi đặt lại mật khẩu: {ex.Message}", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnKhoaMoKhoa_Click(object sender, EventArgs e)
        {
            if (_nguoiDungHienTai == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool khoa = !_nguoiDungHienTai.KhoaTaiKhoan;
            string action = khoa ? "khóa" : "mở khóa";
            
            var result = MessageBox.Show($"Bạn có chắc chắn muốn {action} tài khoản '{_nguoiDungHienTai.HoTen}'?", 
                $"Xác nhận {action}", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var serviceResult = _nguoiDungService.KhoaMoKhoaTaiKhoan(_nguoiDungHienTai.MaND, khoa);
                    
                    if (serviceResult.ThanhCong)
                    {
                        MessageBox.Show(serviceResult.ThongBao, "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachNguoiDung();
                    }
                    else
                    {
                        MessageBox.Show(serviceResult.ThongBao, "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi {action} tài khoản: {ex.Message}", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _nguoiDungService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
