using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingAppBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class RenameDatetoDateUTC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Bookings",
                newName: "DateUTC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateUTC",
                table: "Bookings",
                newName: "Date");
        }
    }
}
