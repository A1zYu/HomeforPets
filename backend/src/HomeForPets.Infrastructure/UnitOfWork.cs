﻿using System.Data;
using HomeForPets.Core;
using Microsoft.EntityFrameworkCore.Storage;

namespace HomeForPets.Infrastructure;

// public class UnitOfWork : IUnitOfWork
// {
//     private readonly WriteDbContext _dbContext;
//
//     public UnitOfWork(WriteDbContext dbContext)
//     {
//         _dbContext = dbContext;
//     }
//
//     public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
//     {
//         var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
//
//         return transaction.GetDbTransaction();
//     }
//
//     public async Task SaveChanges(CancellationToken cancellationToken = default)
//     {
//         await _dbContext.SaveChangesAsync(cancellationToken);
//     }
// }