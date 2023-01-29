using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CQRS_Mediator_Dapper_Api.CQRS.Commands.Users.StoredProcedure
{
    public class DeleteUserByIdSpCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public class DeleteUserByIdSpCommandHandler : IRequestHandler<DeleteUserByIdSpCommand, bool>
        {
            private readonly IConfiguration _configuration;

            public DeleteUserByIdSpCommandHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }
            public async Task<bool> Handle(DeleteUserByIdSpCommand command, CancellationToken cancellationToken)
            {
                #region sql stored procedure
                var sql = "spDeleteUser";
                #endregion

                #region option 1# 

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(sql, new { Id = command.Id }, commandType: CommandType.StoredProcedure);
                    if (result > 0) return true;
                    else return false;
                }

                #endregion

                #region option 2# to use Dynamic Parameters
                /*
                DynamicParameters parameters = new();
                parameters.Add("Id", command.Id, DbType.Int32);

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                    if (result > 0) return true;
                    else return false;
                }
                */
                #endregion

                #region option 3# to use transection
                /*
                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (IDbTransaction transection = connection.BeginTransaction())
                    {
                        try
                        {
                            var result = await connection.ExecuteAsync(sql, new { Id = command.Id }, transaction: transection, commandType: CommandType.StoredProcedure);
                            if (result > 0)
                            {
                                transection.Commit();
                                return true;
                            }
                            else
                            {
                                transection.Rollback();
                                return false;
                            }
                        }
                        catch (Exception)
                        {
                            transection.Rollback();
                            return false;
                        }
                        finally
                        {
                            if (connection is not null)
                            {
                                transection.Dispose();
                                connection.Close();
                            }
                        }
                    }
                }
                */
                #endregion
            }
        }
    }
}
