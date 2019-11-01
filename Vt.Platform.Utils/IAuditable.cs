using System;
using System.Collections.Generic;
using System.Text;

namespace Vt.Platform.Utils
{
    public interface IAuditable
    {
        DateTime Created { get; }
        string CreatedBy { get; }
        DateTime Modified { get; }
        string ModifiedBy { get; }
    }
}
