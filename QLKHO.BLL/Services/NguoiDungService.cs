using System;
using System.Collections.Generic;
using System.Linq;
using QLKHO.DAL;
using QLKHO.DAL.Models;
using QLKHO.DAL.Repositories;

namespace QLKHO.BLL.Services
{
    public class NguoiDungService : IDisposable
    {
        private readonly UnitOfWork _unitOfWork;
        private bool _disposed = false;

        public NguoiDungService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Lấy danh sách tất cả người dùng
        /// </summary>
        public List<NguoiDung> LayDanhSachNguoiDung()
        {
            return _unitOfWork.NguoiDungRepository.GetAll()
                .OrderBy(nd => nd.HoTen)
                .ToList();
        }

        /// <summary>
        /// Lấy người dùng theo ID
        /// </summary>
        public NguoiDung LayNguoiDungTheoId(int maND)
        {
            return _unitOfWork.NguoiDungRepository.GetById(maND);
        }

        /// <summary>
        /// Lấy người dùng theo tên đăng nhập
        /// </summary>
        public NguoiDung LayNguoiDungTheoTenDangNhap(string tenDangNhap)
        {
            return _unitOfWork.NguoiDungRepository.GetAll()
                .FirstOrDefault(nd => nd.TenDangNhap == tenDangNhap);
        }

        /// <summary>
        /// Thêm người dùng mới
        /// </summary>
        public ServiceResult ThemNguoiDung(NguoiDung nguoiDung)
        {
            try
            {
                // Validation
                var validationResult = ValidateNguoiDung(nguoiDung);
                if (!validationResult.ThanhCong)
                {
                    return validationResult;
                }

                // Kiểm tra tên đăng nhập đã tồn tại chưa
                if (KiemTraTenDangNhapTonTai(nguoiDung.TenDangNhap))
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Tên đăng nhập đã tồn tại"
                    };
                }

                // Kiểm tra email đã tồn tại chưa (nếu có)
                if (!string.IsNullOrEmpty(nguoiDung.Email) && KiemTraEmailTonTai(nguoiDung.Email))
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Email đã được sử dụng"
                    };
                }

                // Set thông tin mặc định
                nguoiDung.NgayTao = DateTime.Now;
                nguoiDung.NgayCapNhat = DateTime.Now;
                nguoiDung.TrangThai = true;
                nguoiDung.SoLanDangNhapSai = 0;
                nguoiDung.KhoaTaiKhoan = false;

                _unitOfWork.NguoiDungRepository.Add(nguoiDung);
                _unitOfWork.SaveChanges();

                return new ServiceResult
                {
                    ThanhCong = true,
                    ThongBao = "Thêm người dùng thành công"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = $"Lỗi khi thêm người dùng: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        public ServiceResult CapNhatNguoiDung(NguoiDung nguoiDung)
        {
            try
            {
                // Validation
                var validationResult = ValidateNguoiDung(nguoiDung);
                if (!validationResult.ThanhCong)
                {
                    return validationResult;
                }

                // Lấy thông tin cũ
                var nguoiDungCu = _unitOfWork.NguoiDungRepository.GetById(nguoiDung.MaND);
                if (nguoiDungCu == null)
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Không tìm thấy người dùng"
                    };
                }

                // Kiểm tra tên đăng nhập đã tồn tại chưa (trừ chính nó)
                if (KiemTraTenDangNhapTonTai(nguoiDung.TenDangNhap, nguoiDung.MaND))
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Tên đăng nhập đã tồn tại"
                    };
                }

                // Kiểm tra email đã tồn tại chưa (trừ chính nó)
                if (!string.IsNullOrEmpty(nguoiDung.Email) && KiemTraEmailTonTai(nguoiDung.Email, nguoiDung.MaND))
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Email đã được sử dụng"
                    };
                }

                // Cập nhật thông tin
                nguoiDungCu.TenDangNhap = nguoiDung.TenDangNhap;
                nguoiDungCu.MatKhau = nguoiDung.MatKhau;
                nguoiDungCu.HoTen = nguoiDung.HoTen;
                nguoiDungCu.Email = nguoiDung.Email;
                nguoiDungCu.DienThoai = nguoiDung.DienThoai;
                nguoiDungCu.VaiTro = nguoiDung.VaiTro;
                nguoiDungCu.TrangThai = nguoiDung.TrangThai;
                nguoiDungCu.NgayCapNhat = DateTime.Now;

                _unitOfWork.NguoiDungRepository.Update(nguoiDungCu);
                _unitOfWork.SaveChanges();

                return new ServiceResult
                {
                    ThanhCong = true,
                    ThongBao = "Cập nhật người dùng thành công"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = $"Lỗi khi cập nhật người dùng: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Xóa người dùng (soft delete)
        /// </summary>
        public ServiceResult XoaNguoiDung(int maND)
        {
            try
            {
                var nguoiDung = _unitOfWork.NguoiDungRepository.GetById(maND);
                if (nguoiDung == null)
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Không tìm thấy người dùng"
                    };
                }

                // Kiểm tra có phải admin cuối cùng không
                if (nguoiDung.VaiTro == "Admin" && DemSoAdmin() <= 1)
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Không thể xóa admin cuối cùng"
                    };
                }

                // Soft delete - chỉ đổi trạng thái
                nguoiDung.TrangThai = false;
                nguoiDung.NgayCapNhat = DateTime.Now;

                _unitOfWork.NguoiDungRepository.Update(nguoiDung);
                _unitOfWork.SaveChanges();

                return new ServiceResult
                {
                    ThanhCong = true,
                    ThongBao = "Xóa người dùng thành công"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = $"Lỗi khi xóa người dùng: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Đặt lại mật khẩu
        /// </summary>
        public ServiceResult DatLaiMatKhau(int maND, string matKhauMoi)
        {
            try
            {
                var nguoiDung = _unitOfWork.NguoiDungRepository.GetById(maND);
                if (nguoiDung == null)
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Không tìm thấy người dùng"
                    };
                }

                if (string.IsNullOrWhiteSpace(matKhauMoi) || matKhauMoi.Length < 6)
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Mật khẩu phải có ít nhất 6 ký tự"
                    };
                }

                nguoiDung.MatKhau = matKhauMoi;
                nguoiDung.SoLanDangNhapSai = 0;
                nguoiDung.KhoaTaiKhoan = false;
                nguoiDung.ThoiGianKhoa = null;
                nguoiDung.NgayCapNhat = DateTime.Now;

                _unitOfWork.NguoiDungRepository.Update(nguoiDung);
                _unitOfWork.SaveChanges();

                return new ServiceResult
                {
                    ThanhCong = true,
                    ThongBao = "Đặt lại mật khẩu thành công"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = $"Lỗi khi đặt lại mật khẩu: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Khóa/Mở khóa tài khoản
        /// </summary>
        public ServiceResult KhoaMoKhoaTaiKhoan(int maND, bool khoa)
        {
            try
            {
                var nguoiDung = _unitOfWork.NguoiDungRepository.GetById(maND);
                if (nguoiDung == null)
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Không tìm thấy người dùng"
                    };
                }

                nguoiDung.KhoaTaiKhoan = khoa;
                nguoiDung.ThoiGianKhoa = khoa ? (DateTime?)DateTime.Now : null;
                nguoiDung.NgayCapNhat = DateTime.Now;

                _unitOfWork.NguoiDungRepository.Update(nguoiDung);
                _unitOfWork.SaveChanges();

                return new ServiceResult
                {
                    ThanhCong = true,
                    ThongBao = khoa ? "Khóa tài khoản thành công" : "Mở khóa tài khoản thành công"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = $"Lỗi khi khóa/mở khóa tài khoản: {ex.Message}"
                };
            }
        }

        #region Private Methods

        private ServiceResult ValidateNguoiDung(NguoiDung nguoiDung)
        {
            if (string.IsNullOrWhiteSpace(nguoiDung.TenDangNhap))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Tên đăng nhập không được để trống"
                };
            }

            if (nguoiDung.TenDangNhap.Length < 3)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Tên đăng nhập phải có ít nhất 3 ký tự"
                };
            }

            if (string.IsNullOrWhiteSpace(nguoiDung.MatKhau))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Mật khẩu không được để trống"
                };
            }

            if (nguoiDung.MatKhau.Length < 6)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Mật khẩu phải có ít nhất 6 ký tự"
                };
            }

            if (string.IsNullOrWhiteSpace(nguoiDung.HoTen))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Họ tên không được để trống"
                };
            }

            if (string.IsNullOrWhiteSpace(nguoiDung.VaiTro))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Vai trò không được để trống"
                };
            }

            if (!new[] { "Admin", "QuanLy", "NhanVien" }.Contains(nguoiDung.VaiTro))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Vai trò không hợp lệ"
                };
            }

            // Validate email format
            if (!string.IsNullOrEmpty(nguoiDung.Email) && !IsValidEmail(nguoiDung.Email))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Email không đúng định dạng"
                };
            }

            return new ServiceResult { ThanhCong = true };
        }

        private bool KiemTraTenDangNhapTonTai(string tenDangNhap, int? maND = null)
        {
            var query = _unitOfWork.NguoiDungRepository.GetAll()
                .Where(nd => nd.TenDangNhap == tenDangNhap);

            if (maND.HasValue)
            {
                query = query.Where(nd => nd.MaND != maND.Value);
            }

            return query.Any();
        }

        private bool KiemTraEmailTonTai(string email, int? maND = null)
        {
            var query = _unitOfWork.NguoiDungRepository.GetAll()
                .Where(nd => nd.Email == email);

            if (maND.HasValue)
            {
                query = query.Where(nd => nd.MaND != maND.Value);
            }

            return query.Any();
        }

        private int DemSoAdmin()
        {
            return _unitOfWork.NguoiDungRepository.GetAll()
                .Count(nd => nd.VaiTro == "Admin" && nd.TrangThai);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion

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
    /// Kết quả trả về từ service
    /// </summary>
    public class ServiceResult
    {
        public bool ThanhCong { get; set; }
        public string ThongBao { get; set; }
        public object DuLieu { get; set; }
    }
}
