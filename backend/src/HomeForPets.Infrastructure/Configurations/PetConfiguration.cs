﻿using HomeForPets.Domain.Constraints;
using HomeForPets.Domain.Shared.Ids;
using HomeForPets.Domain.Volunteers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeForPets.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value)
            );
        builder.Property(p => p.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(Constraints.LOW_VALUE_LENGTH);
        builder.ComplexProperty(p => p.Description, pb =>
        {
            pb.Property(x => x.Text)
                .HasColumnName("description")
                .IsRequired();
        });
        builder.ComplexProperty(p => p.PetDetails, pb =>
        {
            pb.Property(x => x.Color)
                .HasColumnName("color")
                .IsRequired();
            pb.Property(x => x.Height)
                .HasColumnName("height")
                .IsRequired();
            pb.Property(x => x.Weight)
                .HasColumnName("weight")
                .IsRequired();
            pb.Property(x => x.HealthInfo)
                .HasColumnName("help_info")
                .IsRequired();
            pb.Property(x => x.BirthOfDate)
                .HasColumnName("birth_of_date")
                .IsRequired();
            pb.Property(x => x.IsVaccinated)
                .HasColumnName("is_vaccinated")
                .IsRequired();
            pb.Property(x => x.IsVaccinated)
                .HasColumnName("is_vaccinated")
                .IsRequired();
        });

        builder.ComplexProperty(p => p.SpeciesBreed, s =>
        {
            s.Property(x => x.BreedId)
                .IsRequired();
            s.Property(x => x.SpeciesId)
                .HasConversion(
                    id => id.Value,
                    value => SpeciesId.Create(value)
                ).IsRequired();
        });
        
        builder.ComplexProperty(p => p.Address, a =>
        {
            a.Property(x => x.City)
                .HasColumnName("city")
                .IsRequired();
            a.Property(x => x.District)
                .HasColumnName("district")
                .IsRequired();
            a.Property(x => x.FlatNumber)
                .HasColumnName("flat_number")
                .IsRequired();
            a.Property(x => x.HouseNumber)
                .HasColumnName("house_number")
                .IsRequired();
        });
        builder.ComplexProperty(p => p.PhoneNumberOwner, a =>
        {
            a.Property(x => x.Number)
                .IsRequired()
                .HasMaxLength(Constraints.LOW_VALUE_LENGTH)
                .HasColumnName("phone_number");
        });
        builder.HasMany(x => x.PetPhotos)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("pet_photos");
        builder.OwnsMany(x => x.PaymentDetailsList, p =>
        {
            p.ToJson("payment_details");
            p.Property(y => y.Name)
                .IsRequired()
                .HasColumnName("payment_details_name");
            p.Property(y => y.Description)
                .IsRequired()
                .HasColumnName("payment_details_description");
        });
    }
}