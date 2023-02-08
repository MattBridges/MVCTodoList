using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCTodoList.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        [Required]
        public string? AppUserID { get; set; }

        [Required]
        [Display(Name = "Task Name")]//<-this line is a decorator
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? TaskName { get; set; }

        [Required]
        [Display(Name = "Created" )]
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [Display(Name = "DueDate")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; }


        //Navigation Properties
        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<Accessory> Accessories { get; set; } = new HashSet<Accessory>();
    }
}
