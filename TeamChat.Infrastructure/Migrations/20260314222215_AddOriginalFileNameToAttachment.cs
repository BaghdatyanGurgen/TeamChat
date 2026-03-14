using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamChat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOriginalFileNameToAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "MessageAttachments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "MessageAttachments");
        }
    }
}
