using Amazon.DynamoDBv2.DataModel;

namespace API.Entities
{
    [DynamoDBTable("Appointmets")]
    public class Appointment : BaseEntity
    {
        [DynamoDBHashKey]
        public string? Id { get; set; }
        [DynamoDBProperty("coachId")]
        public string? CoachId { get; set; }
        [DynamoDBProperty("date")]
        public DateTime Date { get; set; }
        [DynamoDBProperty("comment")]
        public string? Comment { get; set; }
        [DynamoDBProperty("meetingType")]
        public string? MeetingType { get; set; }
        [DynamoDBProperty("userId")]
        public string? UserId { get; set; } = "";
        [DynamoDBProperty("approoved")]
        public int Approoved { get; set; } = 0;

    }
}