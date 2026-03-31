using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FaultReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaultReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaultReports_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "123456", "Admin", "Admin" },
                    { 2, "123456", "User", "TestUser1" },
                    { 3, "123456", "User", "TestUser2" }
                });

            migrationBuilder.InsertData(
                table: "FaultReports",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "Description", "Location", "Priority", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 31, 10, 17, 43, 825, DateTimeKind.Utc).AddTicks(3856), 2, "Ofiste elektrik kesildi", "İstanbul", 2, 0, "Elektrik Kesintisi", null },
                    { 2, new DateTime(2026, 3, 31, 10, 17, 43, 825, DateTimeKind.Utc).AddTicks(3860), 2, "WiFi çalışmıyor", "Ankara", 1, 1, "İnternet Bağlantısı", null },
                    { 3, new DateTime(2026, 3, 31, 10, 17, 43, 825, DateTimeKind.Utc).AddTicks(3862), 2, "Printer kağıt sıkıştı", "İzmir", 0, 0, "Printer Arızası", null },
                    { 4, new DateTime(2026, 3, 31, 10, 17, 43, 825, DateTimeKind.Utc).AddTicks(3863), 2, "Klima çalışmıyor", "Bursa", 1, 2, "Klima Sorunu", null },
                    { 5, new DateTime(2026, 3, 31, 10, 17, 43, 825, DateTimeKind.Utc).AddTicks(3864), 2, "Bilgisayar sürekli donuyor", "Antalya", 2, 0, "Bilgisayar Donması", null },
                    { 6, new DateTime(2026, 3, 31, 10, 17, 43, 825, DateTimeKind.Utc).AddTicks(3865), 2, "Ofiste su sızıntısı var", "İstanbul", 1, 1, "Su Kaçağı", null },
                    { 7, new DateTime(2026, 3, 31, 10, 17, 43, 825, DateTimeKind.Utc).AddTicks(3866), 2, "Telefon çalışmıyor", "Ankara", 0, 0, "Telefon Hattı Sorunu", null },
                    { 8, new DateTime(2026, 3, 31, 10, 17, 43, 825, DateTimeKind.Utc).AddTicks(3868), 3, "Projeksiyon ışığı yanmıyor", "İzmir", 1, 2, "Projeksiyon Arızası", null },
                    { 9, new DateTime(2026, 3, 31, 10, 17, 43, 825, DateTimeKind.Utc).AddTicks(3869), 3, "Giriş kapısı kilitlenmiyor", "Bursa", 2, 0, "Kapı Kilidi Bozuk", null },
                    { 10, new DateTime(2026, 3, 31, 10, 17, 43, 825, DateTimeKind.Utc).AddTicks(3870), 3, "Ağ bağlantısı çok yavaş", "Antalya", 0, 1, "Internet Yavaşlığı", null },
                    { 11, new DateTime(2026, 3, 31, 10, 17, 43, 825, DateTimeKind.Utc).AddTicks(3871), 3, "Acil durum ışığı yanmıyor", "İstanbul", 2, 4, "Acil Durum Işığı", null },
                    { 12, new DateTime(2026, 3, 31, 10, 17, 43, 825, DateTimeKind.Utc).AddTicks(3872), 1, "Bu arıza Admin tarafından açıldı", "İstanbul", 2, 0, "Admin Test Arızası", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FaultReports_CreatedByUserId",
                table: "FaultReports",
                column: "CreatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaultReports");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
