
using School_Management_System.Models.StreamX;
using Stream = School_Management_System.Models.StreamX.Stream;

namespace School_Management_System.Models.ClassX
{
    public class Class : GeneralFields
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? More_info { get; set; }
        public virtual ICollection<Stream>? Streams { get; set; }
    }
    public class ClassDto : GeneralFields
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? More_info { get; set; }
        public int? StreamCount { get; set; }
        public ICollection<StreamDto>? Streams { get; set; }
    }

    public class classInfo : GeneralFields
    {
        public string Name { get; set; }
        public string? More_info { get; set; }
        public int? StreamCount { get; set;}
    } 
}
