using School_Management_System.Models.ClassX;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Management_System.Models.StreamX
{
    public class Stream : GeneralFields
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? More_info { get; set; }
        public Guid ClassId { get; set; }
        [ForeignKey("ClassId")]
        public virtual Class? Class { get; set; }
    }

    public class StreamDto : GeneralFields
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? More_info { get; set; }
        public Guid ClassId { get; set; }
        public classInfo? Class { get; set; }
    }
}
