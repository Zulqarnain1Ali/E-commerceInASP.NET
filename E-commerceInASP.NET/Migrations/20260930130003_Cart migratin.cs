using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerceInASP.NET.Migrations
{
    /// <inheritdoc />
    public partial class Cartmigratin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_cart",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product_Quantity = table.Column<int>(type: "int", nullable: false),
                    Cart_Status = table.Column<bool>(type: "bit", nullable: false),
                    Product_Id = table.Column<int>(type: "int", nullable: false),
                    Customer_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_cart", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_cart");
        }
    }
}
