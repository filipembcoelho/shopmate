using Microsoft.AspNetCore.Identity;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.Services.Implementations;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<Account> _hasher = new();

    public string HashPassword(Account account, string password)
    {
        return _hasher.HashPassword(account, password);
    }

    public bool VerifyPassword(Account account, string password)
    {
        var result = _hasher.VerifyHashedPassword(account, account.PasswordHash, password);

        return result != PasswordVerificationResult.Failed;
    }
}