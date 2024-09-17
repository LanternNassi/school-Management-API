
namespace School_Management_System.Models.UserGroupX
{
    public class UserGroup : GeneralFields 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }


    public class UserGroupDto : GeneralFields
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
