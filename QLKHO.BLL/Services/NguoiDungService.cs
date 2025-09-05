using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLKHO.DAL;
using QLKHO.DAL.Models;
using QLKHO.DAL.Repositories;

namespace QLKHO.BLL.Services
{
    public class NguoiDungService : IDisposable
    {
        private UnitOfWork _unitOfWork;
        private LichSuService _lichSuService;
        private bool _disposed = false;

        public NguoiDungService()
        {
            _unitOfWork = new UnitOfWork();
            _lichSuService = new LichSuService();
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
        /// Lấy danh sách người dùng hoạt động (TrangThai = true)
        /// </summary>
        public List<NguoiDung> LayDanhSachNguoiDungHoatDong()
        {
            return _unitOfWork.NguoiDungRepository.GetAll()
                .Where(nd => nd.TrangThai == true)
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
                // Kiểm tra connection trước khi thực hiện
                if (!_unitOfWork.IsContextValid())
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Kết nối cơ sở dữ liệu không ổn định. Vui lòng thử lại."
                    };
                }

                // Validation
                var validationResult = ValidateNguoiDung(nguoiDung, false); // false = thêm mới
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

                // Ghi lịch sử thay đổi
                try
                {
                    var duLieuMoi = SerializeNguoiDung(nguoiDung);
                    
                    _lichSuService.GhiLichSuThayDoi("NguoiDung", nguoiDung.MaND, "INSERT", 
                        null, duLieuMoi, nguoiDung.MaND, GetCurrentIPAddress());
                }
                catch
                {
                    // Bỏ qua lỗi ghi lịch sử để không ảnh hưởng đến thao tác chính
                }

                return new ServiceResult
                {
                    ThanhCong = true,
                    ThongBao = "Thêm người dùng thành công"
                };
            }
            catch (Exception ex)
            {
                // Debug: Log toàn bộ exception details
                var fullErrorDetails = GetFullExceptionDetails(ex);
                
                // Xử lý các loại lỗi cụ thể - kiểm tra cả InnerException Level 2
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " " + ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += " " + ex.InnerException.InnerException.Message;
                    }
                }
                
                if (ex.InnerException != null)
                {
                    var innerEx = ex.InnerException;
                    if (innerEx.Message.Contains("UNIQUE constraint") || innerEx.Message.Contains("duplicate key"))
                    {
                        return new ServiceResult
                        {
                            ThanhCong = false,
                            ThongBao = "❌ Thông tin đã tồn tại trong hệ thống!\n\n" +
                                      "📋 Vấn đề:\n" +
                                      "• Tên đăng nhập hoặc email đã được sử dụng\n" +
                                      "• Mỗi tài khoản phải có thông tin duy nhất\n\n" +
                                      "💡 Vui lòng kiểm tra và sử dụng tên đăng nhập/email khác."
                        };
                    }
                    else if (innerEx.Message.Contains("CHECK constraint") || 
                             innerEx.Message.Contains("CK__NguoiDung__Email") ||
                             innerEx.Message.Contains("CK__NhaCungCap__Email") ||
                             innerEx.Message.Contains("CK__NguoiDung__DienThoai") ||
                             innerEx.Message.Contains("CK__NhaCungCap__DienThoai") ||
                             innerEx.Message.Contains("CK_TenDangNhap") ||
                             innerEx.Message.Contains("CK__NguoiDung__TenDangNhap") ||
                             innerEx.Message.Contains("CK_MatKhau") ||
                             innerEx.Message.Contains("CK__NguoiDung__MatKhau") ||
                             innerEx.Message.Contains("CK__NguoiDung__VaiTro") ||
                             innerEx.Message.Contains("CK_SoLanDangNhapSai") ||
                             innerEx.Message.Contains("CK__NguoiDung__SoLanDangNhapSai"))
                    {
                        // Xử lý lỗi CHECK constraint cụ thể
                        return HandleCheckConstraintError(innerEx.Message);
                    }
                    else if (innerEx.Message.Contains("constraint"))
                    {
                        return new ServiceResult
                        {
                            ThanhCong = false,
                            ThongBao = "❌ Dữ liệu không hợp lệ!\n\n" +
                                      "📋 Vấn đề:\n" +
                                      "• Thông tin nhập vào không đúng quy tắc\n" +
                                      "• Vi phạm ràng buộc dữ liệu\n\n" +
                                      "💡 Vui lòng kiểm tra lại thông tin nhập vào."
                        };
                    }
                    else if (innerEx.Message.Contains("timeout"))
                    {
                        return new ServiceResult
                        {
                            ThanhCong = false,
                            ThongBao = "⏰ Kết nối cơ sở dữ liệu bị timeout!\n\n" +
                                      "📋 Vấn đề:\n" +
                                      "• Thao tác mất quá nhiều thời gian\n" +
                                      "• Kết nối mạng có thể không ổn định\n\n" +
                                      "💡 Vui lòng thử lại sau vài giây."
                        };
                    }
                    else if (innerEx.Message.Contains("connection"))
                    {
                        return new ServiceResult
                        {
                            ThanhCong = false,
                            ThongBao = "🔌 Lỗi kết nối cơ sở dữ liệu!\n\n" +
                                      "📋 Vấn đề:\n" +
                                      "• Không thể kết nối đến database\n" +
                                      "• Kết nối mạng bị gián đoạn\n\n" +
                                      "💡 Vui lòng kiểm tra kết nối mạng và thử lại."
                        };
                    }
                }
                
                // Kiểm tra toàn bộ error message (bao gồm cả InnerException Level 2)
                if (errorMessage.Contains("CHECK constraint") || 
                    errorMessage.Contains("CK__NguoiDung__Email") ||
                    errorMessage.Contains("CK__NhaCungCap__Email") ||
                    errorMessage.Contains("CK__NguoiDung__DienThoai") ||
                    errorMessage.Contains("CK__NhaCungCap__DienThoai") ||
                    errorMessage.Contains("CK_TenDangNhap") ||
                    errorMessage.Contains("CK__NguoiDung__TenDangNhap") ||
                    errorMessage.Contains("CK_MatKhau") ||
                    errorMessage.Contains("CK__NguoiDung__MatKhau") ||
                    errorMessage.Contains("CK__NguoiDung__VaiTro") ||
                    errorMessage.Contains("CK_SoLanDangNhapSai") ||
                    errorMessage.Contains("CK__NguoiDung__SoLanDangNhapSai"))
                {
                    return HandleCheckConstraintError(errorMessage);
                }

                // Trả về thông tin lỗi chi tiết cho debug
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = $"❌ Lỗi khi thêm người dùng!\n\n{fullErrorDetails}"
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
                // Kiểm tra connection trước khi thực hiện
                if (!_unitOfWork.IsContextValid())
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Kết nối cơ sở dữ liệu không ổn định. Vui lòng thử lại."
                    };
                }

                // Validation
                var validationResult = ValidateNguoiDung(nguoiDung, true); // true = cập nhật
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

                // Debug: Kiểm tra trạng thái entity trước khi update
                var entityState = _unitOfWork.Context.Entry(nguoiDungCu).State;
                
                // Lưu dữ liệu cũ TRƯỚC KHI cập nhật để ghi lịch sử
                var duLieuCu = SerializeNguoiDung(nguoiDungCu);
                
                // Cập nhật thông tin - sử dụng cách tiếp cận đơn giản hơn
                nguoiDungCu.TenDangNhap = nguoiDung.TenDangNhap;
                // Chỉ cập nhật mật khẩu nếu có giá trị mới
                if (!string.IsNullOrEmpty(nguoiDung.MatKhau))
                {
                    nguoiDungCu.MatKhau = nguoiDung.MatKhau;
                }
                nguoiDungCu.HoTen = nguoiDung.HoTen;
                nguoiDungCu.Email = nguoiDung.Email;
                nguoiDungCu.DienThoai = nguoiDung.DienThoai;
                nguoiDungCu.VaiTro = nguoiDung.VaiTro;
                nguoiDungCu.TrangThai = nguoiDung.TrangThai;
                nguoiDungCu.NgayCapNhat = DateTime.Now;

                // Sử dụng Repository Update method thay vì trực tiếp với Context
                _unitOfWork.NguoiDungRepository.Update(nguoiDungCu);
                _unitOfWork.SaveChanges();

                // Ghi lịch sử thay đổi
                try
                {
                    var duLieuMoi = SerializeNguoiDung(nguoiDungCu);
                    
                    _lichSuService.GhiLichSuThayDoi("NguoiDung", nguoiDungCu.MaND, "UPDATE", 
                        duLieuCu, duLieuMoi, nguoiDungCu.MaND, GetCurrentIPAddress());
                }
                catch
                {
                    // Bỏ qua lỗi ghi lịch sử để không ảnh hưởng đến thao tác chính
                }

                return new ServiceResult
                {
                    ThanhCong = true,
                    ThongBao = "Cập nhật người dùng thành công"
                };
            }
            catch (Exception ex)
            {
                // Debug: Log toàn bộ exception details
                var fullErrorDetails = GetFullExceptionDetails(ex);
                
                // Xử lý các loại lỗi cụ thể - kiểm tra cả InnerException Level 2
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " " + ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += " " + ex.InnerException.InnerException.Message;
                    }
                }
                
                if (ex.InnerException != null)
                {
                    var innerEx = ex.InnerException;
                    if (innerEx.Message.Contains("UNIQUE constraint") || innerEx.Message.Contains("duplicate key"))
                    {
                        return new ServiceResult
                        {
                            ThanhCong = false,
                            ThongBao = "❌ Thông tin đã tồn tại trong hệ thống!\n\n" +
                                      "📋 Vấn đề:\n" +
                                      "• Tên đăng nhập hoặc email đã được sử dụng\n" +
                                      "• Mỗi tài khoản phải có thông tin duy nhất\n\n" +
                                      "💡 Vui lòng kiểm tra và sử dụng tên đăng nhập/email khác."
                        };
                    }
                    else if (innerEx.Message.Contains("CHECK constraint") || 
                             innerEx.Message.Contains("CK__NguoiDung__Email") ||
                             innerEx.Message.Contains("CK__NhaCungCap__Email") ||
                             innerEx.Message.Contains("CK__NguoiDung__DienThoai") ||
                             innerEx.Message.Contains("CK__NhaCungCap__DienThoai") ||
                             innerEx.Message.Contains("CK_TenDangNhap") ||
                             innerEx.Message.Contains("CK__NguoiDung__TenDangNhap") ||
                             innerEx.Message.Contains("CK_MatKhau") ||
                             innerEx.Message.Contains("CK__NguoiDung__MatKhau") ||
                             innerEx.Message.Contains("CK__NguoiDung__VaiTro") ||
                             innerEx.Message.Contains("CK_SoLanDangNhapSai") ||
                             innerEx.Message.Contains("CK__NguoiDung__SoLanDangNhapSai"))
                    {
                        // Xử lý lỗi CHECK constraint cụ thể
                        return HandleCheckConstraintError(innerEx.Message);
                    }
                    else if (innerEx.Message.Contains("constraint"))
                    {
                        return new ServiceResult
                        {
                            ThanhCong = false,
                            ThongBao = "❌ Dữ liệu không hợp lệ!\n\n" +
                                      "📋 Vấn đề:\n" +
                                      "• Thông tin nhập vào không đúng quy tắc\n" +
                                      "• Vi phạm ràng buộc dữ liệu\n\n" +
                                      "💡 Vui lòng kiểm tra lại thông tin nhập vào."
                        };
                    }
                    else if (innerEx.Message.Contains("foreign key") || innerEx.Message.Contains("reference"))
                    {
                        return new ServiceResult
                        {
                            ThanhCong = false,
                            ThongBao = "🔗 Không thể thực hiện do ràng buộc dữ liệu!\n\n" +
                                      "📋 Vấn đề:\n" +
                                      "• Dữ liệu đang được sử dụng ở nơi khác\n" +
                                      "• Không thể xóa/cập nhật do liên kết\n\n" +
                                      "💡 Vui lòng kiểm tra và xử lý các dữ liệu liên quan trước."
                        };
                    }
                    else if (innerEx.Message.Contains("timeout"))
                    {
                        return new ServiceResult
                        {
                            ThanhCong = false,
                            ThongBao = "⏰ Kết nối cơ sở dữ liệu bị timeout!\n\n" +
                                      "📋 Vấn đề:\n" +
                                      "• Thao tác mất quá nhiều thời gian\n" +
                                      "• Kết nối mạng có thể không ổn định\n\n" +
                                      "💡 Vui lòng thử lại sau vài giây."
                        };
                    }
                    else if (innerEx.Message.Contains("connection"))
                    {
                        return new ServiceResult
                        {
                            ThanhCong = false,
                            ThongBao = "🔌 Lỗi kết nối cơ sở dữ liệu!\n\n" +
                                      "📋 Vấn đề:\n" +
                                      "• Không thể kết nối đến database\n" +
                                      "• Kết nối mạng bị gián đoạn\n\n" +
                                      "💡 Vui lòng kiểm tra kết nối mạng và thử lại."
                        };
                    }
                }
                
                // Kiểm tra toàn bộ error message (bao gồm cả InnerException Level 2)
                if (errorMessage.Contains("CHECK constraint") || 
                    errorMessage.Contains("CK__NguoiDung__Email") ||
                    errorMessage.Contains("CK__NhaCungCap__Email") ||
                    errorMessage.Contains("CK__NguoiDung__DienThoai") ||
                    errorMessage.Contains("CK__NhaCungCap__DienThoai") ||
                    errorMessage.Contains("CK_TenDangNhap") ||
                    errorMessage.Contains("CK__NguoiDung__TenDangNhap") ||
                    errorMessage.Contains("CK_MatKhau") ||
                    errorMessage.Contains("CK__NguoiDung__MatKhau") ||
                    errorMessage.Contains("CK__NguoiDung__VaiTro") ||
                    errorMessage.Contains("CK_SoLanDangNhapSai") ||
                    errorMessage.Contains("CK__NguoiDung__SoLanDangNhapSai"))
                {
                    return HandleCheckConstraintError(errorMessage);
                }

                // Trả về thông tin lỗi chi tiết cho debug
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = $"❌ Lỗi khi cập nhật người dùng!\n\n{fullErrorDetails}"
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

                // Lưu dữ liệu cũ để ghi lịch sử
                var duLieuCu = SerializeNguoiDung(nguoiDung);

                // Soft delete - chỉ đổi trạng thái
                nguoiDung.TrangThai = false;
                nguoiDung.NgayCapNhat = DateTime.Now;

                _unitOfWork.NguoiDungRepository.Update(nguoiDung);
                _unitOfWork.SaveChanges();

                // Ghi lịch sử thay đổi
                try
                {
                    var duLieuMoi = SerializeNguoiDung(nguoiDung);
                    
                    _lichSuService.GhiLichSuThayDoi("NguoiDung", nguoiDung.MaND, "DELETE", 
                        duLieuCu, duLieuMoi, nguoiDung.MaND, GetCurrentIPAddress());
                }
                catch
                {
                    // Bỏ qua lỗi ghi lịch sử để không ảnh hưởng đến thao tác chính
                }

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

        private ServiceResult ValidateNguoiDung(NguoiDung nguoiDung, bool isUpdate = false)
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

            // Chỉ validate mật khẩu khi thêm mới hoặc khi cập nhật có nhập mật khẩu
            if (!isUpdate || !string.IsNullOrWhiteSpace(nguoiDung.MatKhau))
            {
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

            // Chỉ validate email và số điện thoại khi thêm mới
            if (!isUpdate)
            {
                // Validate email format
                if (!string.IsNullOrEmpty(nguoiDung.Email) && !IsValidEmail(nguoiDung.Email))
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Email không đúng định dạng. Ví dụ: user@example.com"
                    };
                }

                // Validate phone number format
                if (!string.IsNullOrEmpty(nguoiDung.DienThoai) && !IsValidPhoneNumber(nguoiDung.DienThoai))
                {
                    return new ServiceResult
                    {
                        ThanhCong = false,
                        ThongBao = "Số điện thoại không đúng định dạng. Ví dụ: 0123456789 hoặc +84123456789"
                    };
                }
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

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return true; // Cho phép để trống

            // Loại bỏ khoảng trắng và ký tự đặc biệt
            var cleanPhone = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            // Kiểm tra định dạng số điện thoại Việt Nam
            // Bắt đầu bằng 0 hoặc +84, theo sau là 9-10 chữ số
            if (cleanPhone.StartsWith("+84"))
            {
                cleanPhone = "0" + cleanPhone.Substring(3);
            }

            // Kiểm tra theo database constraint: chỉ chứa số, độ dài tối thiểu 10
            if (cleanPhone.Length >= 10 && cleanPhone.All(char.IsDigit))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Debug Helper Methods

        /// <summary>
        /// Xử lý lỗi CHECK constraint cụ thể - Thống nhất cho cả thêm mới và cập nhật
        /// Hỗ trợ tất cả constraint names (có tên và tự động tạo)
        /// </summary>
        private ServiceResult HandleCheckConstraintError(string errorMessage)
        {
            // Kiểm tra lỗi Email constraint - hỗ trợ tất cả tên constraint có thể có
            if (errorMessage.Contains("CK__NguoiDung__Email") || 
                errorMessage.Contains("CK__NhaCungCap__Email") ||
                errorMessage.Contains("Email") || errorMessage.Contains("email"))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "❌ Email không hợp lệ!\n\n" +
                              "📋 Yêu cầu:\n" +
                              "• Phải có định dạng: user@domain.com\n" +
                              "• Độ dài tối thiểu: 5 ký tự\n" +
                              "• Ví dụ: admin@company.com\n\n" +
                              "💡 Vui lòng kiểm tra và nhập lại email đúng định dạng."
                };
            }
            
            // Kiểm tra lỗi DienThoai constraint - hỗ trợ tất cả tên constraint có thể có
            if (errorMessage.Contains("CK__NguoiDung__DienThoai") ||
                errorMessage.Contains("CK__NhaCungCap__DienThoai") ||
                errorMessage.Contains("DienThoai") || errorMessage.Contains("dienThoai") || 
                errorMessage.Contains("phone") || errorMessage.Contains("Phone"))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "❌ Số điện thoại không hợp lệ!\n\n" +
                              "📋 Yêu cầu:\n" +
                              "• Chỉ chứa số (0-9)\n" +
                              "• Độ dài tối thiểu: 10 ký tự\n" +
                              "• Ví dụ: 0123456789, 0987654321\n\n" +
                              "💡 Vui lòng nhập số điện thoại chỉ chứa số và có ít nhất 10 chữ số."
                };
            }
            
            // Kiểm tra lỗi TenDangNhap constraint - chỉ có ở NguoiDung
            if (errorMessage.Contains("CK_TenDangNhap") ||
                errorMessage.Contains("CK__NguoiDung__TenDangNhap") ||
                errorMessage.Contains("TenDangNhap") || errorMessage.Contains("tenDangNhap") || 
                errorMessage.Contains("username") || errorMessage.Contains("Username"))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "❌ Tên đăng nhập không hợp lệ!\n\n" +
                              "📋 Yêu cầu:\n" +
                              "• Độ dài tối thiểu: 3 ký tự\n" +
                              "• Chỉ chứa: chữ cái, số, dấu gạch dưới\n" +
                              "• Ví dụ: admin, user123, nhan_vien\n\n" +
                              "💡 Vui lòng nhập tên đăng nhập chỉ chứa chữ cái, số và dấu gạch dưới."
                };
            }
            
            // Kiểm tra lỗi MatKhau constraint - chỉ có ở NguoiDung
            if (errorMessage.Contains("CK_MatKhau") ||
                errorMessage.Contains("CK__NguoiDung__MatKhau") ||
                errorMessage.Contains("MatKhau") || errorMessage.Contains("matKhau") || 
                errorMessage.Contains("password") || errorMessage.Contains("Password"))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "❌ Mật khẩu không hợp lệ!\n\n" +
                              "📋 Yêu cầu:\n" +
                              "• Độ dài tối thiểu: 6 ký tự\n" +
                              "• Ví dụ: password123, mypass456\n\n" +
                              "💡 Vui lòng nhập mật khẩu có ít nhất 6 ký tự."
                };
            }
            
            // Kiểm tra lỗi VaiTro constraint
            if (errorMessage.Contains("CK__NguoiDung__VaiTro") ||
                errorMessage.Contains("VaiTro") || errorMessage.Contains("vaiTro") ||
                errorMessage.Contains("role") || errorMessage.Contains("Role"))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "❌ Vai trò không hợp lệ!\n\n" +
                              "📋 Yêu cầu:\n" +
                              "• Chỉ được chọn: Admin, QuanLy, NhanVien\n" +
                              "• Ví dụ: Admin, QuanLy, NhanVien\n\n" +
                              "💡 Vui lòng chọn vai trò từ danh sách có sẵn."
                };
            }
            
            // Kiểm tra lỗi SoLanDangNhapSai constraint
            if (errorMessage.Contains("CK_SoLanDangNhapSai") ||
                errorMessage.Contains("CK__NguoiDung__SoLanDangNhapSai") ||
                errorMessage.Contains("SoLanDangNhapSai") || errorMessage.Contains("soLanDangNhapSai"))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "❌ Số lần đăng nhập sai không hợp lệ!\n\n" +
                              "📋 Yêu cầu:\n" +
                              "• Phải từ 0 đến 5 lần\n" +
                              "• Hệ thống tự động quản lý\n\n" +
                              "💡 Vui lòng liên hệ quản trị viên để được hỗ trợ."
                };
            }
            
            // Lỗi CHECK constraint khác
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "❌ Dữ liệu không đúng định dạng yêu cầu!\n\n" +
                          "💡 Vui lòng kiểm tra lại thông tin nhập vào và đảm bảo tuân thủ các quy tắc định dạng."
            };
        }

        /// <summary>
        /// Lấy thông tin chi tiết của exception để debug
        /// </summary>
        private string GetFullExceptionDetails(Exception ex)
        {
            var details = new System.Text.StringBuilder();
            
            details.AppendLine($"Exception Type: {ex.GetType().Name}");
            details.AppendLine($"Message: {ex.Message}");
            details.AppendLine($"Source: {ex.Source}");
            
            // Thêm thông tin về Entity Framework nếu có
            if (ex.Message.Contains("Entity Framework") || ex.Message.Contains("DbContext"))
            {
                details.AppendLine("\n--- Entity Framework Debug Info ---");
                try
                {
                    var context = _unitOfWork.Context;
                    details.AppendLine($"Context State: {context.Database.Connection.State}");
                    details.AppendLine($"Change Tracker Count: {context.ChangeTracker.Entries().Count()}");
                    
                    foreach (var entry in context.ChangeTracker.Entries())
                    {
                        details.AppendLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
                    }
                }
                catch (Exception debugEx)
                {
                    details.AppendLine($"Debug info error: {debugEx.Message}");
                }
            }
            
            details.AppendLine($"\nStack Trace: {ex.StackTrace}");
            
            if (ex.InnerException != null)
            {
                details.AppendLine("\n--- Inner Exception ---");
                details.AppendLine($"Type: {ex.InnerException.GetType().Name}");
                details.AppendLine($"Message: {ex.InnerException.Message}");
                details.AppendLine($"Source: {ex.InnerException.Source}");
                details.AppendLine($"Stack Trace: {ex.InnerException.StackTrace}");
                
                // Kiểm tra thêm inner exception của inner exception
                if (ex.InnerException.InnerException != null)
                {
                    details.AppendLine("\n--- Inner Exception Level 2 ---");
                    details.AppendLine($"Type: {ex.InnerException.InnerException.GetType().Name}");
                    details.AppendLine($"Message: {ex.InnerException.InnerException.Message}");
                }
            }
            
            return details.ToString();
        }

        #endregion

        /// <summary>
        /// Reset UnitOfWork khi có lỗi connection
        /// </summary>
        public void ResetUnitOfWork()
        {
            try
            {
                _unitOfWork?.Dispose();
            }
            catch
            {
                // Ignore disposal errors
            }
            finally
            {
                _unitOfWork = new UnitOfWork();
            }
        }

        /// <summary>
        /// Serialize thông tin người dùng thành JSON string
        /// </summary>
        private string SerializeNguoiDung(NguoiDung nguoiDung)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            sb.Append($"\"TenDangNhap\":\"{nguoiDung.TenDangNhap}\",");
            sb.Append($"\"HoTen\":\"{nguoiDung.HoTen}\",");
            sb.Append($"\"Email\":\"{nguoiDung.Email ?? ""}\",");
            sb.Append($"\"DienThoai\":\"{nguoiDung.DienThoai ?? ""}\",");
            sb.Append($"\"VaiTro\":\"{nguoiDung.VaiTro}\",");
            sb.Append($"\"TrangThai\":{nguoiDung.TrangThai.ToString().ToLower()}");
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// Lấy địa chỉ IP hiện tại
        /// </summary>
        private string GetCurrentIPAddress()
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "127.0.0.1"; // Localhost fallback
            }
            catch
            {
                return "127.0.0.1"; // Localhost fallback
            }
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
                    _lichSuService?.Dispose();
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
