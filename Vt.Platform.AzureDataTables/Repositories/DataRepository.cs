using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vt.Platform.Domain.Models.Persistence;
using Vt.Platform.Domain.Repositories;

namespace Vt.Platform.AzureDataTables.Repositories
{
    public class DataRepository : RepositoryBase, IDataRepository
    {
        public async Task<EventDto> GetEventAsync(string volteerEventCode)
        {
            throw new NotImplementedException();
        }

        public async Task<ParticipantDto[]> GetParticipantsAsync(string volteerEventCode)
        {
            throw new NotImplementedException();
        }

        public async Task LogNotificationAsync(NotificationDto notification)
        {
            throw new NotImplementedException();
        }

        public async Task SaveOrUpdateEvent(EventDto volteerEvent)
        {
            throw new NotImplementedException();
        }

        public async Task SaveOrUpdateParticipantAsync(ParticipantDto participant)
        {
            throw new NotImplementedException();
        }
    }
}
