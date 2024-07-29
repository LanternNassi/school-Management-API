using School_Management_System.Models.ContactInfoX;
using School_Management_System.Models.UserGroupX;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Management_System.Models.UserX
{
    public class User : GeneralFields
    {
        public Guid id { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid Group { get; set; }

        [ForeignKey("Group")]
        public virtual UserGroup UserGroup { get; set; }

        public virtual ICollection<ContactInfo>? Contacts { get; set; }
    }

    public class UserDto : GeneralFields
    {
        public Guid id { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid Group { get; set; }
        public ICollection<ContactInfoDto>? Contacts { get; set; }
    }
}
