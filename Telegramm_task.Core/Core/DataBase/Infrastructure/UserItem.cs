using System.ComponentModel.DataAnnotations;

namespace Telegramm_task.Core.Core.DataBase.Infrastructure;

public class UserItem
{

    public UserItem()
    {
        TaskItems = new List<TaskItem>();
    }

    [Key]
    public int SubdivisionId { get; set; }

    public SubdivisionEnum SubdivisionItem { get; set; }

    //public string Name { get; set; }
    //public long ChatId { get; set; }

    public ICollection<TaskItem> TaskItems { get; set; }

}