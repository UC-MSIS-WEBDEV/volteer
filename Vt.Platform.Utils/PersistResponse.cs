using System;
using System.Collections.Generic;
using System.Text;
using Vt.Platform.Utils.Enums;

namespace Vt.Platform.Utils
{
    public class PersistResponse
    {
        public PersistStatus Status { get; set; }

        public static PersistResponse Created()
        {
            return new PersistResponse
            {
                Status = PersistStatus.Created
            };
        }

        public static PersistResponse NotCreatedKeyAlreadyExists()
        {
            return new PersistResponse
            {
                Status = PersistStatus.NotCreatedKeyAlreadyExists
            };
        }

        public static PersistResponse Error()
        {
            return new PersistResponse
            {
                Status = PersistStatus.Error
            };
        }
    }
}
