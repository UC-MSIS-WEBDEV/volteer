using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Vt.Platform.Utils.Exceptions;
using Vt.Platform.Utils.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using Tokenize.Client;

namespace Vt.Platform.Utils
{
    public static class FunctionExtensions
    {
        static readonly JsonSerializerSettings Settings;
        static readonly JsonSerializerSettings SampleSettings;

        static FunctionExtensions()
        {
            Settings = new JsonSerializerSettings();
            Settings.Converters.Add(new EnumConverter());
            Settings.Converters.Add(new TokenPropertyConverter());
            Settings.Formatting = Formatting.Indented;
            Settings.NullValueHandling = NullValueHandling.Ignore;
            Settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            SampleSettings = new JsonSerializerSettings();
            SampleSettings.Converters.Add(new EnumConverter());
            SampleSettings.Converters.Add(new TokenPropertyConverter());
            SampleSettings.Formatting = Formatting.Indented;
            SampleSettings.NullValueHandling = NullValueHandling.Include;
            SampleSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public static async Task<HttpResponseMessage> CallService<TReq, TRes>(
            this HttpRequestMessage httpReq,
            Func<BaseService<TReq, TRes>> service,
            IObjectTokenizer objectTokenizer,
            params HttpMethod[] methods)
            where TReq : BaseRequest, new()
            where TRes : BaseResponse, new()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            methods = methods.Length > 0 ? methods : new[] { HttpMethod.Post };

            var svc = service();

            if (httpReq.Method == HttpMethod.Get)
            {
                var schemaResponse = CreateSchemaIfRequested<TReq, TRes>(httpReq, svc);
                if (schemaResponse != null)
                {
                    return schemaResponse;
                }
            }

            if (!methods.Contains(httpReq.Method))
            {
                return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed)
                {
                    Content = new StringContent(
                        $"Method {httpReq.Method} not allowed for {httpReq.RequestUri.PathAndQuery}",
                        Encoding.UTF8,
                        "text/text")
                };
            }

            var req = await httpReq.ReadAsJsonAsync<TReq>();
            svc.Logger.LogInformation($"START: {svc.ServiceName} {req.CorrelationId}");

            Guid correlationId = req.CorrelationId;

            try
            {
                await objectTokenizer.TokenizeObject(req, req.CorrelationId);
            }
            catch (Exception ex)
            {
                svc.Logger.LogError(ex, "There was a problem tokenizing the request");

                return CreateResponse(
                    correlationId,
                    HttpStatusCode.InternalServerError,
                    new ExceptionResponse(ex, correlationId), sw);
            }

            try
            {
                var result = await svc.Run(req);
                svc.Logger.LogInformation($"END: {svc.ServiceName} {req.CorrelationId}");

                // Generate HTTP Response
                var httpStatus = ConvertServiceResponseToHttpStatusCode(result);
                svc.Logger.LogInformation($"HTTP STATUS CODE: {svc.ServiceName} {req.CorrelationId}");

                return CreateResponse(correlationId, httpStatus, result, sw);
            }
            catch (ResourceNotFoundException)
            {
                svc.Logger.LogInformation($"Resource not found for {httpReq.RequestUri}");
                return CreateResponse(correlationId, HttpStatusCode.NotFound, new ResourceNotFoundResponse(), sw);
            }
            catch (Exception ex)
            {
                svc.Logger.LogError(ex, "There was a problem processing the request");
                return CreateResponse(
                    correlationId,
                    HttpStatusCode.InternalServerError,
                    new ExceptionResponse(ex, correlationId), sw);
            }
        }

        private static HttpStatusCode ConvertServiceResponseToHttpStatusCode(BaseResponse response)
        {
            if (response.MetaData == null)
            {
                throw new Exception("Service response with no metadata");
            }

            switch (response.MetaData.Status)
            {
                case ResponseStatus.Created:
                    return HttpStatusCode.Created;
                case ResponseStatus.Successful:
                    return HttpStatusCode.OK;
                case ResponseStatus.FulfilledByExisting:
                    return HttpStatusCode.OK;
                case ResponseStatus.Queued:
                    return HttpStatusCode.Accepted;
                case ResponseStatus.ValidationError:
                    return HttpStatusCode.BadRequest;
                case ResponseStatus.AuthenticationError:
                    return HttpStatusCode.Forbidden;
                case ResponseStatus.UpstreamTimeoutError:
                    return HttpStatusCode.GatewayTimeout;
                case ResponseStatus.TransientError:
                    return HttpStatusCode.ServiceUnavailable;
                case ResponseStatus.PermanentError:
                    return HttpStatusCode.InternalServerError;
                case ResponseStatus.ResourceNotFound:
                    return HttpStatusCode.NotFound;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static HttpResponseMessage CreateSchemaIfRequested<TReq, TRes>(HttpRequestMessage httpReq, BaseService<TReq, TRes> svc)
            where TReq : BaseRequest, new()
            where TRes : BaseResponse, new()
        {
            if (!httpReq.RequestUri.Query.StartsWith("?schema", StringComparison.InvariantCultureIgnoreCase))
                return null;

            if (httpReq.RequestUri.Query.Equals("?schema=request&format=json", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(new TReq(), SampleSettings),
                        Encoding.UTF8,
                        "application/json")
                };
            }

            if (httpReq.RequestUri.Query.Equals("?schema=request", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        JsonSchema.FromType<TReq>().ToJson(Formatting.Indented),
                        Encoding.UTF8,
                        "application/json")
                };
            }


            if (httpReq.RequestUri.Query.Equals("?schema=response&format=json", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(new TRes(), SampleSettings),
                        Encoding.UTF8,
                        "application/json")
                };
            }

            if (httpReq.RequestUri.Query.Equals("?schema=response", StringComparison.InvariantCultureIgnoreCase))
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        JsonSchema.FromType<TRes>().ToJson(Formatting.Indented),
                        Encoding.UTF8,
                        "application/json")
                };
            }

            if (httpReq.RequestUri.Query.Equals("?schema=errors", StringComparison.InvariantCultureIgnoreCase))
            {
                IDictionary<int, string> errorCodes = new Dictionary<int, string>();
                bool errors = false;
                try
                {
                    errorCodes = svc.GetErrorCodes();
                }
                catch (Exception ex)
                {
                    svc.Logger.LogError(ex, $"Error writing error codes for {svc.ServiceName}");
                    errors = true;
                }

                return new HttpResponseMessage(!errors ? HttpStatusCode.OK : HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(errorCodes, Settings),
                        Encoding.UTF8,
                        "application/json")
                };
            }


            return new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(
                    "No schema resource found matching this URI",
                    Encoding.UTF8,
                    "text/text")
            };

        }

        private static HttpResponseMessage CreateResponse(Guid correlationId, HttpStatusCode httpStatus, BaseResponse serviceResponse, Stopwatch sw)
        {
            var response = new HttpResponseMessage(httpStatus);
            response.Headers.Add("correlation-id", correlationId.ToString());

            serviceResponse.MetaData.Duration = sw.ElapsedMilliseconds;
            sw.Stop();

            response.Content = new StringContent(
                JsonConvert.SerializeObject(serviceResponse, Settings),
                Encoding.UTF8,
                "application/json");

            return response;
        }
    }
}
