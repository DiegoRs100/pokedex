﻿namespace Pokedex.Business.Core.Auth
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}