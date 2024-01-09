using Microsoft.EntityFrameworkCore;
using Telegramm_task.Core.Core.DataBase.Infrastructure;

namespace Telegramm_task.Core.Core.DataBase;

public interface IDataBaseService
{
    DbSet<UserItem> User { get; set; }
    DbSet<TaskItem> TaskItems { get; set; }

    int SaveChanges();
}