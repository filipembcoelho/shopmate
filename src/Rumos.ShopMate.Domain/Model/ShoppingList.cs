using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Interfaces;
using Rumos.ShopMate.Domain.Model.Common;
using Rumos.ShopMate.Domain.Model.Enums;
using Rumos.ShopMate.Domain.Utils;

namespace Rumos.ShopMate.Domain.Model;

public class ShoppingList : AuditableEntity
{
    private readonly List<ShoppingListMember> _members;
    private readonly List<ShoppingListItem> _items;
    private readonly List<Activity> _activities;

    public string Name { get; set; }
    public User Owner { get; set; }
    public IReadOnlyList<ShoppingListMember> Members { get; set; }
    public IReadOnlyList<ShoppingListItem> Items { get; set; }
    public IReadOnlyList<Activity> Activities { get; set; }
    public DateTime ExpireDate { get; set; }
    public bool IsArchived { get; set; }

    private ShoppingList()
    {
        _members = new List<ShoppingListMember>();
        _items = new List<ShoppingListItem>();
        _activities = new List<Activity>();
        Members = _members.AsReadOnly();
        Items = _items.AsReadOnly();
        Activities = _activities.AsReadOnly();
    }

    public ShoppingList(string name, User owner) : this()
    {
        ValidateName(name);

        if (owner == null)
        {
            throw new InvalidShoppingListException("Owner is required.");
        }

        Name = name.Trim();
        Owner = owner;
        ExpireDate = DateTime.Today.AddDays(7);
        IsArchived = false;

        _members.Add(new ShoppingListMember(owner, ShoppingListRole.Owner));
    }

    public void Rename(string name, User requestedBy)
    {
        EnsureCanEdit(requestedBy);
        ValidateName(name);

        Name = name.Trim();
    }

    public ShoppingListItem AddItem(string name, int quantity, Unit unit, User requestedBy)
    {
        EnsureCanEdit(requestedBy);
        EnsureItemNameIsAvailable(name);

        var item = new ShoppingListItem(name, quantity, unit);
        _items.Add(item);
        AssignCategory(item);
        RecordActivity(requestedBy.Name + " added " + item.Name + ".");

        return item;
    }

    public ShoppingListItem AddItem(string name, int quantity, Unit unit, Category category, User requestedBy)
    {
        EnsureCanEdit(requestedBy);
        EnsureItemNameIsAvailable(name);
        EnsureCategoryIsValid(category);

        var item = new ShoppingListItem(name, quantity, unit, category);
        _items.Add(item);
        RecordActivity(requestedBy.Name + " added " + item.Name + ".");

        return item;
    }

    public void ChangeItemName(ShoppingListItem item, string name, User requestedBy)
    {
        EnsureCanEdit(requestedBy);
        EnsureItemBelongsToThisList(item);
        EnsureItemNameIsAvailable(name, item);

        item.ChangeName(name);
        RecordActivity(requestedBy.Name + " renamed an item to " + item.Name + ".");
    }

    public void ChangeItemQuantity(ShoppingListItem item, int quantity, Unit unit, User requestedBy)
    {
        EnsureCanEdit(requestedBy);
        EnsureItemBelongsToThisList(item);

        item.ChangeQuantity(quantity, unit);
        RecordActivity(requestedBy.Name + " changed quantity for " + item.Name + ".");
    }

    public void CompleteItem(ShoppingListItem item, User requestedBy)
    {
        EnsureCanEdit(requestedBy);
        EnsureItemBelongsToThisList(item);

        item.MarkAsCompleted();
        RecordActivity(requestedBy.Name + " completed " + item.Name + ".");
    }

    public void ReopenItem(ShoppingListItem item, User requestedBy)
    {
        EnsureCanEdit(requestedBy);
        EnsureItemBelongsToThisList(item);

        item.MarkAsPending();
        RecordActivity(requestedBy.Name + " reopened " + item.Name + ".");
    }

    public void RemoveItem(ShoppingListItem item, User requestedBy)
    {
        EnsureCanEdit(requestedBy);
        EnsureItemBelongsToThisList(item);

        _items.Remove(item);
        RecordActivity(requestedBy.Name + " removed " + item.Name + ".");
    }

    public void ShareWith(User user, ShoppingListRole role, User requestedBy)
    {
        EnsureActive();
        EnsureOwner(requestedBy);
        EnsureRoleIsValid(role);

        if (user == null)
        {
            throw new InvalidUserException("User is required.");
        }

        if (role == ShoppingListRole.Owner)
        {
            throw new PermissionDeniedException("Cannot add another owner.");
        }

        if (HasMember(user))
        {
            throw new DuplicateMemberException("User already has access to this list.");
        }

        _members.Add(new ShoppingListMember(user, role));
        RecordActivity(requestedBy.Name + " shared the list with " + user.Name + ".");
    }

    public void ChangeMemberRole(User user, ShoppingListRole role, User requestedBy)
    {
        EnsureActive();
        EnsureOwner(requestedBy);
        EnsureRoleIsValid(role);

        if (IsOwner(user))
        {
            throw new PermissionDeniedException("Owner role cannot be changed.");
        }

        var member = GetMember(user);
        member.ChangeRole(role);
        RecordActivity(requestedBy.Name + " changed " + user.Name + " to " + role + ".");
    }

    public void RemoveMember(User user, User requestedBy)
    {
        EnsureActive();
        EnsureOwner(requestedBy);

        if (IsOwner(user))
        {
            throw new PermissionDeniedException("Owner cannot be removed.");
        }

        var member = GetMember(user);
        _members.Remove(member);
        RecordActivity(requestedBy.Name + " removed " + user.Name + " from the list.");
    }

    public void Archive(User requestedBy)
    {
        EnsureOwner(requestedBy);

        IsArchived = true;
        RecordActivity(requestedBy.Name + " archived the list.");
    }

    public int CountCompletedItems()
    {
        var count = 0;

        foreach (var item in _items)
        {
            if (item.IsCompleted)
            {
                count++;
            }
        }

        return count;
    }

    public int CountPendingItems()
    {
        return _items.Count - CountCompletedItems();
    }

    public int GetProgressPercentage()
    {
        if (_items.Count == 0)
        {
            return 0;
        }

        return CountCompletedItems() * 100 / _items.Count;
    }

    public bool HasMember(User user)
    {
        if (user == null)
        {
            return false;
        }

        foreach (var member in _members)
        {
            if (member.User.Account.Username == user.Account.Username)
            {
                return true;
            }
        }

        return false;
    }

    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidShoppingListException("Shopping list name is required.");
        }

        if (name.Trim().Length < 2)
        {
            throw new InvalidShoppingListException("Shopping list name must be at least 2 characters long.");
        }

        if (name.Trim().Length > 60)
        {
            throw new InvalidShoppingListException("Shopping list name must be at most 60 characters long.");
        }
    }

    private void EnsureItemNameIsAvailable(string name)
    {
        EnsureItemNameIsAvailable(name, null);
    }

    private void EnsureItemNameIsAvailable(string name, ShoppingListItem ignoredItem)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidShoppingListItemException("Item name is required.");
        }

        foreach (var existingItem in _items)
        {
            if (existingItem == ignoredItem)
            {
                continue;
            }

            if (existingItem.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                throw new DuplicateItemException("This item already exists in the list.");
            }
        }
    }

    private void EnsureCanEdit(User user)
    {
        EnsureActive();

        var member = GetMember(user);

        if (member.Role == ShoppingListRole.Viewer)
        {
            throw new PermissionDeniedException("Viewer users cannot change the list.");
        }
    }

    private void EnsureOwner(User user)
    {
        var member = GetMember(user);

        if (member.Role != ShoppingListRole.Owner)
        {
            throw new PermissionDeniedException("Only the owner can perform this action.");
        }
    }

    private ShoppingListMember GetMember(User user)
    {
        if (user == null)
        {
            throw new InvalidUserException("User is required.");
        }

        foreach (var member in _members)
        {
            if (member.User.Account.Username == user.Account.Username)
            {
                return member;
            }
        }

        throw new MemberNotFoundException("User does not have access to this list.");
    }

    private void EnsureItemBelongsToThisList(ShoppingListItem item)
    {
        if (item == null)
        {
            throw new InvalidShoppingListItemException("Item is required.");
        }

        if (!_items.Contains(item))
        {
            throw new ItemNotFoundException("Item does not belong to this list.");
        }
    }

    private void EnsureActive()
    {
        if (IsArchived)
        {
            throw new ArchivedShoppingListException("Archived lists cannot be changed.");
        }
    }

    private void EnsureRoleIsValid(ShoppingListRole role)
    {
        if (!Enum.IsDefined(typeof(ShoppingListRole), role))
        {
            throw new InvalidShoppingListException("Invalid shopping list role.");
        }
    }

    private void EnsureCategoryIsValid(Category category)
    {
        if (category == null)
        {
            throw new InvalidShoppingListItemException("Category is required.");
        }
    }

    private bool IsOwner(User user)
    {
        if (user == null)
        {
            return false;
        }

        return Owner.Account.Username == user.Account.Username;
    }

    private void AssignCategory(ShoppingListItem item)
    {
        var category = CategoryUtils.GuessCategory(item.Name);

        item.ChangeCategory(category);
    }

    private void RecordActivity(string description)
    {
        _activities.Add(new Activity(description));
    }
}
