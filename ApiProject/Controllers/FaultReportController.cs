using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FaultReportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FaultReportController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ CREATE
        [HttpPost]
        [HttpPost]
        public IActionResult Create([FromBody] FaultReportCreateDto dto)
        {
            var userName = User.Identity?.Name ?? throw new Exception("User bulunamadı");
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Unauthorized(new { success = false, message = "Kullanıcı bulunamadı" });

            var oneHourAgo = DateTime.UtcNow.AddHours(-1);
            if (_context.FaultReports.Any(f => f.Location == dto.Location && f.CreatedAt >= oneHourAgo))
            {
                return UnprocessableEntity(new { success = false, message = "Aynı lokasyona 1 saat içinde yeni bildirim açılamaz" });
            }

            var fault = new FaultReport
            {
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                Priority = dto.Priority,
                CreatedByUserId = user.Id,
                CreatedByUser = user,
                CreatedAt = DateTime.UtcNow,
                Status = FaultStatus.New
            };

            _context.FaultReports.Add(fault);
            _context.SaveChanges();

            return Ok(new ApiResponse<FaultReport>
            {
                Success = true,
                Data = fault,
                Message = "Bildirim oluşturuldu"
            });
        }

        // ✅ GET ALL (Filtre + Sayfalama + Sıralama)
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] FaultStatus? status,
            [FromQuery] PriorityLevel? priority,
            [FromQuery] string? location,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = "CreatedAt",
            [FromQuery] string? sortDir = "desc"
        )
        {
            var userName = User.Identity?.Name ?? throw new Exception("User bulunamadı");
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Unauthorized(new { success = false, message = "Kullanıcı bulunamadı" });

            var query = _context.FaultReports
                                .Include(f => f.CreatedByUser)
                                .AsQueryable();

            bool isAdmin = user.Role == "Admin";

            if (!isAdmin)
                query = query.Where(f => f.CreatedByUserId == user.Id);

            if (status.HasValue)
                query = query.Where(f => f.Status == status);

            if (priority.HasValue)
                query = query.Where(f => f.Priority == priority);

            if (!string.IsNullOrEmpty(location))
                query = query.Where(f => f.Location.Contains(location));

            query = (sortBy?.ToLower(), sortDir?.ToLower()) switch
            {
                ("priority", "asc") => query.OrderBy(f => f.Priority),
                ("priority", "desc") => query.OrderByDescending(f => f.Priority),
                (_, "asc") => query.OrderBy(f => f.CreatedAt),
                _ => query.OrderByDescending(f => f.CreatedAt),
            };

            var totalItems = query.Count();

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Data = new
                {
                    items,
                    page,
                    pageSize,
                    totalItems
                },
                Message = "Listeleme başarılı"
            });
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var userName = User.Identity?.Name ?? throw new Exception("User bulunamadı");
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Unauthorized();

            var report = _context.FaultReports
                .Include(f => f.CreatedByUser)
                .FirstOrDefault(f => f.Id == id);

            if (report == null)
                return NotFound(new { success = false, message = "Kayıt bulunamadı" });

            if (user.Role != "Admin" && report.CreatedByUserId != user.Id)
                return Forbid();

            return Ok(new ApiResponse<FaultReport>
            {
                Success = true,
                Data = report,
                Message = "Kayıt bulundu"
            });
        }

        // ✅ UPDATE
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] FaultReport dto)
        {
            var userName = User.Identity?.Name ?? throw new Exception("User bulunamadı");
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Unauthorized();

            var report = _context.FaultReports.FirstOrDefault(f => f.Id == id);
            if (report == null)
                return NotFound(new { success = false, message = "Kayıt bulunamadı" });

            if (user.Role != "Admin" && report.CreatedByUserId != user.Id)
                return Forbid();

            report.Title = dto.Title;
            report.Description = dto.Description;
            report.Location = dto.Location;
            report.Priority = dto.Priority;
            report.UpdatedAt = DateTime.UtcNow;

            if (user.Role == "Admin")
            {
                report.Status = dto.Status;
            }
            _context.SaveChanges();

            return Ok(new ApiResponse<FaultReport>
            {
                Success = true,
                Data = report,
                Message = "Güncellendi"
            });
        }

        // ✅ STATUS CHANGE (SADECE ADMIN)
        [HttpPatch("{id}/status")]
        public IActionResult ChangeStatus(int id, [FromBody] FaultStatus newStatus)
        {
            var userName = User.Identity?.Name ?? throw new Exception("User bulunamadı");
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Unauthorized();

            if (user.Role != "Admin")
                return Forbid();

            var report = _context.FaultReports.FirstOrDefault(f => f.Id == id);
            if (report == null)
                return NotFound(new { success = false, message = "Kayıt bulunamadı" });
            if (!FaultStatusHelper.StatusTransitions.TryGetValue(report.Status, out var allowedStatuses) ||
        !allowedStatuses.Contains(newStatus))
            {
                return UnprocessableEntity(new
                {
                    success = false,
                    message = $"Geçersiz durum geçişi: {report.Status} → {newStatus}"
                });
            }
            report.Status = newStatus;
            report.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok(new ApiResponse<FaultReport>
            {
                Success = true,
                Data = report,
                Message = "Status güncellendi"
            });
        }
        // FaultReportController.cs içinde
        [HttpGet("ratelimit-test")]
        [AllowAnonymous] // login gerekmez
        public IActionResult RateLimitTest()
        {
            return Ok("Rate Limiting testi başarılı!");
        }
    }
}
