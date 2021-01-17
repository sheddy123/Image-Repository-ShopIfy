using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepo.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        
        public string Role { get; set; }
        
        public string Token { get; set; }
        [Required]
        public string EmailAddress { get; set; }

        public string message { get; set; }
    }
}
