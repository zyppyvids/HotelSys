using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false),
                    PersonalId = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    MiddleName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    DateАppointment = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateDismissal = table.Column<DateTime>(nullable: true)
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Аdult = table.Column<bool>(nullable: false)
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Number = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: false),
                    Capacity = table.Column<int>(nullable: false),
                    Free = table.Column<bool>(nullable: false),
                    BedPriceAdult = table.Column<float>(nullable: false),
                    BedPriceChild = table.Column<float>(nullable: false)
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    RoomNumber = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ClientIds = table.Column<int[]>(nullable: false),
                    DateAccommodation = table.Column<DateTime>(nullable: false),
                    DateRelease = table.Column<DateTime>(nullable: false),
                    BreakfastIncluded = table.Column<bool>(nullable: false),
                    AllInclusive = table.Column<bool>(nullable: false),
                    PaymentAmount = table.Column<float>(nullable: false)
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Users");
            migrationBuilder.DropTable(name: "Clients");
            migrationBuilder.DropTable(name: "Rooms");
            migrationBuilder.DropTable(name: "Reservations");
        }
    }
}
