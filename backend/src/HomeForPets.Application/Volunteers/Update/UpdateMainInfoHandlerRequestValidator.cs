﻿using FluentValidation;
using HomeForPets.Application.Dtos;
using HomeForPets.Application.Validation;
using HomeForPets.Application.Volunteers.CreateVolunteer;
using HomeForPets.Domain.Shared;
using HomeForPets.Domain.Shared.ValueObjects;
using HomeForPets.Domain.Volunteers;

namespace HomeForPets.Application.Volunteers.Update;

public class UpdateMainInfoHandlerRequestValidator : AbstractValidator<UpdateMainInfoRequest>
{
    public UpdateMainInfoHandlerRequestValidator()
    {
        RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
public class UpdateMainInfoHandlerDtoValidator : AbstractValidator<UpdateMainInfoDto>
{
    public UpdateMainInfoHandlerDtoValidator()
    {
        RuleFor(x => x.FullNameDto)
            .MustBeValueObject(x => 
                FullName.Create(x.FirstName, x.LastName, x.MiddleName));
        RuleFor(r => r.Description).MustBeValueObject(Description.Create);
        RuleFor(r => r.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        RuleFor(r => r.WorkExperience).MustBeValueObject(YearsOfExperience.Create);
    }
}