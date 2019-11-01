using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vt.Platform.Domain.Enums;
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.Messaging
{
    public interface IMessagePublisher
    {
        Task Publish(MessageTopic topic, ITraceable message, Guid correlationId);
    }
}
