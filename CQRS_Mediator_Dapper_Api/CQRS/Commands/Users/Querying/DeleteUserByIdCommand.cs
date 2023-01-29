using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CQRS_Mediator_Dapper_Api.CQRS.Commands.Users.Querying
{
    public class DeleteUserByIdCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, bool>
        {
            private readonly IConfiguration _configuration;

            public DeleteUserByIdCommandHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }
            public async Task<bool> Handle(DeleteUserByIdCommand command, CancellationToken cancellationToken)
            {
                #region sql query string
                var sql = "DELETE FROM [dbo].[User] WHERE Id = @Id";
                #endregion

                #region option 1# 

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(sql, new { Id = command.Id });
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
                    var result = await connection.ExecuteAsync(sql, parameters);
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
                            var result = await connection.ExecuteAsync(sql, new { Id = command.Id }, transaction: transection);
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
