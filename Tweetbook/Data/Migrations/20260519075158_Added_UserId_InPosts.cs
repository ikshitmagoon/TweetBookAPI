using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TweetBook.Data.Migrations
{
    /// <inheritdoc />
    public partial class Added_UserId_InPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_userId",
                table: "Posts",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_userId",
                table: "Posts",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_userId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_userId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Posts");
        }
    }
}
