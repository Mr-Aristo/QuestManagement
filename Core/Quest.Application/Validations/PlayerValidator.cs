using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Quest.Domain.Entities;

namespace Quest.Application.Validations
{
    public class PlayerValidator : AbstractValidator<Player>
    {
        public PlayerValidator()
        {
            RuleFor(player => player.Id).NotEmpty().WithMessage("Player ID cannot be empty.");
            RuleFor(player => player.ExperiencePoints).GreaterThanOrEqualTo(0).WithMessage("Experience points must be a non-negative number.");
            RuleFor(player => player.Currency).GreaterThanOrEqualTo(0).WithMessage("Currency must be a non-negative number.");
            RuleFor(player => player.PlayerItems).NotNull().WithMessage("Player must have an inventory.");
        }
    }
}
