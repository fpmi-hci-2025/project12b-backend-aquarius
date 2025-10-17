namespace Entities.Base;

public abstract class EntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    protected EntityBase()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
