using Rumos.ShopMate.Domain.Interfaces;

namespace Rumos.ShopMate.Domain.Model.Common;

public abstract class Entity : IEntity
{
    public int Id { get; set; }
}
