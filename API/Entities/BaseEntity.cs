using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class BaseEntity
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedOn { get; set; }
        public int LastUpdatedBy { get; set; }

        [DefaultValue(1)]
        public int Active { get; set; }

        [DefaultValue(1)]
        public int RowVersionStamp { get; set; }
    }
}