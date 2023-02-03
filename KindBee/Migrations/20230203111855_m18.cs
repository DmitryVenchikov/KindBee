using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KindBee.Migrations
{
    /// <inheritdoc />
    public partial class m18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_ProductId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProductId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Orders",
                newName: "PositionId");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Position",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Position_OrderId",
                table: "Position",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Position_Orders_OrderId",
                table: "Position",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Position_Orders_OrderId",
                table: "Position");

            migrationBuilder.DropIndex(
                name: "IX_Position_OrderId",
                table: "Position");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Position");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                table: "Orders",
                newName: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_ProductId",
                table: "Orders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
