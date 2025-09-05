# QLKHO.BLL - Business Logic Layer

## Tổng quan
Dự án QLKHO.BLL cung cấp lớp business logic cho hệ thống quản lý kho, chứa các service và validation logic.

## Cấu trúc thư mục

```
QLKHO.BLL/
├── Services/               # Business logic services
│   ├── DonViTinhService.cs
│   └── SanPhamService.cs
├── Demo/                   # Demo và test business logic
│   └── BusinessDemo.cs
├── Program.cs              # Entry point cho BLL
└── README.md               # Tài liệu này
```

## Dependencies
- **QLKHO.DAL**: Data Access Layer (Models, DbContext, Repositories)

## Services

### DonViTinhService
Quản lý business logic cho đơn vị tính:

```csharp
using (var service = new DonViTinhService())
{
    // Lấy tất cả đơn vị tính
    var donViTinhs = service.GetAllActive();
    
    // Tìm kiếm
    var results = service.Search("cái");
    
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
    
    // Xóa (soft delete)
    service.Delete(newDVT.MaDVT);
}
```

**Các method chính:**
- `GetAllActive()`: Lấy tất cả đơn vị tính đang hoạt động
- `GetById(int id)`: Lấy đơn vị tính theo ID
- `GetByTenDVT(string tenDVT)`: Tìm theo tên
- `GetByKyHieu(string kyHieu)`: Tìm theo ký hiệu
- `Add(DonViTinh donViTinh)`: Thêm mới với validation
- `Update(DonViTinh donViTinh)`: Cập nhật với validation
- `Delete(int id)`: Xóa (soft delete) với kiểm tra ràng buộc
- `Search(string keyword)`: Tìm kiếm
- `IsInUse(int id)`: Kiểm tra đang được sử dụng
- `GetActiveCount()`: Đếm số lượng

### SanPhamService
Quản lý business logic cho sản phẩm:

```csharp
using (var service = new SanPhamService())
{
    // Lấy tất cả sản phẩm
    var sanPhams = service.GetAllActive();
    
    // Lấy sản phẩm theo danh mục
    var spByCategory = service.GetByDanhMuc(1);
    
    // Lấy sản phẩm sắp hết hàng
    var sapHetHang = service.GetSanPhamSapHetHang();
    
    // Lấy sản phẩm hết hàng
    var hetHang = service.GetSanPhamHetHang();
    
    // Cập nhật tồn kho
    service.UpdateTonKho(1, 10, true); // Nhập 10 sản phẩm
    service.UpdateTonKho(1, 5, false); // Xuất 5 sản phẩm
}
```

**Các method chính:**
- `GetAllActive()`: Lấy tất cả sản phẩm đang hoạt động
- `GetById(int id)`: Lấy sản phẩm theo ID
- `GetByDanhMuc(int maDM)`: Lấy theo danh mục
- `Search(string keyword)`: Tìm kiếm
- `GetSanPhamSapHetHang()`: Lấy sản phẩm sắp hết hàng
- `GetSanPhamHetHang()`: Lấy sản phẩm hết hàng
- `Add(SanPham sanPham)`: Thêm mới với validation
- `Update(SanPham sanPham)`: Cập nhật với validation
- `Delete(int id)`: Xóa (soft delete) với kiểm tra ràng buộc
- `UpdateTonKho(int maSP, int soLuong, bool isNhap)`: Cập nhật tồn kho
- `GetActiveCount()`: Đếm số lượng

## Business Rules & Validation

### DonViTinh
- Tên đơn vị tính phải unique
- Ký hiệu phải unique
- Không thể xóa đơn vị tính đang được sử dụng bởi sản phẩm
- Soft delete thông qua trường `TrangThai`

### SanPham
- Giá nhập phải > 0
- Giá bán phải > 0
- Giá bán không được nhỏ hơn giá nhập
- Không thể xóa sản phẩm đang được sử dụng trong phiếu nhập/xuất
- Cập nhật tồn kho với kiểm tra đủ hàng khi xuất
- Soft delete thông qua trường `TrangThai`

## Demo
Chạy demo để test các chức năng business logic:

```csharp
BusinessDemo.RunDemo();
```

Demo sẽ test:
1. Kết nối database
2. DonViTinhService (CRUD, Search, Validation)
3. SanPhamService (CRUD, Tồn kho, Business rules)

## Cách sử dụng

### 1. Chạy BLL Demo
```bash
# Build và chạy QLKHO.BLL project
dotnet run
```

### 2. Sử dụng trong GUI
```csharp
// Trong form hoặc controller
using (var donViTinhService = new DonViTinhService())
{
    var donViTinhs = donViTinhService.GetAllActive();
    // Bind vào DataGridView hoặc ComboBox
}
```

### 3. Error Handling
Tất cả services đều throw exception với message rõ ràng:

```csharp
try
{
    service.Add(donViTinh);
}
catch (Exception ex)
{
    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```

## Lưu ý
- Tất cả services đều implement `IDisposable`
- Sử dụng `using` statement để đảm bảo dispose resources
- Business logic được tách biệt khỏi data access
- Validation được thực hiện ở business layer
- Soft delete được áp dụng cho tất cả entities
