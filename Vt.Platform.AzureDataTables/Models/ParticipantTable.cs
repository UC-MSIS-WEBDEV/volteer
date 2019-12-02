namespace Vt.Platform.AzureDataTables.Models
{
    public class ParticipantTable : BaseTable
    {
        // PARTITION KEY WILL BE THE EVENT CODE
        // ROW KEY WILL BE THE PARTICIPANT CODE

        public string ConfirmationCode { get; set; }
        public string ParticipantStatus { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantEmail { get; set; }
        public bool ParticpantValidated { get; set; }
    }
}
