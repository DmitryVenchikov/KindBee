using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KindBee.Migrations
{
    /// <inheritdoc />
    public partial class m28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Baskets_BasketId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_BasketId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Baskets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_CustomerId",
                table: "Baskets",
                column: "CustomerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Customers_CustomerId",
                table: "Baskets",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Customers_CustomerId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_CustomerId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Baskets");

            migrationBuilder.AddColumn<int>(
                name: "BasketId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_BasketId",
                table: "Customers",
                column: "BasketId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Baskets_BasketId",
                table: "Customers",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
