using System;
using MediatR;
using System.Collections.Generic;
using System.Text;
using Scaffold.Domain.Entities;
using Scaffold.Application.Wrappers;
using System.Threading.Tasks;
using System.Threading;
using Scaffold.Application.Interfaces.Repositories;
using Scaffold.Application.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Scaffold.Application.Entities.Jwt;
using Microsoft.Extensions.Options;
using Scaffold.Application.Handlers;

namespace Scaffold.Application.Features.Token.Commands
{
    public class TokenGenerationCommand_V1 :
        IRequest<ApiResponse<TokenGenerationCommand_V1.TokenGenerationResponse_V1>>
    {

        public TokenGenerationRequest_V1 Input { get; set; }

        public class TokenGenerationCommandHandler : BaseRequestHandler,
            IRequestHandler<TokenGenerationCommand_V1, ApiResponse<TokenGenerationResponse_V1>>
        {
            public TokenGenerationCommandHandler(IUnitOfWork uow, 
                ITenantService service,
                IOptions<Domain.Settings.JWTSettings> jwt) : base(uow, service, true)
            {
                _jwt = jwt.Value;
            }

            public Domain.Settings.JWTSettings _jwt { get; set; }

            public async Task<ApiResponse<TokenGenerationResponse_V1>> Handle(TokenGenerationCommand_V1 request, CancellationToken cancellationToken)
            {
                return await GetTokenAsync(request.Input, _jwt);
            }

            public async Task<ApiResponse<TokenGenerationResponse_V1>> GetTokenAsync(TokenGenerationRequest_V1 model, 
                Domain.Settings.JWTSettings jwt)
            {
                var tokenIdentity = new TokenGenerationResponse_V1();
                //var user = await _userManager.FindByEmailAsync(model.Email);

                var user = (from u in dbContext.Set<User>()
                            where  u.Email.ToLower() == model.Email.ToLower()
                            select u).FirstOrDefault();

                if (user == null)
                {
                    tokenIdentity.IsAuthenticated = false;
                    tokenIdentity.Message = $"No Accounts Registered with {model.Email}.";
                    return await ApiResponse<TokenGenerationResponse_V1>.FailAsync(tokenIdentity.Message);
                }
                else
                {
                    var userIdentity = (from u in dbContext.Set<User>()
                                        where u.Email.ToLower() == model.Email.ToLower()
                                        && u.Firstname.ToLower() == model.FirstName.ToLower()
                                        && u.Lastname.ToLower() == model.LastName.ToLower()
                                        select u).FirstOrDefault();
                    if (userIdentity != null)
                    {
                        tokenIdentity.IsAuthenticated = true;
                        JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user, jwt);
                        tokenIdentity.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                        tokenIdentity.Email = user.Email;
                        //tokenIdentity.UserName = user.Email;
                        //var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                        //TokenGenerationResponse_V1.Roles = rolesList.ToList();
                        return await ApiResponse<TokenGenerationResponse_V1>.SuccessAsync(tokenIdentity, "");
                    }
                    else
                    {
                        tokenIdentity.IsAuthenticated = false;
                        tokenIdentity.Message = $"Incorrect Credentials for user {user.Email}.";
                        return await ApiResponse<TokenGenerationResponse_V1>.FailAsync(tokenIdentity.Message);

                    }
                }
            }
            private async Task<JwtSecurityToken> CreateJwtToken(User user, 
                Domain.Settings.JWTSettings _jwt)
            {
                // var userClaims = await _userManager.GetClaimsAsync(user);
                //var roles = await _userManager.GetRolesAsync(user);

                var roleId = (from u in dbContext.Set<User>()
                             join ur in dbContext.Set<UserRoleMapping>()
                                 on u.Id equals ur.UserId
                              where u.Email.ToLower() == user.Email.ToLower()
                              select ur.RoleId).FirstOrDefault();

                var roleName = (from r in dbContext.Set<Role>()
                                where r.Id == roleId
                                select r.Name).FirstOrDefault();

                var roleClaims = new List<Claim>();

                roleClaims.Add(new Claim("roles", roleName));

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, $"{user.Firstname} {user.Lastname}"),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                }
                
                .Union(roleClaims);

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwt.Issuer,
                    audience: _jwt.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                    signingCredentials: signingCredentials);
                return await Task.FromResult(jwtSecurityToken);
            }

        }


        public class TokenGenerationRequest_V1
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class TokenGenerationResponse_V1
        {
            public string Message { get; set; }
            public bool IsAuthenticated { get; set; }
        //    public string UserName { get; set; }
            public string Email { get; set; }
          //  public List<string> Roles { get; set; }
            public string Token { get; set; }
        }
    }
}
