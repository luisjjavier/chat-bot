using ChatBot.Core.Models;
using FluentValidation;

namespace ChatBot.Core.Validations
{
    public  class RoomValidator: AbstractValidator<Room>
    {
        public RoomValidator()
        {
            RuleFor(room =>  room.Code).NotNull();
            RuleFor(room => room.Name).NotEmpty();
        }
    }
}
