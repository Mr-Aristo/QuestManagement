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
            RuleFor(quest => quest.Id).NotEmpty().WithMessage("Quest ID cannot be empty.");
            RuleFor(quest => quest.RequiredProgress).GreaterThan(0).WithMessage("Quest progress requirement must be greater than 0.");
            RuleFor(quest => quest.Title).MinimumLength(5).WithMessage("Quest title must be more than 5 character. ");
        }
    }
}
