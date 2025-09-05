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
        private LichSuService _lichSuService;
        private List<NguoiDung> _danhSachNguoiDung;
        private NguoiDung _nguoiDungHienTai;
        private bool _dangChinhSua = false;
        private bool _dangThemMoi = false;

        public FrmNguoiDung()
        {
            InitializeComponent();
            _nguoiDungService = new NguoiDungService();
            _lichSuService = new LichSuService();
            SetupForm();
        }

        private void SetupForm()
        {
            // Setup DataGridView
            SetupDataGridView();
            SetupHistoryDataGridViews();
            
            // Load dữ liệu
            LoadDanhSachNguoiDung();
            
            // Setup trạng thái ban đầu
            SetTrangThaiForm(false);
            
            // Đảm bảo button states được cập nhật
            UpdateButtonStates();
            
            // Setup events
            SetupEvents();
            
            // Setup form properties
            this.Text = "Quản lý người dùng";
            this.StartPosition = FormStartPosition.CenterParent;
            
            // Ẩn tab control ban đầu
            tabControl1.Visible = false;
        }

        private void SetupDataGridView()
        {
            dgvNguoiDung.AutoGenerateColumns = false;
            dgvNguoiDung.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNguoiDung.MultiSelect = false;
            dgvNguoiDung.ReadOnly = true;
            dgvNguoiDung.AllowUserToAddRows = false;
            dgvNguoiDung.AllowUserToDeleteRows = false;
            dgvNguoiDung.SelectionChanged += dgvNguoiDung_SelectionChanged;

            // Thêm columns
            dgvNguoiDung.Columns.Clear();
            dgvNguoiDung.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaND",
                HeaderText = "Mã ND",
                DataPropertyName = "MaND",
                Width = 80,
                Visible = false // Ẩn cột MaND
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

        private void SetupHistoryDataGridViews()
        {
            // Setup lịch sử đăng nhập
            dgvLichSuDangNhap.AutoGenerateColumns = false;
            dgvLichSuDangNhap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLichSuDangNhap.MultiSelect = false;
            dgvLichSuDangNhap.ReadOnly = true;
            dgvLichSuDangNhap.AllowUserToAddRows = false;
            dgvLichSuDangNhap.AllowUserToDeleteRows = false;

            // Thêm columns cho lịch sử đăng nhập
            dgvLichSuDangNhap.Columns.Clear();
            dgvLichSuDangNhap.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenDangNhap",
                HeaderText = "Tên đăng nhập",
                DataPropertyName = "TenDangNhap",
                Width = 120
            });
            dgvLichSuDangNhap.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Trạng thái",
                DataPropertyName = "TrangThai",
                Width = 100
            });
            dgvLichSuDangNhap.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IPAddress",
                HeaderText = "IP Address",
                DataPropertyName = "IPAddress",
                Width = 120
            });
            dgvLichSuDangNhap.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThoiGian",
                HeaderText = "Thời gian",
                DataPropertyName = "ThoiGian",
                Width = 150
            });

            // Setup lịch sử thay đổi
            dgvLichSuThayDoi.AutoGenerateColumns = false;
            dgvLichSuThayDoi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLichSuThayDoi.MultiSelect = false;
            dgvLichSuThayDoi.ReadOnly = true;
            dgvLichSuThayDoi.AllowUserToAddRows = false;
            dgvLichSuThayDoi.AllowUserToDeleteRows = false;

            // Thêm columns cho lịch sử thay đổi
            dgvLichSuThayDoi.Columns.Clear();
            dgvLichSuThayDoi.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThaoTac",
                HeaderText = "Thao tác",
                DataPropertyName = "ThaoTac",
                Width = 80
            });
            dgvLichSuThayDoi.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaBanGhi",
                HeaderText = "Mã bản ghi",
                DataPropertyName = "MaBanGhi",
                Width = 80
            });
            dgvLichSuThayDoi.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DuLieuCu",
                HeaderText = "Dữ liệu cũ",
                DataPropertyName = "DuLieuCu",
                Width = 200
            });
            dgvLichSuThayDoi.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DuLieuMoi",
                HeaderText = "Dữ liệu mới",
                DataPropertyName = "DuLieuMoi",
                Width = 200
            });
            dgvLichSuThayDoi.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IPAddress",
                HeaderText = "IP Address",
                DataPropertyName = "IPAddress",
                Width = 120
            });
            dgvLichSuThayDoi.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThoiGian",
                HeaderText = "Thời gian",
                DataPropertyName = "ThoiGian",
                Width = 150
            });
        }

        private void SetupEvents()
        {
            // Text change events để enable/disable buttons
            // Validation sẽ được thực hiện khi click Lưu
        }

        private void LoadDanhSachNguoiDung()
        {
            try
            {
                // Kiểm tra checkbox để quyết định load dữ liệu nào
                if (chkShowInactive.Checked)
                {
                    // Hiển thị tất cả người dùng (bao gồm không hoạt động)
                    _danhSachNguoiDung = _nguoiDungService.LayDanhSachNguoiDung();
                }
                else
                {
                    // Chỉ lấy người dùng hoạt động (TrangThai = true)
                    _danhSachNguoiDung = _nguoiDungService.LayDanhSachNguoiDungHoatDong();
                }
                
                dgvNguoiDung.DataSource = _danhSachNguoiDung;
                UpdateTotalLabel();
                
                // Cập nhật button states sau khi load dữ liệu
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải danh sách người dùng.\nVui lòng kiểm tra kết nối cơ sở dữ liệu.", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                UpdateTotalLabel(filteredList.Count);
                
                // Cập nhật button states sau khi filter
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thực hiện tìm kiếm.", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateTotalLabel(int? count = null)
        {
            if (count.HasValue)
            {
                if (chkShowInactive.Checked)
                {
                    lblTotal.Text = $"Tổng: {count.Value} người dùng (bao gồm không hoạt động)";
                }
                else
                {
                    lblTotal.Text = $"Tổng: {count.Value} người dùng hoạt động";
                }
            }
            else if (_danhSachNguoiDung != null)
            {
                if (chkShowInactive.Checked)
                {
                    lblTotal.Text = $"Tổng: {_danhSachNguoiDung.Count} người dùng (bao gồm không hoạt động)";
                }
                else
                {
                    lblTotal.Text = $"Tổng: {_danhSachNguoiDung.Count} người dùng hoạt động";
                }
            }
            else
            {
                lblTotal.Text = "Tổng: 0 người dùng";
            }
        }

        private void LoadLichSuTatCa()
        {
            try
            {
                // Load lịch sử đăng nhập
                var lichSuDangNhap = _lichSuService.LayLichSuDangNhapTatCa(100);
                dgvLichSuDangNhap.DataSource = lichSuDangNhap;

                // Load lịch sử thay đổi
                var lichSuThayDoi = _lichSuService.LayLichSuThayDoiTatCa(100);
                dgvLichSuThayDoi.DataSource = lichSuThayDoi;
                
                // Hiển thị tab control
                tabControl1.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải lịch sử: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadLichSuNguoiDung(int maND)
        {
            try
            {
                // Load lịch sử đăng nhập của người dùng
                var lichSuDangNhap = _lichSuService.LayLichSuDangNhap(maND, 50);
                dgvLichSuDangNhap.DataSource = lichSuDangNhap;

                // Load lịch sử thay đổi của người dùng
                var lichSuThayDoi = _lichSuService.LayLichSuThayDoi(maND, 50);
                dgvLichSuThayDoi.DataSource = lichSuThayDoi;
                
                // Hiển thị tab control
                tabControl1.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải lịch sử người dùng: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearForm()
        {
            _dangChinhSua = false;
            _dangThemMoi = false;
            _nguoiDungHienTai = null;
            txtMaND.Clear();
            txtTenDangNhap.Clear();
            txtHoTen.Clear();
            txtMatKhau.Clear();
            txtEmail.Clear();
            txtDienThoai.Clear();
            cboVaiTro.SelectedIndex = -1;
            chkTrangThai.Checked = true;
            UpdateButtonStates();
            txtTenDangNhap.Focus();
            
            // Bỏ chọn trong DataGridView
            if (dgvNguoiDung.SelectedRows.Count > 0)
            {
                dgvNguoiDung.ClearSelection();
            }
            
            // Ẩn tab control
            tabControl1.Visible = false;
        }

        private void SetTrangThaiForm(bool editing)
        {
            _dangChinhSua = editing;
            _dangThemMoi = editing && _nguoiDungHienTai == null;

            // Enable/disable controls
            txtTenDangNhap.ReadOnly = !editing;
            txtHoTen.ReadOnly = !editing;
            txtEmail.ReadOnly = !editing;
            txtDienThoai.ReadOnly = !editing;
            cboVaiTro.Enabled = editing;
            chkTrangThai.Enabled = editing;


            // Update button text
            if (_nguoiDungHienTai != null && _nguoiDungHienTai.KhoaTaiKhoan)
            {
                btnKhoaMoKhoa.Text = "Mở khóa";
            }
            else
            {
                btnKhoaMoKhoa.Text = "Khóa tài khoản";
            }
            
            // Cập nhật trạng thái button
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            // Nếu có người dùng được chọn từ DataGridView
            if (_nguoiDungHienTai != null)
            {
                btnLuu.Text = "Cập nhật";
                btnLuu.BackColor = Color.LightBlue;
                btnHuy.Text = "Hủy";
                btnXoa.Enabled = true;
                btnDatLaiMatKhau.Enabled = true;
                btnKhoaMoKhoa.Enabled = true;
            }
            else
            {
                // Không có người dùng được chọn
                btnLuu.Text = "Thêm mới";
                btnLuu.BackColor = Color.LightGreen;
                btnHuy.Text = "Làm mới";
                btnXoa.Enabled = false;
                btnDatLaiMatKhau.Enabled = false;
                btnKhoaMoKhoa.Enabled = false;
            }
        }

        private bool ValidateInput()
        {
            if (_dangChinhSua)
            {
                if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenDangNhap.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHoTen.Focus();
                    return false;
                }

                // Kiểm tra mật khẩu
                if (_nguoiDungHienTai == null)
                {
                    // Thêm mới: Bắt buộc nhập mật khẩu
                    if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                    {
                        MessageBox.Show("Vui lòng nhập mật khẩu cho người dùng mới", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMatKhau.Focus();
                        return false;
                    }

                    if (txtMatKhau.Text.Trim().Length < 6)
                    {
                        MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMatKhau.Focus();
                        txtMatKhau.SelectAll();
                        return false;
                    }
                }
                else
                {
                    // Cập nhật: Mật khẩu là tùy chọn
                    if (!string.IsNullOrWhiteSpace(txtMatKhau.Text) && txtMatKhau.Text.Trim().Length < 6)
                    {
                        MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự (để trống nếu không muốn thay đổi)", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMatKhau.Focus();
                        txtMatKhau.SelectAll();
                        return false;
                    }
                }

                // Chỉ validate email và số điện thoại khi thêm mới
                if (_nguoiDungHienTai == null)
                {
                    // Validate email (nếu có)
                    if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                    {
                        if (!IsValidEmail(txtEmail.Text.Trim()))
                        {
                            MessageBox.Show("❌ Email không hợp lệ!\n\n" +
                                          "📋 Yêu cầu:\n" +
                                          "• Phải có định dạng: user@domain.com\n" +
                                          "• Độ dài tối thiểu: 5 ký tự\n" +
                                          "• Ví dụ: admin@company.com\n\n" +
                                          "💡 Vui lòng kiểm tra và nhập lại email đúng định dạng.", "Lỗi", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtEmail.Focus();
                            txtEmail.SelectAll();
                            return false;
                        }
                    }

                    // Validate số điện thoại (nếu có)
                    if (!string.IsNullOrWhiteSpace(txtDienThoai.Text))
                    {
                        if (!IsValidPhoneNumber(txtDienThoai.Text.Trim()))
                        {
                            MessageBox.Show("❌ Số điện thoại không hợp lệ!\n\n" +
                                          "📋 Yêu cầu:\n" +
                                          "• Chỉ chứa số (0-9)\n" +
                                          "• Độ dài tối thiểu: 10 ký tự\n" +
                                          "• Ví dụ: 0123456789, 0987654321\n\n" +
                                          "💡 Vui lòng nhập số điện thoại chỉ chứa số và có ít nhất 10 chữ số.", "Lỗi", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtDienThoai.Focus();
                            txtDienThoai.SelectAll();
                            return false;
                        }
                    }
                }

                if (cboVaiTro.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn vai trò", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboVaiTro.Focus();
                    return false;
                }
            }
            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return true; // Cho phép để trống

            // Loại bỏ khoảng trắng và ký tự đặc biệt
            var cleanPhone = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            // Kiểm tra định dạng số điện thoại Việt Nam
            // Bắt đầu bằng 0 hoặc +84, theo sau là 9-10 chữ số
            if (cleanPhone.StartsWith("+84"))
            {
                cleanPhone = "0" + cleanPhone.Substring(3);
            }

            // Kiểm tra theo database constraint: chỉ chứa số, độ dài tối thiểu 10
            if (cleanPhone.Length >= 10 && cleanPhone.All(char.IsDigit))
            {
                return true;
            }

            return false;
        }

        private void LoadNguoiDungToForm(NguoiDung nguoiDung)
        {
            if (nguoiDung == null) return;

            txtMaND.Text = nguoiDung.MaND.ToString();
            txtTenDangNhap.Text = nguoiDung.TenDangNhap;
            txtHoTen.Text = nguoiDung.HoTen;
            txtMatKhau.Text = ""; // Không hiển thị mật khẩu cũ
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
                MatKhau = "", // Sẽ được xử lý riêng
                HoTen = txtHoTen.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                DienThoai = string.IsNullOrWhiteSpace(txtDienThoai.Text) ? null : txtDienThoai.Text.Trim(),
                VaiTro = cboVaiTro.Text,
                TrangThai = chkTrangThai.Checked
            };

            // Xử lý mật khẩu
            if (_nguoiDungHienTai == null)
            {
                // Thêm mới: Sử dụng mật khẩu từ form
                nguoiDung.MatKhau = txtMatKhau.Text.Trim();
            }
            else
            {
                // Cập nhật: Nếu có nhập mật khẩu mới thì dùng, không thì giữ nguyên
                if (!string.IsNullOrWhiteSpace(txtMatKhau.Text.Trim()))
                {
                    nguoiDung.MatKhau = txtMatKhau.Text.Trim();
                }
                else
                {
                    // Giữ nguyên mật khẩu cũ (sẽ được xử lý trong service)
                    nguoiDung.MatKhau = null; // Đánh dấu không thay đổi mật khẩu
                }
            }

            // Xử lý MaND
            if (_nguoiDungHienTai == null)
            {
                // Thêm mới: MaND = 0 (sẽ được auto-increment)
                nguoiDung.MaND = 0;
            }
            else
            {
                // Cập nhật: Sử dụng MaND của người dùng hiện tại
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
            try
            {
                var searchTerm = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadDanhSachNguoiDung();
                    return;
                }

                Cursor = Cursors.WaitCursor;
                LoadDanhSachNguoiDung(searchTerm);
                
                if (_danhSachNguoiDung.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy người dùng nào phù hợp với từ khóa '{searchTerm}'.\n\nVui lòng thử từ khóa khác hoặc kiểm tra chính tả.", 
                        "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearch.Clear();
                LoadDanhSachNguoiDung();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi làm mới dữ liệu: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkShowInactive_CheckedChanged(object sender, EventArgs e)
        {
            // Reload danh sách khi thay đổi checkbox
            LoadDanhSachNguoiDung();
        }

        // Xử lý sự kiện Enter key trong textbox tìm kiếm
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btnSearch_Click(sender, e);
            }
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
                    SetTrangThaiForm(true); // Enable edit mode khi chọn người dùng
                    UpdateButtonStates();
                    
                    // Load lịch sử của người dùng được chọn
                    LoadLichSuNguoiDung(nguoiDung.MaND);
                }
            }
            else
            {
                // Không có dòng nào được chọn
                _nguoiDungHienTai = null;
                UpdateButtonStates();
                
                // Ẩn tab control khi không chọn người dùng
                tabControl1.Visible = false;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            _nguoiDungHienTai = null;
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

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa người dùng này?\n\nLưu ý: Chỉ có thể xóa người dùng không có dữ liệu liên quan.", 
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    
                    var serviceResult = _nguoiDungService.XoaNguoiDung(_nguoiDungHienTai.MaND);
                    
                    if (serviceResult.ThanhCong)
                    {
                        MessageBox.Show(serviceResult.ThongBao, "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachNguoiDung();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(serviceResult.ThongBao, "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show("Không thể xóa người dùng: " + ex.Message, "Cảnh báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Nếu đang ở chế độ "Thêm mới" (button text = "Thêm mới"), đảm bảo _nguoiDungHienTai = null
                if (btnLuu.Text == "Thêm mới")
                {
                    _nguoiDungHienTai = null;
                }

                if (ValidateInput())
                {
                    Cursor = Cursors.WaitCursor;
                    
                    var nguoiDung = GetNguoiDungFromForm();
                    ServiceResult result;

                    // Nếu có người dùng được chọn từ DataGridView -> Cập nhật
                    if (_nguoiDungHienTai != null)
                    {
                        result = _nguoiDungService.CapNhatNguoiDung(nguoiDung);
                    }
                    else
                    {
                        // Không có người dùng được chọn -> Thêm mới
                        result = _nguoiDungService.ThemNguoiDung(nguoiDung);
                    }

                    if (result.ThanhCong)
                    {
                        MessageBox.Show(result.ThongBao, "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachNguoiDung();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(result.ThongBao, "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Dữ liệu không hợp lệ: " + ex.Message, "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Không thể thực hiện thao tác: " + ex.Message, "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                // Nếu có lỗi connection, reset UnitOfWork
                if (ex.Message.Contains("connection") || ex.Message.Contains("database") || 
                    ex.Message.Contains("timeout") || ex.Message.Contains("broken"))
                {
                    try
                    {
                        _nguoiDungService.ResetUnitOfWork();
                        MessageBox.Show("Đã reset kết nối cơ sở dữ liệu. Vui lòng thử lại.", "Thông báo", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Lỗi kết nối cơ sở dữ liệu. Vui lòng khởi động lại ứng dụng.", "Lỗi nghiêm trọng", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (_nguoiDungHienTai != null)
            {
                // Đang có người dùng được chọn -> Hủy (chuyển về chế độ thêm mới)
                _nguoiDungHienTai = null;
                ClearForm();
            }
            else
            {
                // Không có người dùng được chọn -> Làm mới (xóa hết thông tin)
                ClearForm();
            }
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
                "Nhập mật khẩu mới cho người dùng '" + _nguoiDungHienTai.HoTen + "':", 
                "Đặt lại mật khẩu", "", -1, -1);

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
                    MessageBox.Show("Không thể đặt lại mật khẩu.\nVui lòng thử lại sau.", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                $"Xác nhận {action}", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

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
                    MessageBox.Show($"Không thể {action} tài khoản.\nVui lòng thử lại sau.", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _nguoiDungService?.Dispose();
                _lichSuService?.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
