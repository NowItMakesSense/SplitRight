namespace SplitRight.Domain.Commom
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        public DateTimeOffset CreatedAt { get; protected set; }

        public DateTimeOffset? UpdatedAt { get; protected set; }

        public bool IsDeleted { get; protected set; }

        public DateTimeOffset? DeletedAt { get; private set; }

        protected BaseEntity(DateTimeOffset now)
        {
            Id = Guid.NewGuid();
            CreatedAt = now;
            IsDeleted = false;
        }

        protected BaseEntity() { } // EF

        protected void MarkAsUpdated(DateTimeOffset now)
        {
            UpdatedAt = now;
        }

        public void MarkAsDeleted(DateTimeOffset now)
        {
            if (IsDeleted) return;

            IsDeleted = true;
            DeletedAt = now;
            MarkAsUpdated(now);
        }
    }
}
