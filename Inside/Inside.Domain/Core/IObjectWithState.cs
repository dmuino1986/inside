using Inside.Domain.Enum;

namespace Inside.Domain.Core
{
    public interface IObjectWithState
    {
        ObjectState ObjectState { get; set; }
    }
}