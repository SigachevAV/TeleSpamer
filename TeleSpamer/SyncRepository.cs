using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSpamer.model;

namespace TeleSpamer
{
    internal class SyncRepository
    {
        private DataDbContext db;

        public SyncRepository(DataDbContext _db)
        {
            db = _db;
        }

        public List<TelegramNotification> GetNotificationsForToday()
        {
            List<TelegramNotification> messages;
            lock (db)
            {
                messages = db.telegramNotifications
                    .Where(i => i.day == DateTime.Now.Day)
                    .ToList();
            }
            return messages;
        }

        public void SaveNotification(TelegramNotification _notification)
        {
            lock (db)
            {
                if (db.telegramNotifications.Find(_notification.Username) == null)
                {
                    db.telegramNotifications.Add(_notification);
                }
                else
                {
                    TelegramNotification updating = db.telegramNotifications.Find(_notification.Username);
                    updating.day = _notification.day;
                    updating.message = _notification.message;
                }
                db.SaveChanges();
            }
        }

        public TelegramUser FindUser(string _username) 
        {
            TelegramUser user;
            lock (db) 
            {
                user = db.telegramUsers.Find(_username);
            }
            return user;
        }

        public List<TelegramNotification> GetAllNotifications()
        {
            List<TelegramNotification> result;
            lock (db) 
            {
                result = db.telegramNotifications.ToList();
            }
            return result;
        }

        public void RemoveNotificationsByUsername(string _username)
        {
            lock (db) 
            {
                db.telegramNotifications
                    .Where(i => i.Username == _username)
                    .ToList()
                    .ForEach(i => db.Remove(i));
                db.SaveChanges();
            }
        }

        public void AddUserIfNotExist(TelegramUser _user)
        {
            lock (db)
            {
                if (db.telegramUsers.Find(_user.Username) == null)
                {
                    db.Add(_user);
                }
                db.SaveChanges();
            }
        }
    }
}
