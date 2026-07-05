
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class removemanualseading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "Description", "DurationDays", "IsActive", "Name", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Access to gym equipment during staffed hours", 30, true, "Basic Plan", 300m, null },
                    { 2, "Includes gym equipment and 2 group classes per week", 60, false, "Standard Plan", 500m, null },
                    { 3, "Unlimited access to equipment, classes, and sauna", 90, false, "Premium Plan", 900m, null },
                    { 4, "Full year access with personal trainer sessions", 365, true, "Annual Plan", 3000m, null }
                });
        }
    }
}
