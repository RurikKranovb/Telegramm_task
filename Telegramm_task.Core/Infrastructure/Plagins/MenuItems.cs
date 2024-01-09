using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegramm_task.Core.Core;

namespace Telegramm_task.Core.Infrastructure.Plagins;

public class MenuItems
{
    private ITelegramBotClient _botClient;
    private Update _update;
    private SubdivisionEnum _subdivisionEnum;
    private bool _isActive;
    

    public MenuItems( /*ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken*/)
    {
        SubdivisionEnum = new SubdivisionEnum();
        CommandEnum = new CommandEnum();
        IsAdmin = new bool();
        RemoveTaskItem = new bool();
        ActiveTask = new ActiveTask();
        DeleteTask = new RemoveTask();
        CompletedTask = new CompletedTask();


        //_botClient = botClient;
        //_update = update;  
        //_cancellationToken = cancellationToken;
    }

    public SubdivisionEnum SubdivisionEnum { get; set; }
    public CommandEnum CommandEnum { get; set; }
    public bool? IsAdmin { get; set; }
    public bool RemoveTaskItem { get; set; }
    public ActiveTask ActiveTask { get; set; }
    public RemoveTask DeleteTask { get; set; }
    public CompletedTask CompletedTask { get; set; }


    public async Task MenuItem(ITelegramBotClient botClient,
        Update update, CancellationToken cancellationToken, EventHandler addTaskEvent)
    {
        var data = update.CallbackQuery?.Data;

        var chatId = update.CallbackQuery?.From.Id;
        if (chatId == null) return;


        string name;
        InlineKeyboardMarkup inlineKeyboard;

        switch (data)
        {
            case nameof(MenuEnum.Supervisor):

                CommandEnum = CommandEnum.Supervisor;
                IsAdmin = true;

                inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    // first row
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Добавить задачу", nameof(MenuEnum.AddTask)),
                    },

                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Удалить задачу", nameof(MenuEnum.RemoveTask)),
                    },
                    // second row
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Активные задачи", nameof(MenuEnum.ActiveTasks)),

                        InlineKeyboardButton.WithCallbackData("Статус задач", nameof(MenuEnum.Completed)),
                    }
                });

                await botClient.SendTextMessageAsync(chatId, "Меню руководителя:", replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
                break;

            case nameof(MenuEnum.Subdivision):

                CommandEnum = CommandEnum.Subdivision;

                IsAdmin = false;

                inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Гибка/пробивной", nameof(SubdivisionEnum.Stamping)),

                        InlineKeyboardButton.WithCallbackData("Сварка", nameof(SubdivisionEnum.Welding)),

                        InlineKeyboardButton.WithCallbackData("Лазер", nameof(SubdivisionEnum.Laser))
                    },
                });

                await botClient.SendTextMessageAsync(chatId, "Выберите подразделение:",
                    replyMarkup: inlineKeyboard);
                break;

            case nameof(MenuEnum.AddTask):

                CommandEnum = CommandEnum.Add;

                inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Гибка/пробивной", nameof(SubdivisionEnum.Stamping)),

                        InlineKeyboardButton.WithCallbackData("Сварка", nameof(SubdivisionEnum.Welding)),

                        InlineKeyboardButton.WithCallbackData("Лазер", nameof(SubdivisionEnum.Laser))
                    },
                });
                await botClient.SendTextMessageAsync(chatId, "Выберите подразделение:",
                    replyMarkup: inlineKeyboard, cancellationToken: cancellationToken);

                break;

            case nameof(MenuEnum.RemoveTask):

                IsAdmin = null;
                CommandEnum = CommandEnum.Remove;

                inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Гибка/пробивной", nameof(SubdivisionEnum.Stamping)),

                        InlineKeyboardButton.WithCallbackData("Сварка", nameof(SubdivisionEnum.Welding)),

                        InlineKeyboardButton.WithCallbackData("Лазер", nameof(SubdivisionEnum.Laser))
                    },
                });
                await botClient.SendTextMessageAsync(chatId, "Выберите подразделение:",
                    replyMarkup: inlineKeyboard, cancellationToken: cancellationToken);

                break;

            case nameof(MenuEnum.ActiveTasks):

                if (IsAdmin == true)
                {
                    await ActiveTask.GetTasks(SubdivisionEnum, IsAdmin, botClient, cancellationToken, chatId);
                }
                else
                {
                    await ActiveTask.GetTasks(SubdivisionEnum, IsAdmin, botClient, cancellationToken, chatId);
                }

                break;

            case nameof(MenuEnum.Completed):

                CommandEnum = CommandEnum.Completed;

                await JobSubdivision((long)chatId, "", SubdivisionEnum, botClient, addTaskEvent, cancellationToken);
                
                break;

            case nameof(SubdivisionEnum.Stamping):

                SubdivisionEnum = SubdivisionEnum.Stamping;
                name = "\"Гибка/пробивной\"";

                await JobSubdivision((long)chatId, name, SubdivisionEnum, botClient, addTaskEvent, cancellationToken);

                break;

            case nameof(SubdivisionEnum.Welding):

                SubdivisionEnum = SubdivisionEnum.Welding;
                name = "\"Сварка\"";

                await JobSubdivision((long)chatId, name, SubdivisionEnum, botClient, addTaskEvent, cancellationToken);

                break;

            case nameof(SubdivisionEnum.Laser):

                SubdivisionEnum = SubdivisionEnum.Laser;
                name = "\"Лазер\"";

                await JobSubdivision((long)chatId, name, SubdivisionEnum, botClient, addTaskEvent, cancellationToken);

                break;
        }
    }

    public async Task JobSubdivision(long chatId, string name, SubdivisionEnum subdivisionEnum,
        ITelegramBotClient botClient, EventHandler addTaskEvent, CancellationToken cancellationToken)
    {
        switch (CommandEnum)
        {
            case CommandEnum.Subdivision:
            {
                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Активные задачи", nameof(MenuEnum.ActiveTasks)),

                        InlineKeyboardButton.WithCallbackData("Ввести данные", nameof(MenuEnum.Completed)),
                    },
                });

                await botClient.SendTextMessageAsync(chatId, string.Format($"Выбрано подразделение {name}:"),
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);

                break;
            }

            case CommandEnum.Remove:
               await ActiveTask.GetTasks(SubdivisionEnum, IsAdmin, botClient, cancellationToken, chatId);

                addTaskEvent?.Invoke(this, EventArgs.Empty);

                break;

            case CommandEnum.Add:
                addTaskEvent?.Invoke(this, EventArgs.Empty);

                await botClient.SendTextMessageAsync(chatId,
                    string.Format($"Выбрано подразделение {name} . \nВведите задачу формата \"Название=Количество\" : "),
                    cancellationToken: cancellationToken);

                break;

            case CommandEnum.Completed:

                if (IsAdmin == true)
                {
                   await CompletedTask.Complete(SubdivisionEnum, IsAdmin, botClient, cancellationToken, chatId);
                }
                else if (IsAdmin == false)
                {
                    await ActiveTask.GetTasks(SubdivisionEnum, IsAdmin, botClient, cancellationToken, chatId);
                    addTaskEvent?.Invoke(this, EventArgs.Empty);

                    await botClient.SendTextMessageAsync(chatId,
                        string.Format($"\nВведите данные формата \"Номер=Количество\" : "),
                        cancellationToken: cancellationToken);
                }
            

                //if (IsAdmin == true)
                //{
                //    CompletedTask.Complete(SubdivisionEnum, IsAdmin, botClient, cancellationToken, chatId);
                //    addTaskEvent?.Invoke(this, EventArgs.Empty);
                //}
                //else
                //{
                //    CompletedTask.Complete(SubdivisionEnum, IsAdmin, botClient, cancellationToken, chatId);
                //    addTaskEvent?.Invoke(this, EventArgs.Empty);
                //}

                break;
        }
    }
}