﻿using HomeForPets.Application.Database;
using HomeForPets.Application.Files;
using HomeForPets.Application.Messaging;
using HomeForPets.Application.Volunteers;
using HomeForPets.Infrastructure.BackgroundServices;
using HomeForPets.Infrastructure.Files;
using HomeForPets.Infrastructure.MessageQueues;
using HomeForPets.Infrastructure.Options;
using HomeForPets.Infrastructure.Providers;
using HomeForPets.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using FileInfo = HomeForPets.Application.Files.FileInfo;

namespace HomeForPets.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection service,
        IConfiguration configuration)
    {
        service.AddScoped<ApplicationDbContext>();
        service.AddScoped<IVolunteersRepository, VolunteersRepository>();
        service.AddScoped<IUnitOfWork, UnitOfWork>();
        service.AddScoped<IFilesCleanerService, FilesCleanerService>();

        service.AddMinio(configuration);
        
        service.AddHostedService<FilesCleanerBackgroundService>();

        service.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>,InMemoryMessageQueue<IEnumerable<FileInfo>>>();

        return service;
    }

    private static IServiceCollection AddMinio(this IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));
        service.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");
            
            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });
        service.AddScoped<IFileProvider, MinioProvider>();
        return service;
    }
}