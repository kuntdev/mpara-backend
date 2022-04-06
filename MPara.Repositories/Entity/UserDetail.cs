using System;
namespace MPara.Repositories.Entity
{
    public class UserDetail : BaseEntity
    {
        public int UserDetailId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthdate { get; set; }


        // relation
        public int AppUserId { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        None
    }


}
