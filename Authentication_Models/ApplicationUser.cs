using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ToDo.Models;

namespace ToDo.Authentication_Models
{
    public class ApplicationUser : IdentityUser
    {

        public List<ToDos> Tasks { get; set; } = new List<ToDos>();
    }
}
