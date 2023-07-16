using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeanScene.Migrations
{
    /// <inheritdoc />
    public partial class TimeslotAndSittingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SittingType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SittingType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Timeslot",
                columns: table => new
                {
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timeslot", x => x.Time);
                });

            migrationBuilder.InsertData(
                table: "SittingType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Breakfast" },
                    { 2, "Lunch" },
                    { 3, "Dinner" }
                });

            migrationBuilder.InsertData(
                table: "Timeslot",
                column: "Time",
                values: new object[]
                {
                    new DateTime(2000, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 8, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 9, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 10, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 11, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 11, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 12, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 13, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 13, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 14, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 14, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 15, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 15, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 16, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 17, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 17, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 18, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 18, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 19, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 19, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 20, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 20, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 21, 30, 0, 0, DateTimeKind.Unspecified),
                    new DateTime(2000, 1, 1, 22, 0, 0, 0, DateTimeKind.Unspecified)
                });

            migrationBuilder.CreateIndex(
                name: "IX_SittingType_Name",
                table: "SittingType",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SittingType");

            migrationBuilder.DropTable(
                name: "Timeslot");
        }
    }
}
