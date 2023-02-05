using Microsoft.EntityFrameworkCore;

namespace Skrmiish.Models;

public class StateContext : DbContext
{
    public StateContext(DbContextOptions<StateContext> options)
        : base(options)
    {
    }

    public DbSet<State> States { get; set; } = null!;
}