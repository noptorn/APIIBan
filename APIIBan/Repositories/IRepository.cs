using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AllnowRiderAPI.Data
{
    public interface IRepository<T> where T : class
    {

        IList<T> GetAll();

        //IEnumerable<T> Query(Func<T, bool> criteria);   Not Work!!!  it gets all rows behind the scene
        IEnumerable<T> Query(Expression<Func<T, Boolean>> criteria);

        IEnumerable<T> QueryReadOnly(Expression<Func<T, Boolean>> criteria);

        TResult GetFirstOrDefault<TResult>(Expression<Func<T, TResult>> selector,
                                           Expression<Func<T, bool>> predicate = null,
                                           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                           bool disableTracking = true);

        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProps);

        T FindByKey(params object[] keyValues);

        //T FindByKeyInclude(params object[] keyValues);
        IEnumerable<T> FindByInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProps);

        T Add(T entity);

        T Remove(T entity);

        void Remove(IEnumerable<T> entity);

        void Commit();

        IDbContextTransaction BeginTransaction();

        Task<int> CommitAsync();

        #region Async Version
        Task<IEnumerable<T>> FindByIncludeAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProps);

        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, Boolean>> criteria);

        Task<IEnumerable<T>> QueryReadOnlyAsync(Expression<Func<T, Boolean>> criteria);
        #endregion
    }
}
