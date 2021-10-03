using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
	public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
	{
		protected readonly OrderContext dbContext;

		public RepositoryBase(OrderContext dbContext)
		{
			this.dbContext = dbContext;
		}

	
		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			return await dbContext.Set<T>().ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> Predicate)
		{
			return await dbContext.Set<T>().Where(Predicate).ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> Predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null, string IncludeString = null, bool DisableTracking = true)
		{
			IQueryable<T> query = dbContext.Set<T>();
			if (DisableTracking)
			{
				query = query.AsNoTracking();
			}
			if (!string.IsNullOrWhiteSpace(IncludeString))
			{
				query = query.Include(IncludeString);
			}

			if (Predicate != null)
			{
				query = query.Where(Predicate);
			}

			if (OrderBy != null)
			{
				return await OrderBy(query).ToListAsync();
			}
			return await query.ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> Predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null, List<Expression<Func<T, object>>> Includes = null, bool DisableTracking = true)
		{
			IQueryable<T> query = dbContext.Set<T>();
			if (DisableTracking)
			{
				query = query.AsNoTracking();
			}

			if (Includes != null)
			{
				Includes.Aggregate(query, (current, include) => current.Include(include));
			}

			if (Predicate != null)
			{
				query = query.Where(Predicate);
			}

			if (OrderBy != null)
			{
				return await OrderBy(query).ToListAsync();
			}

			return await query.ToListAsync();

		}

		public async Task<T> GetByIdAsync(int Id)
		{
			return await dbContext.Set<T>().FindAsync(Id);
		}

		public async Task<T> AddAsync(T Entity)
		{
			dbContext.Set<T>().Add(Entity);
			await dbContext.SaveChangesAsync();
			return Entity;
		}

		public async Task DeleteAsync(T Entity)
		{
			 dbContext.Set<T>().Remove(Entity);
			 await dbContext.SaveChangesAsync();
		}


		public async Task UpdatedAsync(T Entity)
		{
			dbContext.Entry(Entity).State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
		}
	}
}
