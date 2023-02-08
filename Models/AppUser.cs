using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCTodoList.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]//<-this line is a decorator
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? LastName { get; set; }


        [NotMapped]
        public string? FullName { get { return $"{FirstName} {LastName}"; } }

        //Navigation Properties
        public virtual ICollection<ToDoItem> ToDoItems { get; set; } = new HashSet<ToDoItem>();
        public virtual ICollection<Accessory> Accessories { get; set; } = new HashSet<Accessory>();
    }
}
