using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace S3FileLoadApi.Framework.Migrations
{
    public partial class InitRequestStatDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestStat",
                columns: table => new
                {
                    RequestUuid = table.Column<Guid>(nullable: false),
                    RequestStartDateTime = table.Column<DateTime>(nullable: false),
                    RequestCompleateDateTime = table.Column<DateTime>(nullable: false),
                    ObjectName = table.Column<string>(nullable: true),
                    ContentLength = table.Column<long>(nullable: false),
                    ElapsedMs = table.Column<long>(nullable: false),
                    ResponseCode = table.Column<int>(nullable: false),
                    ErrMsg = table.Column<string>(nullable: true),
                    ContentMd5Hash = table.Column<string>(nullable: true),
                    Md5HashMatch = table.Column<bool>(nullable: false),
                    ProcessingHost = table.Column<string>(nullable: true),
                    RemoteClientHost = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStat", x => x.RequestUuid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestStat");
        }
    }
}
