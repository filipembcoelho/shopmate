namespace Rumos.ShopMate.Services.Exceptions;

public class ServiceException(string message) : Exception(message);

public class ServiceNotFoundException(string message) : ServiceException(message);

public class InvalidCredentialsException(string message) : ServiceException(message);
