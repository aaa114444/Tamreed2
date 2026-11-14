
namespace Tamreed2.Domain.Common.Models;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; private set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    protected Entity(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Entity ID cannot be empty.",
                nameof(id));
        }

        Id = id;
    }

    public bool Equals(Entity? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return GetType() == other.GetType()
            && Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity entity && Equals(entity);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        return ReferenceEquals(left, right)
            || (left is not null && left.Equals(right));
    }

    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }
}
