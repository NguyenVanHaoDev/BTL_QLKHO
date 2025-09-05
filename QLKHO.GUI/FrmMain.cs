using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLKHO.DAL.Models;

namespace QLKHO.GUI
{
    public partial class FrmMain : Form
    {
        private Timer timer;
        private ToolStripStatusLabel lblUser;
        private ToolStripStatusLabel lblDatabase;
        private ToolStripStatusLabel lblVersion;
        
        public NguoiDung NguoiDungDangNhap { get; set; }

        public FrmMain()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }
        
        public FrmMain(NguoiDung nguoiDung) : this()
        {
            NguoiDungDangNhap = nguoiDung;
            if (NguoiDungDangNhap != null)
            {
                lblUser.Text = $"User: {NguoiDungDangNhap.HoTen} ({NguoiDungDangNhap.VaiTro})";
                CapNhatMenuTheoVaiTro(NguoiDungDangNhap.VaiTro);
            }
        }

        private void InitializeCustomComponents()
        {
            // Khởi tạo timer
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            // Thêm các label vào status bar
            lblUser = new ToolStripStatusLabel("User: Admin");
            lblDatabase = new ToolStripStatusLabel("DB: Connected");
            lblVersion = new ToolStripStatusLabel("v1.0.0");

            statusStrip1.Items.Add(new ToolStripSeparator());
            statusStrip1.Items.Add(lblUser);
            statusStrip1.Items.Add(new ToolStripSeparator());
            statusStrip1.Items.Add(lblDatabase);
            statusStrip1.Items.Add(new ToolStripSeparator());
            statusStrip1.Items.Add(lblVersion);

            // Thiết lập màu sắc chuyên nghiệp
            this.BackColor = Color.FromArgb(240, 240, 240);
            menuStrip1.BackColor = Color.FromArgb(63, 114, 237);
            menuStrip1.ForeColor = Color.White;
            statusStrip1.BackColor = Color.FromArgb(52, 73, 94);
            statusStrip1.ForeColor = Color.White;

            // Thiết lập icon cho form
            this.Icon = SystemIcons.Application;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Khởi tạo form
            this.WindowState = FormWindowState.Maximized;
            this.Text = "HỆ THỐNG QUẢN LÝ KHO HÀNG - QLKHO v1.0";
            
            // Kiểm tra kết nối database
            CheckDatabaseConnection();
            
            // Hiển thị thông báo chào mừng
            ShowWelcomeMessage();
        }

        private void CheckDatabaseConnection()
        {
            try
            {
                if (QLKHO.DAL.DatabaseHelper.TestConnection())
                {
                    lblDatabase.Text = "DB: Connected ✓";
                    lblDatabase.ForeColor = Color.LightGreen;
                    lblStatus.Text = "Hệ thống sẵn sàng hoạt động";
                }
                else
                {
                    lblDatabase.Text = "DB: Disconnected ✗";
                    lblDatabase.ForeColor = Color.Red;
                    lblStatus.Text = "Không thể kết nối database";
                }
            }
            catch (Exception ex)
            {
                lblDatabase.Text = "DB: Error ✗";
                lblDatabase.ForeColor = Color.Red;
                lblStatus.Text = $"Lỗi kết nối: {ex.Message}";
            }
        }

        private void ShowWelcomeMessage()
        {
            lblStatus.Text = "Chào mừng đến với hệ thống quản lý kho hàng QLKHO";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        }

        #region Menu Quản lý

        private void menuQuanLySanPham_Click(object sender, EventArgs e)
        {
            OpenChildForm<FrmSanPham>("Quản lý sản phẩm");
        }

        private void menuQuanLyDanhMuc_Click(object sender, EventArgs e)
        {
            OpenChildForm<FramDanhMuc>("Quản lý danh mục");
        }

        private void menuQuanLyDonViTinh_Click(object sender, EventArgs e)
        {
            OpenChildForm<FrmDonViTinh>("Quản lý đơn vị tính");
        }

        private void menuQuanLyKhachHang_Click(object sender, EventArgs e)
        {
            OpenChildForm<FrmKhachHang>("Quản lý khách hàng");
        }

        private void menuQuanLyNhaCungCap_Click(object sender, EventArgs e)
        {
            OpenChildForm<FrmNhaCungCap>("Quản lý nhà cung cấp");
        }

        #endregion

        #region Menu Kho hàng

        private void menuNhapHang_Click(object sender, EventArgs e)
        {
            OpenChildForm<FrmNhapHang>("Nhập hàng");
        }

        private void menuXuatHang_Click(object sender, EventArgs e)
        {
            OpenChildForm<FrmXuatHang>("Xuất hàng");
        }

        private void menuBaoCaoTonKho_Click(object sender, EventArgs e)
        {
            OpenChildForm<FrmBaoCaoTonKho>("Báo cáo tồn kho");
        }

        #endregion

        #region Menu Trình bày

        private void menuCascade_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
            lblStatus.Text = "Đã sắp xếp form theo kiểu Cascade";
        }

        private void menuTileHorizontal_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
            lblStatus.Text = "Đã sắp xếp form theo kiểu Tile Horizontal";
        }

        private void menuTileVertical_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
            lblStatus.Text = "Đã sắp xếp form theo kiểu Tile Vertical";
        }

        private void menuCloseAll_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đóng tất cả form con?", 
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                foreach (Form childForm in this.MdiChildren)
                {
                    childForm.Close();
                }
                lblStatus.Text = "Đã đóng tất cả form con";
            }
        }

        #endregion

        #region Menu Hệ thống

        private void menuDangNhap_Click(object sender, EventArgs e)
        {
            var frmDangNhap = new FrmDangNhap();
            if (frmDangNhap.ShowDialog() == DialogResult.OK)
            {
                lblUser.Text = $"User: {frmDangNhap.NguoiDungDangNhap.HoTen} ({frmDangNhap.NguoiDungDangNhap.VaiTro})";
                lblStatus.Text = $"Đăng nhập thành công: {frmDangNhap.NguoiDungDangNhap.HoTen}";
                
                // Cập nhật menu theo vai trò
                CapNhatMenuTheoVaiTro(frmDangNhap.NguoiDungDangNhap.VaiTro);
            }
        }

        private void CapNhatMenuTheoVaiTro(string vaiTro)
        {
            // Ẩn/hiện menu theo vai trò
            switch (vaiTro.ToLower())
            {
                case "admin":
                    // Admin có quyền truy cập tất cả
                    break;
                case "quanly":
                    // Quản lý có quyền hạn chế
                    break;
                case "nhanvien":
                    // Nhân viên có quyền hạn chế nhất
                    break;
            }
        }

        private void menuCaiDat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng cài đặt đang được phát triển", "Thông báo", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuThoat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", 
                "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        #endregion

        #region Menu Trợ giúp

        private void menuAbout_Click(object sender, EventArgs e)
        {
            var aboutForm = new FrmAbout();
            aboutForm.ShowDialog();
        }

        private void menuHuongDan_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hướng dẫn sử dụng đang được phát triển", "Thông báo", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Helper Methods

        private void OpenChildForm<T>(string title) where T : Form, new()
        {
            try
            {
                // Kiểm tra form đã mở chưa
                foreach (Form frm in this.MdiChildren)
                {
                    if (frm is T)
                    {
                        frm.Activate();
                        lblStatus.Text = $"{title} đã được mở";
                        return;
                    }
                }

                // Tạo form mới
                var newForm = new T();
                newForm.MdiParent = this;
                newForm.Text = title;
                newForm.Show();
                lblStatus.Text = $"Đã mở {title}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở {title}: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = $"Lỗi mở {title}";
            }
        }

        private void menuQuanLyNguoiDung_Click(object sender, EventArgs e)
        {
            try
            {
                var frmNguoiDung = new FrmNguoiDung();
                frmNguoiDung.MdiParent = this;
                frmNguoiDung.Show();
                lblStatus.Text = "Mở form quản lý người dùng";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form quản lý người dùng: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Form Events

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", 
                "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                timer?.Stop();
                timer?.Dispose();
            }
        }

        #endregion

    }
}