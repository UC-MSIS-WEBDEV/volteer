using System;
using System.Collections.Generic;
using System.Text;
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.Models.Messaging
{
    public abstract class BaseMessage : ITraceable
    {
        protected BaseMessage()
        {
            Created = DateTime.UtcNow;
        }

        public Guid CorrelationId { get; set; }
        public DateTime Created { get; set; }
    }
}
