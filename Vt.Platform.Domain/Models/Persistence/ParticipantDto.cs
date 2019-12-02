namespace Vt.Platform.Domain.Models.Persistence
{
    public class ParticipantDto
    {
        public string EventCode { get; set; }
        public string ParticipantCode { get; set; }
        public string ConfirmationCode { get; set; }
        public string ParticipantStatus { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantEmail { get; set; }
        public bool ParticipantValidated { get; set; }
    }
}
