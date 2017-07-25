using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using drs_backend_phase1.Models.DTOs;
using FluentValidation;
using Microsoft.VisualBasic.ApplicationServices;

namespace drs_backend_phase1.Validation
{
    public class ProfileDTOValidator : AbstractValidator<ProfileDTO>
    {
        public ProfileDTOValidator()
        {
            //RuleFor(x => x.FirstName).NotEmpty().WithMessage("The First Name cannot be blank.")
            //    .Length(0, 100).WithMessage("The First Name cannot be more than 100 characters.");

            //RuleFor(x => x.LastName).NotEmpty().WithMessage("The Last Name cannot be blank.");

            //RuleFor(x => x.BirthDate).LessThan(DateTime.Today).WithMessage("You cannot enter a birth date in the future.");

            //RuleFor(x => x.Username).Length(8, 999).WithMessage("The user name must be at least 8 characters long.");
        }
    }
}