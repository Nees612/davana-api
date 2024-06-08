namespace API.Data.DTO
{
    public class AppointmentDTO
    {
        public int Id { get; set; }   
        public int CoachId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string? Comment { get; set; }
        public string? MeetingType { get; set; }
        public int Approoved { get; set; }

    }
}