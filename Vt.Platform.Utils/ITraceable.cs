using System;
using System.Collections.Generic;
using System.Text;

namespace Vt.Platform.Utils
{
    public interface ITraceable
    {
        Guid CorrelationId { get; set; }
    }
}
