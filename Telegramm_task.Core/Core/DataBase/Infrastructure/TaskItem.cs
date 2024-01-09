using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Telegramm_task.Core.Core.DataBase.Infrastructure;

public class TaskItem
{

    public TaskItem()
    {
        //UserItems = new List<UserItem>();
    }

    //public int SubdivisionId { get; set; }

    [ForeignKey("SubdivisionId")]
    public UserItem UserItem { get; set; }

    //public virtual ICollection<UserItem> UserItems { get; set; }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int TaskId { get; set; }

    public string TaskName { get; set; }

    public int Value { get; set; }
    public int ValueCompleted { get; set; }
}