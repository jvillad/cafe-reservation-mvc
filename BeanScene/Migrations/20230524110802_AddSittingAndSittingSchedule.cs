using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeanScene.Migrations
{
    /// <inheritdoc />
    public partial class AddSittingAndSittingSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SittingSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SittingTypeId = table.Column<int>(type: "int", nullable: false),
                    StartTimeId = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTimeId = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Monday = table.Column<bool>(type: "bit", nullable: false),
                    Tuesday = table.Column<bool>(type: "bit", nullable: false),
                    Wednesday = table.Column<bool>(type: "bit", nullable: false),
                    Thursday = table.Column<bool>(type: "bit", nullable: false),
                    Friday = table.Column<bool>(type: "bit", nullable: false),
                    Saturday = table.Column<bool>(type: "bit", nullable: false),
                    Sunday = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SittingSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SittingSchedule_SittingType_SittingTypeId",
                        column: x => x.SittingTypeId,
                        principalTable: "SittingType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SittingSchedule_Timeslot_EndTimeId",
                        column: x => x.EndTimeId,
                        principalTable: "Timeslot",
                        principalColumn: "Time",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SittingSchedule_Timeslot_StartTimeId",
                        column: x => x.StartTimeId,
                        principalTable: "Timeslot",
                        principalColumn: "Time",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sitting",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SittingTypeId = table.Column<int>(type: "int", nullable: false),
                    StartTimeId = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTimeId = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sitting", x => new { x.Date, x.SittingTypeId });
                    table.ForeignKey(
                        name: "FK_Sitting_SittingSchedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "SittingSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sitting_SittingType_SittingTypeId",
                        column: x => x.SittingTypeId,
                        principalTable: "SittingType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sitting_Timeslot_EndTimeId",
                        column: x => x.EndTimeId,
                        principalTable: "Timeslot",
                        principalColumn: "Time",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sitting_Timeslot_StartTimeId",
                        column: x => x.StartTimeId,
                        principalTable: "Timeslot",
                        principalColumn: "Time",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sitting_EndTimeId",
                table: "Sitting",
                column: "EndTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sitting_ScheduleId",
                table: "Sitting",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sitting_SittingTypeId",
                table: "Sitting",
                column: "SittingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sitting_StartTimeId",
                table: "Sitting",
                column: "StartTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_SittingSchedule_EndTimeId",
                table: "SittingSchedule",
                column: "EndTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_SittingSchedule_SittingTypeId",
                table: "SittingSchedule",
                column: "SittingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SittingSchedule_StartTimeId",
                table: "SittingSchedule",
                column: "StartTimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sitting");

            migrationBuilder.DropTable(
                name: "SittingSchedule");
        }
    }
}
