using Scaffold.Application.Interfaces.Repositories;
using Scaffold.Application.Wrappers;
using Scaffold.Domain.Entities.Multitenancy;
using MediatR;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Scaffold.Application.Handlers;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.IO;

namespace Application.Features.Tokens.Queries
{
    public class GetTokenQuery_V1 : IRequest<ApiResponse<StringValues>>
    {
        public StringValues Token { get; set; }

        public class GetTokenQueryHandler : BaseRequestHandler, IRequestHandler<GetTokenQuery_V1, 
            ApiResponse<StringValues>>
        {
            public GetTokenQueryHandler(IUnitOfWork context,
                Scaffold.Application.Interfaces.Services.ITenantService TokenService) : base(context, TokenService) { }
            public async Task<ApiResponse<StringValues>> Handle(GetTokenQuery_V1 query, 
                CancellationToken cancellationToken)
            {
                var stream = query?.Token[0]?.Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = jsonToken as JwtSecurityToken;
                var roles = tokenS?.Claims.First(claim => claim.Type == "roles").Value;
                return await ApiResponse<StringValues>.SuccessAsync(roles, "");
            }
        }

        public class GetTokenQueryResponse
        {
            public int Id { get; set; }
            public string TokenConfig { get; set; }
            public GetTokenQueryResponse()
            {
                TokenConfig = string.Empty; Id = int.MinValue;
            }
        }
    }
}
