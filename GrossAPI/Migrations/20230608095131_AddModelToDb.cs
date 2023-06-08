using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrossAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddModelToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackOrders_Statuses_StatusId",
                table: "FeedbackOrders");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackOrders_StatusId",
                table: "FeedbackOrders");

            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "FeedbackOrders");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "FeedbackOrders");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Orders",
                newName: "OrdersId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Responses",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "FeedbackOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Responses",
                table: "Responses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "OrdersServices",
                columns: table => new
                {
                    OrdersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersServices", x => new { x.OrdersId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_OrdersServices_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "OrdersId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersServices_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdersServices_ServicesId",
                table: "OrdersServices",
                column: "ServicesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdersServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Responses",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FeedbackOrders");

            migrationBuilder.RenameColumn(
                name: "OrdersId",
                table: "Orders",
                newName: "Id");

            migrationBuilder.AddColumn<Guid>(
                name: "StatusID",
                table: "FeedbackOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "FeedbackOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackOrders_StatusId",
                table: "FeedbackOrders",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackOrders_Statuses_StatusId",
                table: "FeedbackOrders",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
