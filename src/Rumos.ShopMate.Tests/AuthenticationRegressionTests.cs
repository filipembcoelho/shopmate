using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Constants;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Implementations;

namespace Rumos.ShopMate.Tests;

public static class AuthenticationRegressionTests
{
    public static int Run()
    {
        var failed = 0;
        Check("Account construction does not retain the password", () =>
        {
            var account = new Account("student", "Classroom1!");
            Require(account.PasswordHash == string.Empty, "Expected an empty hash until registration hashes the password.");
        });
        Check("New accounts start with the User role", () =>
            Require(new Account("student", "Classroom1!").Role == AppRoles.User, "Expected the User role."));
        Check("Missing stored hashes reject login attempts", () =>
        {
            var account = new Account("student", "Classroom1!") { PasswordHash = null! };
            Require(!new PasswordService().VerifyPassword(account, "Classroom1!"), "A missing hash cannot authenticate.");
        });
        Check("Successful login persists its upgraded hash", () =>
        {
            var name = Guid.NewGuid().ToString();
            string previous;
            using (var database = Database(name))
            {
                new AuthenticationService(database, new PasswordService()).Register(new RegisterUserDto
                {
                    FullName = "Class Student", Username = "student", Password = "Classroom1!"
                });
                previous = database.Accounts.Single().PasswordHash;
            }
            using (var database = Database(name))
                new AuthenticationService(database, new PasswordService()).Login("student", "Classroom1!");
            using (var database = Database(name))
            {
                var account = database.Accounts.Single();
                Require(account.PasswordHash != previous, "The new salted hash must be persisted, not left on a detached entity.");
                Require(new PasswordService().VerifyPassword(account, "Classroom1!"), "The persisted hash must still verify.");
            }
        });
        Check("Token service accepts the classroom's fifteen-minute lifetime", () =>
        {
            var token = new TokenService(Settings(15)).GenerateToken(User());
            var decoded = new JwtSecurityTokenHandler().ReadJwtToken(token.AccessToken);
            Require(decoded.Subject == "42", "The token must represent the application user ID.");
            Require(decoded.Claims.Single(c => c.Type == "role").Value == AppRoles.User, "The server-selected role must be retained.");
            Require(Math.Abs((token.ExpiresAtUtc - DateTime.UtcNow).TotalMinutes - 15) < 0.1, "Expected fifteen minutes, not seconds or hours.");
        });
        Check("Token service rejects non-positive lifetimes", () =>
        {
            foreach (var minutes in new[] { 0, -1 })
            {
                var rejected = false;
                try { new TokenService(Settings(minutes)).GenerateToken(User()); }
                catch (ArgumentException) { rejected = true; }
                Require(rejected, "A non-positive token lifetime must be rejected.");
            }
        });
        Check("EF model supplies the User role default", () =>
        {
            using var database = Database(Guid.NewGuid().ToString());
            var role = database.Model.FindEntityType(typeof(Account))!.FindProperty(nameof(Account.Role))!;
            Require(Equals(role.GetDefaultValue(), AppRoles.User), "The database role default must match the domain default.");
        });
        return failed;

        void Check(string name, Action test)
        {
            try { test(); Console.WriteLine("[PASS] " + name); }
            catch (Exception ex) { failed++; Console.WriteLine("[FAIL] " + name + ": " + ex.Message); }
        }
    }

    private static ApplicationContext Database(string name) => new(
        new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(name).Options);

    private static UserDto User() => new()
    {
        Id = 42, FullName = "Class Student", Username = "student", Role = AppRoles.User
    };

    private static IConfiguration Settings(int minutes) => new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["JwtSettings:Key"] = Convert.ToBase64String(new byte[32]),
            ["JwtSettings:Issuer"] = "ShopMate.Test",
            ["JwtSettings:Audience"] = "ShopMate.TestApi",
            ["JwtSettings:ExpirationInMinutes"] = minutes.ToString()
        }).Build();

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }
}
