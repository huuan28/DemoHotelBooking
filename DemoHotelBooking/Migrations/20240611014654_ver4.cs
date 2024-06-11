using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoHotelBooking.Migrations
{
    public partial class ver4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_CusID",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FinalCost",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Surchage",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "TotalCost",
                table: "Invoices",
                newName: "Amount");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CusID",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Bookings",
                type: "float",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_CusID",
                table: "Bookings",
                column: "CusID",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_CusID",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Invoices",
                newName: "TotalCost");

            migrationBuilder.AddColumn<double>(
                name: "FinalCost",
                table: "Invoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Surchage",
                table: "Invoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CusID",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalCost",
                table: "Bookings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_CusID",
                table: "Bookings",
                column: "CusID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
