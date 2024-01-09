using Newtonsoft.Json;
using Telegram.Bot;
using Telegramm_task.Core.Infrastructure;

class Program
{
     private static string? _token ;

     private static ITelegramBotClient _bot;


    static void Main(string[] args)
    {
        using (StreamReader reader = new StreamReader("Config/StartConfig.json"))
        {
            _token = reader.ReadToEnd();
        }

        _bot = new TelegramBotClient(_token);

        Console.WriteLine("Запущен бот " + _bot.GetMeAsync().Result.FirstName);
        Runner runner = new Runner(_bot);        

        Console.ReadLine();

        Console.WriteLine("Hello, World!");
    }
}