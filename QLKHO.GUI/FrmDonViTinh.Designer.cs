namespace QLKHO.GUI
{
    partial class FrmDonViTinh
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvDonViTinh = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.lblTimKiem = new System.Windows.Forms.Label();
            this.cboTrangThaiFilter = new System.Windows.Forms.ComboBox();
            this.lblTrangThaiFilter = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.chkTrangThai = new System.Windows.Forms.CheckBox();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.txtKyHieu = new System.Windows.Forms.TextBox();
            this.txtTenDVT = new System.Windows.Forms.TextBox();
            this.txtMaDVT = new System.Windows.Forms.TextBox();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.lblKyHieu = new System.Windows.Forms.Label();
            this.lblTenDVT = new System.Windows.Forms.Label();
            this.lblMaDVT = new System.Windows.Forms.Label();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.lblFormTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDonViTinh)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDonViTinh
            // 
            this.dgvDonViTinh.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDonViTinh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDonViTinh.Location = new System.Drawing.Point(12, 110);
            this.dgvDonViTinh.Name = "dgvDonViTinh";
            this.dgvDonViTinh.Size = new System.Drawing.Size(1660, 270);
            this.dgvDonViTinh.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.txtTimKiem);
            this.panel1.Controls.Add(this.lblTimKiem);
            this.panel1.Controls.Add(this.cboTrangThaiFilter);
            this.panel1.Controls.Add(this.lblTrangThaiFilter);
            this.panel1.Controls.Add(this.lblTotal);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1684, 100);
            this.panel1.TabIndex = 1;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Location = new System.Drawing.Point(15, 45);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(200, 20);
            this.txtTimKiem.TabIndex = 6;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);
            // 
            // lblTimKiem
            // 
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Location = new System.Drawing.Point(15, 30);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(49, 13);
            this.lblTimKiem.TabIndex = 5;
            this.lblTimKiem.Text = "Tìm kiếm:";
            // 
            // cboTrangThaiFilter
            // 
            this.cboTrangThaiFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThaiFilter.FormattingEnabled = true;
            this.cboTrangThaiFilter.Items.AddRange(new object[] {
            "Tất cả",
            "Hoạt động",
            "Không hoạt động"});
            this.cboTrangThaiFilter.Location = new System.Drawing.Point(250, 45);
            this.cboTrangThaiFilter.Name = "cboTrangThaiFilter";
            this.cboTrangThaiFilter.Size = new System.Drawing.Size(120, 21);
            this.cboTrangThaiFilter.TabIndex = 4;
            this.cboTrangThaiFilter.SelectedIndexChanged += new System.EventHandler(this.cboTrangThaiFilter_SelectedIndexChanged);
            // 
            // lblTrangThaiFilter
            // 
            this.lblTrangThaiFilter.AutoSize = true;
            this.lblTrangThaiFilter.Location = new System.Drawing.Point(250, 30);
            this.lblTrangThaiFilter.Name = "lblTrangThaiFilter";
            this.lblTrangThaiFilter.Size = new System.Drawing.Size(58, 13);
            this.lblTrangThaiFilter.TabIndex = 3;
            this.lblTrangThaiFilter.Text = "Trạng thái:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(400, 48);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(56, 13);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "Đang tải...";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(100, 35);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý đơn vị tính";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.LightGray;
            this.panel2.Controls.Add(this.btnXoa);
            this.panel2.Controls.Add(this.btnHuy);
            this.panel2.Controls.Add(this.btnLuu);
            this.panel2.Controls.Add(this.chkTrangThai);
            this.panel2.Controls.Add(this.txtMoTa);
            this.panel2.Controls.Add(this.txtKyHieu);
            this.panel2.Controls.Add(this.txtTenDVT);
            this.panel2.Controls.Add(this.txtMaDVT);
            this.panel2.Controls.Add(this.lblMoTa);
            this.panel2.Controls.Add(this.lblKyHieu);
            this.panel2.Controls.Add(this.lblTenDVT);
            this.panel2.Controls.Add(this.lblMaDVT);
            this.panel2.Controls.Add(this.lblTrangThai);
            this.panel2.Controls.Add(this.lblFormTitle);
            this.panel2.Location = new System.Drawing.Point(12, 390);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1660, 200);
            this.panel2.TabIndex = 2;
            // 
            // btnXoa
            // 
            this.btnXoa.Enabled = false;
            this.btnXoa.Location = new System.Drawing.Point(300, 150);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(75, 30);
            this.btnXoa.TabIndex = 13;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.Location = new System.Drawing.Point(200, 150);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(75, 30);
            this.btnHuy.TabIndex = 12;
            this.btnHuy.Text = "Làm mới";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.Location = new System.Drawing.Point(100, 150);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(75, 30);
            this.btnLuu.TabIndex = 11;
            this.btnLuu.Text = "Thêm mới";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // chkTrangThai
            // 
            this.chkTrangThai.AutoSize = true;
            this.chkTrangThai.Checked = true;
            this.chkTrangThai.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrangThai.Location = new System.Drawing.Point(100, 120);
            this.chkTrangThai.Name = "chkTrangThai";
            this.chkTrangThai.Size = new System.Drawing.Size(77, 17);
            this.chkTrangThai.TabIndex = 10;
            this.chkTrangThai.Text = "Hoạt động";
            this.chkTrangThai.UseVisualStyleBackColor = true;
            // 
            // txtMoTa
            // 
            this.txtMoTa.Location = new System.Drawing.Point(100, 90);
            this.txtMoTa.MaxLength = 200;
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.Size = new System.Drawing.Size(300, 20);
            this.txtMoTa.TabIndex = 9;
            // 
            // txtKyHieu
            // 
            this.txtKyHieu.Location = new System.Drawing.Point(100, 65);
            this.txtKyHieu.MaxLength = 10;
            this.txtKyHieu.Name = "txtKyHieu";
            this.txtKyHieu.Size = new System.Drawing.Size(100, 20);
            this.txtKyHieu.TabIndex = 8;
            // 
            // txtTenDVT
            // 
            this.txtTenDVT.Location = new System.Drawing.Point(100, 40);
            this.txtTenDVT.MaxLength = 50;
            this.txtTenDVT.Name = "txtTenDVT";
            this.txtTenDVT.Size = new System.Drawing.Size(200, 20);
            this.txtTenDVT.TabIndex = 7;
            // 
            // txtMaDVT
            // 
            this.txtMaDVT.Location = new System.Drawing.Point(100, 15);
            this.txtMaDVT.Name = "txtMaDVT";
            this.txtMaDVT.ReadOnly = true;
            this.txtMaDVT.Size = new System.Drawing.Size(100, 20);
            this.txtMaDVT.TabIndex = 6;
            // 
            // lblMoTa
            // 
            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Location = new System.Drawing.Point(20, 93);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new System.Drawing.Size(34, 13);
            this.lblMoTa.TabIndex = 5;
            this.lblMoTa.Text = "Mô tả";
            // 
            // lblKyHieu
            // 
            this.lblKyHieu.AutoSize = true;
            this.lblKyHieu.Location = new System.Drawing.Point(20, 68);
            this.lblKyHieu.Name = "lblKyHieu";
            this.lblKyHieu.Size = new System.Drawing.Size(44, 13);
            this.lblKyHieu.TabIndex = 4;
            this.lblKyHieu.Text = "Ký hiệu";
            // 
            // lblTenDVT
            // 
            this.lblTenDVT.AutoSize = true;
            this.lblTenDVT.Location = new System.Drawing.Point(20, 43);
            this.lblTenDVT.Name = "lblTenDVT";
            this.lblTenDVT.Size = new System.Drawing.Size(79, 13);
            this.lblTenDVT.TabIndex = 3;
            this.lblTenDVT.Text = "Tên đơn vị tính";
            // 
            // lblMaDVT
            // 
            this.lblMaDVT.AutoSize = true;
            this.lblMaDVT.Location = new System.Drawing.Point(20, 18);
            this.lblMaDVT.Name = "lblMaDVT";
            this.lblMaDVT.Size = new System.Drawing.Size(47, 13);
            this.lblMaDVT.TabIndex = 2;
            this.lblMaDVT.Text = "Mã DVT";
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Location = new System.Drawing.Point(20, 123);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(55, 13);
            this.lblTrangThai.TabIndex = 1;
            this.lblTrangThai.Text = "Trạng thái";
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormTitle.Location = new System.Drawing.Point(500, 15);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(150, 17);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "Thông tin đơn vị tính";
            // 
            // FrmDonViTinh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1684, 602);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvDonViTinh);
            this.Name = "FrmDonViTinh";
            this.Text = "Quản lý đơn vị tính";
            this.Load += new System.EventHandler(this.FrmDonViTinh_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDonViTinh)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDonViTinh;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.CheckBox chkTrangThai;
        private System.Windows.Forms.TextBox txtMoTa;
        private System.Windows.Forms.TextBox txtKyHieu;
        private System.Windows.Forms.TextBox txtTenDVT;
        private System.Windows.Forms.TextBox txtMaDVT;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.Label lblKyHieu;
        private System.Windows.Forms.Label lblTenDVT;
        private System.Windows.Forms.Label lblMaDVT;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.ComboBox cboTrangThaiFilter;
        private System.Windows.Forms.Label lblTrangThaiFilter;
    }
}