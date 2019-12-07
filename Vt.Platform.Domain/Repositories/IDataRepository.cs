using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vt.Platform.Domain.Models.Persistence;

namespace Vt.Platform.Domain.Repositories
{
    public interface IDataRepository
    {
        // EVENT REPOSITORY ACTIONS
        Task<EventDto> GetEventAsync(string volteerEventCode);
        Task SaveOrUpdateEvent(EventDto volteerEvent);

        // PARTICIPANT REPOSITORY ACTIONS
        Task<ParticipantDto[]> GetParticipantsAsync(string volteerEventCode);
        Task<ParticipantDto> GetParticipantAsync(string volteerEventCode, string participantCode);
        Task SaveOrUpdateParticipantAsync(ParticipantDto participant);

        // NOTIFICATION REPOSITORY ACTIONS
        Task LogNotificationAsync(NotificationDto notification);
        Task<string> GetMyEventsAsync(string email, string name);
    }
}
