using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class VehicleMinorChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Users_OwnerID",
                table: "Vehicles");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerID",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Vehicles",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.DropForeignKey(
                name: "FK_Refuels_Vehicles_VehicleID",
                table: "Refuels");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleID",
                table: "Refuels",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

             migrationBuilder.AddForeignKey(
                name: "FK_Refuels_Vehicles_VehicleID",
                table: "Refuels",
                column: "VehicleID",
                principalTable: "Vehicles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Users_OwnerID",
                table: "Vehicles",
                column: "OwnerID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Users_OwnerID",
                table: "Vehicles");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerID",
                table: "Vehicles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Vehicles",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.DropForeignKey(
                name: "FK_Refuels_Vehicles_VehicleID",
                table: "Refuels");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleID",
                table: "Refuels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

             migrationBuilder.AddForeignKey(
                name: "FK_Refuels_Vehicles_VehicleID",
                table: "Refuels",
                column: "VehicleID",
                principalTable: "Vehicles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Users_OwnerID",
                table: "Vehicles",
                column: "OwnerID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
