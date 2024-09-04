﻿using CSharpFunctionalExtensions;

namespace HomeForPets.Domain.Shared.ValueObjects;

public record FilePath
{
    private FilePath(string path)
    {
        Path = path;
    }

    public string Path { get; }

    public static Result<FilePath, Error> Create(Guid path, string extension)
    {
        // валидация на доступные расширения файлов

        var fullPath = path + extension;

        return new FilePath(fullPath);
    }
    
    public static Result<FilePath, Error> Create(string fullPath)
    {
        return new FilePath(fullPath);
    }
}