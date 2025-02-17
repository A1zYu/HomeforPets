﻿using HomeForPets.Core;
using HomeForPets.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HomeForPets.Volunteers.Infrastucture;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            var entries = eventData.Context.ChangeTracker
                .Entries<ISoftDeletable>()
                .Where(e=>e.State==EntityState.Deleted);
            foreach (var entry in entries)
            {
                entry.State = EntityState.Modified;
                entry.Entity.Delete();
            }
        }
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}