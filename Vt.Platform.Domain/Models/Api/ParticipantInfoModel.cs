using System;
using System.Collections.Generic;
using System.Text;
using Vt.Platform.Domain.Enums;

namespace Vt.Platform.Domain.Models.Api
{
    public class ParticipantDisplayInfoModel
    {
        public string Name { get; set; }
        public ParticipantEventStatus Status { get; set; }
    }
}
