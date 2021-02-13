using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer;
namespace BAL.IRepositry
{
    public interface IGenericRepositry<T>
    {
        void Insert(T entity);
        IEnumerable<T> GetAll();
        T GetById(long id);
        void Delete(long id);
        void Update(T entity);
        void Save();
        T GetById(int id);
        void Delete(int id);
    }
}
