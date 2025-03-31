using TeleSpamer;

class Program
{
    public static void Main(string[] args)
    {
        DataDbContext db = new DataDbContext();
        db.init();
        TelegramHandler handler = new TelegramHandler("", db);
        handler.Start();
        Console.ReadLine();
        return;
    }
}