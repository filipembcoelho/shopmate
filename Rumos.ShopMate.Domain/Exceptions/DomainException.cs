namespace Rumos.ShopMate.Domain.Exceptions;

public class DomainException(string message) : Exception(message);

public class DuplicateMemberException(string message) : DomainException(message);