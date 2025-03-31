using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
