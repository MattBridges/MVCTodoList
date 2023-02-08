using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCTodoList.Models;

namespace MVCTodoList.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MVCTodoList.Models.ToDoItem> ToDoItem { get; set; } = default!;
        public DbSet<MVCTodoList.Models.Accessory> Accessory { get; set; } = default!;
    }
}