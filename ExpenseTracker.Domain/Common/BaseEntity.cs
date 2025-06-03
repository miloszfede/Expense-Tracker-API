namespace ExpenseTracker.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; private set; }
        public Guid ExternalId { get; private set; }

        public void SetId(int id)
        {
            Id = id;
        }

        public void SetExternalId(Guid externalId)
        {
            ExternalId = externalId;
        }
    }
}