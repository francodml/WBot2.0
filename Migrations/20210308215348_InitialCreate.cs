using Microsoft.EntityFrameworkCore.Migrations;

namespace WBot2.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    ID = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WatchedMessages",
                columns: table => new
                {
                    ID = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmojiRolesJS = table.Column<string>(type: "TEXT", nullable: true),
                    GuildID = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchedMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WatchedMessages_Guilds_GuildID",
                        column: x => x.GuildID,
                        principalTable: "Guilds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchedMessages_GuildID",
                table: "WatchedMessages",
                column: "GuildID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchedMessages");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
