using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Enums;
using API.extensions;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class Users : IdentityUser<int>
    {
        public GenderEnum Gender { get; set; }
        public override string UserName { get; set; }
        public override string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastActive { get; set; }
        public string KnownAs { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        #region Relations
        [InverseProperty("Users")]
        public ICollection<Photo> Photos { get; set; }
        public ICollection<UserLike> SourceUserLikes { get; set; }
        public ICollection<UserLike> TargetUserLikes { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
        public ICollection<UserRole> UserRole { get; set; }
        #endregion

        //get age
        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }
}