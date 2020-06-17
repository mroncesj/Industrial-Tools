using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Industrial_Tools.Repository
{
    public interface IRepository<Entidad> where Entidad : class
    {
        Entidad GetLastRecord();
        IEnumerable<Entidad> GetAllRecords();
        IQueryable<Entidad> GetAllRecordsQueryable();
        int GetAllRecordCount();
        void Add(Entidad entity);
        void Update(Entidad entity);
        void UpdateByWhere(Expression<Func<Entidad, bool>> wherePredict, Action<Entidad> ForEachPredict);
        Entidad GetFirstOrDefault(int id);
        void Remove(Entidad entidad);
        void RemoveByWhereClause(Expression<Func<Entidad, bool>> wherePredict);
        void RemoveRangeByWhereClause(Expression<Func<Entidad, bool>> wherePredict);
        void InactiveAndDeleteByWhereClause(Expression<Func<Entidad, bool>> wherePredict, Action<Entidad> ForEachPredict);
        Entidad GetFirstOrDefaultByParameter(Expression<Func<Entidad, bool>> wherePredict);
        IEnumerable<Entidad> GetListParameter(Expression<Func<Entidad, bool>> wherePredict);
        IEnumerable<Entidad> GetResultBySqlProcedure(String query, params object[] parameters);
        IEnumerable<Entidad> GetRecordsToShow(int PageNo, int PageSize, int CurrentPage, Expression<Func<Entidad, bool>> wherePredict, Expression<Func<Entidad, int>> orderByPredict);
    }
}
