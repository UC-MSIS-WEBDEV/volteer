using System;
using System.Collections.Generic;
using System.Linq;

namespace Vt.Platform.Utils
{
    public class ResponseMetaData : ITraceable
    {
        public ResponseMetaData()
        {
            ResponseCreated = DateTime.UtcNow;
            Errors = new Dictionary<string, string[]>();
            Data = new Dictionary<string, string[]>();
        }
        public bool Success
        {
            get
            {
                var success = new[]
                {
                    ResponseStatus.Successful, 
                    ResponseStatus.Created,
                    ResponseStatus.FulfilledByExisting,
                    ResponseStatus.Queued
                };
                return success.Contains(Status);
            }
        }
        public ResponseStatus Status { get; set; }
        public DateTime ResponseCreated { get; }
        public Guid CorrelationId { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
        public Dictionary<string, string[]> Data { get; set; }
        public string Description { get; set; } = "";
    }
}