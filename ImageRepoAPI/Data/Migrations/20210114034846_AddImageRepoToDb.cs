using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageRepoAPI.Migrations
{
    public partial class AddImageRepoToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageUploads",
                columns: table => new
                {
                    ImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Images = table.Column<byte[]>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ImageDescription = table.Column<string>(nullable: false),
                    FileType = table.Column<string>(nullable: false),
                    FileExtension = table.Column<string>(nullable: false),
                    ImageName = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageUploads", x => x.ImageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageUploads");
        }
    }
}
