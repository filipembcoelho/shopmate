using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Services.Interfaces;

public interface IPasswordService
{
    string HashPassword(Account account, string password);
    bool VerifyPassword(Account account, string password);
}