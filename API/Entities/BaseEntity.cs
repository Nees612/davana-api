namespace API.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public int LastUpdatedBy { get; set; }
        public int Active { get; set; }
        public int RowVersionStamp { get; set; }
    }
}