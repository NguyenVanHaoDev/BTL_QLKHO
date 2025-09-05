# QLKHO.DAL - Data Access Layer

## Tổng quan
Dự án QLKHO.DAL cung cấp lớp truy cập dữ liệu cho hệ thống quản lý kho sử dụng Entity Framework 6.5.1.

## Cấu trúc thư mục

```
QLKHO.DAL/
├── Models/                 # Các model entity
│   ├── DonViTinh.cs
│   ├── NguoiDung.cs
│   ├── DanhMucSanPham.cs
│   ├── SanPham.cs
│   ├── NhaCungCap.cs
│   ├── PhieuNhap.cs
│   ├── ChiTietPhieuNhap.cs
│   ├── PhieuXuat.cs
│   ├── ChiTietPhieuXuat.cs
│   ├── LichSuDangNhap.cs
│   └── LichSuThayDoi.cs
├── Configurations/         # Cấu hình Entity Framework
│   ├── NguoiDungConfiguration.cs
│   ├── DonViTinhConfiguration.cs
│   ├── DanhMucSanPhamConfiguration.cs
│   ├── SanPhamConfiguration.cs
│   ├── NhaCungCapConfiguration.cs
│   ├── PhieuNhapConfiguration.cs
│   ├── ChiTietPhieuNhapConfiguration.cs
│   ├── PhieuXuatConfiguration.cs
│   ├── ChiTietPhieuXuatConfiguration.cs
│   ├── LichSuDangNhapConfiguration.cs
│   └── LichSuThayDoiConfiguration.cs
├── Repositories/           # Repository pattern
│   ├── IRepository.cs
│   ├── Repository.cs
│   └── UnitOfWork.cs
├── Services/               # Business logic services
│   └── DonViTinhService.cs
├── Demo/                   # Demo và test
│   └── ModelDemo.cs
├── QuanLyKhoDbContext.cs   # DbContext chính
├── DatabaseHelper.cs       # Helper cho database
└── App.config              # Cấu hình connection string
```

## Cài đặt và cấu hình

### 1. Connection String
Cập nhật connection string trong `App.config`:

```xml
<connectionStrings>
  <add name="QuanLyKhoConnectionString" 
       connectionString="Data Source=.;Initial Catalog=BTL_QUANLYKHO;Integrated Security=True;MultipleActiveResultSets=True;App=EntityFramework" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 2. Khởi tạo Database
```csharp
// Khởi tạo database
DatabaseHelper.InitializeDatabase();

// Kiểm tra kết nối
bool isConnected = DatabaseHelper.TestConnection();
```

## Sử dụng

### 1. Sử dụng DbContext trực tiếp
```csharp
using (var context = new QuanLyKhoDbContext())
{
    var donViTinhs = context.DonViTinhs.Where(d => d.TrangThai).ToList();
    
    var newDVT = new DonViTinh
    {
        TenDVT = "Cái",
        KyHieu = "cái",
        MoTa = "Đơn vị tính cho các sản phẩm đếm được"
    };
    
    context.DonViTinhs.Add(newDVT);
    context.SaveChanges();
}
```

### 2. Sử dụng Repository Pattern
```csharp
using (var unitOfWork = new UnitOfWork())
{
    var donViTinhs = unitOfWork.DonViTinhRepository.GetAll();
    
    var newDVT = new DonViTinh
    {
        TenDVT = "Bộ",
        KyHieu = "bộ",
        MoTa = "Đơn vị tính cho bộ sản phẩm"
    };
    
    unitOfWork.DonViTinhRepository.Add(newDVT);
    unitOfWork.SaveChanges();
}
```

### 3. Sử dụng Service Layer
```csharp
using (var service = new DonViTinhService())
{
    // Lấy tất cả đơn vị tính
    var allDVT = service.GetAllActive();
    
    // Tìm kiếm
    var searchResults = service.Search("cái");
    
    // Thêm mới
    var newDVT = new DonViTinh
    {
        TenDVT = "Thùng",
        KyHieu = "thùng",
        MoTa = "Đơn vị tính cho sản phẩm đóng thùng"
    };
    service.Add(newDVT);
    
    // Cập nhật
    newDVT.MoTa = "Cập nhật mô tả";
    service.Update(newDVT);
    
    // Xóa
    service.Delete(newDVT.MaDVT);
}
```

## Các Model chính

### DonViTinh (Đơn vị tính)
- `MaDVT`: ID đơn vị tính (Primary Key, Identity)
- `TenDVT`: Tên đơn vị tính (Required, MaxLength: 50, Unique)
- `KyHieu`: Ký hiệu (Required, MaxLength: 10, Unique)
- `MoTa`: Mô tả (MaxLength: 200)
- `TrangThai`: Trạng thái hoạt động (Default: true)
- `NgayTao`: Ngày tạo (Default: DateTime.Now)
- `NgayCapNhat`: Ngày cập nhật (Default: DateTime.Now)

### NguoiDung (Người dùng)
- `MaND`: ID người dùng (Primary Key, Identity)
- `TenDangNhap`: Tên đăng nhập (Required, MaxLength: 50, Unique)
- `MatKhau`: Mật khẩu (Required, MaxLength: 255)
- `HoTen`: Họ tên (Required, MaxLength: 100)
- `Email`: Email (MaxLength: 100)
- `DienThoai`: Điện thoại (MaxLength: 15)
- `VaiTro`: Vai trò (Required, MaxLength: 20, Default: "NhanVien")
- `TrangThai`: Trạng thái hoạt động (Default: true)
- `SoLanDangNhapSai`: Số lần đăng nhập sai (Default: 0)
- `KhoaTaiKhoan`: Khóa tài khoản (Default: false)
- `ThoiGianKhoa`: Thời gian khóa
- `NgayTao`: Ngày tạo (Default: DateTime.Now)
- `NgayCapNhat`: Ngày cập nhật (Default: DateTime.Now)
- `NgayDangNhapCuoi`: Ngày đăng nhập cuối

### SanPham (Sản phẩm)
- `MaSP`: ID sản phẩm (Primary Key, Identity)
- `TenSP`: Tên sản phẩm (Required, MaxLength: 100)
- `MaDM`: ID danh mục (Required, Foreign Key)
- `MaDVT`: ID đơn vị tính (Required, Foreign Key)
- `GiaNhap`: Giá nhập (Required, Decimal(18,2))
- `GiaBan`: Giá bán (Required, Decimal(18,2))
- `SoLuongTon`: Số lượng tồn (Default: 0)
- `SoLuongToiThieu`: Số lượng tối thiểu (Default: 10)
- `MoTa`: Mô tả (MaxLength: 500)
- `TrangThai`: Trạng thái hoạt động (Default: true)
- `NgayTao`: Ngày tạo (Default: DateTime.Now)
- `NgayCapNhat`: Ngày cập nhật (Default: DateTime.Now)
- `NguoiTao`: ID người tạo (Foreign Key)

## Relationships

### One-to-Many
- `DonViTinh` → `SanPham` (1 đơn vị tính có nhiều sản phẩm)
- `DanhMucSanPham` → `SanPham` (1 danh mục có nhiều sản phẩm)
- `NguoiDung` → `SanPham` (1 người dùng tạo nhiều sản phẩm)
- `NhaCungCap` → `PhieuNhap` (1 nhà cung cấp có nhiều phiếu nhập)
- `PhieuNhap` → `ChiTietPhieuNhap` (1 phiếu nhập có nhiều chi tiết)
- `PhieuXuat` → `ChiTietPhieuXuat` (1 phiếu xuất có nhiều chi tiết)

### Self-Reference
- `DanhMucSanPham` → `DanhMucSanPham` (danh mục cha - con)

## Demo
Chạy demo để test các chức năng:

```csharp
ModelDemo.RunDemo();
```

## Lưu ý
- Tất cả các model đều hỗ trợ soft delete thông qua trường `TrangThai`
- Sử dụng `using` statement để đảm bảo dispose resources
- Các service đều implement IDisposable
- Database được cấu hình với collation Vietnamese_CI_AS để hỗ trợ tiếng Việt tốt nhất
