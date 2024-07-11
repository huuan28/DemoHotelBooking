using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoHotelBooking.Migrations
{
    public partial class updateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetail_Invoices_InvoiceId",
                table: "InvoiceDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetail_Rooms_RoomId",
                table: "InvoiceDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceDetail",
                table: "InvoiceDetail");

            migrationBuilder.RenameTable(
                name: "InvoiceDetail",
                newName: "InvoiceDetails");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDetail_RoomId",
                table: "InvoiceDetails",
                newName: "IX_InvoiceDetails_RoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceDetails",
                table: "InvoiceDetails",
                columns: new[] { "InvoiceId", "RoomId" });

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Invoices_InvoiceId",
                table: "InvoiceDetails",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Rooms_RoomId",
                table: "InvoiceDetails",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Invoices_InvoiceId",
                table: "InvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Rooms_RoomId",
                table: "InvoiceDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceDetails",
                table: "InvoiceDetails");

            migrationBuilder.RenameTable(
                name: "InvoiceDetails",
                newName: "InvoiceDetail");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDetails_RoomId",
                table: "InvoiceDetail",
                newName: "IX_InvoiceDetail_RoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceDetail",
                table: "InvoiceDetail",
                columns: new[] { "InvoiceId", "RoomId" });

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetail_Invoices_InvoiceId",
                table: "InvoiceDetail",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetail_Rooms_RoomId",
                table: "InvoiceDetail",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
