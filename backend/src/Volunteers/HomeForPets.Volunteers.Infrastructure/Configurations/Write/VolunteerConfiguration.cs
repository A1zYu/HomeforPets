﻿using HomeForPets.Core;
using HomeForPets.Core.Dtos.Volunteers;
using HomeForPets.Core.Extensions;
using HomeForPets.SharedKernel.Ids;
using HomeForPets.Volunteers.Domain;
using HomeForPets.Volunteers.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeForPets.Volunteers.Infrastucture.Configurations.Write;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value)
            );

        builder.ComplexProperty(v => v.Description, vb =>
        {
            vb.Property(x => x.Text)
                .HasColumnName("description")
                .IsRequired();
        });
        builder.ComplexProperty(v => v.PhoneNumber, vb =>
        {
            vb.Property(x => x.Number)
                .HasColumnName("phone_number")
                .IsRequired();
        });
        builder.ComplexProperty(v => v.YearsOfExperience, yb =>
        {
            yb.Property(x => x.Value)
                .IsRequired(false)
                .HasColumnName("years_of_experience")
                .IsRequired();
        });
        builder.ComplexProperty(x => x.FullName,
            f =>
            {
                f.Property(p => p.FirstName)
                    .HasMaxLength(Core.Constants.LOW_VALUE_LENGTH)
                    .HasColumnName("first_name")
                    .IsRequired();
                f.Property(p => p.LastName)
                    .HasMaxLength(Core.Constants.LOW_VALUE_LENGTH)
                    .HasColumnName("last_name")
                    .IsRequired();
                f.Property(p => p.MiddleName)
                    .HasMaxLength(Core.Constants.LOW_VALUE_LENGTH)
                    .HasColumnName("middle_name")
                    .IsRequired(false);
            });
        
        builder.Property(v => v.PaymentDetails)
            .ValueObjectCollectionJsonConversion(
                details => new PaymentDetailsDto{Description = details.Description,Name = details.Name},
                dto => PaymentDetails.Create(dto.Description, dto.Name).Value)
            .HasColumnName("payment_details");
        
        builder.Property(v => v.SocialNetworks)
            .ValueObjectCollectionJsonConversion(
                network=> new SocialNetworkDto{Name = network.Name,Path = network.Path},
                dto=> SocialNetwork.Create(dto.Name, dto.Path).Value)
            .HasColumnName("social_networks");
       
        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(v => v.Pets).AutoInclude();

        builder.Property("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}