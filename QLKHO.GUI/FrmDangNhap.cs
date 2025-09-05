using System;
using System.Drawing;
using System.Windows.Forms;
using QLKHO.BLL.Services;
using QLKHO.DAL.Models;

namespace QLKHO.GUI
{
    public partial class FrmDangNhap : Form
    {
        public string Username { get; private set; }
        public NguoiDung NguoiDungDangNhap { get; private set; }
        public bool DangNhapThanhCong { get; private set; } = false;

        private AuthenticationService _authService;

        public FrmDangNhap()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
            SetupEventHandlers();
        }

        private void SetupEventHandlers()
        {
            // Event handlers cho các controls
            btnLogin.Click += BtnLogin_Click;
            btnExit.Click += BtnExit_Click;
            txtPassword.KeyPress += TxtPassword_KeyPress;
            txtUser.KeyPress += TxtUser_KeyPress;
            
            // Focus vào textbox đầu tiên
            txtUser.Focus();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            ThucHienDangNhap();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ThucHienDangNhap();
                e.Handled = true;
            }
        }

        private void TxtUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPassword.Focus();
                e.Handled = true;
            }
        }

        private void ThucHienDangNhap()
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(txtUser.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUser.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // Disable controls trong khi đăng nhập
            SetControlsEnabled(false);
            
            try
            {
                // Lấy thông tin IP và User Agent
                string ipAddress = GetLocalIPAddress();
                string userAgent = Environment.OSVersion.ToString();

                // Thực hiện đăng nhập
                var result = _authService.DangNhap(txtUser.Text.Trim(), txtPassword.Text, ipAddress, userAgent);

                if (result.ThanhCong)
                {
                    // Đăng nhập thành công
                    Username = result.NguoiDung.TenDangNhap;
                    NguoiDungDangNhap = result.NguoiDung;
                    DangNhapThanhCong = true;

                    MessageBox.Show($"Chào mừng {result.NguoiDung.HoTen}!", "Đăng nhập thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // Đăng nhập thất bại
                    MessageBox.Show(result.ThongBao, "Đăng nhập thất bại", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    // Clear password và focus vào password field
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Re-enable controls
                SetControlsEnabled(true);
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            txtUser.Enabled = enabled;
            txtPassword.Enabled = enabled;
            btnLogin.Enabled = enabled;
            btnExit.Enabled = enabled;
            chkRemember.Enabled = enabled;
        }

        private string GetLocalIPAddress()
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _authService?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
