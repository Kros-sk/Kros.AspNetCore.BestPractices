﻿using MediatR;

namespace Kros.Authorization.Api.Application.Commands
{
    /// <summary>
    /// Create user command.
    /// </summary>
    public class CreateUserCommand : IRequest<int>
    {
        /// <summary>
        /// First name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User's email.
        /// </summary>
        public string Email { get; set; }
    }
}