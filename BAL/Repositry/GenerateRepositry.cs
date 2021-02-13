using BAL.IRepositry;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL.Repositry
{
    public class GenerateRepositry<T> : IGenericRepositry<T> where T : class
    {
        private readonly TechnicalContext taskContext;


        public GenerateRepositry(TechnicalContext _taskContext)
        {
            taskContext = _taskContext;
        }

        public void Delete(long id)
        {
            T existing = taskContext.Set<T>().Find(id);
            taskContext.Set<T>().Remove(existing);

        }

        public IEnumerable<T> GetAll()
        {
            return taskContext.Set<T>().ToList();
        }

        public T GetById(long id)
        {
            return taskContext.Set<T>().Find(id);
        }

        public void Insert(T entity)
        {
            taskContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            taskContext.Set<T>().Attach(entity);
            taskContext.Entry(entity).State = EntityState.Modified;

        }
        public void Save()
        {
            taskContext.SaveChanges();
        }
        public T GetById(int id)
        {
            return taskContext.Set<T>().Find(id);
        }
        public void Delete(int id)
        {
            T existing = taskContext.Set<T>().Find(id);
            taskContext.Set<T>().Remove(existing);

        }
    }
}
