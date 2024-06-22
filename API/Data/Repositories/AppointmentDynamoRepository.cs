using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using API.Data.DTO;
using API.Data.Repositories.Interfaces;
using API.Entities;

namespace API.Data.Repositories
{
    public class AppointmentDynamoRepository(IDavanaDynamoDBContext context) : DynamoDbRepository<Appointment>(context), IAppointmentDynamoRepository
    {
        private readonly Table _table = context.Appointments;
        private readonly IDavanaDynamoDBContext _context = context;

        public async Task<Appointment?> GetAppointment(string appointmentID)
        {
            GetItemOperationConfig config = new()
            {
                AttributesToGet = AppointmentDTO.AttributesToGet,
                ConsistentRead = true
            };

            try
            {
                var result = await _table.GetItemAsync(appointmentID, config);
                Appointment appointment = _context.FromDocument<Appointment>(result);

                return appointment;

            }
            catch (Exception ex)
            {
                //log error
                return null;
            }

        }

        public async Task<IEnumerable<Appointment>> GetAppointments()
        {
            ScanFilter scanFilter = new();

            scanFilter.AddCondition("active", ScanOperator.Equal, 1);

            ScanOperationConfig config = new()
            {
                AttributesToGet = AppointmentDTO.AttributesToGet,
                Filter = scanFilter,
                Select = SelectValues.SpecificAttributes
            };

            Search search = _table.Scan(config);

            var result = await search.GetNextSetAsync();

            IEnumerable<Appointment> appointments = _context.FromDocuments<Appointment>(result);

            return appointments;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByCoachID(string coachID, bool skipBooked = true)
        {
            ScanFilter scanFilter = new();

            scanFilter.AddCondition("coachID", ScanOperator.Equal, coachID);
            scanFilter.AddCondition("active", ScanOperator.Equal, 1);
            if (skipBooked)
                scanFilter.AddCondition("userID", ScanOperator.IsNull);

            ScanOperationConfig config = new()
            {
                AttributesToGet = AppointmentDTO.AttributesToGet,
                Filter = scanFilter,
                Select = SelectValues.SpecificAttributes
            };

            Search search = _table.Scan(config);

            var result = await search.GetNextSetAsync();

            IEnumerable<Appointment> appointments = _context.FromDocuments<Appointment>(result);

            return appointments;
        }

        public Task<Appointment> GetAppointmentsByIDHash(string IDHash)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> PutAppointment(Appointment appointment)
        {
            var doc = new Document();

            doc["Id"] = appointment.Id;
            doc["coachID"] = appointment.CoachId;
            doc["date"] = appointment.Date;
            doc["comment"] = appointment.Comment;
            doc["meetingType"] = appointment.MeetingType;
            doc["userId"] = null;
            doc["approoved"] = appointment.Approoved;
            doc["active"] = appointment.Active;
            doc["createdOn"] = appointment.CreatedOn;
            doc["rowVersionStamp"] = appointment.RowVersionStamp;

            try
            {
                await _table.PutItemAsync(doc);
            }
            catch (Exception ex)
            {
                throw;
            }

            return true;
        }
    }
}