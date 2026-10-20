namespace Rumos.ShopMate.Services.Interfaces;

public interface IUserResolverService
{
    int GetById();
    string? GetFullName();
    string? GetUserName();
    string? GetRole();
}
