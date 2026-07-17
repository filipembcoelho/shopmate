using System.Data;
using Microsoft.Data.SqlClient;

namespace Rumos.Acd.AdoDemo;

public class UserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public User Create(User user)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                Console.WriteLine("Opening connection...");
                connection.Open();

                const string sql = "INSERT INTO Users (FirstName, LastName, IdCard, TaxNumber) OUTPUT INSERTED.Id VALUES (@FirstName, @LastName, @IdCard, @TaxNumber)";

                using var command = new SqlCommand(sql, connection);
                AddUserParameters(command, user);

                user.Id = Convert.ToInt32(command.ExecuteScalar());
                return user;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return null;
            }
            finally
            {
                connection.Close();
                Console.WriteLine($"Connection state after Close(): {connection.State}");
            }
        }
    }

    public User GetById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                Console.WriteLine("Opening connection...");
                connection.Open();

                const string sql = "SELECT Id, FirstName, LastName, IdCard, TaxNumber FROM Users WHERE Id = @Id";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                using var reader = command.ExecuteReader(CommandBehavior.SingleRow);

                if (!reader.Read())
                {
                    return null;
                }

                return ReadUser(reader);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return null;
            }
            finally
            {
                connection.Close();
                Console.WriteLine($"Connection state after Close(): {connection.State}");
            }
        }
    }

    public bool Update(User user)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                Console.WriteLine("Opening connection...");
                connection.Open();

                const string sql = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, IdCard = @IdCard, TaxNumber = @TaxNumber WHERE Id = @Id";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Id", SqlDbType.Int).Value = user.Id;
                AddUserParameters(command, user);

                return command.ExecuteNonQuery() == 1;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
            finally
            {
                connection.Close();
                Console.WriteLine($"Connection state after Close(): {connection.State}");
            }
        }
    }

    public bool Delete(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            try
            {
                Console.WriteLine("Opening connection...");
                connection.Open();

                const string sql = "DELETE FROM Users WHERE Id = @Id";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                return command.ExecuteNonQuery() == 1;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
            finally
            {
                connection.Close();
                Console.WriteLine($"Connection state after Close(): {connection.State}");
            }
        }
    }

    private static void AddUserParameters(SqlCommand command, User user)
    {
        command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 100).Value = user.FirstName;
        command.Parameters.Add("@LastName", SqlDbType.NVarChar, 100).Value = user.LastName;
        command.Parameters.Add("@IdCard", SqlDbType.NVarChar, 20).Value = user.IdCard;
        command.Parameters.Add("@TaxNumber", SqlDbType.NVarChar, 20).Value = user.TaxNumber;
    }

    private static User ReadUser(SqlDataReader reader)
    {
        return new User
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
            LastName = reader.GetString(reader.GetOrdinal("LastName")),
            IdCard = reader.GetString(reader.GetOrdinal("IdCard")),
            TaxNumber = reader.GetString(reader.GetOrdinal("TaxNumber"))
        };
    }
}
