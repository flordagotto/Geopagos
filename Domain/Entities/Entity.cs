namespace Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id = Guid.NewGuid();
    }
}
