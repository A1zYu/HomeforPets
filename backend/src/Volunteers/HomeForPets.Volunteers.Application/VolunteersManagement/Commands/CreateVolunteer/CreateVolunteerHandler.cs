﻿using CSharpFunctionalExtensions;
using FluentValidation;
using HomeForPets.Core;
using HomeForPets.Core.Abstactions;
using HomeForPets.Core.Extensions;
using HomeForPets.SharedKernel;
using HomeForPets.SharedKernel.Ids;
using HomeForPets.Volunteers.Domain;
using HomeForPets.Volunteers.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HomeForPets.Volunteers.Application.VolunteersManagement.Commands.CreateVolunteer;

public class CreateVolunteerHandler : ICommandHandler<Guid,CreateVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateVolunteerCommand> _validator;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository, ILogger<CreateVolunteerHandler> logger, IUnitOfWork unitOfWork, IValidator<CreateVolunteerCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validatorResult.IsValid == false)
        {
            return validatorResult.ToErrorList();
        }
        
        var volunteerId = VolunteerId.NewId();
        
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        
        var fullname = FullName.Create(command.FullNameDto.FirstName, command.FullNameDto.LastName,
            command.FullNameDto.MiddleName).Value;
        
        var description = Description.Create(command.Description).Value;
        
        var yearsOfExperience = YearsOfExperience.Create(command.WorkExperience).Value;
        
        var existVolunteerByPhone = await _volunteersRepository.GetByPhoneNumber(phoneNumber);
        if (existVolunteerByPhone is not null)
            return Errors.Volunteer.AlreadyExist().ToErrorList();
        
        var volunteer = Volunteer.Create(
            volunteerId,
            fullname,
            phoneNumber,
            description,
            yearsOfExperience);
        
        if (volunteer.IsFailure)
        {
            return volunteer.Error.ToErrorList();
        }

        if (command.PaymentDetails.Any())
        {
            var paymentDetails = command.PaymentDetails
                .Select(x=>PaymentDetails
                    .Create(x.Name, x.Description).Value).ToList();
            volunteer.Value.AddPaymentDetails(paymentDetails);
        }

        if (command.SocialNetworks.Any())
        {
            var socialNetwork = command.SocialNetworks
                .Select(x=>SocialNetwork.Create(x.Name, x.Path).Value);
            volunteer.Value.AddSocialNetworks(socialNetwork);
        }
        
        await _volunteersRepository.Add(volunteer.Value, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Create volunteer : {volunteerId} ", volunteerId.Value); 
        
        return volunteerId.Value;
    }
}