using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TeamChat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUsers_Companies_CompanyId1",
                table: "CompanyUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUsers_Positions_PositionId1",
                table: "CompanyUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUsers_Users_UserId1",
                table: "CompanyUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Companies_CompanyId1",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_CompanyId1",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUsers_CompanyId1",
                table: "CompanyUsers");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUsers_PositionId1",
                table: "CompanyUsers");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUsers_UserId1",
                table: "CompanyUsers");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "CompanyUsers");

            migrationBuilder.DropColumn(
                name: "PositionId1",
                table: "CompanyUsers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "CompanyUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "PinnedMessageId",
                table: "Chats",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "Scope",
                table: "Chats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChatPositionAccess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PositionId = table.Column<int>(type: "integer", nullable: false),
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionOverride = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatPositionAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatPositionAccess_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatPositionAccess_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatPositionAccess_ChatId",
                table: "ChatPositionAccess",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatPositionAccess_PositionId_ChatId",
                table: "ChatPositionAccess",
                columns: new[] { "PositionId", "ChatId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatPositionAccess");

            migrationBuilder.DropColumn(
                name: "Scope",
                table: "Chats");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId1",
                table: "Positions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId1",
                table: "CompanyUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PositionId1",
                table: "CompanyUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "CompanyUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PinnedMessageId",
                table: "Chats",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_CompanyId1",
                table: "Positions",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_CompanyId1",
                table: "CompanyUsers",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_PositionId1",
                table: "CompanyUsers",
                column: "PositionId1");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_UserId1",
                table: "CompanyUsers",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUsers_Companies_CompanyId1",
                table: "CompanyUsers",
                column: "CompanyId1",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUsers_Positions_PositionId1",
                table: "CompanyUsers",
                column: "PositionId1",
                principalTable: "Positions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUsers_Users_UserId1",
                table: "CompanyUsers",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Companies_CompanyId1",
                table: "Positions",
                column: "CompanyId1",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
