using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerceInASP.NET.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedProductCategoryRelationshipforiegnkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cat_Id",
                table: "tbl_product",
                newName: "Category_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_product",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "tbl_category",
                newName: "Category_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_Category_id",
                table: "tbl_product",
                column: "Category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_category_Category_id",
                table: "tbl_product",
                column: "Category_id",
                principalTable: "tbl_category",
                principalColumn: "Category_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_category_Category_id",
                table: "tbl_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_Category_id",
                table: "tbl_product");

            migrationBuilder.RenameColumn(
                name: "Category_id",
                table: "tbl_product",
                newName: "Cat_Id");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "tbl_product",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Category_id",
                table: "tbl_category",
                newName: "Key");
        }
    }
}
