using System;
using System.Collections.Generic;
using System.Text;

namespace Vt.Platform.AzureDataTables.Models
{
    public class EventTable : BaseTable
    {

        // PARTITION KEY WILL BE THE EVENT CODE
        // ROW KEY WILL BE THE VALUE "EVENT"

        // SYSTEM GENERATED DATA
        public string OrganizerCode { get; set; }
        public string ConfirmationCode { get; set; }
        public bool OrganizerValidated { get; set; }

        // THIS IS THE USER PROVIDED DATA
        public string OrganizerName { get; set; }
        public string OrganizerEmail { get; set; }
        public DateTime EventDate { get; set; }
        public string EventSummary { get; set; }
        public string EventDetails { get; set; }
        public string EventLocation { get; set; }
        public int? NumberOfParticipants { get; set; }
        public bool EventCancelled { get; set; }
    }
}
