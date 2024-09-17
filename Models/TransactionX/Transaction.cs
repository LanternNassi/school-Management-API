using System.ComponentModel.DataAnnotations.Schema;
using School_Management_System.Models.StudentFeesStructureX;


namespace School_Management_System.Models.TransactionX
{
    public class Transaction : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid StudentFeesStructureId { get; set; }
        public decimal Amount { get; set; }

        [ForeignKey("StudentFeesStructureId")]
        public virtual StudentFeesStructure StudentFeesStructure { get; set; }

    }

    public class TransactionDto : GeneralFields
    {
        public Guid Id { get; set; }
        public Guid StudentFeesStructureId { get; set; }
        public decimal Amount { get; set; }
        public StudentFeesStructureDto? StudentFeesStructure { get; set; }
    }
    
}