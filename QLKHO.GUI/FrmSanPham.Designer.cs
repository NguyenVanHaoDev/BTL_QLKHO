namespace QLKHO.GUI
{
    partial class FrmSanPham
    {
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dgvSanPham = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.lblTimKiem = new System.Windows.Forms.Label();
            this.cboTrangThaiFilter = new System.Windows.Forms.ComboBox();
            this.lblTrangThaiFilter = new System.Windows.Forms.Label();
            this.cboDanhMucFilter = new System.Windows.Forms.ComboBox();
            this.lblDanhMucFilter = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.chkTrangThai = new System.Windows.Forms.CheckBox();
            this.cboDonViTinh = new System.Windows.Forms.ComboBox();
            this.cboDanhMuc = new System.Windows.Forms.ComboBox();
            this.txtSoLuongToiThieu = new System.Windows.Forms.TextBox();
            this.txtSoLuongTon = new System.Windows.Forms.TextBox();
            this.txtGiaBan = new System.Windows.Forms.TextBox();
            this.txtGiaNhap = new System.Windows.Forms.TextBox();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.txtTenSP = new System.Windows.Forms.TextBox();
            this.txtMaSP = new System.Windows.Forms.TextBox();
            this.lblSoLuongToiThieu = new System.Windows.Forms.Label();
            this.lblSoLuongTon = new System.Windows.Forms.Label();
            this.lblGiaBan = new System.Windows.Forms.Label();
            this.lblGiaNhap = new System.Windows.Forms.Label();
            this.lblDonViTinh = new System.Windows.Forms.Label();
            this.lblDanhMuc = new System.Windows.Forms.Label();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.lblTenSP = new System.Windows.Forms.Label();
            this.lblMaSP = new System.Windows.Forms.Label();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.lblFormTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            
            // dgvSanPham
            this.dgvSanPham.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSanPham.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSanPham.Location = new System.Drawing.Point(12, 110);
            this.dgvSanPham.Name = "dgvSanPham";
            this.dgvSanPham.Size = new System.Drawing.Size(1660, 270);
            this.dgvSanPham.TabIndex = 0;
            
            // panel1
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.txtTimKiem);
            this.panel1.Controls.Add(this.lblTimKiem);
            this.panel1.Controls.Add(this.cboTrangThaiFilter);
            this.panel1.Controls.Add(this.lblTrangThaiFilter);
            this.panel1.Controls.Add(this.cboDanhMucFilter);
            this.panel1.Controls.Add(this.lblDanhMucFilter);
            this.panel1.Controls.Add(this.lblTotal);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1684, 100);
            this.panel1.TabIndex = 1;
            
            // txtTimKiem
            this.txtTimKiem.Location = new System.Drawing.Point(15, 45);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(200, 20);
            this.txtTimKiem.TabIndex = 8;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);
            // 
            // lblTimKiem
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Location = new System.Drawing.Point(15, 30);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(49, 13);
            this.lblTimKiem.TabIndex = 7;
            this.lblTimKiem.Text = "Tìm kiếm:";
            // 
            // cboTrangThaiFilter
            this.cboTrangThaiFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThaiFilter.FormattingEnabled = true;
            this.cboTrangThaiFilter.Items.AddRange(new object[] {
            "Tất cả",
            "Hoạt động",
            "Không hoạt động"});
            this.cboTrangThaiFilter.Location = new System.Drawing.Point(250, 45);
            this.cboTrangThaiFilter.Name = "cboTrangThaiFilter";
            this.cboTrangThaiFilter.Size = new System.Drawing.Size(120, 21);
            this.cboTrangThaiFilter.TabIndex = 6;
            this.cboTrangThaiFilter.SelectedIndexChanged += new System.EventHandler(this.cboTrangThaiFilter_SelectedIndexChanged);
            // 
            // lblTrangThaiFilter
            this.lblTrangThaiFilter.AutoSize = true;
            this.lblTrangThaiFilter.Location = new System.Drawing.Point(250, 30);
            this.lblTrangThaiFilter.Name = "lblTrangThaiFilter";
            this.lblTrangThaiFilter.Size = new System.Drawing.Size(58, 13);
            this.lblTrangThaiFilter.TabIndex = 5;
            this.lblTrangThaiFilter.Text = "Trạng thái:";
            // 
            // cboDanhMucFilter
            this.cboDanhMucFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDanhMucFilter.FormattingEnabled = true;
            this.cboDanhMucFilter.Location = new System.Drawing.Point(400, 45);
            this.cboDanhMucFilter.Name = "cboDanhMucFilter";
            this.cboDanhMucFilter.Size = new System.Drawing.Size(150, 21);
            this.cboDanhMucFilter.TabIndex = 4;
            this.cboDanhMucFilter.SelectedIndexChanged += new System.EventHandler(this.cboDanhMucFilter_SelectedIndexChanged);
            // 
            // lblDanhMucFilter
            this.lblDanhMucFilter.AutoSize = true;
            this.lblDanhMucFilter.Location = new System.Drawing.Point(400, 30);
            this.lblDanhMucFilter.Name = "lblDanhMucFilter";
            this.lblDanhMucFilter.Size = new System.Drawing.Size(58, 13);
            this.lblDanhMucFilter.TabIndex = 3;
            this.lblDanhMucFilter.Text = "Danh mục:";
            // 
            // lblTotal
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(580, 48);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(56, 13);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "Đang tải...";
            
            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(100, 35);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý sản phẩm";
            
            // panel2
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.LightGray;
            this.panel2.Controls.Add(this.btnXoa);
            this.panel2.Controls.Add(this.btnHuy);
            this.panel2.Controls.Add(this.btnLuu);
            this.panel2.Controls.Add(this.chkTrangThai);
            this.panel2.Controls.Add(this.cboDonViTinh);
            this.panel2.Controls.Add(this.cboDanhMuc);
            this.panel2.Controls.Add(this.txtSoLuongToiThieu);
            this.panel2.Controls.Add(this.txtSoLuongTon);
            this.panel2.Controls.Add(this.txtGiaBan);
            this.panel2.Controls.Add(this.txtGiaNhap);
            this.panel2.Controls.Add(this.txtMoTa);
            this.panel2.Controls.Add(this.txtTenSP);
            this.panel2.Controls.Add(this.txtMaSP);
            this.panel2.Controls.Add(this.lblSoLuongToiThieu);
            this.panel2.Controls.Add(this.lblSoLuongTon);
            this.panel2.Controls.Add(this.lblGiaBan);
            this.panel2.Controls.Add(this.lblGiaNhap);
            this.panel2.Controls.Add(this.lblDonViTinh);
            this.panel2.Controls.Add(this.lblDanhMuc);
            this.panel2.Controls.Add(this.lblMoTa);
            this.panel2.Controls.Add(this.lblTenSP);
            this.panel2.Controls.Add(this.lblMaSP);
            this.panel2.Controls.Add(this.lblTrangThai);
            this.panel2.Controls.Add(this.lblFormTitle);
            this.panel2.Location = new System.Drawing.Point(12, 390);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1660, 200);
            this.panel2.TabIndex = 2;
            
            // Controls setup
            this.btnXoa.Enabled = false;
            this.btnXoa.Location = new System.Drawing.Point(300, 150);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(75, 30);
            this.btnXoa.TabIndex = 23;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            
            this.btnHuy.Location = new System.Drawing.Point(200, 150);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(75, 30);
            this.btnHuy.TabIndex = 22;
            this.btnHuy.Text = "Làm mới";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            
            this.btnLuu.Location = new System.Drawing.Point(100, 150);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(75, 30);
            this.btnLuu.TabIndex = 21;
            this.btnLuu.Text = "Thêm mới";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            
            this.chkTrangThai.AutoSize = true;
            this.chkTrangThai.Checked = true;
            this.chkTrangThai.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrangThai.Location = new System.Drawing.Point(100, 120);
            this.chkTrangThai.Name = "chkTrangThai";
            this.chkTrangThai.Size = new System.Drawing.Size(77, 17);
            this.chkTrangThai.TabIndex = 20;
            this.chkTrangThai.Text = "Hoạt động";
            this.chkTrangThai.UseVisualStyleBackColor = true;
            
            this.cboDonViTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDonViTinh.FormattingEnabled = true;
            this.cboDonViTinh.Location = new System.Drawing.Point(500, 40);
            this.cboDonViTinh.Name = "cboDonViTinh";
            this.cboDonViTinh.Size = new System.Drawing.Size(150, 21);
            this.cboDonViTinh.TabIndex = 19;
            
            this.cboDanhMuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDanhMuc.FormattingEnabled = true;
            this.cboDanhMuc.Location = new System.Drawing.Point(500, 15);
            this.cboDanhMuc.Name = "cboDanhMuc";
            this.cboDanhMuc.Size = new System.Drawing.Size(150, 21);
            this.cboDanhMuc.TabIndex = 18;
            
            this.txtSoLuongToiThieu.Location = new System.Drawing.Point(800, 40);
            this.txtSoLuongToiThieu.Name = "txtSoLuongToiThieu";
            this.txtSoLuongToiThieu.Size = new System.Drawing.Size(100, 20);
            this.txtSoLuongToiThieu.TabIndex = 17;
            
            this.txtSoLuongTon.Location = new System.Drawing.Point(800, 15);
            this.txtSoLuongTon.Name = "txtSoLuongTon";
            this.txtSoLuongTon.Size = new System.Drawing.Size(100, 20);
            this.txtSoLuongTon.TabIndex = 16;
            
            this.txtGiaBan.Location = new System.Drawing.Point(100, 90);
            this.txtGiaBan.Name = "txtGiaBan";
            this.txtGiaBan.Size = new System.Drawing.Size(100, 20);
            this.txtGiaBan.TabIndex = 15;
            
            this.txtGiaNhap.Location = new System.Drawing.Point(100, 65);
            this.txtGiaNhap.Name = "txtGiaNhap";
            this.txtGiaNhap.Size = new System.Drawing.Size(100, 20);
            this.txtGiaNhap.TabIndex = 14;
            
            this.txtMoTa.Location = new System.Drawing.Point(300, 40);
            this.txtMoTa.MaxLength = 500;
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.Size = new System.Drawing.Size(150, 20);
            this.txtMoTa.TabIndex = 13;
            
            this.txtTenSP.Location = new System.Drawing.Point(100, 40);
            this.txtTenSP.MaxLength = 100;
            this.txtTenSP.Name = "txtTenSP";
            this.txtTenSP.Size = new System.Drawing.Size(150, 20);
            this.txtTenSP.TabIndex = 12;
            
            this.txtMaSP.Location = new System.Drawing.Point(100, 15);
            this.txtMaSP.Name = "txtMaSP";
            this.txtMaSP.ReadOnly = true;
            this.txtMaSP.Size = new System.Drawing.Size(100, 20);
            this.txtMaSP.TabIndex = 11;
            
            // Labels
            this.lblSoLuongToiThieu.AutoSize = true;
            this.lblSoLuongToiThieu.Location = new System.Drawing.Point(700, 43);
            this.lblSoLuongToiThieu.Name = "lblSoLuongToiThieu";
            this.lblSoLuongToiThieu.Size = new System.Drawing.Size(94, 13);
            this.lblSoLuongToiThieu.TabIndex = 10;
            this.lblSoLuongToiThieu.Text = "Số lượng tối thiểu";
            
            this.lblSoLuongTon.AutoSize = true;
            this.lblSoLuongTon.Location = new System.Drawing.Point(700, 18);
            this.lblSoLuongTon.Name = "lblSoLuongTon";
            this.lblSoLuongTon.Size = new System.Drawing.Size(70, 13);
            this.lblSoLuongTon.TabIndex = 9;
            this.lblSoLuongTon.Text = "Số lượng tồn";
            
            this.lblGiaBan.AutoSize = true;
            this.lblGiaBan.Location = new System.Drawing.Point(20, 93);
            this.lblGiaBan.Name = "lblGiaBan";
            this.lblGiaBan.Size = new System.Drawing.Size(44, 13);
            this.lblGiaBan.TabIndex = 8;
            this.lblGiaBan.Text = "Giá bán";
            
            this.lblGiaNhap.AutoSize = true;
            this.lblGiaNhap.Location = new System.Drawing.Point(20, 68);
            this.lblGiaNhap.Name = "lblGiaNhap";
            this.lblGiaNhap.Size = new System.Drawing.Size(50, 13);
            this.lblGiaNhap.TabIndex = 7;
            this.lblGiaNhap.Text = "Giá nhập";
            
            this.lblDonViTinh.AutoSize = true;
            this.lblDonViTinh.Location = new System.Drawing.Point(400, 43);
            this.lblDonViTinh.Name = "lblDonViTinh";
            this.lblDonViTinh.Size = new System.Drawing.Size(65, 13);
            this.lblDonViTinh.TabIndex = 6;
            this.lblDonViTinh.Text = "Đơn vị tính";
            
            this.lblDanhMuc.AutoSize = true;
            this.lblDanhMuc.Location = new System.Drawing.Point(400, 18);
            this.lblDanhMuc.Name = "lblDanhMuc";
            this.lblDanhMuc.Size = new System.Drawing.Size(55, 13);
            this.lblDanhMuc.TabIndex = 5;
            this.lblDanhMuc.Text = "Danh mục";
            
            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Location = new System.Drawing.Point(270, 43);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new System.Drawing.Size(34, 13);
            this.lblMoTa.TabIndex = 4;
            this.lblMoTa.Text = "Mô tả";
            
            this.lblTenSP.AutoSize = true;
            this.lblTenSP.Location = new System.Drawing.Point(20, 43);
            this.lblTenSP.Name = "lblTenSP";
            this.lblTenSP.Size = new System.Drawing.Size(76, 13);
            this.lblTenSP.TabIndex = 3;
            this.lblTenSP.Text = "Tên sản phẩm";
            
            this.lblMaSP.AutoSize = true;
            this.lblMaSP.Location = new System.Drawing.Point(20, 18);
            this.lblMaSP.Name = "lblMaSP";
            this.lblMaSP.Size = new System.Drawing.Size(39, 13);
            this.lblMaSP.TabIndex = 2;
            this.lblMaSP.Text = "Mã SP";
            
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Location = new System.Drawing.Point(20, 123);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(55, 13);
            this.lblTrangThai.TabIndex = 1;
            this.lblTrangThai.Text = "Trạng thái";
            
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormTitle.Location = new System.Drawing.Point(1000, 15);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(150, 17);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "Thông tin sản phẩm";
            
            // FrmSanPham
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1684, 602);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvSanPham);
            this.Name = "FrmSanPham";
            this.Text = "Quản lý sản phẩm";
            this.Load += new System.EventHandler(this.FrmSanPham_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSanPham;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.CheckBox chkTrangThai;
        private System.Windows.Forms.ComboBox cboDonViTinh;
        private System.Windows.Forms.ComboBox cboDanhMuc;
        private System.Windows.Forms.TextBox txtSoLuongToiThieu;
        private System.Windows.Forms.TextBox txtSoLuongTon;
        private System.Windows.Forms.TextBox txtGiaBan;
        private System.Windows.Forms.TextBox txtGiaNhap;
        private System.Windows.Forms.TextBox txtMoTa;
        private System.Windows.Forms.TextBox txtTenSP;
        private System.Windows.Forms.TextBox txtMaSP;
        private System.Windows.Forms.Label lblSoLuongToiThieu;
        private System.Windows.Forms.Label lblSoLuongTon;
        private System.Windows.Forms.Label lblGiaBan;
        private System.Windows.Forms.Label lblGiaNhap;
        private System.Windows.Forms.Label lblDonViTinh;
        private System.Windows.Forms.Label lblDanhMuc;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.Label lblTenSP;
        private System.Windows.Forms.Label lblMaSP;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.ComboBox cboTrangThaiFilter;
        private System.Windows.Forms.Label lblTrangThaiFilter;
        private System.Windows.Forms.ComboBox cboDanhMucFilter;
        private System.Windows.Forms.Label lblDanhMucFilter;
    }
}
