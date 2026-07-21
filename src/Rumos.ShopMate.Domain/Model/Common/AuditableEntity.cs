using Rumos.ShopMate.Domain.Interfaces;

namespace Rumos.ShopMate.Domain.Model.Common;

public class  AuditableEntity : Entity, IAuditable
{
    public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime Updated { get; set; }
}