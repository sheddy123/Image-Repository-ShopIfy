using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageRepoAPI.Migrations
{
    public partial class ImageDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageUploads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Images = table.Column<byte[]>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ImageDescription = table.Column<string>(nullable: false),
                    FileType = table.Column<string>(nullable: false),
                    FileExtension = table.Column<string>(nullable: false),
                    ImageName = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ImageClassification = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageUploads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageUploads");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
