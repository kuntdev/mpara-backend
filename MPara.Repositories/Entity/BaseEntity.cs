using System;
namespace MPara.Repositories.Entity
{
    public class BaseEntity
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdateOn { get; set; }
        public bool IsActive { get; set; }
    }
}
