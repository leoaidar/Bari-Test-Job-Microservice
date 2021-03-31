using System;

namespace Bari.Test.Job.Domain.Entities
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Entity()
        {
            Id = Guid.NewGuid();
            CreateDate = LastUpdateDate = DateTime.Now;
        }

        public Guid Id { get; private set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool Invalid { get; set; }

        public abstract void Validate();

        public bool Equals(Entity other)
        {
            return Id == other.Id;
        }
        public void EntityModified()
        {
            LastUpdateDate = DateTime.Now;
        }
    }
}
