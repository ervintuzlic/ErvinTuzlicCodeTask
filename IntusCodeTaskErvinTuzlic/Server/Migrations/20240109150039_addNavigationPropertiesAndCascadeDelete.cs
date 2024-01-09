using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntusCodeTaskErvinTuzlic.Server.Migrations
{
    /// <inheritdoc />
    public partial class addNavigationPropertiesAndCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubElements_Windows_WindowId",
                table: "SubElements");

            migrationBuilder.DropForeignKey(
                name: "FK_Windows_Orders_OrderId",
                table: "Windows");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Windows",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WindowId",
                table: "SubElements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubElements_Windows_WindowId",
                table: "SubElements",
                column: "WindowId",
                principalTable: "Windows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Windows_Orders_OrderId",
                table: "Windows",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubElements_Windows_WindowId",
                table: "SubElements");

            migrationBuilder.DropForeignKey(
                name: "FK_Windows_Orders_OrderId",
                table: "Windows");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Windows",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "WindowId",
                table: "SubElements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SubElements_Windows_WindowId",
                table: "SubElements",
                column: "WindowId",
                principalTable: "Windows",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Windows_Orders_OrderId",
                table: "Windows",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
