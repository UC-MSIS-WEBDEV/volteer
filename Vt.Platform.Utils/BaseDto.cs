using System;
using System.Collections.Generic;
using System.Text;
using DateTime = System.DateTime;

namespace Vt.Platform.Utils
{
    public class BaseDto : IAuditable
    {
        public BaseDto()
        {
            Created = DateTime.UtcNow;
            Modified = Created;
        }

        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }

        public void SetModified(string modifiedBy)
        {
            ModifiedBy = modifiedBy;
            Modified = DateTime.UtcNow;
        }
    }
}
