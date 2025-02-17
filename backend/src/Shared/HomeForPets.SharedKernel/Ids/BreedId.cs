﻿namespace HomeForPets.SharedKernel.Ids;

public record BreedId
{
    public Guid Value { get; }
    private BreedId(Guid id) => Value = id;
    public static BreedId NewId => new (Guid.NewGuid());
    public static BreedId Empty => new (Guid.Empty);

    public static BreedId Create(Guid id) => new(id);
    
    public static implicit operator BreedId(Guid id) => new(id);

    public static implicit operator Guid(BreedId breedId)
    {
        ArgumentNullException.ThrowIfNull(breedId);
        return breedId.Value;
    }
}