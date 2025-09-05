using System;
using System.Data.Entity;
using System.Linq;
using QLKHO.DAL.Models;

namespace QLKHO.DAL.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private readonly QuanLyKhoDbContext _context;
        private bool _disposed = false;

        // Repositories
        private IRepository<NguoiDung> _nguoiDungRepository;
        private IRepository<DonViTinh> _donViTinhRepository;
        private IRepository<DanhMucSanPham> _danhMucSanPhamRepository;
        private IRepository<SanPham> _sanPhamRepository;
        private IRepository<NhaCungCap> _nhaCungCapRepository;
        private IRepository<PhieuNhap> _phieuNhapRepository;
        private IRepository<ChiTietPhieuNhap> _chiTietPhieuNhapRepository;
        private IRepository<PhieuXuat> _phieuXuatRepository;
        private IRepository<ChiTietPhieuXuat> _chiTietPhieuXuatRepository;
        private IRepository<LichSuDangNhap> _lichSuDangNhapRepository;
        private IRepository<LichSuThayDoi> _lichSuThayDoiRepository;

        public UnitOfWork()
        {
            _context = new QuanLyKhoDbContext();
        }

        public UnitOfWork(QuanLyKhoDbContext context)
        {
            _context = context;
        }

        // Repository properties
        public IRepository<NguoiDung> NguoiDungRepository
        {
            get
            {
                if (_nguoiDungRepository == null)
                    _nguoiDungRepository = new Repository<NguoiDung>(_context);
                return _nguoiDungRepository;
            }
        }

        public IRepository<DonViTinh> DonViTinhRepository
        {
            get
            {
                if (_donViTinhRepository == null)
                    _donViTinhRepository = new Repository<DonViTinh>(_context);
                return _donViTinhRepository;
            }
        }

        public IRepository<DanhMucSanPham> DanhMucSanPhamRepository
        {
            get
            {
                if (_danhMucSanPhamRepository == null)
                    _danhMucSanPhamRepository = new Repository<DanhMucSanPham>(_context);
                return _danhMucSanPhamRepository;
            }
        }

        public IRepository<SanPham> SanPhamRepository
        {
            get
            {
                if (_sanPhamRepository == null)
                    _sanPhamRepository = new Repository<SanPham>(_context);
                return _sanPhamRepository;
            }
        }

        public IRepository<NhaCungCap> NhaCungCapRepository
        {
            get
            {
                if (_nhaCungCapRepository == null)
                    _nhaCungCapRepository = new Repository<NhaCungCap>(_context);
                return _nhaCungCapRepository;
            }
        }

        public IRepository<PhieuNhap> PhieuNhapRepository
        {
            get
            {
                if (_phieuNhapRepository == null)
                    _phieuNhapRepository = new Repository<PhieuNhap>(_context);
                return _phieuNhapRepository;
            }
        }

        public IRepository<ChiTietPhieuNhap> ChiTietPhieuNhapRepository
        {
            get
            {
                if (_chiTietPhieuNhapRepository == null)
                    _chiTietPhieuNhapRepository = new Repository<ChiTietPhieuNhap>(_context);
                return _chiTietPhieuNhapRepository;
            }
        }

        public IRepository<PhieuXuat> PhieuXuatRepository
        {
            get
            {
                if (_phieuXuatRepository == null)
                    _phieuXuatRepository = new Repository<PhieuXuat>(_context);
                return _phieuXuatRepository;
            }
        }

        public IRepository<ChiTietPhieuXuat> ChiTietPhieuXuatRepository
        {
            get
            {
                if (_chiTietPhieuXuatRepository == null)
                    _chiTietPhieuXuatRepository = new Repository<ChiTietPhieuXuat>(_context);
                return _chiTietPhieuXuatRepository;
            }
        }

        public IRepository<LichSuDangNhap> LichSuDangNhapRepository
        {
            get
            {
                if (_lichSuDangNhapRepository == null)
                    _lichSuDangNhapRepository = new Repository<LichSuDangNhap>(_context);
                return _lichSuDangNhapRepository;
            }
        }

        public IRepository<LichSuThayDoi> LichSuThayDoiRepository
        {
            get
            {
                if (_lichSuThayDoiRepository == null)
                    _lichSuThayDoiRepository = new Repository<LichSuThayDoi>(_context);
                return _lichSuThayDoiRepository;
            }
        }

        // Context property
        public QuanLyKhoDbContext Context => _context;

        // Save changes with proper error handling
        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                // Rollback any pending changes
                Rollback();
                throw;
            }
        }

        // Rollback changes
        public void Rollback()
        {
            var changedEntries = _context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Unchanged)
                .ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        // Begin transaction
        public DbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        // Check and reset connection if needed
        public void EnsureConnection()
        {
            try
            {
                // Test connection
                _context.Database.Connection.Open();
                _context.Database.Connection.Close();
            }
            catch
            {
                // If connection is broken, dispose and create new context
                _context.Dispose();
                // Note: This will require recreating the UnitOfWork
                throw new InvalidOperationException("Database connection is broken. Please recreate the UnitOfWork.");
            }
        }

        // Check if context is in a valid state
        public bool IsContextValid()
        {
            try
            {
                return _context.Database.Connection.State == System.Data.ConnectionState.Closed ||
                       _context.Database.Connection.State == System.Data.ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }

        // Dispose
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
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
