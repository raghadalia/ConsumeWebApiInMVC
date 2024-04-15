using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ToDo.Models;

namespace ToDo.Authentication_Models
{
    public class ApplicationUser : IdentityUser
    {

        public ICollection<ToDos> Tasks { get; set; }
    }
}
