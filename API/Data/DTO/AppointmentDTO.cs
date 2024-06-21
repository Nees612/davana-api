namespace API.Data.DTO
{
    public class AppointmentDTO
    {
        private static readonly List<string> attributesToGet = [
                    "Id",
                    "coachId",
                    "userId",
                    "date",
                    "comment",
                    "meetingType",
                    "approoved"
            ];
        public static List<string> AttributesToGet { get => attributesToGet; }
    }
}