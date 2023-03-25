using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace RetroStore.DataAccess.Migrations {
    /// <inheritdoc />
    public partial class ChangeReleaseDateToSQLDate : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Products",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
