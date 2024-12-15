using designclub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace designclub
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }  // Добавление свойства для Users
        public DbSet<Minutepackage> Minutepackages { get; set; }  // Добавьте эту строк
        public DbSet<Client> Clients { get; set; }

        public DbSet<Providedminutepackage> Providedminutepackages { get; set; }

        // Убедитесь, что правильно настроены строки подключения к базе данных
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(DatabaseConfiguration.GetConnectionString(),
                    DatabaseConfiguration.ServerVersion); // Укажите свою строку подключения
            }
        }

        // Если необходимо, добавьте дополнительные конфигурации
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Дополнительные настройки для моделей (например, уникальные ограничения, индексы)
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Phone).IsUnique();  // Уникальность номера телефона
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Email).IsUnique();  // Уникальность Email
        }
    }
}
