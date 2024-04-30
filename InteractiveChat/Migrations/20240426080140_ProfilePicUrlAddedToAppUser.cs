using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteractiveChat.Migrations
{
    /// <inheritdoc />
    public partial class ProfilePicUrlAddedToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicUrl",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicUrl",
                table: "AspNetUsers");
        }
    }
}
