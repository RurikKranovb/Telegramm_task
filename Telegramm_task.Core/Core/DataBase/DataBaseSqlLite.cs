using Microsoft.EntityFrameworkCore;
using Telegramm_task.Core.Core.DataBase.Infrastructure;

namespace Telegramm_task.Core.Core.DataBase;

public class DataBaseSqlLite : DbContext, IDataBaseService
{
    public DbSet<UserItem> User { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }

    public DataBaseSqlLite() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=helloapp.db");

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>().Property(item => item.TaskId).IsRequired();
    }


}