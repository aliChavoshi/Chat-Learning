using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class MemberUpdateDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public string Introduction { get; set; }
        [Required]
        public string LookingFor { get; set; }
        [Required]
        public string Interests { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
    }
}