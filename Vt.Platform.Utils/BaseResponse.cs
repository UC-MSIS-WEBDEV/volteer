using System;
using System.Text;
using Newtonsoft.Json;

namespace Vt.Platform.Utils
{
    public abstract class BaseResponse : ITraceable
    {
        protected BaseResponse()
        {
            MetaData = new ResponseMetaData();
        }

        [JsonIgnore]
        public Guid CorrelationId { get; set; }

        public ResponseMetaData MetaData { get; set; }

    }

    public static class BaseResponseExtensions
    {
        public static T WithStatus<T>(this T response, ResponseStatus status) where T : BaseResponse
        {
            response.MetaData.Status = status;
            return response;
        }
    }
}
