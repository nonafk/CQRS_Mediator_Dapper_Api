using CQRS_Mediator_Dapper_Api.Model;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CQRS_Mediator_Dapper_Api.CQRS.Queries.Users.Querying
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public int Id { get; set; }
        public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
        {
            private readonly IConfiguration _configuration;
            public GetUserByIdQueryHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<User> Handle(GetUserByIdQuery command, CancellationToken cancellationToken)
            {
                var sql = "select * from [dbo].[User] where Id = @Id";

                #region option 1# 
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = command.Id });
                    return result;
                }
                #endregion

                #region option 2# to use Dynamic Parameters
                /*
                DynamicParameters parameters = new();
                parameters.Add("Id", command.Id, DbType.Int32);

                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryFirstOrDefaultAsync<User>(sql, parameters);
                    return result;
                }
                */
                #endregion
            }
        }
    }
}
