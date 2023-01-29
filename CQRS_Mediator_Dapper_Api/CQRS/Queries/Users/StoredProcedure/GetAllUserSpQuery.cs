using CQRS_Mediator_Dapper_Api.Model;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CQRS_Mediator_Dapper_Api.CQRS.Queries.Users.StoredProcedure
{
    public class GetAllUserSpQuery : IRequest<IEnumerable<User>>
    {
        public class GetAllUserSpQueryHandler : IRequestHandler<GetAllUserSpQuery, IEnumerable<User>>
        {
            private readonly IConfiguration _configuration;
            public GetAllUserSpQueryHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<IEnumerable<User>> Handle(GetAllUserSpQuery command, CancellationToken cancellationToken)
            {
                var sql = "spGetAllUser";
                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<User>(sql,commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
        }
    }
}
