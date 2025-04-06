using Microsoft.EntityFrameworkCore;
using TeleSpamer.model;

namespace TeleSpamer
{
    internal class DataDbContext : DbContext
    {
        public DbSet<TelegramUser> telegramUsers { get; set; }
        public DbSet<TelegramNotification> telegramNotifications { get; set; }

        public string DbPath { get; set; }


        public DataDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            Console.WriteLine("database Path " + path);
            DbPath = System.IO.Path.Join(path, "TeleSpamer.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        public void init()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
