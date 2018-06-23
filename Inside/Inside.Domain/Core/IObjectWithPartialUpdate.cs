using System.Collections.Generic;

namespace Inside.Domain.Core
{
    public interface IObjectWithPartialUpdate
    {
        IList<string> ModifiedProperties { get; set; }
    }
}