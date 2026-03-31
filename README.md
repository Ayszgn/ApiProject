# Elektrik Dağıtım Arıza Bildirim API

Bu proje, bir elektrik dağıtım şirketinin saha ekibi tarafından bildirilen arızaları alan, izleyen ve yöneten bir REST API sağlamaktadır. API, arızaların kaydedilmesi, durum takibi ve yönetimi için gerekli CRUD operasyonlarını desteklemektedir.
## Projeyi Çalıştırma

### 1️⃣ Local .NET ile

Projeyi kendi bilgisayarınızda çalıştırmak için:

1. Repo’yu klonlayın:
```bash
git clone <repo-link>
cd ApiProject
2. Paketleri yükleyin:
dotnet restore
3. Veritabanını oluşturun ve migrationları uygulayın:
dotnet ef database update
4. API'yi çalıştırın:
dotnet run
5.Swagger UI’ye terminalde görünen port üzerinden erişebilirsiniz (ör. http://localhost:5000/swagger)

### 2️⃣ Docker Compose (Opsiyonel Bonus)

Projeyi Docker ile tek komutla ayağa kaldırmak için:

1. Docker ve Docker Compose kurulu olmalı.
2. Kök klasörde çalıştırın:
```bash
docker compose up --build
3.Swagger'a erişim:
http://localhost:8080/swagger
4.Sql Server'a erişim (host üzerinden, opsiyonel):
Server=localhost,1433;Database=ApiDb;User Id=sa;Password=YourStrong@Passw0rd;

## Kullanılan Teknolojiler ve Kütüphaneler

### Teknolojiler
- **.NET 8** — Web API geliştirme  
- **C#** — Programlama dili  
- **SQL Server 2022** — Veritabanı  
- **Entity Framework Core** — ORM ve migration yönetimi  
- **JWT (JSON Web Token)** — Kimlik doğrulama ve yetkilendirme  
- **Swagger / OpenAPI** — API dokümantasyonu ve test arayüzü  
- **ILogger (built-in) / Serilog** — Loglama ve hata takibi  
- **Docker & Docker Compose** — Opsiyonel tek komutla çalıştırma

### Kütüphaneler / NuGet Paketleri
- `Microsoft.AspNetCore.Authentication.JwtBearer` — JWT kimlik doğrulama  
- `Microsoft.EntityFrameworkCore` — EF Core temel paket  
- `Microsoft.EntityFrameworkCore.Design` — EF Core tasarım zamanı araçları  
- `Microsoft.EntityFrameworkCore.SqlServer` — SQL Server desteği  
- `Microsoft.EntityFrameworkCore.Tools` — Migration ve CLI araçları  
- `Serilog.AspNetCore` — Loglama için Serilog entegrasyonu  
- `Swashbuckle.AspNetCore` — Swagger/OpenAPI dokümantasyonu  
- `Swashbuckle.AspNetCore.Annotations` — Swagger açıklamaları için

## Durum Makinesi ve Öncelik Seviyeleri

### Öncelik Seviyeleri
Projede arıza bildirimleri **öncelik seviyelerine** göre sınıflandırılmıştır:

- **Low (0)** — Düşük öncelikli bildirimler  
- **Medium (1)** — Orta öncelikli bildirimler  
- **High (2)** — Yüksek öncelikli bildirimler

### Bildirim Durumları
Bildirimler aşağıdaki durumları alabilir (enum değerleri parantez içinde gösterilmiştir):

- **New (0)** — Yeni kayıt, henüz işleme alınmamış  
- **InReview (1)** — İnceleniyor, Admin tarafından değerlendirme aşamasında  
- **Assigned (2)** — Atandı, belirli personele atanmış  
- **Working (3)** — Çalışılıyor, saha ekibi müdahale ediyor  
- **Completed (4)** — Tamamlandı, iş tamamlanmış  
- **Cancelled (5)** — İptal edilmiş  
- **Rejected (6)** — Asılsız veya reddedilmiş bildirim

### Durum Geçiş Kuralları
- **New → InReview** — yalnızca **Admin**  
- **InReview → Assigned / Rejected** — yalnızca **Admin**  
- **Assigned → Working / Cancelled** — yalnızca **Admin**  
- **Working → Completed / Cancelled** — yalnızca **Admin**  
- **New / InReview / Assigned / Working → Cancelled** — yalnızca **Admin**  
- **Completed / Cancelled / Rejected** — terminal durumlar, buradan geçiş yok  

### Rol Bazlı Yetkiler
- **User:** Sadece kendi bildirimlerini görebilir ve güncelleyebilir. Durum geçişi yapamaz  
- **Admin:** Tüm bildirimleri görebilir ve yukarıdaki geçişleri gerçekleştirebilir  

> **Business Rule Örneği:** Aynı lokasyona 1 saat içinde ikinci bildirim açılamaz. Kurallar ihlal edildiğinde API `422 Unprocessable Entity` döner ve açıklayıcı hata mesajı sağlar.

## Alınan Mimari Kararlar ve Gerekçeleri
### 1️⃣ Katmanlı Mimari (Layered Architecture)
Karar: Proje Controllers, Models, Data, Services, Middleware şeklinde katmanlara ayrıldı.
Gerekçe:
-Kodun okunabilirliği ve bakımı kolaylaşır.
-Controller’lar sadece API uç noktalarını yönetir, iş mantığı Services içinde tutulur.
-Veri erişimi AppDbContext üzerinden merkezi ve güvenli şekilde yapılır.
### 2️⃣ Entity Framework Core (EF Core) Kullanımı
Karar: Veri tabanı işlemleri için EF Core kullanıldı (DbContext, DbSet).
Gerekçe:
-SQL Server ile sorunsuz ORM desteği sağlar.
-Migrations ile veri tabanı versiyon kontrolü ve seed data yönetimi kolaydır.
-LINQ sorguları ile güçlü ve tip güvenli sorgulama sağlar.
### 3️⃣ JWT ile Yetkilendirme
Karar: Kullanıcı girişleri ve yetkilendirme JWT ile yapıldı.
Gerekçe:
-Stateless authentication ile API güvenliği sağlanır.
-Rollere göre erişim kontrolü kolayca uygulanabilir (Admin / User ayrımı).
-Token tabanlı sistem, Docker veya dağıtık sistemlerde ölçeklenebilir.
### 4️⃣ State Machine ile Arıza Durum Yönetimi
Karar: Arıza (FaultReport) durum geçişleri FaultStatusHelper ile kontrol ediliyor.
Gerekçe:
-Belirli iş kuralları çerçevesinde geçişleri sınırlandırır.
-Hatalı durum değişikliklerinin önüne geçer.
-Kodun okunabilirliği ve bakımını kolaylaştırır.
### 5️⃣ Middleware ile Hata Yönetimi
Karar: Tüm API çağrılarında yakalanmamış hatalar ExceptionMiddleware ile yönetildi.
Gerekçe:
-Tek bir noktadan merkezi hata yönetimi sağlar.
-API kullanıcılarına tutarlı JSON hata yanıtı döner.
-Logging ile hata takibi kolaylaşır.
### 6️⃣ Docker ve Docker Compose Kullanımı
Karar: Proje ve SQL Server için Docker Compose yapılandırması hazırlandı.
Gerekçe:
-Tek komutla çalıştırılabilir, taşınabilir geliştirme ortamı sağlar.
-Veritabanı ve API aynı anda ayağa kaldırılabilir.
-Farklı sistemlerde minimum kurulum ve konfigürasyon ile çalıştırma imkânı.
### 7️⃣ DTO Kullanımı
Karar: UserLoginDto, UserRegisterDto, FaultReportCreateDto gibi Data Transfer Object’ler kullanıldı.
Gerekçe:
-API ile veri tabanı modelleri arasında ayrım sağlar.
-Gereksiz veya hassas alanların istemciye gönderilmesini engeller.
-Kodun güvenliğini ve okunabilirliğini artırır.

### Yetiştirilemeyen / Eksik Kısımlar

Proje Docker ve Docker Compose ile çalışacak şekilde yapılandırıldı ve Swagger üzerinden temel API fonksiyonları test edildi. Ancak container ortamının tam olarak çalışıp çalışmadığı, veritabanı bağlantıları ve diğer servislerin üretim ortamında doğrulanması gerçekleştirilemedi. Bu nedenle Docker ile ilgili bazı optimizasyonlar ve otomatik migration/seed işlemleri tamamlanmadı.
