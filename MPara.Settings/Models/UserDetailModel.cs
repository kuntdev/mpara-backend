using System;
using MPara.Repositories.Entity;

namespace MPara.Settings.Models
{
    public class CreateUserDetailRequest
    {
        public DateTime Birthdate { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }

    }

    public class UpdateUserDetailRequest
    {
        public DateTime Birthdate { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }

    }

}
