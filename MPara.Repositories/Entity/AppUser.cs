using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPara.Repositories.Entity
{
    public class AppUser : BaseEntity
    {
        [Key]
        public int AppUserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        // relations
        public List<Account> Accounts { get; set; }


    }
}
