using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
	public class OrderContext : DbContext
	{
		public DbSet<Order> Orders { get; set; }

		public OrderContext(DbContextOptions<OrderContext> options): base(options)
		{

		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			SetTracker();
			return base.SaveChangesAsync(cancellationToken);
		}

		private void SetTracker()
		{
			IEnumerator<EntityEntry<EntityBase>> enumrator = ChangeTracker.Entries<EntityBase>().GetEnumerator();
			while (enumrator.MoveNext())
			{
				EntityEntry<EntityBase> item = enumrator.Current;
				switch (item.State)
				{
					case EntityState.Added:
						item.Entity.CreatedDate = DateTime.Now;
						//ToDo After implementing user model fill this section
						item.Entity.CreatedBy = "EMPTY-VALUE";
						break;
					case EntityState.Modified:
						item.Entity.LastModifiedDate = DateTime.Now;
						item.Entity.LastModifiedBy = "EMPTY-VALUE";
						break;
				}
			}
		}
	}
}
