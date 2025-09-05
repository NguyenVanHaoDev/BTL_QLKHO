using System;
using System.Collections.Generic;
using System.Linq;
using QLKHO.DAL.Models;
using QLKHO.DAL.Repositories;

namespace QLKHO.BLL.Services
{
    public class DanhMucService : IDisposable
    {
        private readonly UnitOfWork _unitOfWork;
        private bool _disposed = false;

        public DanhMucService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public DanhMucService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Lấy tất cả danh mục đang hoạt động
        /// </summary>
        /// <returns>Danh sách danh mục</returns>
        public IEnumerable<DanhMucSanPham> GetAllActive()
        {
            return _unitOfWork.DanhMucSanPhamRepository.Get(d => d.TrangThai)
                .OrderBy(d => d.ThuTuHienThi)
                .ThenBy(d => d.TenDM);
        }

        /// <summary>
        /// Lấy tất cả danh mục (bao gồm không hoạt động)
        /// </summary>
        /// <returns>Danh sách danh mục</returns>
        public IEnumerable<DanhMucSanPham> GetAll()
        {
            return _unitOfWork.DanhMucSanPhamRepository.GetAll()
                .OrderBy(d => d.ThuTuHienThi)
                .ThenBy(d => d.TenDM);
        }

        /// <summary>
        /// Lấy danh mục theo ID
        /// </summary>
        /// <param name="id">ID danh mục</param>
        /// <returns>Danh mục</returns>
        public DanhMucSanPham GetById(int id)
        {
            return _unitOfWork.DanhMucSanPhamRepository.GetById(id);
        }

        /// <summary>
        /// Lấy danh mục gốc (không có danh mục cha)
        /// </summary>
        /// <returns>Danh sách danh mục gốc</returns>
        public IEnumerable<DanhMucSanPham> GetRootCategories()
        {
            return _unitOfWork.DanhMucSanPhamRepository.Get(d => d.TrangThai && d.MaDMCapTren == null)
                .OrderBy(d => d.ThuTuHienThi)
                .ThenBy(d => d.TenDM);
        }

        /// <summary>
        /// Lấy danh mục con theo danh mục cha
        /// </summary>
        /// <param name="maDMCapTren">Mã danh mục cha</param>
        /// <returns>Danh sách danh mục con</returns>
        public IEnumerable<DanhMucSanPham> GetSubCategories(int maDMCapTren)
        {
            return _unitOfWork.DanhMucSanPhamRepository.Get(d => d.TrangThai && d.MaDMCapTren == maDMCapTren)
                .OrderBy(d => d.ThuTuHienThi)
                .ThenBy(d => d.TenDM);
        }

        /// <summary>
        /// Tìm danh mục theo tên
        /// </summary>
        /// <param name="tenDM">Tên danh mục</param>
        /// <returns>Danh mục</returns>
        public DanhMucSanPham GetByTenDM(string tenDM)
        {
            return _unitOfWork.DanhMucSanPhamRepository.GetSingle(d => d.TenDM == tenDM && d.TrangThai);
        }

        /// <summary>
        /// Kiểm tra tên danh mục đã tồn tại chưa
        /// </summary>
        /// <param name="tenDM">Tên danh mục</param>
        /// <param name="excludeId">ID danh mục cần loại trừ (khi cập nhật)</param>
        /// <returns>True nếu đã tồn tại</returns>
        public bool IsTenDMExists(string tenDM, int? excludeId = null)
        {
            var query = _unitOfWork.DanhMucSanPhamRepository.Get(d => d.TenDM == tenDM);
            
            if (excludeId.HasValue)
            {
                query = query.Where(d => d.MaDM != excludeId.Value);
            }
            
            return query.Any();
        }

        /// <summary>
        /// Thêm danh mục mới
        /// </summary>
        /// <param name="danhMuc">Danh mục cần thêm</param>
        /// <returns>True nếu thành công</returns>
        public bool Add(DanhMucSanPham danhMuc)
        {
            try
            {
                if (IsTenDMExists(danhMuc.TenDM))
                {
                    throw new InvalidOperationException("Tên danh mục đã tồn tại!");
                }

                danhMuc.NgayTao = DateTime.Now;
                danhMuc.NgayCapNhat = DateTime.Now;
                danhMuc.TrangThai = true;

                _unitOfWork.DanhMucSanPhamRepository.Add(danhMuc);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Cập nhật danh mục
        /// </summary>
        /// <param name="danhMuc">Danh mục cần cập nhật</param>
        /// <returns>True nếu thành công</returns>
        public bool Update(DanhMucSanPham danhMuc)
        {
            try
            {
                if (IsTenDMExists(danhMuc.TenDM, danhMuc.MaDM))
                {
                    throw new InvalidOperationException("Tên danh mục đã tồn tại!");
                }

                danhMuc.NgayCapNhat = DateTime.Now;

                _unitOfWork.DanhMucSanPhamRepository.Update(danhMuc);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Xóa danh mục (soft delete)
        /// </summary>
        /// <param name="id">ID danh mục</param>
        /// <returns>True nếu thành công</returns>
        public bool Delete(int id)
        {
            try
            {
                var danhMuc = GetById(id);
                if (danhMuc == null) return false;

                // Kiểm tra có danh mục con không
                if (GetSubCategories(id).Any())
                {
                    throw new InvalidOperationException("Không thể xóa danh mục có danh mục con!");
                }

                // Kiểm tra có sản phẩm không
                if (_unitOfWork.SanPhamRepository.Get(s => s.MaDM == id).Any())
                {
                    throw new InvalidOperationException("Không thể xóa danh mục có sản phẩm!");
                }

                // Soft delete
                danhMuc.TrangThai = false;
                danhMuc.NgayCapNhat = DateTime.Now;

                _unitOfWork.DanhMucSanPhamRepository.Update(danhMuc);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Xóa cứng danh mục
        /// </summary>
        /// <param name="id">ID danh mục</param>
        /// <returns>True nếu thành công</returns>
        public bool HardDelete(int id)
        {
            try
            {
                var danhMuc = GetById(id);
                if (danhMuc == null) return false;

                // Kiểm tra có danh mục con không
                if (GetSubCategories(id).Any())
                {
                    throw new InvalidOperationException("Không thể xóa danh mục có danh mục con!");
                }

                // Kiểm tra có sản phẩm không
                if (_unitOfWork.SanPhamRepository.Get(s => s.MaDM == id).Any())
                {
                    throw new InvalidOperationException("Không thể xóa danh mục có sản phẩm!");
                }

                _unitOfWork.DanhMucSanPhamRepository.Remove(danhMuc);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Khôi phục danh mục đã xóa
        /// </summary>
        /// <param name="id">ID danh mục</param>
        /// <returns>True nếu thành công</returns>
        public bool Restore(int id)
        {
            try
            {
                var danhMuc = GetById(id);
                if (danhMuc == null) return false;

                danhMuc.TrangThai = true;
                danhMuc.NgayCapNhat = DateTime.Now;

                _unitOfWork.DanhMucSanPhamRepository.Update(danhMuc);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                return false;
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
