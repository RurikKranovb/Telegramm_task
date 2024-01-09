using Telegram.Bot.Types;
using Telegramm_task.Core.Core;
using Telegramm_task.Core.Core.DataBase;
using Telegramm_task.Core.Core.DataBase.Infrastructure;

namespace Telegramm_task.Core.Infrastructure.Plagins;

public class AddTask
{

    public AddTask(/*IDataBaseService dataBaseService*/)
    {
        //var cts = new CancellationTokenSource();
        //_cancellationToken = cts.Token;
    }

    public async Task AddTaskItem(Message message, SubdivisionEnum subdivisionEnum,
        CancellationToken cancellationToken)
    {
        try
        {
            await using DataBaseSqlLite db = new DataBaseSqlLite();

            var users = db.User.ToList();
            var taskItems = db.TaskItems.ToList();

            var task = users.FirstOrDefault(user => user.SubdivisionItem == subdivisionEnum);

            var taskItem = task?.TaskItems.FirstOrDefault(taskItem => string.Equals(taskItem.TaskName, message.Text, StringComparison.CurrentCultureIgnoreCase));

            var lastId = GetLastId(taskItems);


            if (message?.Text == null) return;
                var textSplit = message.Text.Split('=');

            if (task != null)
            {
                //item.ChatId = message.Chat.Id;
                //item.SubdivisionItem = _subdivisionEnum;
                //item.Name = message.From.FirstName;
                //SetId(task);

                task.TaskItems.Add(new TaskItem
                {
                    TaskId = lastId,
                    TaskName = textSplit[0],
                    UserItem = task,
                    Value = int.Parse(textSplit[1])
                });
            }
            else
            {
                var userItem = new UserItem
                {
                    //userItem.ChatId = message.Chat.Id;
                    SubdivisionItem = subdivisionEnum,
                    //Name = message.From.FirstName
                };

                userItem.TaskItems.Add(new TaskItem
                {
                    TaskId = lastId,
                    TaskName = textSplit[0],
                    Value = int.Parse(textSplit[1])
                    //SubdivisionId = task.SubdivisionId
                });
                db.User.Add(userItem);
            }

            await db.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private int GetLastId(List<TaskItem> task)
    {
        var taskItems = task.OrderBy(item => item.TaskId).ToList();
        var count = task.Count;

        var getLastId = 0;

        for (int i = 0; i < count; i++)
        {
            var item = taskItems[i];
            getLastId++;

            if (getLastId < item.TaskId )
            {
                if (getLastId == item.TaskId)
                {
                    continue;
                }
                return getLastId;
            }
        }

        return getLastId + 1;
    }
}