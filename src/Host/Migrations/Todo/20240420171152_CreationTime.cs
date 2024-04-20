using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Host.Migrations.Todo
{
    /// <inheritdoc />
    public partial class CreationTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "23550fe9-1246-4f5c-8e77-fe9b0d1d2241");

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "276a98c5-cc02-41d6-a8b2-48b22870ae5f");

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "748f1e7d-a21d-4ad9-940e-473f4617b580");

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "bb9126ae-03a9-43d0-b639-c60ac1fb8e95");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Todos",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "CreationDate", "Deed", "Done", "UserName" },
                values: new object[,]
                {
                    { "4b693b24-7a95-4c3d-9a98-817b8686ffd5", new DateTime(2024, 4, 20, 17, 11, 51, 258, DateTimeKind.Local).AddTicks(6518), "Buy stuff", true, "hubibubi@gmail.com" },
                    { "60e8269e-0344-4efa-9c6a-a0da4055e1e6", new DateTime(2024, 4, 20, 17, 11, 51, 258, DateTimeKind.Local).AddTicks(6528), "Go to gym", false, "hubibubi@gmail.com" },
                    { "829af27d-9ac1-4b8c-9046-1f100a3161ff", new DateTime(2024, 4, 20, 17, 11, 51, 258, DateTimeKind.Local).AddTicks(6484), "Clean house", true, "hubibubi@gmail.com" },
                    { "97087dfb-647c-4ae2-90ed-360514fa2674", new DateTime(2024, 4, 20, 17, 11, 51, 258, DateTimeKind.Local).AddTicks(6537), "Eat", false, "hubibubi@gmail.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "4b693b24-7a95-4c3d-9a98-817b8686ffd5");

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "60e8269e-0344-4efa-9c6a-a0da4055e1e6");

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "829af27d-9ac1-4b8c-9046-1f100a3161ff");

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: "97087dfb-647c-4ae2-90ed-360514fa2674");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Todos");

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Deed", "Done", "UserName" },
                values: new object[,]
                {
                    { "23550fe9-1246-4f5c-8e77-fe9b0d1d2241", "Go to gym", false, "hubibubi@gmail.com" },
                    { "276a98c5-cc02-41d6-a8b2-48b22870ae5f", "Buy stuff", true, "hubibubi@gmail.com" },
                    { "748f1e7d-a21d-4ad9-940e-473f4617b580", "Clean house", true, "hubibubi@gmail.com" },
                    { "bb9126ae-03a9-43d0-b639-c60ac1fb8e95", "Eat", false, "hubibubi@gmail.com" }
                });
        }
    }
}
