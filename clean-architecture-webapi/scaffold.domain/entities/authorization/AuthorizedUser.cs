using System;
using System.Collections.Generic;
using System.Text;

namespace Scaffold.Domain.Entities.Authorization
{
    public class AuthorizedUser
    {
        public string? UserName { get; set; }
        public string? sub { get; set; }
        public string? jti { get; set; }

        public string? Name { get; set; }
        public string? email { get; set; }
        public string? roles { get; set; }
        public string? exp { get; set; }
        public string? iss { get; set; }
        public string? aud { get; set; }
    }
}