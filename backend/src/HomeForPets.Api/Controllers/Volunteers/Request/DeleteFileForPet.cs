﻿using HomeForPets.Application.VolunteersManagement.Commands.DeletePhotoFromPet;

namespace HomeForPets.Api.Controllers.Volunteers.Request;

public record DeleteFileForPet(Guid PetId, Guid PhotoId)
{
    public DeleteFileFromPetCommand ToCommand(Guid volunteerId) => new(volunteerId, PetId, PhotoId);
}