using Microsoft.EntityFrameworkCore;
using ApiProject.Models;

namespace ApiProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<FaultReport> FaultReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kullanıcılar (1 Admin, 2 User)
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "Admin", Password = "123456", Role = "Admin" },
                new User { Id = 2, UserName = "TestUser1", Password = "123456", Role = "User" },
                new User { Id = 3, UserName = "TestUser2", Password = "123456", Role = "User" }
            );

            // Arıza Bildirimleri (FaultReports)
            modelBuilder.Entity<FaultReport>().HasData(
                new FaultReport { Id = 1, Title = "Elektrik Kesintisi", Description = "Ofiste elektrik kesildi", Location = "İstanbul", Priority = PriorityLevel.High, Status = FaultStatus.New, CreatedByUserId = 2 },
                new FaultReport { Id = 2, Title = "İnternet Bağlantısı", Description = "WiFi çalışmıyor", Location = "Ankara", Priority = PriorityLevel.Medium, Status = FaultStatus.InReview, CreatedByUserId = 2 },
                new FaultReport { Id = 3, Title = "Printer Arızası", Description = "Printer kağıt sıkıştı", Location = "İzmir", Priority = PriorityLevel.Low, Status = FaultStatus.New, CreatedByUserId = 2 },
                new FaultReport { Id = 4, Title = "Klima Sorunu", Description = "Klima çalışmıyor", Location = "Bursa", Priority = PriorityLevel.Medium, Status = FaultStatus.Assigned, CreatedByUserId = 2 },
                new FaultReport { Id = 5, Title = "Bilgisayar Donması", Description = "Bilgisayar sürekli donuyor", Location = "Antalya", Priority = PriorityLevel.High, Status = FaultStatus.New, CreatedByUserId = 2 },
                new FaultReport { Id = 6, Title = "Su Kaçağı", Description = "Ofiste su sızıntısı var", Location = "İstanbul", Priority = PriorityLevel.Medium, Status = FaultStatus.InReview, CreatedByUserId = 2 },
                new FaultReport { Id = 7, Title = "Telefon Hattı Sorunu", Description = "Telefon çalışmıyor", Location = "Ankara", Priority = PriorityLevel.Low, Status = FaultStatus.New, CreatedByUserId = 2 },
                new FaultReport { Id = 8, Title = "Projeksiyon Arızası", Description = "Projeksiyon ışığı yanmıyor", Location = "İzmir", Priority = PriorityLevel.Medium, Status = FaultStatus.Assigned, CreatedByUserId = 3 },
                new FaultReport { Id = 9, Title = "Kapı Kilidi Bozuk", Description = "Giriş kapısı kilitlenmiyor", Location = "Bursa", Priority = PriorityLevel.High, Status = FaultStatus.New, CreatedByUserId = 3 },
                new FaultReport { Id = 10, Title = "Internet Yavaşlığı", Description = "Ağ bağlantısı çok yavaş", Location = "Antalya", Priority = PriorityLevel.Low, Status = FaultStatus.InReview, CreatedByUserId = 3 },
                new FaultReport { Id = 11, Title = "Acil Durum Işığı", Description = "Acil durum ışığı yanmıyor", Location = "İstanbul", Priority = PriorityLevel.High, Status = FaultStatus.Completed, CreatedByUserId = 3 },
                // Admin tarafından açılmış örnek arıza
                new FaultReport { Id = 12, Title = "Admin Test Arızası", Description = "Bu arıza Admin tarafından açıldı", Location = "İstanbul", Priority = PriorityLevel.High, Status = FaultStatus.New, CreatedByUserId = 1 }
            );
        }
    }
}