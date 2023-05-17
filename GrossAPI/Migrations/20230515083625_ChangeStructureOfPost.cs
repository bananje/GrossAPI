using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrossAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStructureOfPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Posts_PostId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_PostId",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ImageId",
                table: "Posts",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Images_ImageId",
                table: "Posts",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Images_ImageId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ImageId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Posts");

            migrationBuilder.CreateIndex(
                name: "IX_Images_PostId",
                table: "Images",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Posts_PostId",
                table: "Images",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
