using Azure.Core;
using CQRS_Mediator_Dapper_Api.Model;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;
using System.Text;

namespace CQRS_Mediator_Dapper_Api.CQRS.Commands.Users.Querying
{
    public class CreateUserCommand : IRequest<bool>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Level { get; set; }


        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
        {
            private readonly IConfiguration _configuration;

            public CreateUserCommandHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }


            public async Task<bool> Handle(CreateUserCommand command, CancellationToken cancellationToken)
            {

                #region sql query string

                StringBuilder sql = new();
                sql.Append("Insert into [dbo].[User] (Username,Password,Level,CreatedAt,UpdatedAt) ");
                sql.Append("VALUES (@Username,@Password,@Level,GETDATE(),GETDATE())");

                #endregion

                #region option 1# to use object ได้เลย

                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(sql.ToString(), command);
                    if (result > 0) return true;
                    else return false;
                }

                #endregion

                #region option 2# to map ข้อมูลลง object ใหม่
                /*
                var user = new User
                {
                    Username = command.Username,
                    Password = command.Password,
                    Level = command.Level,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(sql.ToString(), user);
                    if (result > 0) return true;
                    else return false;
                }
                */
                #endregion

                #region option 3# to use Dynamic Parameters
                /*
                DynamicParameters parameters = new();
                parameters.Add("Username", command.Username, DbType.String);
                parameters.Add("Password", command.Password, DbType.String);
                parameters.Add("Level", command.Level, DbType.String);
                parameters.Add("CreatedAt", DateTime.Now, DbType.DateTime);
                parameters.Add("UpdatedAt", DateTime.Now, DbType.DateTime);

                //กรณีเราต้องการค่าเริ่มต้นให้เป็น null เมื่อไม่มีข้อมูลมา ในการ insert ลง database
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
