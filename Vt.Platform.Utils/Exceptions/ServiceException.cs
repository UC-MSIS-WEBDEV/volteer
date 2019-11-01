using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Vt.Platform.Utils.Exceptions
{
    [Serializable]
    public class ServiceException : Exception
    {
        public ServiceException()
        {
        }

        public ServiceException(IBaseService service, BaseRequest request, string message) : this($"{message} {service?.ServiceName ?? "UnknownService"} Request:{request?.CorrelationId.ToString() ?? "Unknown CorrelationId"}")
        {

        }

        public ServiceException(IBaseService service, BaseRequest request, Exception ex) : this($"{service?.ServiceName ?? "UnknownService"} Request:{request?.CorrelationId.ToString() ?? "Unknown CorrelationId"}", ex)
        {
            
        }

        public ServiceException(string message) : base(message)
        {
        }

        public ServiceException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ServiceException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
