using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamChat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Positions_ParentPositionId",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_MessageReadStatuses_MessageId",
                table: "MessageReadStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUsers_UserId",
                table: "CompanyUsers");

            migrationBuilder.DropIndex(
                name: "IX_ChatRoles_ChatId",
                table: "ChatRoles");

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

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_CompanyId1",
                table: "Positions",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReadStatuses_MessageId_UserId",
                table: "MessageReadStatuses",
                columns: new[] { "MessageId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_CompanyId1",
                table: "CompanyUsers",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_PositionId1",
                table: "CompanyUsers",
                column: "PositionId1");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_UserId_CompanyId",
                table: "CompanyUsers",
                columns: new[] { "UserId", "CompanyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_UserId1",
                table: "CompanyUsers",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoles_ChatId_Name",
                table: "ChatRoles",
                columns: new[] { "ChatId", "Name" },
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Positions_ParentPositionId",
                table: "Positions",
                column: "ParentPositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Positions_ParentPositionId",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Positions_CompanyId1",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_MessageReadStatuses_MessageId_UserId",
                table: "MessageReadStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUsers_CompanyId1",
                table: "CompanyUsers");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUsers_PositionId1",
                table: "CompanyUsers");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUsers_UserId_CompanyId",
                table: "CompanyUsers");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUsers_UserId1",
                table: "CompanyUsers");

            migrationBuilder.DropIndex(
                name: "IX_ChatRoles_ChatId_Name",
                table: "ChatRoles");

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

            migrationBuilder.CreateIndex(
                name: "IX_MessageReadStatuses_MessageId",
                table: "MessageReadStatuses",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_UserId",
                table: "CompanyUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoles_ChatId",
                table: "ChatRoles",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Positions_ParentPositionId",
                table: "Positions",
                column: "ParentPositionId",
                principalTable: "Positions",
                principalColumn: "Id");
        }
    }
}
