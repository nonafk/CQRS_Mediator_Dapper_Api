using CQRS_Mediator_Dapper_Api.Model;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CQRS_Mediator_Dapper_Api.CQRS.Queries.Users.StoredProcedure
{
    public class GetUserByIdSpQuery : IRequest<User>
    {
        public int Id { get; set; }
        public class GetUserByIdSpQueryHandler : IRequestHandler<GetUserByIdSpQuery, User>
        {
            private readonly IConfiguration _configuration;
            public GetUserByIdSpQueryHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<User> Handle(GetUserByIdSpQuery command, CancellationToken cancellationToken)
            {
                var sql = "spGetUserById";

                #region option 1# 
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = command.Id }, commandType: CommandType.StoredProcedure);
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
                    var result = await connection.QueryFirstOrDefaultAsync<User>(sql, parameters,commandType: CommandType.StoredProcedure);
                    return result;
                }
                */
                #endregion
            }
        }
    }
}
