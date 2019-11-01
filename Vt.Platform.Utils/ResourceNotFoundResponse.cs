using System;

namespace Vt.Platform.Utils
{
    public class ResourceNotFoundResponse : BaseResponse
    {
        public ResourceNotFoundResponse()
        {
            MetaData.Status = ResponseStatus.ResourceNotFound;
        }
    }

    public class ExceptionResponse : BaseResponse
    {
        public ExceptionResponse()
        {
            
        }

        public ExceptionResponse(Exception ex, Guid correlationId)
        {
            CorrelationId = correlationId;
            MetaData.CorrelationId = correlationId;
            if (ex is TimeoutException)
            {
                MetaData.Status = ResponseStatus.TransientError;
            }
            else
            {
                MetaData.Status = ResponseStatus.PermanentError;
            }

            MetaData.Description = ex.ToString();
        }
    }
}