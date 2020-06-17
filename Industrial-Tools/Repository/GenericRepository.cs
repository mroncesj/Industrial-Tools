using Industrial_Tools.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Industrial_Tools.Repository
{
    public class GenericRepository<Entidad> : IRepository<Entidad> where Entidad : class
    {
        DbSet<Entidad> _dbSet;
        private EntitiesBD _DBEntity;

        public GenericRepository(EntitiesBD DBEntity)
        {
            _DBEntity = DBEntity;
            _dbSet = _DBEntity.Set<Entidad>();
        }

        public void Add(Entidad entity)
        {
            _dbSet.Add(entity);
            try
            {
                _DBEntity.SaveChanges();
            }
            catch (Exception ex)
            {
            }

        }

        public int GetAllRecordCount()
        {
            return _dbSet.Count();
        }

        public IEnumerable<Entidad> GetAllRecords()
        {
            return _dbSet.ToList();
        }

        public IQueryable<Entidad> GetAllRecordsQueryable()
        {
            return _dbSet;
        }

        public Entidad GetFirstOrDefault(int id)
        {
            return _dbSet.Find(id);
        }

        public Entidad GetFirstOrDefaultByParameter(Expression<Func<Entidad, bool>> wherePredict)
        {
            return _dbSet.Where(wherePredict).FirstOrDefault();
        }

        public IEnumerable<Entidad> GetListParameter(Expression<Func<Entidad, bool>> wherePredict)
        {
            return _dbSet.Where(wherePredict).ToList();
        }

        public IEnumerable<Entidad> GetResultBySqlProcedure(string query, params object[] parameters)
        {
            if (parameters != null)
            {
                return _DBEntity.Database.SqlQuery<Entidad>(query, parameters).ToList();
            }
            else
                return _DBEntity.Database.SqlQuery<Entidad>(query).ToList();
        }

        public void InactiveAndDeleteByWhereClause(Expression<Func<Entidad, bool>> wherePredict, Action<Entidad> ForEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);
        }

        public void Remove(Entidad entidad)
        {
            if (_DBEntity.Entry(entidad).State == EntityState.Detached)
                _dbSet.Attach(entidad);
            _dbSet.Remove(entidad);
            _DBEntity.SaveChanges();
        }

        public void RemoveByWhereClause(Expression<Func<Entidad, bool>> wherePredict)
        {
            Entidad entity = _dbSet.Where(wherePredict).FirstOrDefault();
            Remove(entity);
        }

        public void RemoveRangeByWhereClause(Expression<Func<Entidad, bool>> wherePredict)
        {
            List<Entidad> entity = _dbSet.Where(wherePredict).ToList();
            foreach (var ent in entity)
            {
                Remove(ent);
            }
        }

        public void Update(Entidad entity)
        {
            _dbSet.Attach(entity);
            _DBEntity.Entry(entity).State = EntityState.Modified;
            _DBEntity.SaveChanges();
        }

        public void UpdateByWhere(Expression<Func<Entidad, bool>> wherePredict, Action<Entidad> ForEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);
        }


        public IEnumerable<Entidad> GetRecordsToShow(int PageNo, int PageSize, int CurrentPage, Expression<Func<Entidad, bool>> wherePredict, Expression<Func<Entidad, int>> orderByPredict)
        {
            if (wherePredict != null)
            {
                return _dbSet.OrderBy(orderByPredict).Where(wherePredict).ToList();
            }
            else
            {
                return _dbSet.OrderBy(orderByPredict).ToList();
            }

        }

        public Entidad GetLastRecord()
        {
            List<Entidad> e = GetAllRecords().ToList();
            if (e.Count() != 0)
            {
                return GetAllRecords().Last();
            }
            return null;
        }
    }
}