using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ToDo.Authentication_Models;

namespace ToDo.Models
{
    public class ToDos
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        public string Categories { get; set; }
        public string PriorityLevel { get; set; }
        public ApplicationUser User { get; set; }

    }
}
