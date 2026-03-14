using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamChat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Department_DepartmentId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Team_TeamId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Companies_CompanyId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentMember_CompanyUsers_CompanyUserId",
                table: "DepartmentMember");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentMember_Department_DepartmentId",
                table: "DepartmentMember");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Companies_CompanyId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMember_CompanyUsers_CompanyUserId",
                table: "TeamMember");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMember_Team_TeamId",
                table: "TeamMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamMember",
                table: "TeamMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Team",
                table: "Team");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentMember",
                table: "DepartmentMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Department",
                table: "Department");

            migrationBuilder.RenameTable(
                name: "TeamMember",
                newName: "TeamMembers");

            migrationBuilder.RenameTable(
                name: "Team",
                newName: "Teams");

            migrationBuilder.RenameTable(
                name: "DepartmentMember",
                newName: "DepartmentMembers");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "Departments");

            migrationBuilder.RenameIndex(
                name: "IX_TeamMember_TeamId",
                table: "TeamMembers",
                newName: "IX_TeamMembers_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamMember_CompanyUserId",
                table: "TeamMembers",
                newName: "IX_TeamMembers_CompanyUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Team_CompanyId",
                table: "Teams",
                newName: "IX_Teams_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentMember_DepartmentId",
                table: "DepartmentMembers",
                newName: "IX_DepartmentMembers_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentMember_CompanyUserId",
                table: "DepartmentMembers",
                newName: "IX_DepartmentMembers_CompanyUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Department_CompanyId",
                table: "Departments",
                newName: "IX_Departments_CompanyId");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Messages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PinnedMessageId",
                table: "Chats",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Chats",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamMembers",
                table: "TeamMembers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentMembers",
                table: "DepartmentMembers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Departments_DepartmentId",
                table: "Chats",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Teams_TeamId",
                table: "Chats",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentMembers_CompanyUsers_CompanyUserId",
                table: "DepartmentMembers",
                column: "CompanyUserId",
                principalTable: "CompanyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentMembers_Departments_DepartmentId",
                table: "DepartmentMembers",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Companies_CompanyId",
                table: "Departments",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_CompanyUsers_CompanyUserId",
                table: "TeamMembers",
                column: "CompanyUserId",
                principalTable: "CompanyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Teams_TeamId",
                table: "TeamMembers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Companies_CompanyId",
                table: "Teams",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Departments_DepartmentId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Teams_TeamId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentMembers_CompanyUsers_CompanyUserId",
                table: "DepartmentMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentMembers_Departments_DepartmentId",
                table: "DepartmentMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Companies_CompanyId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_CompanyUsers_CompanyUserId",
                table: "TeamMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Teams_TeamId",
                table: "TeamMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Companies_CompanyId",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamMembers",
                table: "TeamMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentMembers",
                table: "DepartmentMembers");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "PinnedMessageId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Chats");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Team");

            migrationBuilder.RenameTable(
                name: "TeamMembers",
                newName: "TeamMember");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Department");

            migrationBuilder.RenameTable(
                name: "DepartmentMembers",
                newName: "DepartmentMember");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_CompanyId",
                table: "Team",
                newName: "IX_Team_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamMembers_TeamId",
                table: "TeamMember",
                newName: "IX_TeamMember_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamMembers_CompanyUserId",
                table: "TeamMember",
                newName: "IX_TeamMember_CompanyUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_CompanyId",
                table: "Department",
                newName: "IX_Department_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentMembers_DepartmentId",
                table: "DepartmentMember",
                newName: "IX_DepartmentMember_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentMembers_CompanyUserId",
                table: "DepartmentMember",
                newName: "IX_DepartmentMember_CompanyUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Team",
                table: "Team",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamMember",
                table: "TeamMember",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Department",
                table: "Department",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentMember",
                table: "DepartmentMember",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Department_DepartmentId",
                table: "Chats",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Team_TeamId",
                table: "Chats",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Companies_CompanyId",
                table: "Department",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentMember_CompanyUsers_CompanyUserId",
                table: "DepartmentMember",
                column: "CompanyUserId",
                principalTable: "CompanyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentMember_Department_DepartmentId",
                table: "DepartmentMember",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Companies_CompanyId",
                table: "Team",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMember_CompanyUsers_CompanyUserId",
                table: "TeamMember",
                column: "CompanyUserId",
                principalTable: "CompanyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMember_Team_TeamId",
                table: "TeamMember",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
