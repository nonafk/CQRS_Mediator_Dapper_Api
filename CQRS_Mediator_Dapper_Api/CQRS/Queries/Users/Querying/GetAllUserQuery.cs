using CQRS_Mediator_Dapper_Api.Model;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CQRS_Mediator_Dapper_Api.CQRS.Queries.Users.Querying
{
    public class GetAllUserQuery : IRequest<IEnumerable<User>>
    {
        public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<User>>
        {
            private readonly IConfiguration _configuration;
            public GetAllUserQueryHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<IEnumerable<User>> Handle(GetAllUserQuery command, CancellationToken cancellationToken)
            {
                var sql = "select * from [dbo].[User]";
                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<User>(sql);
                    return result.ToList();
                }
            }
        }
    }
}
