# HƯỚNG DẪN SỬ DỤNG CHỨC NĂNG QUẢN LÝ NGƯỜI DÙNG

## Tổng quan
Chức năng quản lý người dùng cho phép admin và quản lý thêm, sửa, xóa, khóa/mở khóa tài khoản người dùng trong hệ thống.

## Cách truy cập
1. Đăng nhập vào hệ thống với tài khoản Admin hoặc Quản lý
2. Vào menu **Hệ thống** → **Quản lý người dùng**

## Giao diện chính

### Phần bên trái - Danh sách người dùng
- **Tìm kiếm**: Nhập tên, email, vai trò để lọc danh sách
- **Làm mới**: Tải lại toàn bộ danh sách
- **Bảng danh sách**: Hiển thị thông tin cơ bản của người dùng

### Phần bên phải - Thông tin chi tiết
- **Mã ND**: Mã người dùng (tự động)
- **Tên đăng nhập**: Tên đăng nhập (bắt buộc, tối thiểu 3 ký tự)
- **Mật khẩu**: Mật khẩu (bắt buộc, tối thiểu 6 ký tự)
- **Họ tên**: Tên đầy đủ (bắt buộc)
- **Email**: Email (tùy chọn, phải đúng định dạng)
- **Điện thoại**: Số điện thoại (tùy chọn)
- **Vai trò**: Admin, Quản lý, Nhân viên
- **Trạng thái**: Hoạt động/Không hoạt động

## Các chức năng chính

### 1. Thêm người dùng mới
1. Nhấn nút **"Thêm mới"**
2. Nhập thông tin bắt buộc:
   - Tên đăng nhập (không được trùng)
   - Mật khẩu
   - Họ tên
   - Vai trò
3. Nhập thông tin tùy chọn (email, điện thoại)
4. Nhấn **"Lưu"** để thêm hoặc **"Hủy"** để bỏ qua

### 2. Sửa thông tin người dùng
1. Chọn người dùng cần sửa từ danh sách
2. Nhấn nút **"Sửa"**
3. Chỉnh sửa thông tin cần thiết
4. Nhấn **"Lưu"** để cập nhật hoặc **"Hủy"** để bỏ qua

### 3. Xóa người dùng
1. Chọn người dùng cần xóa từ danh sách
2. Nhấn nút **"Xóa"**
3. Xác nhận việc xóa
4. **Lưu ý**: Không thể xóa admin cuối cùng

### 4. Đặt lại mật khẩu
1. Chọn người dùng cần đặt lại mật khẩu
2. Nhấn nút **"Đặt lại mật khẩu"**
3. Nhập mật khẩu mới (tối thiểu 6 ký tự)
4. Xác nhận

### 5. Khóa/Mở khóa tài khoản
1. Chọn người dùng cần khóa/mở khóa
2. Nhấn nút **"Khóa tài khoản"** hoặc **"Mở khóa"**
3. Xác nhận thao tác

## Quy tắc và hạn chế

### Vai trò và quyền hạn
- **Admin**: Có thể quản lý tất cả người dùng
- **Quản lý**: Có thể quản lý nhân viên
- **Nhân viên**: Chỉ có thể xem thông tin của mình

### Ràng buộc dữ liệu
- Tên đăng nhập: Không được trùng, tối thiểu 3 ký tự
- Mật khẩu: Tối thiểu 6 ký tự
- Email: Phải đúng định dạng nếu có
- Vai trò: Chỉ được chọn Admin, Quản lý, Nhân viên

### Bảo mật
- Không thể xóa admin cuối cùng
- Tài khoản bị khóa sẽ không thể đăng nhập
- Mật khẩu được mã hóa trong database

## Xử lý lỗi thường gặp

### Lỗi "Tên đăng nhập đã tồn tại"
- Kiểm tra lại tên đăng nhập
- Thử tên đăng nhập khác

### Lỗi "Email không đúng định dạng"
- Kiểm tra định dạng email (ví dụ: user@domain.com)
- Để trống nếu không cần thiết

### Lỗi "Không thể xóa admin cuối cùng"
- Hệ thống yêu cầu ít nhất 1 admin
- Tạo admin mới trước khi xóa admin hiện tại

### Lỗi kết nối database
- Kiểm tra kết nối mạng
- Liên hệ quản trị viên hệ thống

## Mẹo sử dụng

### Tìm kiếm hiệu quả
- Sử dụng từ khóa ngắn gọn
- Tìm theo họ tên hoặc email
- Sử dụng nút "Làm mới" để reset bộ lọc

### Quản lý tài khoản
- Thường xuyên kiểm tra tài khoản không hoạt động
- Đặt mật khẩu mạnh cho tài khoản quan trọng
- Khóa tài khoản của nhân viên nghỉ việc

### Bảo mật
- Không chia sẻ thông tin đăng nhập
- Thay đổi mật khẩu định kỳ
- Báo cáo ngay khi phát hiện tài khoản bị xâm nhập

## Hỗ trợ
Nếu gặp vấn đề trong quá trình sử dụng, vui lòng liên hệ:
- Email: support@qlkho.com
- Điện thoại: 0123-456-789
- Thời gian hỗ trợ: 8:00 - 17:00 (Thứ 2 - Thứ 6)
