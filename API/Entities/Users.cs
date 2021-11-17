using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.extensions;

namespace API.Entities
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public int Gender { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastActive { get; set; }
        public string KnownAs { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public ICollection<Photo> Photos { get; set; }
        //get age
        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }
}