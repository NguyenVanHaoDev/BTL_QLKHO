using System;
using System.Linq;
using QLKHO.DAL;
using QLKHO.DAL.Models;
using QLKHO.DAL.Repositories;

namespace QLKHO.BLL.Services
{
    public class AuthenticationService : IDisposable
    {
        private readonly UnitOfWork _unitOfWork;
        private bool _disposed = false;

        public AuthenticationService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Xác thực đăng nhập người dùng
        /// </summary>
        /// <param name="tenDangNhap">Tên đăng nhập</param>
        /// <param name="matKhau">Mật khẩu</param>
        /// <param name="ipAddress">Địa chỉ IP</param>
        /// <param name="userAgent">User Agent</param>
        /// <returns>Kết quả đăng nhập</returns>
        public LoginResult DangNhap(string tenDangNhap, string matKhau, string ipAddress = null, string userAgent = null)
        {
            try
            {
                // Tìm người dùng theo tên đăng nhập
                var nguoiDung = _unitOfWork.NguoiDungRepository.GetAll()
                    .FirstOrDefault(nd => nd.TenDangNhap == tenDangNhap && nd.TrangThai);

                if (nguoiDung == null)
                {
                    // Ghi log đăng nhập thất bại
                    GhiLogDangNhap(null, tenDangNhap, ipAddress, userAgent, "ThatBai", "Tài khoản không tồn tại");
                    return new LoginResult
                    {
                        ThanhCong = false,
                        ThongBao = "Tài khoản không tồn tại hoặc đã bị vô hiệu hóa"
                    };
                }

                // Kiểm tra tài khoản có bị khóa không
                if (nguoiDung.KhoaTaiKhoan)
                {
                    // Kiểm tra thời gian khóa (khóa 30 phút)
                    if (nguoiDung.ThoiGianKhoa.HasValue && 
                        DateTime.Now.Subtract(nguoiDung.ThoiGianKhoa.Value).TotalMinutes < 30)
                    {
                        GhiLogDangNhap(nguoiDung.MaND, tenDangNhap, ipAddress, userAgent, "KhoaTaiKhoan", "Tài khoản đang bị khóa");
                        return new LoginResult
                        {
                            ThanhCong = false,
                            ThongBao = "Tài khoản đang bị khóa. Vui lòng thử lại sau 30 phút"
                        };
                    }
                    else
                    {
                        // Mở khóa tài khoản
                        nguoiDung.KhoaTaiKhoan = false;
                        nguoiDung.SoLanDangNhapSai = 0;
                        nguoiDung.ThoiGianKhoa = null;
                        nguoiDung.NgayCapNhat = DateTime.Now;
                        _unitOfWork.NguoiDungRepository.Update(nguoiDung);
                    }
                }

                // Kiểm tra mật khẩu (trong thực tế nên hash mật khẩu)
                if (nguoiDung.MatKhau == matKhau)
                {
                    // Đăng nhập thành công
                    nguoiDung.SoLanDangNhapSai = 0;
                    nguoiDung.NgayDangNhapCuoi = DateTime.Now;
                    nguoiDung.NgayCapNhat = DateTime.Now;
                    _unitOfWork.NguoiDungRepository.Update(nguoiDung);

                    // Ghi log đăng nhập thành công
                    GhiLogDangNhap(nguoiDung.MaND, tenDangNhap, ipAddress, userAgent, "ThanhCong", "Đăng nhập thành công");

                    _unitOfWork.SaveChanges();

                    return new LoginResult
                    {
                        ThanhCong = true,
                        ThongBao = "Đăng nhập thành công",
                        NguoiDung = nguoiDung
                    };
                }
                else
                {
                    // Đăng nhập thất bại
                    nguoiDung.SoLanDangNhapSai += 1;
                    nguoiDung.KhoaTaiKhoan = nguoiDung.SoLanDangNhapSai >= 5;
                    nguoiDung.ThoiGianKhoa = nguoiDung.KhoaTaiKhoan ? (DateTime?)DateTime.Now : null;
                    nguoiDung.NgayCapNhat = DateTime.Now;
                    _unitOfWork.NguoiDungRepository.Update(nguoiDung);

                    // Ghi log đăng nhập thất bại
                    GhiLogDangNhap(nguoiDung.MaND, tenDangNhap, ipAddress, userAgent, "ThatBai", "Mật khẩu không đúng");

                    _unitOfWork.SaveChanges();

                    string thongBao = "Mật khẩu không đúng";
                    if (nguoiDung.KhoaTaiKhoan)
                    {
                        thongBao += ". Tài khoản đã bị khóa do đăng nhập sai quá 5 lần";
                    }
                    else
                    {
                        thongBao += $". Còn {5 - nguoiDung.SoLanDangNhapSai} lần thử";
                    }

                    return new LoginResult
                    {
                        ThanhCong = false,
                        ThongBao = thongBao
                    };
                }
            }
            catch (Exception ex)
            {
                return new LoginResult
                {
                    ThanhCong = false,
                    ThongBao = $"Lỗi hệ thống: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Ghi log đăng nhập
        /// </summary>
        private void GhiLogDangNhap(int? maND, string tenDangNhap, string ipAddress, string userAgent, string trangThai, string moTa)
        {
            try
            {
                var logDangNhap = new LichSuDangNhap
                {
                    MaND = maND ?? 0, // Tạm thời set 0 nếu không có MaND
                    TenDangNhap = tenDangNhap,
                    IPAddress = ipAddress,
                    UserAgent = userAgent,
                    TrangThai = trangThai,
                    ThoiGian = DateTime.Now
                };

                _unitOfWork.LichSuDangNhapRepository.Add(logDangNhap);
            }
            catch
            {
                // Bỏ qua lỗi ghi log để không ảnh hưởng đến quá trình đăng nhập
            }
        }

        /// <summary>
        /// Lấy thông tin người dùng theo tên đăng nhập
        /// </summary>
        public NguoiDung LayThongTinNguoiDung(string tenDangNhap)
        {
            return _unitOfWork.NguoiDungRepository.GetAll()
                .FirstOrDefault(nd => nd.TenDangNhap == tenDangNhap && nd.TrangThai);
        }

        /// <summary>
        /// Kiểm tra tài khoản có tồn tại không
        /// </summary>
        public bool KiemTraTaiKhoanTonTai(string tenDangNhap)
        {
            return _unitOfWork.NguoiDungRepository.GetAll()
                .Any(nd => nd.TenDangNhap == tenDangNhap && nd.TrangThai);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _unitOfWork?.Dispose();
                }
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// Kết quả đăng nhập
    /// </summary>
    public class LoginResult
    {
        public bool ThanhCong { get; set; }
        public string ThongBao { get; set; }
        public NguoiDung NguoiDung { get; set; }
    }
}
