using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Pokedex.Business.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Infra.Interceptors
{
    public class EfMetadataInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var changeDate = DateTime.UtcNow;

            var entries = eventData.Context!.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var entity = entry.Entity as Entity;

                if (entry.State == EntityState.Added)
                    entity!.SetCreationDate(changeDate);

                entity!.SetUpdateDate(changeDate);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}