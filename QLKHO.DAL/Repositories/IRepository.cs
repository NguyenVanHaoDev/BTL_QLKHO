using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QLKHO.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        // Lấy tất cả
        IEnumerable<T> GetAll();
        
        // Lấy theo điều kiện
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        
        // Lấy theo ID
        T GetById(object id);
        
        // Lấy đơn lẻ theo điều kiện
        T GetSingle(Expression<Func<T, bool>> predicate);
        
        // Thêm
        void Add(T entity);
        
        // Thêm nhiều
        void AddRange(IEnumerable<T> entities);
        
        // Cập nhật
        void Update(T entity);
        
        // Xóa
        void Remove(T entity);
        
        // Xóa theo ID
        void RemoveById(object id);
        
        // Xóa nhiều
        void RemoveRange(IEnumerable<T> entities);
        
        // Kiểm tra tồn tại
        bool Exists(Expression<Func<T, bool>> predicate);
        
        // Đếm
        int Count();
        int Count(Expression<Func<T, bool>> predicate);
        
        // Lấy queryable
        IQueryable<T> AsQueryable();
    }
}
