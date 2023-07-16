using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeanScene.Migrations
{
	/// <inheritdoc />
	public partial class SetScheduleIdAndSittingScheduleOptional : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Sitting_SittingSchedule_ScheduleId",
				table: "Sitting");

			migrationBuilder.AlterColumn<int>(
				name: "ScheduleId",
				table: "Sitting",
				type: "int",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "int");

			migrationBuilder.AddForeignKey(
				name: "FK_Sitting_SittingSchedule_ScheduleId",
				table: "Sitting",
				column: "ScheduleId",
				principalTable: "SittingSchedule",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Sitting_SittingSchedule_ScheduleId",
				table: "Sitting");

			migrationBuilder.AlterColumn<int>(
				name: "ScheduleId",
				table: "Sitting",
				type: "int",
				nullable: false,
				defaultValue: 0,
				oldClrType: typeof(int),
				oldType: "int",
				oldNullable: true);

			migrationBuilder.AddForeignKey(
				name: "FK_Sitting_SittingSchedule_ScheduleId",
				table: "Sitting",
				column: "ScheduleId",
				principalTable: "SittingSchedule",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
