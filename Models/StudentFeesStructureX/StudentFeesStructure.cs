using System.ComponentModel.DataAnnotations.Schema;
using School_Management_System.Models.StudentX;
using School_Management_System.Models.TermX;

namespace School_Management_System.Models.StudentFeesStructureX
{
    public class StudentFeesStructure : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid TermId { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [ForeignKey("TermId")]
        public virtual Term Term { get; set; }
    }

    public class StudentFeesStructureDto : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid TermId { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public StudentDto? Student { get; set; }
        public TermDTO? Term { get; set; }
    }
}