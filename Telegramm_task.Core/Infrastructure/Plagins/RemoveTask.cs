using Telegram.Bot;
using Telegramm_task.Core.Core;
using Telegramm_task.Core.Core.DataBase;

namespace Telegramm_task.Core.Infrastructure.Plagins;

public class RemoveTask
{
    public RemoveTask()
    {

    }


    public async Task Delete(SubdivisionEnum subdivisionEnum, CancellationToken cancellationToken, string messageText)
    {
        try
        {
            await using DataBaseSqlLite db = new DataBaseSqlLite();

            var taskId = int.Parse(messageText);
            var tasks = db.TaskItems.ToList();
            var users = db.User.ToList();

            var taskItems = db.User.FirstOrDefault(user => user.SubdivisionItem == subdivisionEnum);

            var task = taskItems?.TaskItems.FirstOrDefault(item => item.TaskId == taskId);

            if (task != null)
            {
                db.TaskItems.Remove(task);

                await db.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception e)
        {
            //отправить сообщение ошибки
            Console.WriteLine(e);
            //throw;
        }
    }
}