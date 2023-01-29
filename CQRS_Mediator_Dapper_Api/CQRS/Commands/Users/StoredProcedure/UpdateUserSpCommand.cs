using CQRS_Mediator_Dapper_Api.Model;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace CQRS_Mediator_Dapper_Api.CQRS.Commands.Users.StoredProcedure
{
    public class UpdateUserSpCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Level { get; set; }

        public class UpdateUserSpCommandHandler : IRequestHandler<UpdateUserSpCommand, bool>
        {
            private readonly IConfiguration _configuration;
            public UpdateUserSpCommandHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }
            public async Task<bool> Handle(UpdateUserSpCommand command, CancellationToken cancellationToken)
            {
                #region sql stored procedure
                var sql = "spUpdateUser";
                #endregion

                #region option 1# to use object ได้เลย

                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(sql, command, commandType: CommandType.StoredProcedure);
                    if (result > 0) return true;
                    else return false;
                }

                #endregion

                #region option 2# to map ข้อมูลลง object ใหม่
                /*
                var user = new User
                {
                    Id = command.Id,
                    Password = command.Password,
                    Level = command.Level
                };

                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(sql, user, commandType: CommandType.StoredProcedure);
                    if (result > 0) return true;
                    else return false;
                }
                */
                #endregion

                #region option 3# to use Dynamic Parameters
                /*
                DynamicParameters parameters = new();
                parameters.Add("Id", command.Id, DbType.Int32);
                parameters.Add("Password", command.Password, DbType.String);
                parameters.Add("Level", command.Level, DbType.String);

                //กรณีเราต้องการค่าเริ่มต้นให้เป็น null เมื่อไม่มีข้อมูลมา ในการ update ลง database
                //parameters.Add("Username", string.IsNullOrEmpty(command.Username) ? DBNull.Value : command.Username, DbType.String);

                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(sql.ToString(), parameters);
                    if (result > 0) return true;
                    else return false;
                }
                */
                #endregion

                #region option 4# to use transection
                /*
                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (IDbTransaction transection = connection.BeginTransaction())
                    {
                        try
                        {
                            var result = await connection.ExecuteAsync(sql.ToString(), command, transaction: transection);
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
