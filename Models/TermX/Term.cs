using System;
using School_Management_System.Models.StudentX;

namespace School_Management_System.Models.TermX
{
    public class Term : GeneralFields
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description {get; set;}
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<Student>? Students { get; set; }

    }

    public class TermDTO : GeneralFields
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description {get; set;}
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<StudentDtoUpdate>? Students { get; set; }
    }

    public class TermDtoUpdate
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string? Description {get; set;}
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}