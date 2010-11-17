using System.ComponentModel.DataAnnotations;

namespace Infrastructure
{
    public interface IEntity
    {
        int Id { get; }
    }

    public abstract class Entity : IEntity
    {
        [Key]
        public virtual int Id { get; protected set; }
    }
}
