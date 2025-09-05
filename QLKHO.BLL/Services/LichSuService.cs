using System;
using System.Collections.Generic;
using System.Linq;
using QLKHO.DAL;
using QLKHO.DAL.Models;
using QLKHO.DAL.Repositories;

namespace QLKHO.BLL.Services
{
    public class LichSuService : IDisposable
    {
        private readonly UnitOfWork _unitOfWork;
        private bool _disposed = false;

        public LichSuService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Lấy lịch sử đăng nhập của người dùng
        /// </summary>
        /// <param name="maND">Mã người dùng</param>
        /// <param name="soLuong">Số lượng bản ghi (mặc định 50)</param>
        /// <returns>Danh sách lịch sử đăng nhập</returns>
        public List<LichSuDangNhap> LayLichSuDangNhap(int maND, int soLuong = 50)
        {
            try
            {
                return _unitOfWork.LichSuDangNhapRepository.GetAll()
                    .Where(ls => ls.MaND == maND)
                    .OrderByDescending(ls => ls.ThoiGian)
                    .Take(soLuong)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy lịch sử đăng nhập: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy lịch sử đăng nhập của tất cả người dùng
        /// </summary>
        /// <param name="soLuong">Số lượng bản ghi (mặc định 100)</param>
        /// <returns>Danh sách lịch sử đăng nhập</returns>
        public List<LichSuDangNhap> LayLichSuDangNhapTatCa(int soLuong = 100)
        {
            try
            {
                return _unitOfWork.LichSuDangNhapRepository.GetAll()
                    .OrderByDescending(ls => ls.ThoiGian)
                    .Take(soLuong)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy lịch sử đăng nhập: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy lịch sử thay đổi của người dùng
        /// </summary>
        /// <param name="maND">Mã người dùng</param>
        /// <param name="soLuong">Số lượng bản ghi (mặc định 50)</param>
        /// <returns>Danh sách lịch sử thay đổi</returns>
        public List<LichSuThayDoi> LayLichSuThayDoi(int maND, int soLuong = 50)
        {
            try
            {
                return _unitOfWork.LichSuThayDoiRepository.GetAll()
                    .Where(ls => ls.MaBanGhi == maND && ls.Bang == "NguoiDung")
                    .OrderByDescending(ls => ls.ThoiGian)
                    .Take(soLuong)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy lịch sử thay đổi: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy lịch sử thay đổi của tất cả người dùng
        /// </summary>
        /// <param name="soLuong">Số lượng bản ghi (mặc định 100)</param>
        /// <returns>Danh sách lịch sử thay đổi</returns>
        public List<LichSuThayDoi> LayLichSuThayDoiTatCa(int soLuong = 100)
        {
            try
            {
                return _unitOfWork.LichSuThayDoiRepository.GetAll()
                    .Where(ls => ls.Bang == "NguoiDung")
                    .OrderByDescending(ls => ls.ThoiGian)
                    .Take(soLuong)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy lịch sử thay đổi: {ex.Message}");
            }
        }

        /// <summary>
        /// Ghi lịch sử thay đổi
        /// </summary>
        /// <param name="bang">Tên bảng</param>
        /// <param name="maBanGhi">Mã bản ghi</param>
        /// <param name="thaoTac">Thao tác (INSERT, UPDATE, DELETE)</param>
        /// <param name="duLieuCu">Dữ liệu cũ (JSON)</param>
        /// <param name="duLieuMoi">Dữ liệu mới (JSON)</param>
        /// <param name="nguoiThucHien">Mã người thực hiện</param>
        /// <param name="ipAddress">Địa chỉ IP</param>
        public void GhiLichSuThayDoi(string bang, int maBanGhi, string thaoTac, 
            string duLieuCu = null, string duLieuMoi = null, int? nguoiThucHien = null, string ipAddress = null)
        {
            try
            {
                var lichSu = new LichSuThayDoi
                {
                    Bang = bang,
                    MaBanGhi = maBanGhi,
                    ThaoTac = thaoTac,
                    DuLieuCu = duLieuCu,
                    DuLieuMoi = duLieuMoi,
                    NguoiThucHien = nguoiThucHien,
                    IPAddress = ipAddress,
                    ThoiGian = DateTime.Now
                };

                _unitOfWork.LichSuThayDoiRepository.Add(lichSu);
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                // Không throw exception để không ảnh hưởng đến thao tác chính
                System.Diagnostics.Debug.WriteLine($"Lỗi ghi lịch sử thay đổi: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy thống kê đăng nhập
        /// </summary>
        /// <param name="maND">Mã người dùng (null = tất cả)</param>
        /// <param name="tuNgay">Từ ngày</param>
        /// <param name="denNgay">Đến ngày</param>
        /// <returns>Thống kê đăng nhập</returns>
        public Dictionary<string, int> LayThongKeDangNhap(int? maND = null, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            try
            {
                var query = _unitOfWork.LichSuDangNhapRepository.GetAll().AsQueryable();

                if (maND.HasValue)
                    query = query.Where(ls => ls.MaND == maND.Value);

                if (tuNgay.HasValue)
                    query = query.Where(ls => ls.ThoiGian >= tuNgay.Value);

                if (denNgay.HasValue)
                    query = query.Where(ls => ls.ThoiGian <= denNgay.Value);

                return query.GroupBy(ls => ls.TrangThai)
                    .ToDictionary(g => g.Key, g => g.Count());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thống kê đăng nhập: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa lịch sử cũ (dọn dẹp)
        /// </summary>
        /// <param name="soNgay">Số ngày giữ lại (mặc định 90 ngày)</param>
        /// <returns>Số bản ghi đã xóa</returns>
        public int XoaLichSuCu(int soNgay = 90)
        {
            try
            {
                var ngayXoa = DateTime.Now.AddDays(-soNgay);
                
                // Xóa lịch sử đăng nhập cũ
                var lichSuDangNhapCu = _unitOfWork.LichSuDangNhapRepository.GetAll()
                    .Where(ls => ls.ThoiGian < ngayXoa)
                    .ToList();

                foreach (var ls in lichSuDangNhapCu)
                {
                    _unitOfWork.LichSuDangNhapRepository.Remove(ls);
                }

                // Xóa lịch sử thay đổi cũ
                var lichSuThayDoiCu = _unitOfWork.LichSuThayDoiRepository.GetAll()
                    .Where(ls => ls.ThoiGian < ngayXoa)
                    .ToList();

                foreach (var ls in lichSuThayDoiCu)
                {
                    _unitOfWork.LichSuThayDoiRepository.Remove(ls);
                }

                _unitOfWork.SaveChanges();
                return lichSuDangNhapCu.Count + lichSuThayDoiCu.Count;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa lịch sử cũ: {ex.Message}");
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
                }
                _disposed = true;
            }
        }
    }
}
