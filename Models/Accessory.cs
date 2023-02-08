using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCTodoList.Models
{
    public class Accessory
    {
        public int Id { get; set; }
        [Required]
        public string? AppUserID { get; set; }

        [Required]
        [Display(Name = "AccesoryName")]//<-this line is a decorator
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? AccessoryName { get; set; }

        //Navigation properties
        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<ToDoItem> ToDoItems { get; set; } = new HashSet<ToDoItem>();
    }
}
