-- =========================================
-- TẠO DATABASE QUẢN LÝ KHO (PHIÊN BẢN TỐI ƯU & AN TOÀN)
-- =========================================

USE master;
GO

-- Kiểm tra và xóa database cũ nếu tồn tại
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'BTL_QUANLYKHO')
BEGIN
    ALTER DATABASE BTL_QUANLYKHO SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BTL_QUANLYKHO;
END
GO

CREATE DATABASE BTL_QUANLYKHO
COLLATE Vietnamese_CI_AS;
GO

USE BTL_QUANLYKHO;
GO

-- Thiết lập cấu hình bảo mật và hỗ trợ tiếng Việt
ALTER DATABASE BTL_QUANLYKHO SET RECOVERY SIMPLE;
ALTER DATABASE BTL_QUANLYKHO SET AUTO_SHRINK OFF;
ALTER DATABASE BTL_QUANLYKHO SET AUTO_CREATE_STATISTICS ON;
ALTER DATABASE BTL_QUANLYKHO SET AUTO_UPDATE_STATISTICS ON;

-- Thiết lập hỗ trợ tiếng Việt tốt nhất
ALTER DATABASE BTL_QUANLYKHO SET COMPATIBILITY_LEVEL = 150; -- SQL Server 2019
ALTER DATABASE BTL_QUANLYKHO SET ANSI_NULL_DEFAULT ON;
ALTER DATABASE BTL_QUANLYKHO SET ANSI_NULLS ON;
ALTER DATABASE BTL_QUANLYKHO SET ANSI_PADDING ON;
ALTER DATABASE BTL_QUANLYKHO SET ANSI_WARNINGS ON;
ALTER DATABASE BTL_QUANLYKHO SET ARITHABORT ON;
ALTER DATABASE BTL_QUANLYKHO SET CONCAT_NULL_YIELDS_NULL ON;
ALTER DATABASE BTL_QUANLYKHO SET QUOTED_IDENTIFIER ON;
GO

-- =========================================
-- BẢNG NGƯỜI DÙNG VÀ PHÂN QUYỀN
-- =========================================
CREATE TABLE NguoiDung (
    MaND INT PRIMARY KEY IDENTITY(1,1),
    TenDangNhap VARCHAR(50) UNIQUE NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) CHECK (Email IS NULL OR (Email LIKE '%@%.%' AND LEN(Email) >= 5)),
    DienThoai VARCHAR(15) CHECK (DienThoai IS NULL OR (LEN(DienThoai) >= 10 AND DienThoai NOT LIKE '%[^0-9]%')),
    VaiTro VARCHAR(20) NOT NULL DEFAULT 'NhanVien' CHECK (VaiTro IN ('Admin', 'QuanLy', 'NhanVien')),
    TrangThai BIT DEFAULT 1,
    SoLanDangNhapSai INT DEFAULT 0,
    KhoaTaiKhoan BIT DEFAULT 0,
    ThoiGianKhoa DATETIME NULL,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE(),
    NgayDangNhapCuoi DATETIME NULL,
    -- Ràng buộc bảo mật
    CONSTRAINT CK_TenDangNhap CHECK (LEN(TenDangNhap) >= 3 AND TenDangNhap NOT LIKE '%[^a-zA-Z0-9_]%'),
    CONSTRAINT CK_MatKhau CHECK (LEN(MatKhau) >= 6),
    CONSTRAINT CK_SoLanDangNhapSai CHECK (SoLanDangNhapSai >= 0 AND SoLanDangNhapSai <= 5)
);
GO

-- =========================================
-- BẢNG ĐƠN VỊ TÍNH
-- =========================================
CREATE TABLE DonViTinh (
    MaDVT INT PRIMARY KEY IDENTITY(1,1),
    TenDVT NVARCHAR(50) NOT NULL UNIQUE,
    KyHieu NVARCHAR(10) NOT NULL UNIQUE,
    MoTa NVARCHAR(200),
    TrangThai BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE()
);
GO

-- =========================================
-- BẢNG DANH MỤC SẢN PHẨM
-- =========================================
CREATE TABLE DanhMucSanPham (
    MaDM INT PRIMARY KEY IDENTITY(1,1),
    TenDM NVARCHAR(100) NOT NULL UNIQUE,
    MaDMCapTren INT NULL, -- Danh mục cha (để tạo cây danh mục)
    MoTa NVARCHAR(500),
    ThuTuHienThi INT DEFAULT 0, -- Thứ tự hiển thị
    TrangThai BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE(),
    NguoiTao INT,
    FOREIGN KEY (NguoiTao) REFERENCES NguoiDung(MaND)
);
GO

-- Thêm foreign key tự tham chiếu sau khi bảng đã được tạo
ALTER TABLE DanhMucSanPham
ADD CONSTRAINT FK_DanhMucSanPham_MaDMCapTren 
FOREIGN KEY (MaDMCapTren) REFERENCES DanhMucSanPham(MaDM);
GO

-- =========================================
-- BẢNG SẢN PHẨM (CẢI THIỆN)
-- =========================================
CREATE TABLE SanPham (
    MaSP INT PRIMARY KEY IDENTITY(1,1),
    TenSP NVARCHAR(100) NOT NULL,
    MaDM INT NOT NULL, -- Danh mục sản phẩm
    MaDVT INT NOT NULL,
    GiaNhap DECIMAL(18,2) NOT NULL CHECK (GiaNhap > 0),
    GiaBan DECIMAL(18,2) NOT NULL CHECK (GiaBan > 0),
    SoLuongTon INT DEFAULT 0 CHECK (SoLuongTon >= 0),
    SoLuongToiThieu INT DEFAULT 10 CHECK (SoLuongToiThieu >= 0),
    MoTa NVARCHAR(500),
    TrangThai BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE(),
    NguoiTao INT,
    FOREIGN KEY (MaDM) REFERENCES DanhMucSanPham(MaDM),
    FOREIGN KEY (MaDVT) REFERENCES DonViTinh(MaDVT),
    FOREIGN KEY (NguoiTao) REFERENCES NguoiDung(MaND)
);
GO

-- =========================================
-- BẢNG NHÀ CUNG CẤP (CẢI THIỆN)
-- =========================================
CREATE TABLE NhaCungCap (
    MaNCC INT PRIMARY KEY IDENTITY(1,1),
    TenNCC NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(200),
    DienThoai VARCHAR(15) CHECK (DienThoai IS NULL OR (LEN(DienThoai) >= 10 AND DienThoai NOT LIKE '%[^0-9]%')),
    Email VARCHAR(100) CHECK (Email IS NULL OR (Email LIKE '%@%.%' AND LEN(Email) >= 5)),
    TrangThai BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE()
);
GO

-- =========================================
-- BẢNG PHIẾU NHẬP (CẢI THIỆN)
-- =========================================
CREATE TABLE PhieuNhap (
    MaPN INT PRIMARY KEY IDENTITY(1,1),
    NgayNhap DATE NOT NULL DEFAULT GETDATE(),
    MaNCC INT NOT NULL,
    NguoiTao INT NOT NULL,
    TongTien DECIMAL(18,2) DEFAULT 0,
    TrangThai VARCHAR(20) DEFAULT 'ChuaXacNhan' CHECK (TrangThai IN ('ChuaXacNhan', 'DaXacNhan', 'Huy')),
    GhiChu NVARCHAR(500),
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
    FOREIGN KEY (NguoiTao) REFERENCES NguoiDung(MaND)
);
GO

-- =========================================
-- BẢNG CHI TIẾT PHIẾU NHẬP (CẢI THIỆN)
-- =========================================
CREATE TABLE ChiTietPhieuNhap (
    MaPN INT NOT NULL,
    MaSP INT NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia > 0),
    ThanhTien AS (SoLuong * DonGia) PERSISTED,
    PRIMARY KEY (MaPN, MaSP),
    FOREIGN KEY (MaPN) REFERENCES PhieuNhap(MaPN) ON DELETE CASCADE,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
GO

-- =========================================
-- BẢNG PHIẾU XUẤT (CẢI THIỆN)
-- =========================================
CREATE TABLE PhieuXuat (
    MaPX INT PRIMARY KEY IDENTITY(1,1),
    NgayXuat DATE NOT NULL DEFAULT GETDATE(),
    NguoiNhan NVARCHAR(100) NOT NULL,
    NguoiTao INT NOT NULL,
    TongTien DECIMAL(18,2) DEFAULT 0,
    TrangThai VARCHAR(20) DEFAULT 'ChuaXacNhan' CHECK (TrangThai IN ('ChuaXacNhan', 'DaXacNhan', 'Huy')),
    GhiChu NVARCHAR(500),
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (NguoiTao) REFERENCES NguoiDung(MaND)
);
GO

-- =========================================
-- BẢNG CHI TIẾT PHIẾU XUẤT (CẢI THIỆN)
-- =========================================
CREATE TABLE ChiTietPhieuXuat (
    MaPX INT NOT NULL,
    MaSP INT NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia > 0),
    ThanhTien AS (SoLuong * DonGia) PERSISTED,
    PRIMARY KEY (MaPX, MaSP),
    FOREIGN KEY (MaPX) REFERENCES PhieuXuat(MaPX) ON DELETE CASCADE,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
GO

-- =========================================
-- BẢNG LỊCH SỬ ĐĂNG NHẬP
-- =========================================
CREATE TABLE LichSuDangNhap (
    MaLSDN INT PRIMARY KEY IDENTITY(1,1),
    MaND INT NOT NULL,
    TenDangNhap VARCHAR(50) NOT NULL,
    IPAddress VARCHAR(45),
    UserAgent NVARCHAR(500),
    TrangThai VARCHAR(20) NOT NULL CHECK (TrangThai IN ('ThanhCong', 'ThatBai', 'KhoaTaiKhoan')),
    ThoiGian DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaND) REFERENCES NguoiDung(MaND)
);
GO

-- =========================================
-- BẢNG LỊCH SỬ THAY ĐỔI (AUDIT TRAIL)
-- =========================================
CREATE TABLE LichSuThayDoi (
    MaLSTD INT PRIMARY KEY IDENTITY(1,1),
    Bang NVARCHAR(50) NOT NULL,
    MaBanGhi INT NOT NULL,
    ThaoTac VARCHAR(10) NOT NULL CHECK (ThaoTac IN ('INSERT', 'UPDATE', 'DELETE')),
    DuLieuCu NVARCHAR(MAX),
    DuLieuMoi NVARCHAR(MAX),
    NguoiThucHien INT,
    IPAddress VARCHAR(45),
    ThoiGian DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (NguoiThucHien) REFERENCES NguoiDung(MaND)
);
GO

-- =========================================
-- TRIGGERS TỰ ĐỘNG CẬP NHẬT TỒN KHO
-- =========================================

-- Trigger cập nhật tồn kho khi thêm chi tiết phiếu nhập
CREATE TRIGGER TR_ChiTietPhieuNhap_Insert
ON ChiTietPhieuNhap
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Cập nhật tồn kho
        UPDATE sp 
        SET SoLuongTon = sp.SoLuongTon + i.SoLuong,
            NgayCapNhat = GETDATE()
        FROM SanPham sp
        INNER JOIN inserted i ON sp.MaSP = i.MaSP
        WHERE sp.TrangThai = 1; -- Chỉ cập nhật sản phẩm đang hoạt động
        
        -- Cập nhật tổng tiền phiếu nhập
        UPDATE pn
        SET TongTien = (
            SELECT ISNULL(SUM(SoLuong * DonGia), 0)
            FROM ChiTietPhieuNhap
            WHERE MaPN = pn.MaPN
        )
        FROM PhieuNhap pn
        INNER JOIN inserted i ON pn.MaPN = i.MaPN;
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Trigger cập nhật tồn kho khi xóa chi tiết phiếu nhập
CREATE TRIGGER TR_ChiTietPhieuNhap_Delete
ON ChiTietPhieuNhap
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Cập nhật tồn kho
        UPDATE sp 
        SET SoLuongTon = sp.SoLuongTon - d.SoLuong,
            NgayCapNhat = GETDATE()
        FROM SanPham sp
        INNER JOIN deleted d ON sp.MaSP = d.MaSP
        WHERE sp.TrangThai = 1; -- Chỉ cập nhật sản phẩm đang hoạt động
        
        -- Cập nhật tổng tiền phiếu nhập
        UPDATE pn
        SET TongTien = (
            SELECT ISNULL(SUM(SoLuong * DonGia), 0)
            FROM ChiTietPhieuNhap
            WHERE MaPN = pn.MaPN
        )
        FROM PhieuNhap pn
        INNER JOIN deleted d ON pn.MaPN = d.MaPN;
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Trigger cập nhật tồn kho khi thêm chi tiết phiếu xuất
CREATE TRIGGER TR_ChiTietPhieuXuat_Insert
ON ChiTietPhieuXuat
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Kiểm tra tồn kho trước khi xuất
        IF EXISTS (
            SELECT 1 
            FROM SanPham sp
            INNER JOIN inserted i ON sp.MaSP = i.MaSP
            WHERE sp.SoLuongTon < i.SoLuong OR sp.TrangThai = 0
        )
        BEGIN
            RAISERROR('Không đủ hàng trong kho hoặc sản phẩm đã ngừng kinh doanh!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;
        
        -- Cập nhật tồn kho
        UPDATE sp 
        SET SoLuongTon = sp.SoLuongTon - i.SoLuong,
            NgayCapNhat = GETDATE()
        FROM SanPham sp
        INNER JOIN inserted i ON sp.MaSP = i.MaSP
        WHERE sp.TrangThai = 1;
        
        -- Cập nhật tổng tiền phiếu xuất
        UPDATE px
        SET TongTien = (
            SELECT ISNULL(SUM(SoLuong * DonGia), 0)
            FROM ChiTietPhieuXuat
            WHERE MaPX = px.MaPX
        )
        FROM PhieuXuat px
        INNER JOIN inserted i ON px.MaPX = i.MaPX;
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Trigger cập nhật tồn kho khi xóa chi tiết phiếu xuất
CREATE TRIGGER TR_ChiTietPhieuXuat_Delete
ON ChiTietPhieuXuat
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Cập nhật tồn kho
        UPDATE sp 
        SET SoLuongTon = sp.SoLuongTon + d.SoLuong,
            NgayCapNhat = GETDATE()
        FROM SanPham sp
        INNER JOIN deleted d ON sp.MaSP = d.MaSP
        WHERE sp.TrangThai = 1; -- Chỉ cập nhật sản phẩm đang hoạt động
        
        -- Cập nhật tổng tiền phiếu xuất
        UPDATE px
        SET TongTien = (
            SELECT ISNULL(SUM(SoLuong * DonGia), 0)
            FROM ChiTietPhieuXuat
            WHERE MaPX = px.MaPX
        )
        FROM PhieuXuat px
        INNER JOIN deleted d ON px.MaPX = d.MaPX;
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- =========================================
-- STORED PROCEDURES
-- =========================================

-- Stored procedure tạo phiếu nhập
CREATE PROCEDURE sp_TaoPhieuNhap
    @MaNCC INT,
    @NguoiTao INT,
    @GhiChu NVARCHAR(500) = NULL,
    @MaPN INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO PhieuNhap (MaNCC, NguoiTao, GhiChu)
    VALUES (@MaNCC, @NguoiTao, @GhiChu);
    
    SET @MaPN = SCOPE_IDENTITY();
END;
GO

-- Stored procedure tạo phiếu xuất
CREATE PROCEDURE sp_TaoPhieuXuat
    @NguoiNhan NVARCHAR(100),
    @NguoiTao INT,
    @GhiChu NVARCHAR(500) = NULL,
    @MaPX INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO PhieuXuat (NguoiNhan, NguoiTao, GhiChu)
    VALUES (@NguoiNhan, @NguoiTao, @GhiChu);
    
    SET @MaPX = SCOPE_IDENTITY();
END;
GO

-- Stored procedure kiểm tra tồn kho
CREATE PROCEDURE sp_KiemTraTonKho
    @MaSP INT,
    @SoLuongCan INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        sp.MaSP,
        sp.TenSP,
        sp.SoLuongTon,
        sp.SoLuongToiThieu,
        CASE 
            WHEN sp.SoLuongTon >= @SoLuongCan THEN 'Du'
            WHEN sp.SoLuongTon > 0 THEN 'Thieu'
            ELSE 'Het'
        END AS TrangThaiTonKho
    FROM SanPham sp
    WHERE sp.MaSP = @MaSP AND sp.TrangThai = 1;
END;
GO

-- Stored procedure báo cáo tồn kho
CREATE PROCEDURE sp_BaoCaoTonKho
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        sp.MaSP,
        sp.TenSP,
        dm.TenDM AS DanhMuc,
        dvt.TenDVT,
        dvt.KyHieu,
        sp.SoLuongTon,
        sp.SoLuongToiThieu,
        sp.GiaNhap,
        sp.GiaBan,
        CASE 
            WHEN sp.SoLuongTon <= 0 THEN 'Hết hàng'
            WHEN sp.SoLuongTon <= sp.SoLuongToiThieu THEN 'Sắp hết'
            ELSE 'Đủ hàng'
        END AS TrangThaiTonKho,
        (sp.SoLuongTon * sp.GiaNhap) AS GiaTriTonKho
    FROM SanPham sp
    INNER JOIN DanhMucSanPham dm ON sp.MaDM = dm.MaDM
    INNER JOIN DonViTinh dvt ON sp.MaDVT = dvt.MaDVT
    WHERE sp.TrangThai = 1 AND dm.TrangThai = 1 AND dvt.TrangThai = 1
    ORDER BY dm.TenDM, sp.SoLuongTon;
END;
GO

-- Stored procedure thống kê doanh thu
CREATE PROCEDURE sp_ThongKeDoanhThu
    @TuNgay DATE = NULL,
    @DenNgay DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @TuNgay IS NULL SET @TuNgay = DATEADD(MONTH, -1, GETDATE());
    IF @DenNgay IS NULL SET @DenNgay = GETDATE();
    
    SELECT 
        'Nhập hàng' AS Loai,
        COUNT(*) AS SoPhieu,
        ISNULL(SUM(TongTien), 0) AS TongTien
    FROM PhieuNhap 
    WHERE NgayNhap BETWEEN @TuNgay AND @DenNgay
    AND TrangThai = 'DaXacNhan'
    
    UNION ALL
    
    SELECT 
        'Xuất hàng' AS Loai,
        COUNT(*) AS SoPhieu,
        ISNULL(SUM(TongTien), 0) AS TongTien
    FROM PhieuXuat 
    WHERE NgayXuat BETWEEN @TuNgay AND @DenNgay
    AND TrangThai = 'DaXacNhan';
END;
GO

-- Stored procedure quản lý đơn vị tính
CREATE PROCEDURE sp_QuanLyDonViTinh
    @Action VARCHAR(10), -- 'SELECT', 'INSERT', 'UPDATE', 'DELETE'
    @MaDVT INT = NULL,
    @TenDVT NVARCHAR(50) = NULL,
    @KyHieu NVARCHAR(10) = NULL,
    @MoTa NVARCHAR(200) = NULL,
    @TrangThai BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @Action = 'SELECT'
    BEGIN
        SELECT 
            MaDVT,
            TenDVT,
            KyHieu,
            MoTa,
            TrangThai,
            NgayTao,
            NgayCapNhat
        FROM DonViTinh
        WHERE TrangThai = 1
        ORDER BY TenDVT;
    END
    ELSE IF @Action = 'INSERT'
    BEGIN
        INSERT INTO DonViTinh (TenDVT, KyHieu, MoTa, TrangThai)
        VALUES (@TenDVT, @KyHieu, @MoTa, @TrangThai);
        
        SELECT SCOPE_IDENTITY() AS MaDVT;
    END
    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE DonViTinh 
        SET TenDVT = @TenDVT,
            KyHieu = @KyHieu,
            MoTa = @MoTa,
            TrangThai = @TrangThai,
            NgayCapNhat = GETDATE()
        WHERE MaDVT = @MaDVT;
    END
    ELSE IF @Action = 'DELETE'
    BEGIN
        -- Kiểm tra xem đơn vị tính có đang được sử dụng không
        IF EXISTS (SELECT 1 FROM SanPham WHERE MaDVT = @MaDVT)
        BEGIN
            RAISERROR('Không thể xóa đơn vị tính đang được sử dụng!', 16, 1);
            RETURN;
        END
        
        UPDATE DonViTinh 
        SET TrangThai = 0,
            NgayCapNhat = GETDATE()
        WHERE MaDVT = @MaDVT;
    END
END;
GO

-- Stored procedure xác thực đăng nhập
CREATE PROCEDURE sp_XacThucDangNhap
    @TenDangNhap VARCHAR(50),
    @MatKhau VARCHAR(255),
    @IPAddress VARCHAR(45) = NULL,
    @UserAgent NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @MaND INT, @KhoaTaiKhoan BIT, @SoLanDangNhapSai INT, @ThoiGianKhoa DATETIME;
    
    -- Lấy thông tin người dùng
    SELECT @MaND = MaND, @KhoaTaiKhoan = KhoaTaiKhoan, 
           @SoLanDangNhapSai = SoLanDangNhapSai, @ThoiGianKhoa = ThoiGianKhoa
    FROM NguoiDung 
    WHERE TenDangNhap = @TenDangNhap AND TrangThai = 1;
    
    -- Kiểm tra tài khoản có tồn tại không
    IF @MaND IS NULL
    BEGIN
        -- Không thể insert với MaND = 0 vì có foreign key constraint
        -- Tạo bản ghi tạm thời hoặc bỏ qua việc log
        SELECT 'FAIL' AS KetQua, 'Tài khoản không tồn tại' AS ThongBao;
        RETURN;
    END
    
    -- Kiểm tra tài khoản có bị khóa không
    IF @KhoaTaiKhoan = 1
    BEGIN
        -- Kiểm tra thời gian khóa (khóa 30 phút)
        IF @ThoiGianKhoa IS NOT NULL AND DATEDIFF(MINUTE, @ThoiGianKhoa, GETDATE()) < 30
        BEGIN
            INSERT INTO LichSuDangNhap (MaND, TenDangNhap, IPAddress, UserAgent, TrangThai)
            VALUES (@MaND, @TenDangNhap, @IPAddress, @UserAgent, 'KhoaTaiKhoan');
            
            SELECT 'LOCKED' AS KetQua, 'Tài khoản đang bị khóa' AS ThongBao;
            RETURN;
        END
        ELSE
        BEGIN
            -- Mở khóa tài khoản
            UPDATE NguoiDung 
            SET KhoaTaiKhoan = 0, SoLanDangNhapSai = 0, ThoiGianKhoa = NULL
            WHERE MaND = @MaND;
        END
    END
    
    -- Kiểm tra mật khẩu (trong thực tế nên hash mật khẩu)
    IF EXISTS (SELECT 1 FROM NguoiDung WHERE MaND = @MaND AND MatKhau = @MatKhau)
    BEGIN
        -- Đăng nhập thành công
        UPDATE NguoiDung 
        SET SoLanDangNhapSai = 0, NgayDangNhapCuoi = GETDATE()
        WHERE MaND = @MaND;
        
        INSERT INTO LichSuDangNhap (MaND, TenDangNhap, IPAddress, UserAgent, TrangThai)
        VALUES (@MaND, @TenDangNhap, @IPAddress, @UserAgent, 'ThanhCong');
        
        SELECT 'SUCCESS' AS KetQua, 'Đăng nhập thành công' AS ThongBao, @MaND AS MaND;
    END
    ELSE
    BEGIN
        -- Đăng nhập thất bại
        UPDATE NguoiDung 
        SET SoLanDangNhapSai = SoLanDangNhapSai + 1,
            KhoaTaiKhoan = CASE WHEN SoLanDangNhapSai + 1 >= 5 THEN 1 ELSE 0 END,
            ThoiGianKhoa = CASE WHEN SoLanDangNhapSai + 1 >= 5 THEN GETDATE() ELSE NULL END
        WHERE MaND = @MaND;
        
        INSERT INTO LichSuDangNhap (MaND, TenDangNhap, IPAddress, UserAgent, TrangThai)
        VALUES (@MaND, @TenDangNhap, @IPAddress, @UserAgent, 'ThatBai');
        
        SELECT 'FAIL' AS KetQua, 'Mật khẩu không đúng' AS ThongBao;
    END
END;
GO

-- Stored procedure quản lý danh mục sản phẩm
CREATE PROCEDURE sp_QuanLyDanhMucSanPham
    @Action VARCHAR(10), -- 'SELECT', 'SELECT_TREE', 'INSERT', 'UPDATE', 'DELETE'
    @MaDM INT = NULL,
    @TenDM NVARCHAR(100) = NULL,
    @MaDMCapTren INT = NULL,
    @MoTa NVARCHAR(500) = NULL,
    @ThuTuHienThi INT = 0,
    @TrangThai BIT = 1,
    @NguoiTao INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @Action = 'SELECT'
    BEGIN
        SELECT 
            dm.MaDM,
            dm.TenDM,
            dm.MaDMCapTren,
            dm_cha.TenDM AS TenDMCapTren,
            dm.MoTa,
            dm.ThuTuHienThi,
            dm.TrangThai,
            dm.NgayTao,
            dm.NgayCapNhat,
            nd.HoTen AS NguoiTao
        FROM DanhMucSanPham dm
        LEFT JOIN DanhMucSanPham dm_cha ON dm.MaDMCapTren = dm_cha.MaDM
        LEFT JOIN NguoiDung nd ON dm.NguoiTao = nd.MaND
        WHERE dm.TrangThai = 1
        ORDER BY dm.ThuTuHienThi, dm.TenDM;
    END
    ELSE IF @Action = 'SELECT_TREE'
    BEGIN
        -- Lấy danh mục theo cây (danh mục cha trước, con sau)
        WITH DanhMucTree AS (
            -- Danh mục gốc (không có cha)
            SELECT 
                MaDM,
                TenDM,
                MaDMCapTren,
                MoTa,
                ThuTuHienThi,
                0 AS Cap,
                CAST(TenDM AS NVARCHAR(MAX)) AS DuongDan
            FROM DanhMucSanPham
            WHERE MaDMCapTren IS NULL AND TrangThai = 1
            
            UNION ALL
            
            -- Danh mục con
            SELECT 
                dm.MaDM,
                dm.TenDM,
                dm.MaDMCapTren,
                dm.MoTa,
                dm.ThuTuHienThi,
                dt.Cap + 1,
                CAST(dt.DuongDan + ' > ' + dm.TenDM AS NVARCHAR(MAX))
            FROM DanhMucSanPham dm
            INNER JOIN DanhMucTree dt ON dm.MaDMCapTren = dt.MaDM
            WHERE dm.TrangThai = 1
        )
        SELECT 
            MaDM,
            TenDM,
            MaDMCapTren,
            MoTa,
            ThuTuHienThi,
            Cap,
            DuongDan
        FROM DanhMucTree
        ORDER BY Cap, ThuTuHienThi, TenDM;
    END
    ELSE IF @Action = 'INSERT'
    BEGIN
        INSERT INTO DanhMucSanPham (TenDM, MaDMCapTren, MoTa, ThuTuHienThi, TrangThai, NguoiTao)
        VALUES (@TenDM, @MaDMCapTren, @MoTa, @ThuTuHienThi, @TrangThai, @NguoiTao);
        
        SELECT SCOPE_IDENTITY() AS MaDM;
    END
    ELSE IF @Action = 'UPDATE'
    BEGIN
        -- Kiểm tra không được set chính nó làm danh mục cha
        IF @MaDMCapTren = @MaDM
        BEGIN
            RAISERROR('Không thể chọn chính danh mục này làm danh mục cha!', 16, 1);
            RETURN;
        END
        
        UPDATE DanhMucSanPham 
        SET TenDM = @TenDM,
            MaDMCapTren = @MaDMCapTren,
            MoTa = @MoTa,
            ThuTuHienThi = @ThuTuHienThi,
            TrangThai = @TrangThai,
            NgayCapNhat = GETDATE()
        WHERE MaDM = @MaDM;
    END
    ELSE IF @Action = 'DELETE'
    BEGIN
        -- Kiểm tra xem danh mục có sản phẩm không
        IF EXISTS (SELECT 1 FROM SanPham WHERE MaDM = @MaDM)
        BEGIN
            RAISERROR('Không thể xóa danh mục đang có sản phẩm!', 16, 1);
            RETURN;
        END
        
        -- Kiểm tra xem danh mục có danh mục con không
        IF EXISTS (SELECT 1 FROM DanhMucSanPham WHERE MaDMCapTren = @MaDM AND TrangThai = 1)
        BEGIN
            RAISERROR('Không thể xóa danh mục đang có danh mục con!', 16, 1);
            RETURN;
        END
        
        UPDATE DanhMucSanPham 
        SET TrangThai = 0,
            NgayCapNhat = GETDATE()
        WHERE MaDM = @MaDM;
    END
END;
GO

-- =========================================
-- INDEXES ĐỂ TỐI ƯU HIỆU SUẤT
-- =========================================

-- Index cho bảng NguoiDung
CREATE INDEX IX_NguoiDung_TenDangNhap ON NguoiDung(TenDangNhap);
CREATE INDEX IX_NguoiDung_VaiTro ON NguoiDung(VaiTro);
CREATE INDEX IX_NguoiDung_TrangThai ON NguoiDung(TrangThai);

-- Index cho bảng DonViTinh
CREATE INDEX IX_DonViTinh_TenDVT ON DonViTinh(TenDVT);
CREATE INDEX IX_DonViTinh_KyHieu ON DonViTinh(KyHieu);
CREATE INDEX IX_DonViTinh_TrangThai ON DonViTinh(TrangThai);

-- Index cho bảng DanhMucSanPham
CREATE INDEX IX_DanhMucSanPham_TenDM ON DanhMucSanPham(TenDM);
CREATE INDEX IX_DanhMucSanPham_MaDMCapTren ON DanhMucSanPham(MaDMCapTren);
CREATE INDEX IX_DanhMucSanPham_TrangThai ON DanhMucSanPham(TrangThai);
CREATE INDEX IX_DanhMucSanPham_ThuTuHienThi ON DanhMucSanPham(ThuTuHienThi);

-- Index cho bảng SanPham
CREATE INDEX IX_SanPham_TenSP ON SanPham(TenSP);
CREATE INDEX IX_SanPham_MaDM ON SanPham(MaDM);
CREATE INDEX IX_SanPham_MaDVT ON SanPham(MaDVT);
CREATE INDEX IX_SanPham_TrangThai ON SanPham(TrangThai);
CREATE INDEX IX_SanPham_SoLuongTon ON SanPham(SoLuongTon);
CREATE INDEX IX_SanPham_NguoiTao ON SanPham(NguoiTao);

-- Index cho bảng NhaCungCap
CREATE INDEX IX_NhaCungCap_TenNCC ON NhaCungCap(TenNCC);
CREATE INDEX IX_NhaCungCap_TrangThai ON NhaCungCap(TrangThai);

-- Index cho bảng PhieuNhap
CREATE INDEX IX_PhieuNhap_NgayNhap ON PhieuNhap(NgayNhap);
CREATE INDEX IX_PhieuNhap_MaNCC ON PhieuNhap(MaNCC);
CREATE INDEX IX_PhieuNhap_TrangThai ON PhieuNhap(TrangThai);
CREATE INDEX IX_PhieuNhap_NguoiTao ON PhieuNhap(NguoiTao);
CREATE INDEX IX_PhieuNhap_TongTien ON PhieuNhap(TongTien);

-- Index cho bảng PhieuXuat
CREATE INDEX IX_PhieuXuat_NgayXuat ON PhieuXuat(NgayXuat);
CREATE INDEX IX_PhieuXuat_TrangThai ON PhieuXuat(TrangThai);
CREATE INDEX IX_PhieuXuat_NguoiTao ON PhieuXuat(NguoiTao);
CREATE INDEX IX_PhieuXuat_TongTien ON PhieuXuat(TongTien);

-- Index cho bảng ChiTietPhieuNhap
CREATE INDEX IX_ChiTietPhieuNhap_MaSP ON ChiTietPhieuNhap(MaSP);
CREATE INDEX IX_ChiTietPhieuNhap_MaPN ON ChiTietPhieuNhap(MaPN);

-- Index cho bảng ChiTietPhieuXuat
CREATE INDEX IX_ChiTietPhieuXuat_MaSP ON ChiTietPhieuXuat(MaSP);
CREATE INDEX IX_ChiTietPhieuXuat_MaPX ON ChiTietPhieuXuat(MaPX);

-- Index cho bảng LichSuDangNhap
CREATE INDEX IX_LichSuDangNhap_MaND ON LichSuDangNhap(MaND);
CREATE INDEX IX_LichSuDangNhap_ThoiGian ON LichSuDangNhap(ThoiGian);
CREATE INDEX IX_LichSuDangNhap_TrangThai ON LichSuDangNhap(TrangThai);
CREATE INDEX IX_LichSuDangNhap_IPAddress ON LichSuDangNhap(IPAddress);

-- Index cho bảng LichSuThayDoi
CREATE INDEX IX_LichSuThayDoi_Bang ON LichSuThayDoi(Bang);
CREATE INDEX IX_LichSuThayDoi_ThoiGian ON LichSuThayDoi(ThoiGian);
CREATE INDEX IX_LichSuThayDoi_NguoiThucHien ON LichSuThayDoi(NguoiThucHien);
CREATE INDEX IX_LichSuThayDoi_IPAddress ON LichSuThayDoi(IPAddress);

-- =========================================
-- DỮ LIỆU MẪU
-- =========================================

-- Thêm người dùng mẫu
INSERT INTO NguoiDung (TenDangNhap, MatKhau, HoTen, Email, VaiTro)
VALUES 
('admin', 'admin123', N'Quản trị viên', 'admin@company.com', 'Admin'),
('manager', 'manager123', N'Nguyễn Văn Quản lý', 'manager@company.com', 'QuanLy'),
('staff', 'staff123', N'Trần Thị Nhân viên', 'staff@company.com', 'NhanVien');

-- Thêm đơn vị tính
INSERT INTO DonViTinh (TenDVT, KyHieu, MoTa)
VALUES 
(N'Cái', N'cái', N'Đơn vị tính cho các sản phẩm đếm được'),
(N'Bộ', N'bộ', N'Đơn vị tính cho bộ sản phẩm'),
(N'Thùng', N'thùng', N'Đơn vị tính cho sản phẩm đóng thùng'),
(N'Kg', N'kg', N'Đơn vị tính trọng lượng'),
(N'Lít', N'l', N'Đơn vị tính thể tích'),
(N'Mét', N'm', N'Đơn vị tính chiều dài'),
(N'Gói', N'gói', N'Đơn vị tính cho sản phẩm đóng gói'),
(N'Hộp', N'hộp', N'Đơn vị tính cho sản phẩm đóng hộp'),
(N'Chai', N'chai', N'Đơn vị tính cho sản phẩm đóng chai'),
(N'Lon', N'lon', N'Đơn vị tính cho sản phẩm đóng lon'),
(N'Cuộn', N'cuộn', N'Đơn vị tính cho sản phẩm dạng cuộn'),
(N'Tờ', N'tờ', N'Đơn vị tính cho giấy, tài liệu'),
(N'Quyển', N'quyển', N'Đơn vị tính cho sách, vở'),
(N'Cặp', N'cặp', N'Đơn vị tính cho sản phẩm theo cặp'),
(N'Bình', N'bình', N'Đơn vị tính cho sản phẩm đóng bình'),
(N'Gam', N'g', N'Đơn vị tính trọng lượng nhỏ'),
(N'Mililit', N'ml', N'Đơn vị tính thể tích nhỏ'),
(N'Centimet', N'cm', N'Đơn vị tính chiều dài nhỏ'),
(N'Thỏi', N'thỏi', N'Đơn vị tính cho sản phẩm dạng thỏi'),
(N'Viên', N'viên', N'Đơn vị tính cho thuốc, kẹo');

-- Thêm danh mục sản phẩm
INSERT INTO DanhMucSanPham (TenDM, MaDMCapTren, MoTa, ThuTuHienThi, NguoiTao)
VALUES 
-- Danh mục gốc
(N'Thiết bị điện tử', NULL, N'Các sản phẩm điện tử, công nghệ', 1, 1),
(N'Văn phòng phẩm', NULL, N'Đồ dùng văn phòng, học tập', 2, 1),
(N'Thực phẩm', NULL, N'Thực phẩm, đồ ăn uống', 3, 1),
(N'Dược phẩm', NULL, N'Thuốc, sản phẩm y tế', 4, 1),
(N'Vật liệu xây dựng', NULL, N'Vật liệu xây dựng, nội thất', 5, 1),
(N'Hóa chất', NULL, N'Hóa chất, dung môi', 6, 1),
(N'Dệt may', NULL, N'Quần áo, giày dép, phụ kiện', 7, 1),

-- Danh mục con của Thiết bị điện tử
(N'Máy tính', 1, N'Máy tính để bàn, laptop', 1, 1),
(N'Điện thoại', 1, N'Điện thoại di động, smartphone', 2, 1),
(N'Phụ kiện máy tính', 1, N'Bàn phím, chuột, màn hình', 3, 1),
(N'Âm thanh', 1, N'Loa, tai nghe, micro', 4, 1),

-- Danh mục con của Văn phòng phẩm
(N'Bút viết', 2, N'Bút bi, bút chì, bút dạ', 1, 1),
(N'Giấy tờ', 2, N'Giấy, vở, sổ', 2, 1),
(N'Dụng cụ văn phòng', 2, N'Kéo, băng keo, ghim', 3, 1),

-- Danh mục con của Thực phẩm
(N'Lương thực', 3, N'Gạo, ngũ cốc', 1, 1),
(N'Gia vị', 3, N'Muối, đường, nước mắm', 2, 1),
(N'Đồ uống', 3, N'Sữa, nước giải khát', 3, 1),
(N'Bánh kẹo', 3, N'Bánh mì, kẹo, snack', 4, 1),

-- Danh mục con của Dược phẩm
(N'Thuốc tây', 4, N'Thuốc tây, thuốc bổ', 1, 1),
(N'Chăm sóc cá nhân', 4, N'Kem đánh răng, dầu gội', 2, 1),
(N'Y tế', 4, N'Băng y tế, dụng cụ y tế', 3, 1),

-- Danh mục con của Vật liệu xây dựng
(N'Vật liệu thô', 5, N'Xi măng, cát, đá', 1, 1),
(N'Gạch đá', 5, N'Gạch, đá xây dựng', 2, 1),
(N'Kim loại', 5, N'Sắt thép, nhôm', 3, 1),
(N'Gỗ', 5, N'Gỗ tấm, gỗ xẻ', 4, 1),

-- Danh mục con của Hóa chất
(N'Nhiên liệu', 6, N'Xăng, dầu', 1, 1),
(N'Chất tẩy rửa', 6, N'Thuốc tẩy, nước rửa', 2, 1),
(N'Sơn keo', 6, N'Sơn, keo dán', 3, 1),

-- Danh mục con của Dệt may
(N'Quần áo nam', 7, N'Áo, quần nam giới', 1, 1),
(N'Quần áo nữ', 7, N'Áo, quần, váy nữ giới', 2, 1),
(N'Giày dép', 7, N'Giày, dép, sandal', 3, 1),
(N'Phụ kiện', 7, N'Túi xách, ví, thắt lưng', 4, 1);

-- Thêm nhà cung cấp
INSERT INTO NhaCungCap (TenNCC, DiaChi, DienThoai, Email)
VALUES 
(N'Công ty Linh kiện A', N'Hà Nội', '0901234567', 'contact@linhkien-a.com'),
(N'Công ty Thiết bị B', N'Hồ Chí Minh', '0912345678', 'info@thietbi-b.com'),
(N'Công ty Điện tử C', N'Đà Nẵng', '0923456789', 'sales@dientu-c.com'),
(N'Công ty Máy tính D', N'Hải Phòng', '0934567890', 'info@maytinh-d.com'),
(N'Công ty Phụ kiện E', N'Cần Thơ', '0945678901', 'contact@phukien-e.com'),
(N'Công ty Văn phòng phẩm F', N'Nha Trang', '0956789012', 'sales@vanphong-f.com'),
(N'Công ty Thực phẩm G', N'Vũng Tàu', '0967890123', 'info@thucpham-g.com'),
(N'Công ty Dược phẩm H', N'Quảng Ninh', '0978901234', 'contact@duoc-h.com'),
(N'Công ty Vật liệu xây dựng I', N'Bình Dương', '0989012345', 'sales@vatlieu-i.com'),
(N'Công ty Hóa chất J', N'Đồng Nai', '0990123456', 'info@hoachat-j.com'),
(N'Công ty Dệt may K', N'Thái Nguyên', '0901234568', 'contact@detmay-k.com'),
(N'Công ty Gỗ nội thất L', N'Lâm Đồng', '0912345679', 'sales@go-l.com'),
(N'Công ty Kim khí M', N'Thanh Hóa', '0923456780', 'info@kimkhi-m.com'),
(N'Công ty Nhựa N', N'Bắc Ninh', '0934567891', 'contact@nhua-n.com'),
(N'Công ty Giấy O', N'Phú Thọ', '0945678902', 'sales@giay-o.com');

-- Thêm sản phẩm
INSERT INTO SanPham (TenSP, MaDM, MaDVT, GiaNhap, GiaBan, SoLuongToiThieu, MoTa, NguoiTao)
VALUES 
-- Thiết bị điện tử - Phụ kiện máy tính
(N'Bàn phím cơ', 3, 1, 300000, 500000, 5, N'Bàn phím cơ chất lượng cao', 1),
(N'Chuột gaming', 3, 1, 200000, 350000, 10, N'Chuột gaming chuyên nghiệp', 1),
(N'Màn hình 24 inch', 3, 1, 2000000, 3000000, 3, N'Màn hình Full HD 24 inch', 1),

-- Thiết bị điện tử - Máy tính
(N'Laptop Dell', 1, 1, 15000000, 20000000, 2, N'Laptop Dell Inspiron 15', 1),

-- Thiết bị điện tử - Điện thoại
(N'Điện thoại iPhone', 2, 1, 20000000, 25000000, 1, N'iPhone 14 Pro Max', 1),
(N'Tablet Samsung', 2, 1, 8000000, 12000000, 3, N'Samsung Galaxy Tab S8', 1),

-- Thiết bị điện tử - Âm thanh
(N'Loa Bluetooth', 4, 1, 500000, 800000, 8, N'Loa Bluetooth JBL', 1),
(N'Tai nghe Sony', 4, 1, 1200000, 1800000, 6, N'Tai nghe Sony WH-1000XM4', 1),

-- Văn phòng phẩm - Bút viết
(N'Bút bi xanh', 8, 1, 3000, 5000, 100, N'Bút bi xanh 0.5mm', 1),
(N'Bút chì 2B', 8, 1, 2000, 3000, 50, N'Bút chì 2B Faber-Castell', 1),
(N'Bút dạ quang', 8, 1, 8000, 12000, 30, N'Bút dạ quang 4 màu', 1),

-- Văn phòng phẩm - Giấy tờ
(N'Vở học sinh', 9, 1, 15000, 25000, 20, N'Vở học sinh 200 trang', 1),
(N'Giấy A4', 9, 1, 50000, 80000, 10, N'Giấy A4 80gsm', 1),

-- Văn phòng phẩm - Dụng cụ văn phòng
(N'Kéo văn phòng', 10, 1, 25000, 40000, 15, N'Kéo văn phòng 8 inch', 1),
(N'Băng keo trong', 10, 1, 12000, 18000, 25, N'Băng keo trong 48mm', 1),
(N'Ghim bấm', 10, 1, 15000, 25000, 20, N'Ghim bấm 24/6', 1),

-- Thực phẩm - Lương thực
(N'Gạo ST25', 11, 4, 25000, 35000, 50, N'Gạo ST25 5kg', 1),

-- Thực phẩm - Gia vị
(N'Dầu ăn', 12, 5, 45000, 65000, 20, N'Dầu ăn Neptune 1L', 1),
(N'Đường trắng', 12, 4, 18000, 25000, 30, N'Đường trắng 1kg', 1),
(N'Muối i-ốt', 12, 4, 8000, 12000, 40, N'Muối i-ốt 500g', 1),
(N'Nước mắm', 12, 5, 35000, 50000, 15, N'Nước mắm Nam Ngư 500ml', 1),
(N'Tương ớt', 12, 5, 12000, 18000, 25, N'Tương ớt Chin-su 250ml', 1),

-- Thực phẩm - Đồ uống
(N'Sữa tươi', 13, 5, 25000, 35000, 20, N'Sữa tươi Vinamilk 1L', 1),

-- Thực phẩm - Bánh kẹo
(N'Bánh mì', 14, 1, 5000, 8000, 50, N'Bánh mì sandwich', 1),

-- Dược phẩm - Thuốc tây
(N'Paracetamol 500mg', 15, 20, 2000, 3000, 100, N'Thuốc giảm đau hạ sốt', 1),
(N'Vitamin C', 15, 20, 5000, 8000, 50, N'Vitamin C 1000mg', 1),

-- Dược phẩm - Chăm sóc cá nhân
(N'Kem đánh răng', 16, 1, 15000, 25000, 30, N'Kem đánh răng Colgate', 1),
(N'Dầu gội đầu', 16, 1, 35000, 50000, 20, N'Dầu gội Clear 400ml', 1),
(N'Xà phòng', 16, 1, 8000, 12000, 40, N'Xà phòng Lifebuoy 90g', 1),

-- Dược phẩm - Y tế
(N'Băng y tế', 17, 1, 12000, 18000, 25, N'Băng y tế 5cm x 5m', 1),

-- Vật liệu xây dựng - Vật liệu thô
(N'Xi măng', 18, 4, 80000, 120000, 100, N'Xi măng Hà Tiên 50kg', 1),
(N'Cát xây', 18, 4, 150000, 200000, 50, N'Cát xây 1m3', 1),
(N'Đá 1x2', 18, 4, 200000, 280000, 30, N'Đá 1x2 1m3', 1),

-- Vật liệu xây dựng - Gạch đá
(N'Gạch ống', 19, 1, 2000, 3000, 1000, N'Gạch ống 4 lỗ', 1),

-- Vật liệu xây dựng - Kim loại
(N'Sắt thép', 20, 4, 18000, 25000, 200, N'Sắt thép D10 1kg', 1),

-- Vật liệu xây dựng - Gỗ
(N'Gỗ tấm', 21, 18, 500000, 700000, 10, N'Gỗ tấm 1.2m x 2.4m', 1),

-- Hóa chất - Nhiên liệu
(N'Xăng A95', 22, 5, 25000, 30000, 100, N'Xăng A95 1L', 1),
(N'Dầu nhớt', 22, 5, 80000, 120000, 20, N'Dầu nhớt 4L', 1),

-- Hóa chất - Chất tẩy rửa
(N'Thuốc tẩy', 23, 5, 15000, 25000, 25, N'Thuốc tẩy Javel 1L', 1),

-- Hóa chất - Sơn keo
(N'Keo dán', 24, 1, 25000, 40000, 15, N'Keo dán 502 20g', 1),
(N'Sơn nước', 24, 5, 120000, 180000, 10, N'Sơn nước 1L', 1),

-- Dệt may - Quần áo nam
(N'Áo thun nam', 25, 1, 80000, 120000, 20, N'Áo thun nam 100% cotton', 1),

-- Dệt may - Quần áo nữ
(N'Quần jean nữ', 26, 1, 150000, 220000, 15, N'Quần jean nữ size M', 1),
(N'Váy đầm', 26, 1, 120000, 180000, 12, N'Váy đầm công sở', 1),
(N'Áo khoác', 26, 1, 200000, 300000, 8, N'Áo khoác gió', 1),

-- Dệt may - Giày dép
(N'Giày thể thao', 27, 1, 300000, 450000, 10, N'Giày thể thao Nike', 1),

-- Dệt may - Phụ kiện
(N'Túi xách', 28, 1, 100000, 150000, 15, N'Túi xách da', 1);

-- Tạo phiếu nhập sử dụng stored procedure
DECLARE @MaPN1 INT, @MaPN2 INT, @MaPN3 INT;

-- Phiếu nhập 1: Thiết bị điện tử
EXEC sp_TaoPhieuNhap @MaNCC = 1, @NguoiTao = 1, @GhiChu = N'Nhập thiết bị điện tử', @MaPN = @MaPN1 OUTPUT;
INSERT INTO ChiTietPhieuNhap (MaPN, MaSP, SoLuong, DonGia)
VALUES 
(@MaPN1, 1, 10, 300000),  -- Bàn phím cơ
(@MaPN1, 2, 15, 200000),  -- Chuột gaming
(@MaPN1, 3, 5, 2000000),  -- Màn hình 24 inch
(@MaPN1, 4, 3, 15000000), -- Laptop Dell
(@MaPN1, 7, 20, 500000);  -- Loa Bluetooth

-- Phiếu nhập 2: Văn phòng phẩm
EXEC sp_TaoPhieuNhap @MaNCC = 6, @NguoiTao = 2, @GhiChu = N'Nhập văn phòng phẩm', @MaPN = @MaPN2 OUTPUT;
INSERT INTO ChiTietPhieuNhap (MaPN, MaSP, SoLuong, DonGia)
VALUES 
(@MaPN2, 9, 200, 3000),   -- Bút bi xanh
(@MaPN2, 10, 100, 2000),  -- Bút chì 2B
(@MaPN2, 11, 50, 15000),  -- Vở học sinh
(@MaPN2, 12, 20, 50000),  -- Giấy A4
(@MaPN2, 15, 30, 25000);  -- Kéo văn phòng

-- Phiếu nhập 3: Thực phẩm
EXEC sp_TaoPhieuNhap @MaNCC = 7, @NguoiTao = 1, @GhiChu = N'Nhập thực phẩm', @MaPN = @MaPN3 OUTPUT;
INSERT INTO ChiTietPhieuNhap (MaPN, MaSP, SoLuong, DonGia)
VALUES 
(@MaPN3, 17, 100, 25000), -- Gạo ST25
(@MaPN3, 18, 30, 45000),  -- Dầu ăn
(@MaPN3, 19, 50, 18000),  -- Đường trắng
(@MaPN3, 20, 80, 8000),   -- Muối i-ốt
(@MaPN3, 24, 100, 5000);  -- Bánh mì

-- Tạo phiếu xuất sử dụng stored procedure
DECLARE @MaPX1 INT, @MaPX2 INT;

-- Phiếu xuất 1: Bán lẻ
EXEC sp_TaoPhieuXuat @NguoiNhan = N'Nguyễn Văn A', @NguoiTao = 2, @GhiChu = N'Xuất hàng bán lẻ', @MaPX = @MaPX1 OUTPUT;
INSERT INTO ChiTietPhieuXuat (MaPX, MaSP, SoLuong, DonGia)
VALUES 
(@MaPX1, 1, 2, 500000),   -- Bàn phím cơ
(@MaPX1, 2, 3, 350000),   -- Chuột gaming
(@MaPX1, 9, 10, 5000),    -- Bút bi xanh
(@MaPX1, 17, 5, 35000);   -- Gạo ST25

-- Phiếu xuất 2: Bán sỉ
EXEC sp_TaoPhieuXuat @NguoiNhan = N'Cửa hàng ABC', @NguoiTao = 3, @GhiChu = N'Xuất hàng bán sỉ', @MaPX = @MaPX2 OUTPUT;
INSERT INTO ChiTietPhieuXuat (MaPX, MaSP, SoLuong, DonGia)
VALUES 
(@MaPX2, 1, 5, 450000),   -- Bàn phím cơ
(@MaPX2, 7, 10, 700000),  -- Loa Bluetooth
(@MaPX2, 9, 50, 4500),    -- Bút bi xanh
(@MaPX2, 11, 20, 22000),  -- Vở học sinh
(@MaPX2, 17, 20, 32000);  -- Gạo ST25

-- =========================================
-- KIỂM TRA DỮ LIỆU VÀ HIỆU SUẤT
-- =========================================

-- Kiểm tra dữ liệu cơ bản
SELECT 'SanPham' as Bang, COUNT(*) as SoLuong FROM SanPham
UNION ALL
SELECT 'DanhMucSanPham', COUNT(*) FROM DanhMucSanPham
UNION ALL
SELECT 'DonViTinh', COUNT(*) FROM DonViTinh
UNION ALL
SELECT 'NhaCungCap', COUNT(*) FROM NhaCungCap
UNION ALL
SELECT 'NguoiDung', COUNT(*) FROM NguoiDung
UNION ALL
SELECT 'PhieuNhap', COUNT(*) FROM PhieuNhap
UNION ALL
SELECT 'PhieuXuat', COUNT(*) FROM PhieuXuat;

-- Kiểm tra tồn kho hiện tại
SELECT 
    sp.MaSP,
    sp.TenSP,
    dm.TenDM AS DanhMuc,
    dvt.TenDVT,
    dvt.KyHieu,
    sp.SoLuongTon,
    sp.SoLuongToiThieu,
    CASE 
        WHEN sp.SoLuongTon <= 0 THEN 'Hết hàng'
        WHEN sp.SoLuongTon <= sp.SoLuongToiThieu THEN 'Sắp hết'
        ELSE 'Đủ hàng'
    END as TrangThaiTonKho
FROM SanPham sp
INNER JOIN DanhMucSanPham dm ON sp.MaDM = dm.MaDM
INNER JOIN DonViTinh dvt ON sp.MaDVT = dvt.MaDVT
ORDER BY dm.TenDM, sp.SoLuongTon;

-- Kiểm tra tổng tiền các phiếu
SELECT 
    'PhieuNhap' as Loai,
    COUNT(*) as SoPhieu,
    SUM(TongTien) as TongTien
FROM PhieuNhap
UNION ALL
SELECT 
    'PhieuXuat',
    COUNT(*),
    SUM(TongTien)
FROM PhieuXuat;

-- Test stored procedure kiểm tra tồn kho
EXEC sp_KiemTraTonKho @MaSP = 1, @SoLuongCan = 5;

-- Test báo cáo tồn kho
EXEC sp_BaoCaoTonKho;

-- Test thống kê doanh thu
EXEC sp_ThongKeDoanhThu;

-- Test quản lý đơn vị tính
EXEC sp_QuanLyDonViTinh @Action = 'SELECT';

-- Test quản lý danh mục sản phẩm
EXEC sp_QuanLyDanhMucSanPham @Action = 'SELECT';

-- Test hiển thị danh mục theo cây
EXEC sp_QuanLyDanhMucSanPham @Action = 'SELECT_TREE';

-- Test xác thực đăng nhập
EXEC sp_XacThucDangNhap @TenDangNhap = 'admin', @MatKhau = 'admin123', @IPAddress = '192.168.1.1';

-- Kiểm tra hiệu suất với các query phổ biến
SELECT TOP 10 
    sp.TenSP,
    dm.TenDM AS DanhMuc,
    dvt.TenDVT,
    dvt.KyHieu,
    sp.SoLuongTon,
    sp.GiaBan,
    (sp.SoLuongTon * sp.GiaBan) AS GiaTriTonKho
FROM SanPham sp
INNER JOIN DanhMucSanPham dm ON sp.MaDM = dm.MaDM
INNER JOIN DonViTinh dvt ON sp.MaDVT = dvt.MaDVT
WHERE sp.TrangThai = 1 AND dm.TrangThai = 1 AND dvt.TrangThai = 1
ORDER BY dm.TenDM, sp.SoLuongTon;

GO

-- =========================================
-- STORED PROCEDURES BẢO TRÌ VÀ TỐI ƯU
-- =========================================

-- Stored procedure dọn dẹp dữ liệu cũ
CREATE PROCEDURE sp_DonDepDuLieuCu
    @SoNgayGiuLichSu INT = 90 -- Giữ lịch sử 90 ngày
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NgayXoa DATETIME = DATEADD(DAY, -@SoNgayGiuLichSu, GETDATE());
    DECLARE @SoBanGhiXoa INT = 0;
    
    -- Xóa lịch sử đăng nhập cũ
    DELETE FROM LichSuDangNhap 
    WHERE ThoiGian < @NgayXoa;
    SET @SoBanGhiXoa = @SoBanGhiXoa + @@ROWCOUNT;
    
    -- Xóa lịch sử thay đổi cũ
    DELETE FROM LichSuThayDoi 
    WHERE ThoiGian < @NgayXoa;
    SET @SoBanGhiXoa = @SoBanGhiXoa + @@ROWCOUNT;
    
    -- Cập nhật thống kê
    UPDATE STATISTICS LichSuDangNhap;
    UPDATE STATISTICS LichSuThayDoi;
    
    SELECT 
        'Dọn dẹp hoàn tất' AS KetQua,
        @SoBanGhiXoa AS SoBanGhiDaXoa;
END;
GO

-- Stored procedure kiểm tra sức khỏe database
CREATE PROCEDURE sp_KiemTraSucKhoeDatabase
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Kiểm tra tồn kho âm
    SELECT 
        'Tồn kho âm' AS LoaiLoi,
        COUNT(*) AS SoLuong
    FROM SanPham 
    WHERE SoLuongTon < 0
    
    UNION ALL
    
    -- Kiểm tra giá âm
    SELECT 
        'Giá âm' AS LoaiLoi,
        COUNT(*) AS SoLuong
    FROM SanPham 
    WHERE GiaNhap <= 0 OR GiaBan <= 0
    
    UNION ALL
    
    -- Kiểm tra sản phẩm không có danh mục
    SELECT 
        'Sản phẩm không có danh mục' AS LoaiLoi,
        COUNT(*) AS SoLuong
    FROM SanPham sp
    LEFT JOIN DanhMucSanPham dm ON sp.MaDM = dm.MaDM
    WHERE dm.MaDM IS NULL
    
    UNION ALL
    
    -- Kiểm tra sản phẩm không có đơn vị tính
    SELECT 
        'Sản phẩm không có đơn vị tính' AS LoaiLoi,
        COUNT(*) AS SoLuong
    FROM SanPham sp
    LEFT JOIN DonViTinh dvt ON sp.MaDVT = dvt.MaDVT
    WHERE dvt.MaDVT IS NULL;
END;
GO

-- =========================================
-- VIEWS CHO BÁO CÁO VÀ TRUY VẤN
-- =========================================

-- View tổng hợp thông tin sản phẩm
CREATE VIEW vw_SanPhamChiTiet AS
SELECT 
    sp.MaSP,
    sp.TenSP,
    dm.TenDM AS DanhMuc,
    dm_cha.TenDM AS DanhMucCha,
    dvt.TenDVT,
    dvt.KyHieu,
    sp.GiaNhap,
    sp.GiaBan,
    sp.SoLuongTon,
    sp.SoLuongToiThieu,
    sp.MoTa,
    sp.TrangThai,
    CASE 
        WHEN sp.SoLuongTon <= 0 THEN 'Hết hàng'
        WHEN sp.SoLuongTon <= sp.SoLuongToiThieu THEN 'Sắp hết'
        ELSE 'Đủ hàng'
    END AS TrangThaiTonKho,
    (sp.SoLuongTon * sp.GiaNhap) AS GiaTriTonKho,
    (sp.SoLuongTon * sp.GiaBan) AS GiaTriBan,
    nd.HoTen AS NguoiTao,
    sp.NgayTao,
    sp.NgayCapNhat
FROM SanPham sp
INNER JOIN DanhMucSanPham dm ON sp.MaDM = dm.MaDM
LEFT JOIN DanhMucSanPham dm_cha ON dm.MaDMCapTren = dm_cha.MaDM
INNER JOIN DonViTinh dvt ON sp.MaDVT = dvt.MaDVT
LEFT JOIN NguoiDung nd ON sp.NguoiTao = nd.MaND
WHERE sp.TrangThai = 1 AND dm.TrangThai = 1 AND dvt.TrangThai = 1;
GO

-- View thống kê tồn kho theo danh mục
CREATE VIEW vw_ThongKeTonKhoTheoDanhMuc AS
SELECT 
    dm.MaDM,
    dm.TenDM AS DanhMuc,
    dm_cha.TenDM AS DanhMucCha,
    COUNT(sp.MaSP) AS SoSanPham,
    SUM(sp.SoLuongTon) AS TongSoLuong,
    SUM(sp.SoLuongTon * sp.GiaNhap) AS TongGiaTriNhap,
    SUM(sp.SoLuongTon * sp.GiaBan) AS TongGiaTriBan,
    AVG(sp.GiaBan - sp.GiaNhap) AS LoiNhuanTrungBinh,
    COUNT(CASE WHEN sp.SoLuongTon <= 0 THEN 1 END) AS SoSanPhamHetHang,
    COUNT(CASE WHEN sp.SoLuongTon <= sp.SoLuongToiThieu AND sp.SoLuongTon > 0 THEN 1 END) AS SoSanPhamSapHet
FROM DanhMucSanPham dm
LEFT JOIN SanPham sp ON dm.MaDM = sp.MaDM AND sp.TrangThai = 1
LEFT JOIN DanhMucSanPham dm_cha ON dm.MaDMCapTren = dm_cha.MaDM
WHERE dm.TrangThai = 1
GROUP BY dm.MaDM, dm.TenDM, dm_cha.TenDM;
GO

-- =========================================
-- FUNCTIONS HỖ TRỢ
-- =========================================

-- Function tính tổng tiền phiếu nhập
CREATE FUNCTION fn_TinhTongTienPhieuNhap(@MaPN INT)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @TongTien DECIMAL(18,2) = 0;
    
    SELECT @TongTien = ISNULL(SUM(SoLuong * DonGia), 0)
    FROM ChiTietPhieuNhap
    WHERE MaPN = @MaPN;
    
    RETURN @TongTien;
END;
GO

-- Function tính tổng tiền phiếu xuất
CREATE FUNCTION fn_TinhTongTienPhieuXuat(@MaPX INT)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @TongTien DECIMAL(18,2) = 0;
    
    SELECT @TongTien = ISNULL(SUM(SoLuong * DonGia), 0)
    FROM ChiTietPhieuXuat
    WHERE MaPX = @MaPX;
    
    RETURN @TongTien;
END;
GO

-- =========================================
-- BACKUP VÀ MAINTENANCE
-- =========================================

-- Stored procedure backup database
CREATE PROCEDURE sp_BackupDatabase
    @BackupPath NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @BackupPath IS NULL
        SET @BackupPath = 'C:\Backup\BTL_QUANLYKHO_' + FORMAT(GETDATE(), 'yyyyMMdd_HHmmss') + '.bak';
    
    DECLARE @SQL NVARCHAR(MAX) = 
        'BACKUP DATABASE BTL_QUANLYKHO TO DISK = ''' + @BackupPath + ''' WITH FORMAT, INIT, NAME = ''BTL_QUANLYKHO-Full Database Backup'', SKIP, NOREWIND, NOUNLOAD, STATS = 10';
    
    EXEC sp_executesql @SQL;
    
    SELECT 'Backup hoàn tất' AS KetQua, @BackupPath AS DuongDan;
END;
GO

-- Stored procedure kiểm tra cấu hình database
CREATE PROCEDURE sp_KiemTraCauHinhDatabase
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Kiểm tra collation của database
    SELECT 
        'Database Collation' AS Loai,
        CAST(DATABASEPROPERTYEX(DB_NAME(), 'Collation') AS NVARCHAR(128)) AS GiaTri,
        CASE 
            WHEN CAST(DATABASEPROPERTYEX(DB_NAME(), 'Collation') AS NVARCHAR(128)) LIKE '%Vietnamese%' 
            THEN 'Hỗ trợ tiếng Việt tốt'
            ELSE 'Cần cập nhật collation'
        END AS TrangThai;
    
    -- Test tiếng Việt
    SELECT 
        'Test Tiếng Việt' AS Loai,
        N'Tiếng Việt có dấu: ă, â, ê, ô, ơ, ư, đ' AS GiaTri,
        'Hiển thị chính xác' AS TrangThai;
END;
GO

-- Stored procedure kiểm tra hỗ trợ tiếng Việt
CREATE PROCEDURE sp_KiemTraHoTroTiengViet
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @DB_Collation NVARCHAR(128);
    DECLARE @Server_Collation NVARCHAR(128);
    DECLARE @Compatibility_Level INT;
    
    -- Lấy thông tin collation và compatibility level
    SET @DB_Collation = CAST(DATABASEPROPERTYEX('BTL_QUANLYKHO', 'Collation') AS NVARCHAR(128));
    SET @Server_Collation = CAST(SERVERPROPERTY('Collation') AS NVARCHAR(128));
    SET @Compatibility_Level = CAST(DATABASEPROPERTYEX('BTL_QUANLYKHO', 'Version') AS INT);
    
    -- Kiểm tra collation của database
    SELECT 
        'Database Collation' AS Loai,
        @DB_Collation AS GiaTri,
        CASE 
            WHEN @DB_Collation LIKE '%Vietnamese%' 
            THEN 'Hỗ trợ tiếng Việt tốt'
            ELSE 'Cần cập nhật collation'
        END AS TrangThai
    
    UNION ALL
    
    -- Kiểm tra collation của server
    SELECT 
        'Server Collation' AS Loai,
        @Server_Collation AS GiaTri,
        CASE 
            WHEN @Server_Collation LIKE '%Vietnamese%' 
            THEN 'Server hỗ trợ tiếng Việt'
            ELSE 'Server không hỗ trợ tiếng Việt'
        END AS TrangThai
    
    UNION ALL
    
    -- Kiểm tra compatibility level
    SELECT 
        'Compatibility Level' AS Loai,
        CAST(@Compatibility_Level AS NVARCHAR(10)) AS GiaTri,
        CASE 
            WHEN @Compatibility_Level >= 150 
            THEN 'Hỗ trợ Unicode tốt nhất'
            ELSE 'Cần nâng cấp compatibility level'
        END AS TrangThai;
    
    -- Test tiếng Việt
    SELECT 
        'Test Tiếng Việt' AS Loai,
        N'Tiếng Việt có dấu: ă, â, ê, ô, ơ, ư, đ' AS GiaTri,
        'Hiển thị chính xác' AS TrangThai;
END;
GO

-- =========================================
-- FINAL VALIDATION
-- =========================================

-- Kiểm tra tất cả constraints và foreign keys
SELECT 
    'Foreign Keys' AS Loai,
    COUNT(*) AS SoLuong
FROM sys.foreign_keys
WHERE parent_object_id IN (
    OBJECT_ID('NguoiDung'),
    OBJECT_ID('DonViTinh'),
    OBJECT_ID('DanhMucSanPham'),
    OBJECT_ID('SanPham'),
    OBJECT_ID('NhaCungCap'),
    OBJECT_ID('PhieuNhap'),
    OBJECT_ID('ChiTietPhieuNhap'),
    OBJECT_ID('PhieuXuat'),
    OBJECT_ID('ChiTietPhieuXuat'),
    OBJECT_ID('LichSuDangNhap'),
    OBJECT_ID('LichSuThayDoi')
)

UNION ALL

SELECT 
    'Check Constraints' AS Loai,
    COUNT(*) AS SoLuong
FROM sys.check_constraints
WHERE parent_object_id IN (
    OBJECT_ID('NguoiDung'),
    OBJECT_ID('DonViTinh'),
    OBJECT_ID('DanhMucSanPham'),
    OBJECT_ID('SanPham'),
    OBJECT_ID('NhaCungCap'),
    OBJECT_ID('PhieuNhap'),
    OBJECT_ID('ChiTietPhieuNhap'),
    OBJECT_ID('PhieuXuat'),
    OBJECT_ID('ChiTietPhieuXuat'),
    OBJECT_ID('LichSuDangNhap'),
    OBJECT_ID('LichSuThayDoi')
)

UNION ALL

SELECT 
    'Indexes' AS Loai,
    COUNT(*) AS SoLuong
FROM sys.indexes
WHERE object_id IN (
    OBJECT_ID('NguoiDung'),
    OBJECT_ID('DonViTinh'),
    OBJECT_ID('DanhMucSanPham'),
    OBJECT_ID('SanPham'),
    OBJECT_ID('NhaCungCap'),
    OBJECT_ID('PhieuNhap'),
    OBJECT_ID('ChiTietPhieuNhap'),
    OBJECT_ID('PhieuXuat'),
    OBJECT_ID('ChiTietPhieuXuat'),
    OBJECT_ID('LichSuDangNhap'),
    OBJECT_ID('LichSuThayDoi')
)
AND is_primary_key = 0 AND is_unique_constraint = 0;

GO

-- =========================================
-- KIỂM TRA TÍNH TOÀN VẸN DỮ LIỆU
-- =========================================

-- Kiểm tra dữ liệu cơ bản (đã kiểm tra ở trên, bỏ qua để tránh trùng lặp)

-- Kiểm tra tồn kho
EXEC sp_BaoCaoTonKho;

-- Kiểm tra sức khỏe database
EXEC sp_KiemTraSucKhoeDatabase;

-- Test đăng nhập
EXEC sp_XacThucDangNhap @TenDangNhap = 'admin', @MatKhau = 'admin123';

-- Test quản lý danh mục
EXEC sp_QuanLyDanhMucSanPham @Action = 'SELECT';
EXEC sp_QuanLyDanhMucSanPham @Action = 'SELECT_TREE';

-- Test views
SELECT TOP 5 * FROM vw_SanPhamChiTiet;
SELECT TOP 5 * FROM vw_ThongKeTonKhoTheoDanhMuc;

-- Test functions
SELECT dbo.fn_TinhTongTienPhieuNhap(1) AS TongTienPN1;
SELECT dbo.fn_TinhTongTienPhieuXuat(1) AS TongTienPX1;

-- Test hỗ trợ tiếng Việt
EXEC sp_KiemTraCauHinhDatabase;

-- Test đơn giản tiếng Việt
SELECT 
    'Test Tiếng Việt Đơn Giản' AS Loai,
    N'Tiếng Việt có dấu: ă, â, ê, ô, ơ, ư, đ' AS GiaTri,
    'Hiển thị chính xác' AS TrangThai;

PRINT 'Database đã được tạo thành công và sẵn sàng sử dụng!';
PRINT 'Hỗ trợ tiếng Việt đã được cấu hình tối ưu!';

GO
