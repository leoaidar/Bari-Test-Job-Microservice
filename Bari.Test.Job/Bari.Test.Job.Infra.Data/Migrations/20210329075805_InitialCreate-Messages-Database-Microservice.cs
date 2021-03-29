using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bari.Test.Job.Infra.Data.Migrations
{
    public partial class InitialCreateMessagesDatabaseMicroservice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    Invalid = table.Column<bool>(nullable: false),
                    Body = table.Column<string>(type: "varchar(255)", nullable: true),
                    Timestamp = table.Column<double>(nullable: false),
                    ServiceId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");
        }
    }
}
