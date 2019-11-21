using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vt.Platform.AzureDataTables.Models;
using Vt.Platform.Domain.Models.Persistence;
using Vt.Platform.Domain.Repositories;

namespace Vt.Platform.AzureDataTables.Repositories
{
    public class DataRepository : RepositoryBase, IDataRepository
    {

        private void MapEventDtoToEventTable(EventDto dto, EventTable table)
        {
            // TODO: MAP EVENT DTO OBJECT TO THE EVENT TABLE OBJECT

            table.ConfirmationCode = dto.ConfirmationCode;
            table.EventDate = dto.EventDate;
            table.EventDetails = dto.EventDetails;
            table.EventLocation = dto.EventLocation;
            table.EventSummary = dto.EventSummary;
            table.NumberOfParticipants = dto.NumberOfParticipants;
            table.OrganizerCode = dto.OrganizerCode;
            table.OrganizerEmail = dto.OrganizerEmail;
            table.OrganizerName = dto.OrganizerName;
            table.OrganizerValidated = dto.OrganizerValidated;

            table.Created = DateTime.UtcNow;
            table.Modified = DateTime.UtcNow;
            table.CreatedBy = "System";
            table.ModifiedBy = "System";
        }

        private void MapEventTableToEventDto(EventTable table, EventDto dto)
        {
            // TODO: MAP EVENT TABLE OBJECT TO THE EVENT DTO OBJECT
        }

        private void MapParticipantDtoToParticipantTable(ParticipantDto dto, ParticipantTable table)
        {
            // TODO: MAP PARTICIPANT DTO OBJECT TO THE PARTICIPANT TABLE OBJECT
        }

        private void MapParticipantTableToParticipantDto(ParticipantTable table, ParticipantDto dto)
        {
            // TODO: MAP PARTICIPANT TABLE OBJECT TO THE PARTICIPANT DTO OBJECT
        }



        public async Task<EventDto> GetEventAsync(string volteerEventCode)
        {
            var table = await GetTable("EventData");
            var result = await table.GetEntity<EventTable>(volteerEventCode, "Event");
            if (result == null)
            {
                return null;
            }

            var dto = new EventDto
            {
                EventCode = result.PartitionKey,
            };

            MapEventTableToEventDto(result, dto);
            return dto;
        }

        public async Task<ParticipantDto[]> GetParticipantsAsync(string volteerEventCode)
        {
            var table = await GetTable("EventData");

            var tq = new TableQuery<ParticipantTable>
            {
                FilterString = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, volteerEventCode)
            };

            var results = await table.ExecuteQueryAsync(tq);
            var skipEventInResults = results.Where(x => x.RowKey != "Event").ToList();

            var dtos = new List<ParticipantDto>();

            foreach (var item in skipEventInResults)
            {
                var dto = new ParticipantDto
                {
                    EventCode = item.PartitionKey,
                    ParticipantCode = item.RowKey,
                };
                MapParticipantTableToParticipantDto(item, dto);
                dtos.Add(dto);
            }

            return dtos.ToArray();
        }

        public async Task<ParticipantDto> GetParticipantAsync(string volteerEventCode, string participantCode)
        {
            var table = await GetTable("EventData");
            var result = await table.GetEntity<ParticipantTable>(volteerEventCode, participantCode);
            if (result == null)
            {
                return null;
            }

            var dto = new ParticipantDto
            {
                EventCode = result.PartitionKey,
                ParticipantCode = result.RowKey,
            };

            MapParticipantTableToParticipantDto(result, dto);

            return dto;
        }

        public async Task LogNotificationAsync(NotificationDto notification)
        {
            throw new NotImplementedException();
        }

        public async Task SaveOrUpdateEvent(EventDto volteerEvent)
        {
            var table = await GetTable("EventData");
            var existingEntry = await GetEventAsync(volteerEvent.EventCode);

            var etag = existingEntry == null ? null : "*";

            var eventTable = new EventTable
            {
                RowKey = volteerEvent.EventCode,
                PartitionKey = "Event",
                ETag = etag
            };

            MapEventDtoToEventTable(volteerEvent, eventTable);

            var insertOperation = TableOperation.InsertOrReplace(eventTable);
            await table.ExecuteAsync(insertOperation);
        }

        public async Task SaveOrUpdateParticipantAsync(ParticipantDto participant)
        {
            var table = await GetTable("EventData");
            var existingEntry = await GetParticipantAsync(participant.EventCode, participant.ParticipantCode);

            var etag = existingEntry == null ? null : "*";

            var participantTable = new ParticipantTable
            {
                RowKey = participant.EventCode,
                PartitionKey = participant.ParticipantCode,
                ETag = etag
            };

            MapParticipantDtoToParticipantTable(participant, participantTable);

            var insertOperation = TableOperation.InsertOrReplace(participantTable);
            await table.ExecuteAsync(insertOperation);
        }
    }
}
