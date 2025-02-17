﻿using HomeForPets.Core.Abstactions;
using HomeForPets.Core.Dtos.Volunteers;

namespace HomeForPets.Volunteers.Application.VolunteersManagement.Commands.UpdatePaymentDetails;

public record UpdatePaymentDetailsCommand(Guid VolunteerId,IEnumerable<PaymentDetailsDto> PaymentDetailsDto) : ICommand;