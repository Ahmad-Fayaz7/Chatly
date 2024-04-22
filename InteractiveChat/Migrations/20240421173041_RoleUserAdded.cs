using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteractiveChat.Migrations
{
    /// <inheritdoc />
    public partial class RoleUserAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cf271777-3446-4818-a0e9-615a2c260e51", null, "user", "user" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf271777-3446-4818-a0e9-615a2c260e51");
        }
    }
}
