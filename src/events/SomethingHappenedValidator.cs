using FluentValidation;

namespace src.events
{
    public class SomethingHappenedValidator : AbstractValidator<SomethingHappened>
    {
        public SomethingHappenedValidator()
        {
            RuleFor(pnc => pnc.Event).NotNull().NotEmpty();
        }
    }
}
