﻿using HomeForPets.Core.Dtos.Volunteers;
using HomeForPets.Volunteers.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HomeForPets.Volunteers.Infrastucture.DbContexts;

public class VolunteerReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    public const string DATABASE = "localDb";

    public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();
    public IQueryable<PetDto> Pets => Set<PetDto>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseSnakeCaseNamingConvention();
        
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolunteerReadDbContext).Assembly,
            type=>type.FullName?.Contains("Configurations.Read") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
    
}