namespace Rumos.ShopMate.Domain.Exceptions;

public class DomainException(string message) : Exception(message);

public class InvalidUserException(string message) : DomainException(message);

public class InvalidAccountException(string message) : DomainException(message);

public class InvalidNameException(string message) : DomainException(message);

public class InvalidShoppingListException(string message) : DomainException(message);

public class InvalidShoppingListItemException(string message) : DomainException(message);

public class DuplicateMemberException(string message) : DomainException(message);

public class DuplicateItemException(string message) : DomainException(message);

public class MemberNotFoundException(string message) : DomainException(message);

public class ItemNotFoundException(string message) : DomainException(message);

public class PermissionDeniedException(string message) : DomainException(message);

public class ArchivedShoppingListException(string message) : DomainException(message);
