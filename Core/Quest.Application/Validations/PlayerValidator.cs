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
            RuleFor(player => player.Id).NotEmpty().WithMessage("Идентификатор игрока не может быть пустым");
            RuleFor(player => player.ExperiencePoints).GreaterThanOrEqualTo(0).WithMessage("Количество очков опыта должно быть неотрицательным.");
            RuleFor(player => player.Currency).GreaterThanOrEqualTo(0).WithMessage("Currency должна быть неотрицательным числом.");
            RuleFor(player => player.PlayerItems).NotNull().WithMessage("Player должен быть инвентарь.");
            RuleFor(player => player.Name).MinimumLength(5).WithMessage("Player имя должно содержать более 5 символо.");
        }
    }
}
