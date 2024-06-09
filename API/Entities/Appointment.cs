using System.ComponentModel;

namespace API.Entities
{
    public class Appointment : BaseEntity
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public DateTime Date { get; set; }
        public string? Comment { get; set; }
        public string? MeetingType { get; set; }

        [DefaultValue(0)]
        public int UserId { get; set; }
        [DefaultValue(0)]
        public int Approoved { get; set; }

    }
}