﻿using ChatBot.Core.Models;
using ChatBot.Core.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Triplex.Validations;

namespace ChatBot.Core.Services
{
    public sealed class UserService: IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task RegisterAUser(User user, string password)
        {
            Arguments.NotNull(user, nameof(user));
            Arguments.NotNull(password, nameof(password));

          var result =  await _userManager.CreateAsync(user, password);

          if (!result.Succeeded)
          {
              throw new InvalidOperationException(
                  string.Join(",", result.Errors.Select( e=> e.Description)));
          }
        }
    }
}