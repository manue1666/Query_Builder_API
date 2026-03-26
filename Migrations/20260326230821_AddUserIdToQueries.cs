using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueryBuilderAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToQueries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Queries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Queries_UserId",
                table: "Queries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Queries_Users_UserId",
                table: "Queries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queries_Users_UserId",
                table: "Queries");

            migrationBuilder.DropIndex(
                name: "IX_Queries_UserId",
                table: "Queries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Queries");
        }
    }
}
