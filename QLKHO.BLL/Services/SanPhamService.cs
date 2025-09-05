using System;
using System.Collections.Generic;
using System.Linq;
using QLKHO.DAL.Models;
using QLKHO.DAL.Repositories;

namespace QLKHO.BLL.Services
{
    public class SanPhamService : IDisposable
    {
        private readonly UnitOfWork _unitOfWork;
        private bool _disposed = false;

        public SanPhamService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public SanPhamService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Lấy tất cả sản phẩm đang hoạt động
        /// </summary>
        /// <returns>Danh sách sản phẩm</returns>
        public IEnumerable<SanPham> GetAllActive()
        {
            return _unitOfWork.SanPhamRepository.Get(s => s.TrangThai);
        }

        /// <summary>
        /// Lấy sản phẩm theo ID
        /// </summary>
        /// <param name="id">ID sản phẩm</param>
        /// <returns>Sản phẩm</returns>
        public SanPham GetById(int id)
        {
            return _unitOfWork.SanPhamRepository.GetById(id);
        }

        /// <summary>
        /// Lấy sản phẩm theo danh mục
        /// </summary>
        /// <param name="maDM">ID danh mục</param>
        /// <returns>Danh sách sản phẩm</returns>
        public IEnumerable<SanPham> GetByDanhMuc(int maDM)
        {
            return _unitOfWork.SanPhamRepository.Get(s => s.MaDM == maDM && s.TrangThai);
        }

        /// <summary>
        /// Tìm kiếm sản phẩm
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách sản phẩm</returns>
        public IEnumerable<SanPham> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return GetAllActive();
            }

            return _unitOfWork.SanPhamRepository.Get(s => 
                s.TrangThai && 
                (s.TenSP.Contains(keyword) || 
                 (s.MoTa != null && s.MoTa.Contains(keyword))));
        }

        /// <summary>
        /// Lấy sản phẩm sắp hết hàng
        /// </summary>
        /// <returns>Danh sách sản phẩm</returns>
        public IEnumerable<SanPham> GetSanPhamSapHetHang()
        {
            return _unitOfWork.SanPhamRepository.Get(s => 
                s.TrangThai && s.SoLuongTon <= s.SoLuongToiThieu);
        }

        /// <summary>
        /// Lấy sản phẩm hết hàng
        /// </summary>
        /// <returns>Danh sách sản phẩm</returns>
        public IEnumerable<SanPham> GetSanPhamHetHang()
        {
            return _unitOfWork.SanPhamRepository.Get(s => 
                s.TrangThai && s.SoLuongTon <= 0);
        }

        /// <summary>
        /// Thêm sản phẩm mới
        /// </summary>
        /// <param name="sanPham">Sản phẩm</param>
        /// <returns>True nếu thành công</returns>
        public bool Add(SanPham sanPham)
        {
            try
            {
                // Validation
                if (sanPham.GiaNhap <= 0)
                    throw new Exception("Giá nhập phải lớn hơn 0!");
                
                if (sanPham.GiaBan <= 0)
                    throw new Exception("Giá bán phải lớn hơn 0!");
                
                if (sanPham.GiaBan < sanPham.GiaNhap)
                    throw new Exception("Giá bán không được nhỏ hơn giá nhập!");

                sanPham.NgayTao = DateTime.Now;
                sanPham.NgayCapNhat = DateTime.Now;
                sanPham.TrangThai = true;

                _unitOfWork.SanPhamRepository.Add(sanPham);
                _unitOfWork.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật sản phẩm
        /// </summary>
        /// <param name="sanPham">Sản phẩm</param>
        /// <returns>True nếu thành công</returns>
        public bool Update(SanPham sanPham)
        {
            try
            {
                var existingSP = GetById(sanPham.MaSP);
                if (existingSP == null)
                {
                    throw new Exception("Sản phẩm không tồn tại!");
                }

                // Validation
                if (sanPham.GiaNhap <= 0)
                    throw new Exception("Giá nhập phải lớn hơn 0!");
                
                if (sanPham.GiaBan <= 0)
                    throw new Exception("Giá bán phải lớn hơn 0!");
                
                if (sanPham.GiaBan < sanPham.GiaNhap)
                    throw new Exception("Giá bán không được nhỏ hơn giá nhập!");

                sanPham.NgayCapNhat = DateTime.Now;

                _unitOfWork.SanPhamRepository.Update(sanPham);
                _unitOfWork.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa sản phẩm (soft delete)
        /// </summary>
        /// <param name="id">ID sản phẩm</param>
        /// <returns>True nếu thành công</returns>
        public bool Delete(int id)
        {
            try
            {
                var sanPham = GetById(id);
                if (sanPham == null)
                {
                    throw new Exception("Sản phẩm không tồn tại!");
                }

                // Kiểm tra sản phẩm có đang được sử dụng trong phiếu nhập/xuất không
                var isInUse = _unitOfWork.ChiTietPhieuNhapRepository.Exists(c => c.MaSP == id) ||
                             _unitOfWork.ChiTietPhieuXuatRepository.Exists(c => c.MaSP == id);
                
                if (isInUse)
                {
                    throw new Exception("Không thể xóa sản phẩm đang được sử dụng trong phiếu nhập/xuất!");
                }

                sanPham.TrangThai = false;
                sanPham.NgayCapNhat = DateTime.Now;

                _unitOfWork.SanPhamRepository.Update(sanPham);
                _unitOfWork.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa sản phẩm: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật tồn kho
        /// </summary>
        /// <param name="maSP">ID sản phẩm</param>
        /// <param name="soLuong">Số lượng thay đổi</param>
        /// <param name="isNhap">True nếu nhập, False nếu xuất</param>
        /// <returns>True nếu thành công</returns>
        public bool UpdateTonKho(int maSP, int soLuong, bool isNhap)
        {
            try
            {
                var sanPham = GetById(maSP);
                if (sanPham == null)
                {
                    throw new Exception("Sản phẩm không tồn tại!");
                }

                if (isNhap)
                {
                    sanPham.SoLuongTon += soLuong;
                }
                else
                {
                    if (sanPham.SoLuongTon < soLuong)
                    {
                        throw new Exception("Không đủ hàng trong kho!");
                    }
                    sanPham.SoLuongTon -= soLuong;
                }

                sanPham.NgayCapNhat = DateTime.Now;

                _unitOfWork.SanPhamRepository.Update(sanPham);
                _unitOfWork.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật tồn kho: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy số lượng sản phẩm đang hoạt động
        /// </summary>
        /// <returns>Số lượng</returns>
        public int GetActiveCount()
        {
            return _unitOfWork.SanPhamRepository.Count(s => s.TrangThai);
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
