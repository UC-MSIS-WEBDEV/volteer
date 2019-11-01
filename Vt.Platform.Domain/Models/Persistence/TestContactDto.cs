using System;
using System.Collections.Generic;
using System.Text;
using Vt.Platform.Utils;

namespace Vt.Platform.Domain.Models.Persistence
{
    public class TestContactDto : BaseDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string EmailToken { get; set; }
        public string EmailMask { get; set; }
    }
}
