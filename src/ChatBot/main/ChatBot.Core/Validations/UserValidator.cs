using ChatBot.Core.Models;
using FluentValidation;

namespace ChatBot.Core.Validations
{
    public  class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.UserName).NotEmpty();
        }
    }
}
