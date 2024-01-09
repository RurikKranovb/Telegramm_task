using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegramm_task.Core.Core;
using Telegramm_task.Core.Core.DataBase;
using Telegramm_task.Core.Core.DataBase.Infrastructure;

namespace Telegramm_task.Core.Infrastructure.Plagins
{
    public class CompletedTask
    {
        public CompletedTask()
        {

        }

        public async Task Complete(SubdivisionEnum subdivisionEnum, bool? isAdmin, ITelegramBotClient botClient,
            CancellationToken cancellationToken, long chatId = 0, Message messages = null)
        {
            try
            {
                await using (DataBaseSqlLite db = new DataBaseSqlLite())
                {
                    var tasks = db.TaskItems.ToList();
                    var users = db.User.ToList();
                   

                    if (isAdmin == true)
                    {
                        await GetItem(tasks, botClient, cancellationToken, chatId);
                    }
                    else
                    {
                        if (messages == null) return;

                        var message = messages.Text;
                        
                        var textSplit = message.Split('=');

                        var task = db.TaskItems.FirstOrDefault(user => user.TaskId == int.Parse(textSplit[0]));

                        if (task == null) return;

                        task.ValueCompleted = int.Parse(textSplit[1]);

                        await db.SaveChangesAsync(cancellationToken);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }
         
        }

        private async Task GetItem(List<TaskItem> task, ITelegramBotClient botClient, CancellationToken cancellationToken,
            long? chatId)
        {
            foreach (var taskTaskItem in task)
            {
                if (chatId != null)
                    await botClient.SendTextMessageAsync(chatId,
                        string.Format($"{taskTaskItem.TaskName} - Количество: {taskTaskItem.ValueCompleted}"),
                        cancellationToken: cancellationToken);
                await Task.Delay(100, cancellationToken);
            }
        }
    }
}
