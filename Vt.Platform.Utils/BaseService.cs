using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vt.Platform.Utils.Exceptions;
using Microsoft.Extensions.Logging;

namespace Vt.Platform.Utils
{
    public abstract class BaseService<TReq, TRes> : IBaseService where TReq : BaseRequest where TRes : BaseResponse, new()
    {
        protected BaseService(ILogger logger)
        {
            Logger = logger;
        }
        public ILogger Logger { get; }
        protected abstract Task<TRes> Implementation(TReq request);
        
        public async Task<TRes> RunHandler(TReq request)
        {
            Logger.LogInformation($"Start Service={this.ServiceName}, CorrelationId={request.CorrelationId}");
            Logger.LogInformation(ObjectLogSerializer.Serialize(request));

            try
            {
                var context = new ValidationContext(request, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(request, context, validationResults, true);
                if (!isValid)
                {
                    var response = new TRes().WithStatus(ResponseStatus.ValidationError);
                    var errs = new Dictionary<string, List<string>>();
                    foreach (var error in validationResults)
                    {
                        foreach (var member in error.MemberNames)
                        {
                            if (!errs.ContainsKey(member))
                            {
                                errs[member] = new List<string>();
                            }
                            errs[member].Add(error.ErrorMessage);
                        }
                    }

                    response.MetaData.Errors = errs.ToDictionary(x => x.Key, y => y.Value.ToArray());
                    return response;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error validating request for service={this.ServiceName}, CorrelationId={request.CorrelationId}");
                var errorResponse = new TRes
                {
                    MetaData = new ResponseMetaData
                    {
                        Status = ResponseStatus.PermanentError,
                        Description = ex.Message,
                    }
                };
                return errorResponse;
            }

            try
            {
                var result = await Implementation(request);
                Logger.LogInformation(ObjectLogSerializer.Serialize(result));
                Logger.LogInformation($"Completed Service={this.ServiceName}, CorrelationId={request.CorrelationId}");
                return result;
            }
            catch (ResourceNotFoundException)
            {
                Logger.LogInformation($"Resource not found calling Service={this.ServiceName}, CorrelationId={request.CorrelationId}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error calling service={this.ServiceName}, CorrelationId={request.CorrelationId}");
                var errorResponse = new TRes
                {
                    MetaData = new ResponseMetaData
                    {
                        Status = ResponseStatus.PermanentError,
                        Description = ex.Message,
                    }
                };
                return errorResponse;
            }
        }

        public async Task<TRes> Run(TReq request)
        {
            request.SyncCorrelationIds();
            var response = await RunHandler(request);
            response.SyncCorrelationIds(request.CorrelationId);
            return response;
        }

        public TRes ErrorCodeResponse(int errorCode)
        {
            var errors = GetErrorCodes();
            var res = new TRes
            {
                MetaData =
                {
                    Status = ResponseStatus.ValidationError
                }
            };
            if (errors != null && errors.ContainsKey(errorCode))
            {
                res.MetaData.Errors.Add(errorCode.ToString(), new[] { errors[errorCode] });
            }
            else
            {
                res.MetaData.Errors.Add(errorCode.ToString(), new[] { "Undefined error code" });
            }

            return res;

        }

        public abstract IDictionary<int, string> GetErrorCodes();

        public string ServiceName => GetType().Name;
    }
}