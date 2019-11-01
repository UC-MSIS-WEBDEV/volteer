using System;
using System.Collections.Generic;
using System.Text;

namespace Vt.Platform.Utils
{
    public abstract class BaseRequest : ITraceable
    {
        protected BaseRequest()
        {
            // set a default correlation ID which can be overridden by client
            CorrelationId = Guid.NewGuid();
        }
        public Guid CorrelationId { get; set; }
    }
}
