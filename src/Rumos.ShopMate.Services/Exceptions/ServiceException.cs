namespace Rumos.ShopMate.Services.Exceptions;

public class ServiceException(string message) : Exception(message);

public class ServiceNotFoundException(string message) : Exception(message);