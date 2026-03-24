namespace Invoice.Domain.Common;

public abstract class AuditableEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid(); // a unique identifier for the entity, generated automatically when a new instance is created
    public DateTime CreatedAt { get; private set; } // the date and time when the entity was created
    public DateTime? UpdatedAt { get; private set; } // the date and time when the entity was last updated, nullable because it may not have been updated yet
    public string? CreatedBy { get; private set; } // the identifier of the user who created the entity, nullable because it may not be set
    public string? UpdatedBy { get; private set; } // the identifier of the user

    public void SetCreatedBy(string userId)
    {
        CreatedBy = userId;
    }

    public void SetUpdatedBy(string userId)
    {
        UpdatedBy = userId;
        UpdatedAt = DateTime.UtcNow; // sets the update time to the current UTC time
    }

}

