using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class pg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 1, 0, 43, 59, 615, DateTimeKind.Utc).AddTicks(1062), "$2a$11$v6JlUe0QjQp3K7keeLMJeuI3Xc40W94LjOO139weMeVfTnrUQ/vai" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 25, 15, 33, 29, 439, DateTimeKind.Utc).AddTicks(680), "$2a$11$mf93zpy8PyZXfL3E1KqvH.SaTC8HXepYIlA.9Q8X8UTWkcyacOBKC" });
        }
    }
}
