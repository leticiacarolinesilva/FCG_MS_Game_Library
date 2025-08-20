using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRegistrationAndGameLibrary.Infra.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterMS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_gamelibraries_users_UserId",
                schema: "game_platform",
                table: "gamelibraries");

            migrationBuilder.DropTable(
                name: "userAuthorizations",
                schema: "game_platform");

            migrationBuilder.DropTable(
                name: "users",
                schema: "game_platform");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "game_platform",
                table: "gamelibraries",
                newName: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userId",
                schema: "game_platform",
                table: "gamelibraries",
                newName: "UserId");

            migrationBuilder.CreateTable(
                name: "users",
                schema: "game_platform",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userAuthorizations",
                schema: "game_platform",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    permission = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userAuthorizations", x => x.id);
                    table.ForeignKey(
                        name: "FK_userAuthorizations_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "game_platform",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userAuthorizations_UserId",
                schema: "game_platform",
                table: "userAuthorizations",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_gamelibraries_users_UserId",
                schema: "game_platform",
                table: "gamelibraries",
                column: "UserId",
                principalSchema: "game_platform",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
