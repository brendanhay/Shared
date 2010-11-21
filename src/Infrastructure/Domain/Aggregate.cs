namespace Infrastructure.Domain
{
    public interface IAggregate : IEntity { }

    public abstract class Aggregate : Entity, IAggregate
    {

    }
}
