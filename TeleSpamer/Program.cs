using TeleSpamer;

class Program
{
    public static void Main(string[] args)
    {
        string telegramToken = Environment.GetEnvironmentVariable("TELEGRAM_TOKEN");
        string admin = Environment.GetEnvironmentVariable ("ADMIN");
        if (telegramToken == null || admin == null)
        {
            Console.WriteLine("unsuficient configuration");
            return;
        }

        DataDbContext db = new DataDbContext();
        db.init();
        TelegramHandler handler = new TelegramHandler(telegramToken, db, admin);
        handler.Start();
        Console.ReadLine();
        return;
    }
}