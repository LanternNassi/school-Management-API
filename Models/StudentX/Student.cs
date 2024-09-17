

using System.ComponentModel.DataAnnotations.Schema;
using School_Management_System.Models.StreamX;
using Stream = School_Management_System.Models.StreamX.Stream;

namespace School_Management_System.Models.StudentX
{
    public class Student : GeneralFields
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public string PayCode { get; set; }
        public Guid StreamId { get; set; }

        [ForeignKey("StreamId")]
        public virtual Stream Stream    { get; set; }
    }

    public class StudentDto : GeneralFields
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public string PayCode { get; set; }
        public Guid StreamId { get; set; }
        public StreamDto? Stream { get; set; }
    }

    public class StudentDtoUpdate
    {
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public string PayCode { get; set; }
        public Guid StreamId { get; set; }
    }
}
