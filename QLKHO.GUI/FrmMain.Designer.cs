namespace QLKHO.GUI
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuQuanLy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQuanLySanPham = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQuanLyDanhMuc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQuanLyDonViTinh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuQuanLyKhachHang = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQuanLyNhaCungCap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuKhoHang = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNhapHang = new System.Windows.Forms.ToolStripMenuItem();
            this.menuXuatHang = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuBaoCaoTonKho = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTrinhBay = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCascade = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTileHorizontal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTileVertical = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHeThong = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDangNhap = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuQuanLyNguoiDung = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCaiDat = new System.Windows.Forms.ToolStripMenuItem();
            this.menuThoat = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTroGiup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHuongDan = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(114)))), ((int)(((byte)(237)))));
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuQuanLy,
            this.menuKhoHang,
            this.menuTrinhBay,
            this.menuHeThong,
            this.menuTroGiup});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1400, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuQuanLy
            // 
            this.menuQuanLy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuQuanLySanPham,
            this.menuQuanLyDanhMuc,
            this.menuQuanLyDonViTinh,
            this.toolStripSeparator1,
            this.menuQuanLyKhachHang,
            this.menuQuanLyNhaCungCap});
            this.menuQuanLy.ForeColor = System.Drawing.Color.White;
            this.menuQuanLy.Name = "menuQuanLy";
            this.menuQuanLy.Size = new System.Drawing.Size(60, 20);
            this.menuQuanLy.Text = "Quản lý";
            // 
            // menuQuanLySanPham
            // 
            this.menuQuanLySanPham.Name = "menuQuanLySanPham";
            this.menuQuanLySanPham.Size = new System.Drawing.Size(190, 22);
            this.menuQuanLySanPham.Text = "Quản lý sản phẩm";
            this.menuQuanLySanPham.Click += new System.EventHandler(this.menuQuanLySanPham_Click);
            // 
            // menuQuanLyDanhMuc
            // 
            this.menuQuanLyDanhMuc.Name = "menuQuanLyDanhMuc";
            this.menuQuanLyDanhMuc.Size = new System.Drawing.Size(190, 22);
            this.menuQuanLyDanhMuc.Text = "Quản lý danh mục";
            this.menuQuanLyDanhMuc.Click += new System.EventHandler(this.menuQuanLyDanhMuc_Click);
            // 
            // menuQuanLyDonViTinh
            // 
            this.menuQuanLyDonViTinh.Name = "menuQuanLyDonViTinh";
            this.menuQuanLyDonViTinh.Size = new System.Drawing.Size(190, 22);
            this.menuQuanLyDonViTinh.Text = "Quản lý đơn vị tính";
            this.menuQuanLyDonViTinh.Click += new System.EventHandler(this.menuQuanLyDonViTinh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(187, 6);
            // 
            // menuQuanLyKhachHang
            // 
            this.menuQuanLyKhachHang.Name = "menuQuanLyKhachHang";
            this.menuQuanLyKhachHang.Size = new System.Drawing.Size(190, 22);
            this.menuQuanLyKhachHang.Text = "Quản lý khách hàng";
            this.menuQuanLyKhachHang.Click += new System.EventHandler(this.menuQuanLyKhachHang_Click);
            // 
            // menuQuanLyNhaCungCap
            // 
            this.menuQuanLyNhaCungCap.Name = "menuQuanLyNhaCungCap";
            this.menuQuanLyNhaCungCap.Size = new System.Drawing.Size(190, 22);
            this.menuQuanLyNhaCungCap.Text = "Quản lý nhà cung cấp";
            this.menuQuanLyNhaCungCap.Click += new System.EventHandler(this.menuQuanLyNhaCungCap_Click);
            // 
            // menuKhoHang
            // 
            this.menuKhoHang.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNhapHang,
            this.menuXuatHang,
            this.toolStripSeparator2,
            this.menuBaoCaoTonKho});
            this.menuKhoHang.ForeColor = System.Drawing.Color.White;
            this.menuKhoHang.Name = "menuKhoHang";
            this.menuKhoHang.Size = new System.Drawing.Size(70, 20);
            this.menuKhoHang.Text = "Kho hàng";
            // 
            // menuNhapHang
            // 
            this.menuNhapHang.Name = "menuNhapHang";
            this.menuNhapHang.Size = new System.Drawing.Size(160, 22);
            this.menuNhapHang.Text = "Nhập hàng";
            this.menuNhapHang.Click += new System.EventHandler(this.menuNhapHang_Click);
            // 
            // menuXuatHang
            // 
            this.menuXuatHang.Name = "menuXuatHang";
            this.menuXuatHang.Size = new System.Drawing.Size(160, 22);
            this.menuXuatHang.Text = "Xuất hàng";
            this.menuXuatHang.Click += new System.EventHandler(this.menuXuatHang_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // menuBaoCaoTonKho
            // 
            this.menuBaoCaoTonKho.Name = "menuBaoCaoTonKho";
            this.menuBaoCaoTonKho.Size = new System.Drawing.Size(160, 22);
            this.menuBaoCaoTonKho.Text = "Báo cáo tồn kho";
            this.menuBaoCaoTonKho.Click += new System.EventHandler(this.menuBaoCaoTonKho_Click);
            // 
            // menuTrinhBay
            // 
            this.menuTrinhBay.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCascade,
            this.menuTileHorizontal,
            this.menuTileVertical,
            this.toolStripSeparator3,
            this.menuCloseAll});
            this.menuTrinhBay.ForeColor = System.Drawing.Color.White;
            this.menuTrinhBay.Name = "menuTrinhBay";
            this.menuTrinhBay.Size = new System.Drawing.Size(68, 20);
            this.menuTrinhBay.Text = "Trình bày";
            // 
            // menuCascade
            // 
            this.menuCascade.Name = "menuCascade";
            this.menuCascade.Size = new System.Drawing.Size(151, 22);
            this.menuCascade.Text = "Cascade";
            this.menuCascade.Click += new System.EventHandler(this.menuCascade_Click);
            // 
            // menuTileHorizontal
            // 
            this.menuTileHorizontal.Name = "menuTileHorizontal";
            this.menuTileHorizontal.Size = new System.Drawing.Size(151, 22);
            this.menuTileHorizontal.Text = "Tile Horizontal";
            this.menuTileHorizontal.Click += new System.EventHandler(this.menuTileHorizontal_Click);
            // 
            // menuTileVertical
            // 
            this.menuTileVertical.Name = "menuTileVertical";
            this.menuTileVertical.Size = new System.Drawing.Size(151, 22);
            this.menuTileVertical.Text = "Tile Vertical";
            this.menuTileVertical.Click += new System.EventHandler(this.menuTileVertical_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(148, 6);
            // 
            // menuCloseAll
            // 
            this.menuCloseAll.Name = "menuCloseAll";
            this.menuCloseAll.Size = new System.Drawing.Size(151, 22);
            this.menuCloseAll.Text = "Đóng tất cả";
            this.menuCloseAll.Click += new System.EventHandler(this.menuCloseAll_Click);
            // 
            // menuHeThong
            // 
            this.menuHeThong.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuDangNhap,
            this.toolStripSeparator4,
            this.menuQuanLyNguoiDung,
            this.menuCaiDat,
            this.menuThoat});
            this.menuHeThong.ForeColor = System.Drawing.Color.White;
            this.menuHeThong.Name = "menuHeThong";
            this.menuHeThong.Size = new System.Drawing.Size(69, 20);
            this.menuHeThong.Text = "Hệ thống";
            // 
            // menuDangNhap
            // 
            this.menuDangNhap.Name = "menuDangNhap";
            this.menuDangNhap.Size = new System.Drawing.Size(132, 22);
            this.menuDangNhap.Text = "Đăng nhập";
            this.menuDangNhap.Click += new System.EventHandler(this.menuDangNhap_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(129, 6);
            // 
            // menuQuanLyNguoiDung
            // 
            this.menuQuanLyNguoiDung.Name = "menuQuanLyNguoiDung";
            this.menuQuanLyNguoiDung.Size = new System.Drawing.Size(132, 22);
            this.menuQuanLyNguoiDung.Text = "Quản lý người dùng";
            this.menuQuanLyNguoiDung.Click += new System.EventHandler(this.menuQuanLyNguoiDung_Click);
            // 
            // menuCaiDat
            // 
            this.menuCaiDat.Name = "menuCaiDat";
            this.menuCaiDat.Size = new System.Drawing.Size(132, 22);
            this.menuCaiDat.Text = "Cài đặt";
            this.menuCaiDat.Click += new System.EventHandler(this.menuCaiDat_Click);
            // 
            // menuThoat
            // 
            this.menuThoat.Name = "menuThoat";
            this.menuThoat.Size = new System.Drawing.Size(132, 22);
            this.menuThoat.Text = "Thoát";
            this.menuThoat.Click += new System.EventHandler(this.menuThoat_Click);
            // 
            // menuTroGiup
            // 
            this.menuTroGiup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHuongDan,
            this.toolStripSeparator5,
            this.menuAbout});
            this.menuTroGiup.ForeColor = System.Drawing.Color.White;
            this.menuTroGiup.Name = "menuTroGiup";
            this.menuTroGiup.Size = new System.Drawing.Size(63, 20);
            this.menuTroGiup.Text = "Trợ giúp";
            // 
            // menuHuongDan
            // 
            this.menuHuongDan.Name = "menuHuongDan";
            this.menuHuongDan.Size = new System.Drawing.Size(134, 22);
            this.menuHuongDan.Text = "Hướng dẫn";
            this.menuHuongDan.Click += new System.EventHandler(this.menuHuongDan_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(131, 6);
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(134, 22);
            this.menuAbout.Text = "Thông tin";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 679);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1400, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(54, 17);
            this.lblStatus.Text = "Sẵn sàng";
            // 
            // lblTime
            // 
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(49, 17);
            this.lblTime.Text = "00:00:00";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1400, 701);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HỆ THỐNG QUẢN LÝ KHO HÀNG - QLKHO v1.0";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuQuanLy;
        private System.Windows.Forms.ToolStripMenuItem menuQuanLySanPham;
        private System.Windows.Forms.ToolStripMenuItem menuQuanLyDanhMuc;
        private System.Windows.Forms.ToolStripMenuItem menuQuanLyDonViTinh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuQuanLyKhachHang;
        private System.Windows.Forms.ToolStripMenuItem menuQuanLyNhaCungCap;
        private System.Windows.Forms.ToolStripMenuItem menuKhoHang;
        private System.Windows.Forms.ToolStripMenuItem menuNhapHang;
        private System.Windows.Forms.ToolStripMenuItem menuXuatHang;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuBaoCaoTonKho;
        private System.Windows.Forms.ToolStripMenuItem menuTrinhBay;
        private System.Windows.Forms.ToolStripMenuItem menuCascade;
        private System.Windows.Forms.ToolStripMenuItem menuTileHorizontal;
        private System.Windows.Forms.ToolStripMenuItem menuTileVertical;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuCloseAll;
        private System.Windows.Forms.ToolStripMenuItem menuHeThong;
        private System.Windows.Forms.ToolStripMenuItem menuDangNhap;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuQuanLyNguoiDung;
        private System.Windows.Forms.ToolStripMenuItem menuCaiDat;
        private System.Windows.Forms.ToolStripMenuItem menuThoat;
        private System.Windows.Forms.ToolStripMenuItem menuTroGiup;
        private System.Windows.Forms.ToolStripMenuItem menuHuongDan;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblTime;
    }
}
