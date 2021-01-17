using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageRepoAPI.Migrations
{
    public partial class RemoveImageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "ImageUploads");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "ImageUploads",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
