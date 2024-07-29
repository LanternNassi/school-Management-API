using School_Management_System.Models.UserX;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Management_System.Models.ContactInfoX
{
    public class ContactInfo : GeneralFields
    {
        public Guid id { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public Guid ContactUser { get; set; }
        public string? Location { get; set; }

        [ForeignKey("ContactUser")]
        public virtual User User { get; set; }

    }

    public class ContactInfoDto : GeneralFields
    {
        public Guid id { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? Location { get; set; }
        public Guid ContactUser { get; set; }
    }
}
