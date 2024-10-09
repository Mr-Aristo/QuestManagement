using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Quest.Domain.Entities;

namespace Quest.Application.Validations
{
    public class QuestValidator : AbstractValidator<Quests>
    {
        public QuestValidator()
        {
            RuleFor(quest => quest.Id).NotEmpty().WithMessage("Quest ID не может быть пустым.");
            RuleFor(quest => quest.RequiredProgress).GreaterThan(0).WithMessage("Quest progress requirement должно быть больше 0.");
            RuleFor(quest => quest.Title).MinimumLength(5).WithMessage("Quest title должно быть более 5 символов.");
        }
    }
}
