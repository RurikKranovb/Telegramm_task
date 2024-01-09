using System.Threading.Tasks;
using System.Threading;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegramm_task.Core.Core;
using Telegramm_task.Core.Core.DataBase;
using Telegramm_task.Core.Core.DataBase.Infrastructure;

namespace Telegramm_task.Core.Infrastructure.Plagins;

public class ActiveTask
{
    public ActiveTask()
    {

    }

    public async Task GetTasks(SubdivisionEnum subdivisionEnum, bool? isAdmin, ITelegramBotClient botClient,
        CancellationToken cancellationToken, long? chatId)
    {
        if (chatId == null) return;

        await using (DataBaseSqlLite db = new DataBaseSqlLite())
        {
            var tasks = db.TaskItems.ToList();
            var users = db.User.ToList();

            if (isAdmin == false)
            {
                var task = db.User.FirstOrDefault(user => user.SubdivisionItem == subdivisionEnum);

                if (task == null) return;

                await GetItem(task, botClient, cancellationToken, chatId);
            }
            else if (isAdmin == true)
            {
                if (tasks.Count == 0) return;

                string name;
                
                    foreach (var userItem in users)
                    {
                        name = userItem.SubdivisionItem switch
                        {
                            SubdivisionEnum.Laser => "Лазер:",
                            SubdivisionEnum.Stamping => "Гибка/пробивной:",
                            SubdivisionEnum.Welding => "Сварка:",
                        };

                      await botClient.SendTextMessageAsync(chatId,
                            name + " ",
                            cancellationToken: cancellationToken);

                      await GetItem(userItem, botClient, cancellationToken, chatId);
                    }
            }
            else
            {
                var task = db.User.FirstOrDefault(user => user.SubdivisionItem == subdivisionEnum);

                if (task == null) return;

                await GetItem(task, botClient, cancellationToken, chatId);


                await botClient.SendTextMessageAsync(chatId, "Введите номер задачи:",
                    cancellationToken: cancellationToken);
            }
        }
    }

    private async Task GetItem(UserItem task, ITelegramBotClient botClient, CancellationToken cancellationToken,
        long? chatId)
    {
        foreach (var taskTaskItem in task.TaskItems)
        {
            if (chatId != null)
                await botClient.SendTextMessageAsync(chatId,
                    string.Format($"{taskTaskItem.TaskId} - {taskTaskItem.TaskName}"),
                    cancellationToken: cancellationToken);
            await Task.Delay(100, cancellationToken);
        }
    }
}