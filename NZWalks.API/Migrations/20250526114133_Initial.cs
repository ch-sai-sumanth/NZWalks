using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Walks_Regions_RegionsId",
                table: "Walks");

            migrationBuilder.RenameColumn(
                name: "RegionsId",
                table: "Walks",
                newName: "RegionId");

            migrationBuilder.RenameIndex(
                name: "IX_Walks_RegionsId",
                table: "Walks",
                newName: "IX_Walks_RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Walks_Regions_RegionId",
                table: "Walks",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Walks_Regions_RegionId",
                table: "Walks");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "Walks",
                newName: "RegionsId");

            migrationBuilder.RenameIndex(
                name: "IX_Walks_RegionId",
                table: "Walks",
                newName: "IX_Walks_RegionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Walks_Regions_RegionsId",
                table: "Walks",
                column: "RegionsId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
