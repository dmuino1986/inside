using Inside.Domain.Enum;

namespace Inside.Domain.Core
{
    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
     //   public ObjectState ObjectState { get; set; }
    }
}