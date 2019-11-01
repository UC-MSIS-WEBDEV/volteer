using System;
using System.Collections.Generic;
using System.Text;

namespace Vt.Platform.Utils
{
    public class EnumDetails
    {
        public EnumDetails(Enum e)
        {
            Name = e.ToString();
            Description = e.ToDescription();
            Value = Convert.ToInt32(e);
        }

        public string Name { get; }
        public string Description { get; }
        public int Value { get; }
    }
}
