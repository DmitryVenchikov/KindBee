using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KindBee.Migrations
{
    /// <inheritdoc />
    public partial class m19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Position_Baskets_BasketId",
                table: "Position");

            migrationBuilder.DropForeignKey(
                name: "FK_Position_Orders_OrderId",
                table: "Position");

            migrationBuilder.DropForeignKey(
                name: "FK_Position_Products_ProductId",
                table: "Position");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Position",
                table: "Position");

            migrationBuilder.RenameTable(
                name: "Position",
                newName: "Positions");

            migrationBuilder.RenameIndex(
                name: "IX_Position_ProductId",
                table: "Positions",
                newName: "IX_Positions_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Position_OrderId",
                table: "Positions",
                newName: "IX_Positions_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Position_BasketId",
                table: "Positions",
                newName: "IX_Positions_BasketId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Positions",
                table: "Positions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Baskets_BasketId",
                table: "Positions",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Orders_OrderId",
                table: "Positions",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Products_ProductId",
                table: "Positions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Baskets_BasketId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Orders_OrderId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Products_ProductId",
                table: "Positions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Positions",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Positions",
                newName: "Position");

            migrationBuilder.RenameIndex(
                name: "IX_Positions_ProductId",
                table: "Position",
                newName: "IX_Position_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Positions_OrderId",
                table: "Position",
                newName: "IX_Position_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Positions_BasketId",
                table: "Position",
                newName: "IX_Position_BasketId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Position",
                table: "Position",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Position_Baskets_BasketId",
                table: "Position",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Position_Orders_OrderId",
                table: "Position",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Position_Products_ProductId",
                table: "Position",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
