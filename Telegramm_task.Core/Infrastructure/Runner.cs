using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegramm_task.Core.Core;
using Telegramm_task.Core.Infrastructure.Plagins;

namespace Telegramm_task.Core.Infrastructure;

public class Runner : ITaskEvent
{
    private readonly ITelegramBotClient _bot;
    private bool? _isAdmin = false;
    private SubdivisionEnum _subdivisionEnum;
    private InlineKeyboardMarkup _inlineKeyboard;
    private CommandEnum _commandEnum = CommandEnum.None;

    public Runner(ITelegramBotClient bot)
    {
        AddTaskEvent += Runner_AddTaskEvent;

        _bot = bot;
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;

        var receiverOption = new ReceiverOptions
        {
            AllowedUpdates = { },
        };

        bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOption,
            cancellationToken
        );

        MenuItems = new MenuItems();
        AddTask = new AddTask();
        DeleteTask = new RemoveTask();
        CompletedTask = new CompletedTask();

        _inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Руководитель", nameof(MenuEnum.Supervisor)),

                InlineKeyboardButton.WithCallbackData("Подразделение", nameof(MenuEnum.Subdivision)),
            },
        });
    }

    private void Runner_AddTaskEvent(object sender, EventArgs e)
    {
        var item = sender as MenuItems;

        if (item == null) return;
        _commandEnum = item.CommandEnum;
        _subdivisionEnum = item.SubdivisionEnum;
    }

    public MenuItems MenuItems { get; set; }
    public AddTask AddTask { get; set; }
    public RemoveTask DeleteTask { get; set; }
    public CompletedTask CompletedTask { get; set; }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;
            if (message == null) return;

            switch (_commandEnum)
            {
                case CommandEnum.Add:
                   await AddTask.AddTaskItem(message, _subdivisionEnum, cancellationToken);

                    break;

                case CommandEnum.Remove:
                    await DeleteTask.Delete(_subdivisionEnum, cancellationToken, message.Text);

                    break;

                case CommandEnum.Completed:
                   await CompletedTask.Complete(_subdivisionEnum, _isAdmin, botClient, cancellationToken, messages:message);
                    break;
            }

            if (message?.Text?.ToLower() == "/start")
            {
              await GetMenu(message.Chat, cancellationToken, botClient);

                return;
            }

            _commandEnum = CommandEnum.None;

            await GetMenu(message.Chat, cancellationToken, botClient);

            _isAdmin = false;
        }

        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
        {
            await MenuItems.MenuItem(botClient, update, cancellationToken, AddTaskEvent);
        }
    }

    private async Task GetMenu(Chat messageChat, CancellationToken cancellationToken, ITelegramBotClient botClient)
    {
        await botClient.SendTextMessageAsync(messageChat, "Выберите меню", replyMarkup: _inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        //await Console.Out.WriteLineAsync();
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        return Task.CompletedTask;
    }

    public event EventHandler AddTaskEvent;
}