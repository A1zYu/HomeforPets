﻿using HomeForPets.Volunteers.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HomeForPets.Volunteers.Infrastucture.DbContexts;

public class VolunteerWriteDbContext(IConfiguration configuration) : DbContext
{
    public const string DATABASE = "localDb";
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseSnakeCaseNamingConvention();

        optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolunteerWriteDbContext).Assembly,
            type=>type.FullName?.Contains("Configurations.Write") ?? false);
        modelBuilder.HasDefaultSchema("volunteers");
    }

    private ILoggerFactory CreateLoggerFactory =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}