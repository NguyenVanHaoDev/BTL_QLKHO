using System;
using System.Collections.Generic;
using System.Linq;
using QLKHO.DAL.Models;
using QLKHO.DAL.Repositories;

namespace QLKHO.BLL.Services
{
    public class DonViTinhService : IDisposable
    {
        private readonly UnitOfWork _unitOfWork;
        private bool _disposed = false;

        public DonViTinhService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public DonViTinhService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Lấy tất cả đơn vị tính đang hoạt động
        /// </summary>
        /// <returns>Danh sách đơn vị tính</returns>
        public IEnumerable<DonViTinh> GetAllActive()
        {
            return _unitOfWork.DonViTinhRepository.Get(d => d.TrangThai);
        }

        /// <summary>
        /// Lấy đơn vị tính theo ID
        /// </summary>
        /// <param name="id">ID đơn vị tính</param>
        /// <returns>Đơn vị tính</returns>
        public DonViTinh GetById(int id)
        {
            return _unitOfWork.DonViTinhRepository.GetById(id);
        }

        /// <summary>
        /// Tìm đơn vị tính theo tên
        /// </summary>
        /// <param name="tenDVT">Tên đơn vị tính</param>
        /// <returns>Đơn vị tính</returns>
        public DonViTinh GetByTenDVT(string tenDVT)
        {
            return _unitOfWork.DonViTinhRepository.GetSingle(d => d.TenDVT == tenDVT && d.TrangThai);
        }

        /// <summary>
        /// Tìm đơn vị tính theo ký hiệu
        /// </summary>
        /// <param name="kyHieu">Ký hiệu đơn vị tính</param>
        /// <returns>Đơn vị tính</returns>
        public DonViTinh GetByKyHieu(string kyHieu)
        {
            return _unitOfWork.DonViTinhRepository.GetSingle(d => d.KyHieu == kyHieu && d.TrangThai);
        }

        /// <summary>
        /// Thêm đơn vị tính mới
        /// </summary>
        /// <param name="donViTinh">Đơn vị tính</param>
        /// <returns>True nếu thành công</returns>
        public bool Add(DonViTinh donViTinh)
        {
            try
            {
                // Kiểm tra tên đơn vị tính đã tồn tại chưa
                if (GetByTenDVT(donViTinh.TenDVT) != null)
                {
                    throw new Exception("Tên đơn vị tính đã tồn tại!");
                }

                // Kiểm tra ký hiệu đã tồn tại chưa
                if (GetByKyHieu(donViTinh.KyHieu) != null)
                {
                    throw new Exception("Ký hiệu đơn vị tính đã tồn tại!");
                }

                donViTinh.NgayTao = DateTime.Now;
                donViTinh.NgayCapNhat = DateTime.Now;
                donViTinh.TrangThai = true;

                _unitOfWork.DonViTinhRepository.Add(donViTinh);
                _unitOfWork.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm đơn vị tính: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật đơn vị tính
        /// </summary>
        /// <param name="donViTinh">Đơn vị tính</param>
        /// <returns>True nếu thành công</returns>
        public bool Update(DonViTinh donViTinh)
        {
            try
            {
                var existingDVT = GetById(donViTinh.MaDVT);
                if (existingDVT == null)
                {
                    throw new Exception("Đơn vị tính không tồn tại!");
                }

                // Kiểm tra tên đơn vị tính đã tồn tại chưa (trừ chính nó)
                var duplicateTen = GetByTenDVT(donViTinh.TenDVT);
                if (duplicateTen != null && duplicateTen.MaDVT != donViTinh.MaDVT)
                {
                    throw new Exception("Tên đơn vị tính đã tồn tại!");
                }

                // Kiểm tra ký hiệu đã tồn tại chưa (trừ chính nó)
                var duplicateKyHieu = GetByKyHieu(donViTinh.KyHieu);
                if (duplicateKyHieu != null && duplicateKyHieu.MaDVT != donViTinh.MaDVT)
                {
                    throw new Exception("Ký hiệu đơn vị tính đã tồn tại!");
                }

                donViTinh.NgayCapNhat = DateTime.Now;

                _unitOfWork.DonViTinhRepository.Update(donViTinh);
                _unitOfWork.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật đơn vị tính: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa đơn vị tính (soft delete)
        /// </summary>
        /// <param name="id">ID đơn vị tính</param>
        /// <returns>True nếu thành công</returns>
        public bool Delete(int id)
        {
            try
            {
                var donViTinh = GetById(id);
                if (donViTinh == null)
                {
                    throw new Exception("Đơn vị tính không tồn tại!");
                }

                // Kiểm tra đơn vị tính có đang được sử dụng không
                var isInUse = _unitOfWork.SanPhamRepository.Exists(s => s.MaDVT == id && s.TrangThai);
                if (isInUse)
                {
                    throw new Exception("Không thể xóa đơn vị tính đang được sử dụng!");
                }

                donViTinh.TrangThai = false;
                donViTinh.NgayCapNhat = DateTime.Now;

                _unitOfWork.DonViTinhRepository.Update(donViTinh);
                _unitOfWork.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa đơn vị tính: {ex.Message}");
            }
        }

        /// <summary>
        /// Tìm kiếm đơn vị tính
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách đơn vị tính</returns>
        public IEnumerable<DonViTinh> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return GetAllActive();
            }

            return _unitOfWork.DonViTinhRepository.Get(d => 
                d.TrangThai && 
                (d.TenDVT.Contains(keyword) || d.KyHieu.Contains(keyword) || 
                 (d.MoTa != null && d.MoTa.Contains(keyword))));
        }

        /// <summary>
        /// Kiểm tra đơn vị tính có đang được sử dụng không
        /// </summary>
        /// <param name="id">ID đơn vị tính</param>
        /// <returns>True nếu đang được sử dụng</returns>
        public bool IsInUse(int id)
        {
            return _unitOfWork.SanPhamRepository.Exists(s => s.MaDVT == id && s.TrangThai);
        }

        /// <summary>
        /// Lấy số lượng đơn vị tính đang hoạt động
        /// </summary>
        /// <returns>Số lượng</returns>
        public int GetActiveCount()
        {
            return _unitOfWork.DonViTinhRepository.Count(d => d.TrangThai);
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
