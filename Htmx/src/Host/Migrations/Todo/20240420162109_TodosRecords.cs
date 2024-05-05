using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Host.Migrations.Todo
{
    /// <inheritdoc />
    public partial class TodosRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Deed = table.Column<string>(type: "TEXT", nullable: false),
                    Done = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}
