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
            dto.ConfirmationCode = table.ConfirmationCode;
            dto.EventDate = table.EventDate;
            dto.EventDetails = table.EventDetails;
            dto.EventLocation = table.EventLocation;
            dto.EventSummary = table.EventSummary;
            dto.NumberOfParticipants = table.NumberOfParticipants.Value;
            dto.OrganizerCode = table.OrganizerCode;
            dto.OrganizerEmail = table.OrganizerEmail;
            dto.OrganizerName = table.OrganizerName;
            dto.OrganizerValidated = table.OrganizerValidated;
        }

        private void MapParticipantDtoToParticipantTable(ParticipantDto dto, ParticipantTable table)
        {
            // TODO: MAP PARTICIPANT DTO OBJECT TO THE PARTICIPANT TABLE OBJECT
            table.ConfirmationCode = dto.ConfirmationCode;
            table.ParticipantName = dto.ParticipantName;
            table.ParticipantEmail = dto.ParticipantEmail;
            table.ParticipantStatus = dto.ParticipantStatus;
            table.ParticpantValidated = dto.ParticipantValidated;


            table.Created = DateTime.UtcNow;
            table.Modified = DateTime.UtcNow;
            table.CreatedBy = "System";
            table.ModifiedBy = "System";
        }

        private void MapParticipantTableToParticipantDto(ParticipantTable table, ParticipantDto dto)
        {
            // TODO: MAP PARTICIPANT TABLE OBJECT TO THE PARTICIPANT DTO OBJECT
            dto.EventCode = table.PartitionKey;
            dto.ParticipantName = table.ParticipantName;
            dto.ParticipantEmail = table.ParticipantEmail;
            dto.ParticipantStatus = table.ParticipantStatus;
            dto.ParticipantValidated = table.ParticpantValidated;
        }



        public async Task<EventDto> GetEventAsync(string volteerEventCode)
        {
            var table = await GetTable("EventData");
            var result = await table.GetEntity<EventTable>("Event", volteerEventCode);
            if (result == null)
            {
                return null;
            }

            var dto = new EventDto
            {
                EventCode = result.RowKey,
                ConfirmationCode = result.ConfirmationCode,
                EventDate = result.EventDate,
                EventDetails = result.EventDetails,
                EventLocation = result.EventLocation,
                EventSummary = result.EventSummary,
                NumberOfParticipants = result.NumberOfParticipants.Value,
                OrganizerCode = result.OrganizerCode,
                OrganizerEmail = result.OrganizerEmail,
                OrganizerName = result.OrganizerName,
                OrganizerValidated = result.OrganizerValidated
            };

            MapEventTableToEventDto(result, dto);
            return dto;
        }

        public async Task<ParticipantDto[]> GetParticipantsAsync(string volteerEventCode)
        {
            var table = await GetTable("ParticipantData");

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
            var table = await GetTable("ParticipantData");
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
            if (etag == null)
            {
                var eventTable = new EventTable();
                eventTable.RowKey = volteerEvent.EventCode;
                eventTable.PartitionKey = "Event";
                eventTable.ETag = etag;

                MapEventDtoToEventTable(volteerEvent, eventTable);

                var insertOperation = TableOperation.InsertOrReplace(eventTable);
                await table.ExecuteAsync(insertOperation);
            }
            else
            {
                TableOperation retrieve = TableOperation.Retrieve<EventTable>("Event", volteerEvent.EventCode);
                TableResult result = await table.ExecuteAsync(retrieve);
                EventTable e = (EventTable)result.Result;
                if (e.ConfirmationCode == volteerEvent.ConfirmationCode)
                {
                    e.OrganizerValidated = true;
                }
                if (result != null)
                {
                    TableOperation update = TableOperation.Replace(e);
                    await table.ExecuteAsync(update);
                }
            }                        
        }

        public async Task SaveOrUpdateParticipantAsync(ParticipantDto participant)
        {
            var table = await GetTable("ParticipantData");
            var existingEntry = await GetParticipantAsync(participant.EventCode, participant.ParticipantCode);
            var etag = existingEntry == null ? null : "*";
            if (etag == null)
            {
                var participantTable = new ParticipantTable
                {
                    RowKey = participant.ParticipantCode,
                    PartitionKey = participant.EventCode,
                    ETag = etag
                };

                MapParticipantDtoToParticipantTable(participant, participantTable);

                var insertOperation = TableOperation.InsertOrReplace(participantTable);
                await table.ExecuteAsync(insertOperation);
            }
            else
            {
                TableOperation retrieve = TableOperation.Retrieve<ParticipantTable>(participant.EventCode, participant.ParticipantCode);
                TableResult result = await table.ExecuteAsync(retrieve);
                ParticipantTable e = (ParticipantTable)result.Result;
                if (e.RowKey == participant.ParticipantCode)
                {
                    e.ParticpantValidated = true; 
                }
                if (result != null)
                {
                    TableOperation update = TableOperation.Replace(e);
                    await table.ExecuteAsync(update); 
                }
            }
        }

        public async Task<string> GetMyEventsAsync(string email, string name)
        {
            string returningEmailBodyString = "Hi " + name + "," + "<br>Following are the vents you are involved in: <br> <strong>Events organized by you:</strong> <br>";
            var eventTable = await GetTable("EventData");
            var eventtq = new TableQuery<EventTable>
            {
                FilterString = TableQuery.GenerateFilterCondition("OrganizerEmail", QueryComparisons.Equal, email)
            };
            var etResults = await eventTable.ExecuteQueryAsync(eventtq);
            var eventResults = etResults.ToList();

            var participantTable = await GetTable("ParticipantData");
            var participanttq = new TableQuery<ParticipantTable>
            {
                FilterString = TableQuery.GenerateFilterCondition("ParticipantEmail", QueryComparisons.Equal, email)
            };
            var ptResults = await participantTable.ExecuteQueryAsync(participanttq);
            var participantResults = ptResults.ToList();

            var eventdtos = new List<EventDto>();
            var participanteventdtos = new List<EventDto>();

            foreach (var item in eventResults)
            {
                returningEmailBodyString += "Event Name: " + item.EventDetails + "<br>" + "https://volteer.us/events?" + item.RowKey + "<br>";
                var dto = new EventDto
                {
                    EventCode = item.RowKey,
                    EventDetails = item.EventDetails,
                };
                MapEventTableToEventDto(item, dto);
                eventdtos.Add(dto);
            }

            returningEmailBodyString += "<strong>Events you are participating in:</strong> <br>";
            foreach (var item in participantResults)
            {
                var retrievedEvent = await GetEventAsync(item.PartitionKey);
                if (retrievedEvent != null)
                {
                    returningEmailBodyString += "Event Name: " + retrievedEvent.EventDetails + "<br>" + "https://volteer.us/events?" + retrievedEvent.EventCode + "<br>";
                    participanteventdtos.Add(retrievedEvent);
                }
            }

            return returningEmailBodyString;
        }
    }
}
